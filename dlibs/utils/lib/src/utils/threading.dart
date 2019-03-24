import 'dart:isolate' show Isolate, ReceivePort, SendPort;

typedef EntryPoint = void Function(List);
typedef List<Thread> GetThreads(ThreadPool pool);
typedef SendMessage(Thread runner);
typedef Msg MsgDecode(List list);
typedef Future<bool> PXOnMsg(ThreadPool pool, Msg msg, Thread proxy);

abstract class Thread {
  // ------------- COMMMON
  SendPort sendPort;
  int id;

  // ------------- WORKER
  MsgDecode wkMsgDecoder;
  ReceivePort _wkReceivePort; // for sending back to POOL

  Thread.wk(this.wkMsgDecoder, List list) {
    final msg = wkMsgDecoder(list);
    id = msg.threadId;
    sendPort = msg.sendPort;
  }

  void wkRun() async {
    try {
      _wkReceivePort = ReceivePort();
      wkSendMsg(WorkerStartedMsg.encode());
      final stream = _wkReceivePort.map((list) => wkMsgDecoder(list));
      await wkOnStream(stream);
    } catch (exp, stacktrace) {
      wkSendMsg(ErrorMsg.encode(exp.toString(), stacktrace.toString()));
    }
    _wkReceivePort.close();
  }

  wkFinish() => wkSendMsg(WorkerFinished.encode());

  void wkSendMsg(List list) =>
      sendPort.send(createMsgLow(_wkReceivePort.sendPort, id, list));

  Future wkOnStream(Stream<Msg> stream) async {
    await for (final msg in stream) {
      if (msg is FinishWorker) break;
    }
  }

  // ------------- PROXY ON POOL SIDE
  ThreadPool pxPool;

  Thread.px(this.pxPool) : id = _idCounter++;
  static int _idCounter = 0;

  Future<Isolate> get pxIsolate async {
    final iso =
        await Isolate.spawn(pxWkCode, pxCreateMsg(WorkerStartedMsg.encode()));
    iso.addOnExitListener(pxPool.receivePort.sendPort,
        response: pxCreateMsg(WorkerFinished.encode()));
    return Future.value(iso);
  }

  List pxCreateMsg(List list) =>
      createMsgLow(pxPool.receivePort.sendPort, id, list);
  void pxSendMsg(List list) => sendPort.send(pxCreateMsg(list));
  void pxFinish() {
    pxPool.threads.remove(id);
    pxSendMsg(FinishWorker.encode());
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
  static Msg decodeMessage(List list) {
    switch (list[0]) {
      case Msg.id:
        return Msg.decode(list);
      case WorkerStartedMsg.id:
        return WorkerStartedMsg.decode(list);
      case WorkerFinished.id:
        return WorkerFinished.decode(list);
      case FinishWorker.id:
        return FinishWorker.decode(list);
      case ErrorMsg.id:
        return ErrorMsg.decode(list);
      default:
        throw Exception('Server: unknown thread mesage: $list[0]');
    }
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
      if (threads[msg.threadId] == null) continue;
      final proxy = threads[msg.threadId];
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

List createMsgLow(SendPort sendPort, int threadId, List list) =>
    [list[0], sendPort, threadId].followedBy(list.skip(1)).toList();

final futureFalse = Future.value(false);
final futureTrue = Future.value(true);

class Msg {
  static const id = 'th.common.Msg';
  final SendPort sendPort;
  final int threadId;

  static List encode() => [id];
  Msg.decode(List list)
      : sendPort = list[1],
        threadId = list[2];
}

class WorkerStartedMsg extends Msg {
  static const id = 'th.common.WorkerStartedMsg';
  static List encode() => [id];
  WorkerStartedMsg.decode(List list) : super.decode(list);
}

class WorkerFinished extends Msg {
  static const id = 'th.common.WorkerFinished';
  static List encode() => [id];
  WorkerFinished.decode(List list) : super.decode(list);
}

class FinishWorker extends Msg {
  static const id = 'th.common.FinishWorker';
  static List encode() => [id];
  FinishWorker.decode(List list) : super.decode(list);
}

class ErrorMsg extends Msg {
  static const id = 'th.common.ErrorMsg';
  String error;
  String stackTrace;

  static List encode(String error, String stackTrace) =>
      [id, error, stackTrace];
  ErrorMsg.decode(List list) : super.decode(list) {
    error = list[3];
    stackTrace = list[4];
  }
}
