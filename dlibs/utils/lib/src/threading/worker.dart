import 'dart:isolate' show ReceivePort;
import 'messages.dart';
import 'threading.dart' show trace;
import 'proxy.dart' show WorkerProxyCommon;

typedef WorkerEntryPoint = void Function(List workerInitMsg);
typedef WorkerRun2 = Future Function(Worker self, Msg input);
typedef WorkerRun1 = Future Function(Worker self, Stream<Msg> stream);

class Worker extends WorkerProxyCommon {
  Worker(List initMsg,
      {this.workerRun2Par, this.workerRun3Par}) {
    initMessageCreators();
    initMessage = decodeMessage(initMsg.iterator);
    threadId = initMessage.threadId;
    sendPort = initMessage.sendPort;
    receivePort = ReceivePort();
  }

  Msg initMessage;

  WorkerRun2 workerRun2Par;
  WorkerRun2 workerRun3Par;

  workerFinishedSelf() => sendMsg(WorkerFinished());

  void run() async {
    try {
      if (trace) print('WORKER START: $threadId-$initMessage');
      final stream = receivePort.map((list) => decodeMessage(list.iterator));
      await processMessage(initMessage);
      await for (final msg in stream) processMessage(msg);
    } catch (exp, stacktrace) {
      print(exp.toString());
      print(stacktrace.toString());
      sendError(exp, stacktrace);
    }
    receivePort.close();
  }

  Future processMessage(Msg msg) async {
    try {
      if (trace) print('WORKER MSG: $threadId-$msg');
      await workerRun2Par == null ? workerRun2(msg) : workerRun2Par(this, msg);
    } catch (exp, stacktrace) {
      sendError(exp, stacktrace);
    }
  }

  Future workerRun2(Msg msg) async {
    if (msg is FinishWorker) return Future.value();
    return await workerRun2Par == null
        ? workerRun3(msg)
        : workerRun2Par(this, msg);
  }

  Future workerRun3(Msg msg) => this.workerRun3Par != null
      ? this.workerRun3Par(this, msg)
      : throw Exception('Missing override');
}
