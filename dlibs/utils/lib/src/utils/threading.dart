import 'dart:isolate' show Isolate, ReceivePort, SendPort;

typedef EntryPoint = void Function(List);
typedef List<ThreadProxy> GetThreads(ThreadPool pool);
typedef SendMessage(Worker runner);
typedef Msg DecodeMessage(List list);

class Msg {
  static const id = 'th.common.Msg';
  final SendPort sendPort;
  final int threadId;

  static List encode() => [id];
  Msg.server(List list)
      : sendPort = list[1],
        threadId = list[2];
}

class WorkerStartedMsg extends Msg {
  static const id = 'th.common.WorkerStartedMsg';
  static List encode() => [id];
  WorkerStartedMsg.server(List list) : super.server(list);
}

class WorkerFinished extends Msg {
  static const id = 'th.common.WorkerFinished';
  static List encode() => [id];
  WorkerFinished.server(List list) : super.server(list);
}

class FinishWorker extends Msg {
  static const id = 'th.common.FinishWorker';
  static List encode() => [id];
  FinishWorker.server(List list) : super.server(list);
}

class ErrorMsg extends Msg {
  static const id = 'th.common.ErrorMsg';
  String error;
  String stackTrace;

  static List encode(String error, String stackTrace) =>
      [id, error, stackTrace];
  ErrorMsg.server(List list) : super.server(list) {
    error = list[3];
    stackTrace = list[4];
  }
}

abstract class Worker {
  Worker(DecodeMessage decodeMessage, List list)
      : decodeMessage = decodeMessage,
        msg = decodeMessage(list);
  final WorkerStartedMsg msg;
  final DecodeMessage decodeMessage;
  ReceivePort receivePort;
  Future doRun() async {
    try {
      receivePort = ReceivePort();
      sendMsg(WorkerStartedMsg.encode());
      await for (final list in receivePort) {
        final msg = decodeMessage(list);
        final quit = await onMsg(msg);
        if (quit) break;
        if (msg is FinishWorker) break;
      }
    } catch (exp, stacktrace) {
      sendMsg(ErrorMsg.encode(exp.toString(), stacktrace.toString()));
    }
    return Future.value();
  }

  finish() => sendMsg(WorkerFinished.encode());

  void sendMsg(List list) =>
      msg.sendPort.send(createMsgLow(receivePort.sendPort, msg.threadId, list));

  Future<bool> onMsg(Msg msg);
}

List createMsgLow(SendPort sendPort, int threadId, List list) =>
    [list[0], sendPort, threadId].followedBy(list.skip(1)).toList();

abstract class ThreadProxy {
  ThreadProxy(this.pool);
  final ThreadPool pool;
  SendPort sendPort;
  final id = _idCounter++;
  Future<Isolate> get isolate async {
    final iso =
        await Isolate.spawn(entryPoint, createMsg(WorkerStartedMsg.encode()));
    iso.addOnExitListener(pool.receivePort.sendPort,
        response: createMsg(WorkerFinished.encode()));
    return Future.value(iso);
  }

  List createMsg(List list) =>
      createMsgLow(pool.receivePort.sendPort, id, list);
  void sendMsg(List list) => sendPort.send(createMsg(list));
  void finish() {
    pool.threads.remove(id);
    sendMsg(FinishWorker.encode());
  }

  static int _idCounter = 0;

  EntryPoint get entryPoint => workerEntryPoint;
  static workerEntryPoint(List msg) => throw Exception(
      'Missing ThreadProxy.entryPoint override'); //??ThreadRunner(msg).run();
}

abstract class ThreadPool {
  ThreadPool(List<ThreadProxy> getThreads(ThreadPool pool)) {
    threads = Map<int, ThreadProxy>.fromIterable(getThreads(this),
        key: (t) => (t as ThreadProxy).id);
  }
  static Msg decodeMessage(List list) {
    switch (list[0]) {
      case Msg.id:
        return Msg.server(list);
      case WorkerStartedMsg.id:
        return WorkerStartedMsg.server(list);
      case WorkerFinished.id:
        return WorkerFinished.server(list);
      case FinishWorker.id:
        return FinishWorker.server(list);
      case ErrorMsg.id:
        return ErrorMsg.server(list);
      default:
        throw Exception('Server: unknown thread mesage: $list[0]');
    }
  }

  final receivePort = ReceivePort();
  Map<int, ThreadProxy> threads;
  List<ErrorMsg> errors;
  Future<List<ErrorMsg>> run() async {
    // start isolates
    await Future.wait(threads.values.map((t) => t.isolate));
    // run client message queue
    final msgStream = receivePort.map((list) => decodeMessage(list));
    await for (var msg in msgStream) {
      if (msg is WorkerStartedMsg)
        threads[msg.threadId].sendPort = msg.sendPort;
      var quit = await onMsg(msg, threads[msg.threadId]);
      if (quit) break;
    }
    return Future.value(errors);
  }

  Future<bool> onMsg(Msg msg, ThreadProxy proxy) {
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
