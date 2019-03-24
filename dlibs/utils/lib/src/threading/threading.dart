import 'dart:isolate' show Isolate, ReceivePort, SendPort;
import 'messages.dart';

typedef EntryPoint = void Function(List);
typedef List<Thread> GetThreads(ThreadPool pool);
typedef SendMessage(Thread runner);
typedef Msg MsgDecoder(List list);
typedef Future<bool> PXOnMsg(ThreadPool pool, Msg msg, Thread proxy);

abstract class Thread {
  Thread.px(this.pxPool, this.decoder) {
    id = _idCounter++;
    receivePort = pxPool.receivePort;
    //sendPort assigned by Pool from WorkerStartedMsg msg:
    //if (msg is WorkerStartedMsg) proxy.sendPort = msg.sendPort;
  }
  Thread.wk(List list, this.decoder) {
    final msg = decoder(list);
    id = msg.threadId;
    sendPort = msg.sendPort;
    receivePort = ReceivePort();
  }

  // ------------- COMMMON
  SendPort sendPort;
  ReceivePort receivePort; // for sending back to POOL
  int id;
  MsgDecoder decoder;

  static List createMsgLow(SendPort sendPort, int threadId, List list) =>
      [list[0], sendPort, threadId].followedBy(list.skip(1)).toList();
  List createMsg(List list) => createMsgLow(receivePort.sendPort, id, list);
  void sendMsg(List list) => sendPort.send(createMsg(list));

  // ------------- WORKER

  void wkRun() async {
    try {
      sendMsg(WorkerStartedMsg.encode()); 
      final stream = receivePort.map((list) => decoder(list));
      await wkOnStream(stream);
    } catch (exp, stacktrace) {
      sendMsg(ErrorMsg.encode(exp.toString(), stacktrace.toString()));
    }
    receivePort.close();
  }

  wkFinish() => sendMsg(WorkerFinished.encode());

  // void wkSendMsg(List list) =>
  //     sendPort.send(createMsgLow(receivePort.sendPort, id, list));

  Future wkOnStream(Stream<Msg> stream) async {
    await for (final msg in stream) {
      if (msg is FinishWorker) break;
    }
  }

  // ------------- PROXY ON POOL SIDE
  ThreadPool pxPool;

  static int _idCounter = 0;

  Future<Isolate> get pxIsolate async {
    final iso =
        await Isolate.spawn(pxWkCode, createMsg(WorkerStartedMsg.encode()));
    iso.addOnExitListener(receivePort.sendPort,
        response: createMsg(WorkerFinished.encode()));
    return Future.value(iso);
  }

  void pxFinish() {
    pxPool.threads.remove(id);
    sendMsg(FinishWorker.encode());
  }

  EntryPoint get pxWkCode => workerEntryPoint;
  static workerEntryPoint(List msg) => throw Exception(
      'Missing ThreadProxy.entryPoint override'); //??ThreadRunner(msg).run();
}

class ThreadPool {
  ThreadPool(GetThreads getThreads, [this.pxOnMsgPar]) {
    threads = Map<int, Thread>.fromIterable(getThreads(this),
        key: (t) => (t as Thread).id);
  }
  final PXOnMsg pxOnMsgPar;
  final receivePort = ReceivePort();
  Map<int, Thread> threads;
  List<ErrorMsg> errors;
  Future<List<ErrorMsg>> run() async {
    // start isolates
    await Future.wait(threads.values.map((t) => t.pxIsolate));
    // run client message queue
    final msgStream = receivePort.map((list) => decodeMessage(list));
    await for (var msg in msgStream) {
      final proxy = threads[msg.threadId];
      if (proxy == null) continue;
      if (msg is WorkerStartedMsg) proxy.sendPort = msg.sendPort;

      var quit = pxOnMsgPar != null
          ? await pxOnMsgPar(this, msg, proxy)
          : await pxOnMsg(msg, proxy);

      if (quit) break;
    }
    return Future.value(errors);
  }

  Future<bool> pxOnMsg(Msg msg, Thread proxy) {
    if (threads.length == 0) return futureTrue;
    if (msg is WorkerStartedMsg) {
    } else if (msg is WorkerFinished) {
      threads.remove(msg.threadId);
      if (threads.length == 0) return futureTrue;
    } else if (msg is ErrorMsg) {
      threads.remove(msg.threadId);
      (errors ?? (errors = List<ErrorMsg>())).add(msg);
      if (threads.length == 0) return futureFalse;
    } else
      throw Exception(
          'Server: unknown thread mesage: ${msg.runtimeType.toString()}');
    return futureFalse;
  }
}

final futureFalse = Future.value(false);
final futureTrue = Future.value(true);
