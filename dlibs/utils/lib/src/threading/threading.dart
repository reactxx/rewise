import 'dart:isolate' show Isolate, ReceivePort, SendPort;

part 'messages.dart';

typedef EntryPoint = void Function(List);
typedef List<Thread> CreateProxies(ThreadPool pool);
typedef SendMessage(Thread runner);
typedef MsgLow MsgDecoder(List list);
typedef Future<bool> MainStreamMsg(ThreadPool pool, Msg msg, Thread proxy);

abstract class Thread {
  Thread.proxy(this.pool, this.decoder, [this.initPar]) {
    id = _idCounter++;
    receivePort = pool.receivePort;
    //sendPort assigned by Pool from WorkerStartedMsg msg:
    //if (msg is WorkerStartedMsg) proxy.sendPort = msg.sendPort;
  }
  Thread.worker(List list, this.decoder) {
    final msg = decoder(list) as WorkerInit;
    initPar = msg.par;
    id = msg.threadId;
    sendPort = msg.sendPort;
    receivePort = ReceivePort();
  }

  // ------------- COMMMON
  SendPort sendPort;
  ReceivePort receivePort; // for sending back to POOL
  int id;
  MsgDecoder decoder;
  List initPar;

  static List createMsgLow(SendPort sendPort, int threadId, List list) =>
      [list[0], sendPort, threadId].followedBy(list.skip(1)).toList();
  List createMsg(List list) => createMsgLow(receivePort.sendPort, id, list);
  void sendMsg(List list) => sendPort.send(createMsg(list));

  // ------------- WORKER

  void workerRun() async {
    try {
      // notify pool
      sendMsg(WorkerStartedMsg.encode()); 
      // listen to stream
      final stream = receivePort.map((list) => decoder(list) as Msg);
      await workerStream(stream);
    } catch (exp, stacktrace) {
      sendMsg(ErrorMsg.encode(exp.toString(), stacktrace.toString()));
    }
    receivePort.close();
  }

  workerFinishedSelf() => sendMsg(WorkerFinished.encode());

  // void wkSendMsg(List list) =>
  //     sendPort.send(createMsgLow(receivePort.sendPort, id, list));

  Future workerStream(Stream<Msg> stream) async {
    await for (final msg in stream) {
      if (msg is FinishWorker) break;
    }
  }

  // ------------- PROXY ON MAIN THREAD
  ThreadPool pool;

  static int _idCounter = 0;

  Future<Isolate> createIsolate() async {
    final iso =
        await Isolate.spawn(entryPoint, createMsg(WorkerInit.encode(initPar)));
    iso.addOnExitListener(receivePort.sendPort,
        response: createMsg(WorkerFinished.encode()));
    return Future.value(iso);
  }

  void mainFinishWorker() {
    pool.proxies.remove(id);
    sendMsg(FinishWorker.encode());
  }

  EntryPoint get entryPoint => workerEntryPoint;
  static workerEntryPoint(List msg) => throw Exception(
      'Missing ThreadProxy.entryPoint override'); //??ThreadRunner(msg).run();
}

class ThreadPool {
  ThreadPool(CreateProxies createProxies, [this.mainStreamMsgPar]) {
    proxies = Map<int, Thread>.fromIterable(createProxies(this),
        key: (t) => (t as Thread).id);
  }
  final MainStreamMsg mainStreamMsgPar;
  final receivePort = ReceivePort();
  Map<int, Thread> proxies;
  List<ErrorMsg> errors;
  Future<List<ErrorMsg>> run(MsgDecoder decoder) async {
    // start isolates
    await Future.wait(proxies.values.map((t) => t.createIsolate()));
    // run client message queue
    final msgStream = receivePort.map((list) => decoder(list) as Msg);
    await for (var msg in msgStream) {
      final proxy = proxies[msg.threadId];
      if (proxy == null) continue;
      if (msg is WorkerStartedMsg) proxy.sendPort = msg.sendPort;

      var quit = mainStreamMsgPar != null
          ? await mainStreamMsgPar(this, msg, proxy)
          : await mainStreamMsg(msg, proxy);

      if (quit) break;
    }
    return Future.value(errors);
  }

  Future<bool> mainStreamMsg(Msg msg, Thread proxy) {
    if (proxies.length == 0) return futureTrue;
    if (msg is WorkerStartedMsg) {
    } if (msg is WorkerFinished) {
      proxies.remove(msg.threadId);
      if (proxies.length == 0) return futureTrue;
    } else if (msg is ErrorMsg) {
      proxies.remove(msg.threadId);
      (errors ?? (errors = List<ErrorMsg>())).add(msg);
      if (proxies.length == 0) return futureTrue;
    } else
      throw Exception(
          'Server: unknown thread mesage: ${msg.runtimeType.toString()}');
    return futureFalse;
  }
}

final futureFalse = Future.value(false);
final futureTrue = Future.value(true);
