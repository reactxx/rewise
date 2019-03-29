import 'dart:isolate' show Isolate, ReceivePort, SendPort;
import 'messages.dart';

typedef EntryPoint = void Function(List);
typedef List<Worker> CreateProxies(WorkerPool pool);
typedef SendMessage(Worker runner);
typedef MsgLow MsgDecoder(List list);
typedef Future/*<bool | Msg> */ MainStreamMsg(
    WorkerPool pool, Msg msg, Worker proxy);
typedef Future WorkerMsg(Worker worker, Msg input);

const trace = false;

class Worker {
  Worker.proxy(this.pool, {List initPar}) {
    id = _idCounter++;
    receivePort = pool.receivePort;
    this.initPar = initPar;
  }
  Worker.worker(List list) {
    initMessages();
    final msg = decodeMessage(list) as WorkerInit;
    initPar = msg.par;
    id = msg.threadId;
    sendPort = msg.sendPort;
    receivePort = ReceivePort();
  }

  // ------------- COMMMON
  SendPort sendPort;
  ReceivePort receivePort; // for sending back to POOL
  int id;
  List initPar;

  static List createMsgLow(SendPort sendPort, int threadId, List list) =>
      [list[0], sendPort, threadId].followedBy(list.skip(1)).toList();
  List createMsg(List list) => createMsgLow(receivePort.sendPort, id, list);
  void sendMsg(List list) => sendPort.send(createMsg(list));
  void sendError(exp, StackTrace stacktrace) =>
      sendMsg(ErrorMsg.encode(exp.toString(), stacktrace.toString()));

  // ------------- WORKER
  workerFinishedSelf() => sendMsg(WorkerFinished.encode());

  void workerRun0() async {
    try {
      if (trace) print('WORKER START: $id');
      // notify pool
      sendMsg(WorkerStartedMsg.encode());
      // listen to stream
      final stream = receivePort.map((list) => decodeMessage(list) as Msg);
      await workerRun1(stream);
    } catch (exp, stacktrace) {
      print(exp.toString());
      print(stacktrace.toString());
      sendMsg(ErrorMsg.encode(exp.toString(), stacktrace.toString()));
    }
    receivePort.close();
  }


  //Future (Msg msg) async {}
  Future workerRun1(Stream<Msg> stream) async {
    await for (final msg in stream) {
      try {
        if (trace) print('WORKER MSG: $id-$msg');
        if (msg is FinishWorker) break;
        await workerRun2(msg);
      } catch (exp, stacktrace) {
        sendError(exp, stacktrace);
      }
    }
  }
  Future workerRun2(Msg input) async =>
      throw Exception('Unknown worker msg type: ${input}');

  // ------------- PROXY ON MAIN THREAD
  WorkerPool pool;

  static int _idCounter = 0;

  Future<Isolate> startWorker() async {
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

class WorkerPool {
  WorkerPool(CreateProxies createProxies, [this.mainStreamMsgPar]) {
    proxies = Map<int, Worker>.fromIterable(createProxies(this),
        key: (t) => (t as Worker).id);
    initMessages();
  }
  final MainStreamMsg mainStreamMsgPar;
  final receivePort = ReceivePort();
  Map<int, Worker> proxies;
  final result = List<Msg>();
  Future<List<Msg>> run() async {
    if (trace) print('MAIN STARTED');
    // start isolates
    await Future.wait(proxies.values.map((t) => t.startWorker()));
    if (trace) print('MAIN PROXIES STARTED');
    // run client message queue
    final msgStream = receivePort.map((list) => decodeMessage(list) as Msg);
    await for (var msg in msgStream) {
      if (trace) print('MAIN RUN: ${msg.threadId}-$msg');
      final proxy = proxies[msg.threadId];
      if (proxy == null) continue;
      if (msg is WorkerStartedMsg) proxy.sendPort = msg.sendPort;

      var res = mainStreamMsgPar != null
          ? await mainStreamMsgPar(this, msg, proxy)
          : await mainStreamMsg(msg, proxy);

      if (res == true) break;
      if (res is Msg) result.add(msg);
      if (proxies.length == 0) break;
    }
    return Future.value(result);
  }

  Future mainStreamMsg(Msg msg, Worker proxy) {
    if (proxies.length == 0) return futureTrue;
    if (msg is WorkerStartedMsg) {}
    if (msg is WorkerFinished) {
      proxies.remove(msg.threadId);
      if (proxies.length == 0) return futureTrue;
    } else if (msg is ErrorMsg)
      return Future.value(msg);
    else
      throw Exception(
          'Server: unknown thread mesage: ${msg.runtimeType.toString()}');
    return futureFalse;
  }
}

final futureFalse = Future.value(false);
final futureTrue = Future.value(true);
