import 'dart:isolate' show Isolate, ReceivePort, SendPort;

typedef EntryPoint = void Function(List);
typedef List<Thread> GetThreads(ThreadPool pool);
typedef SendMessage(ThreadRunner runner);

class Msg {
  static const id = 'th.common.Msg';
  final SendPort sendPort;
  final int threadId;

  static List encode() => [id];
  Msg.server(List list)
      : sendPort = list[0],
        threadId = list[1];
}

class StartMsg extends Msg {
  static const id = 'th.common.StartMsg';

  static List encode() => [id];
  StartMsg.server(List list) : super.server(list);
}

class EndMsg extends Msg {
  static const id = 'th.common.EndMsg';

  static List encode() => [id];
  EndMsg.server(List list) : super.server(list);
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

abstract class ThreadRunner {
  ThreadRunner(this.msg);
  StartMsg msg;
  ReceivePort receivePort;
  Future doRun() async {
    try {
      receivePort = ReceivePort();
      await run();
    } catch (exp, stacktrace) {
      send(ErrorMsg.encode(exp.toString(), stacktrace.toString()));
    }
  }

  void send(List list) => createMsg(receivePort.sendPort, msg.threadId, list);

  Future run();
}

List createMsg(SendPort sendPort, int threadId, List list) =>
    [list[0], sendPort, threadId].followedBy(list.skip(1));

abstract class Thread {
  Thread(this.pool);
  final ThreadPool pool;
  SendPort sendPort;
  final id = _idCounter++;
  Future<Isolate> get isolate async {
    final iso =
        await Isolate.spawn(entryPoint, createMsg(pool.receivePort.sendPort, id, StartMsg.encode()));
    iso.addOnExitListener(pool.receivePort.sendPort,
        response: 'exit'); //EndMsg(null, id));
    return Future.value(iso);
  }

  static int _idCounter = 0;

  EntryPoint get entryPoint => _a;
  static _a(List msg) => null; //??ThreadRunner(msg).run();
}

abstract class ThreadPool {
  ThreadPool(List<Thread> getThreads(ThreadPool pool)) {
    threads = Map<int, Thread>.fromIterable(getThreads(this),
        key: (t) => (t as Thread).id);
  }
  static Msg decodeMessage(List list) {
    switch (list[0]) {
      case Msg.id: return Msg.server(list);
      case StartMsg.id: return StartMsg.server(list);
      case EndMsg.id: return EndMsg.server(list);
      case ErrorMsg.id: return ErrorMsg.server(list);
      default:
        throw Exception('Unknown thread mesage: $list[0]');
    }
  }

  final receivePort = ReceivePort();
  Map<int, Thread> threads;
  Future<List<ErrorMsg>> run() async {
    // start isolates
    await Future.wait(threads.values.map((t) => t.isolate));
    List<ErrorMsg> errors;
    await for (var msg
        in receivePort.map((list) => decodeMessage(list))) {
      if (msg is StartMsg)
        threads[msg.threadId].sendPort = msg.sendPort;
      else if (msg is EndMsg) {
        threads.remove(msg.threadId);
        if (threads.length == 0) break;
      } else if (msg is ErrorMsg) {
        threads.remove(msg.threadId);
        (errors ?? (errors = List<ErrorMsg>())).add(msg);
        if (threads.length == 0) break;
      } else if (msg is Msg) await onMsg(msg);
    }
    return Future.value(errors);
  }

  onMsg(Msg msg);
}
