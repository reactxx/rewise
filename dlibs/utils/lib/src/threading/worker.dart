import 'dart:isolate' show ReceivePort;
import 'messages.dart';
import 'threading.dart' show trace;
import 'proxy.dart' show WorkerProxyCommon;

typedef WorkerEntryPoint = void Function(List workerInitMsg);
typedef WorkerRun2 = Future Function(Worker self, Msg input);
typedef WorkerRun1 = Future Function(Worker self, Stream<Msg> stream);

/*
void _exampleEntryPoint(List workerInitMsg) {
  Worker(workerInitMsg, workerRun1Par: (self, stream) => null).run();
  //Worker(workerInitMsg, workerRun2Par: (self, msg) => null).run();
  //Worker(workerInitMsg, workerRun3Par: (self, msg) => null).run();
}
*/

class Worker extends WorkerProxyCommon {
  Worker(List list,
      {this.workerRun1Par, this.workerRun2Par, this.workerRun3Par}) {
    initMessages();
    initMessage = decodeMessage(list);
    id = initMessage.threadId;
    sendPort = initMessage.sendPort;
    receivePort = ReceivePort();
  }

  Msg initMessage;

  WorkerRun1 workerRun1Par;
  WorkerRun2 workerRun2Par;
  WorkerRun2 workerRun3Par;

  workerFinishedSelf() => sendMsg(WorkerFinished.encode());

  void run() async {
    try {
      initMessages();
      if (trace) print('WORKER START: $id');
      // listen to stream
      final stream = receivePort.map((list) => decodeMessage(list));
      await workerRun1Par == null
          ? workerRun1(stream)
          : workerRun1Par(this, stream);
    } catch (exp, stacktrace) {
      print(exp.toString());
      print(stacktrace.toString());
      sendMsg(ErrorMsg.encode(exp.toString(), stacktrace.toString()));
    }
    receivePort.close();
  }

  Future workerRun1(Stream<Msg> stream) async {
    await for (final msg in stream) {
      try {
        if (trace) print('WORKER MSG: $id-$msg');
        await workerRun2Par == null
            ? workerRun2(msg)
            : workerRun2Par(this, msg);
      } catch (exp, stacktrace) {
        sendError(exp, stacktrace);
      }
    }
    return Future.value();
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
