import 'messages.dart';
import 'threading.dart';

typedef Future WorkerRun2(Worker self, Msg input);
typedef Future WorkerRun1(Worker self, Stream<Msg> stream);

void workerRun1(Worker self, WorkerRun1 workerRun1) async =>
    _workerRun0(self, workerRun1);
Future workerRun2(Worker self, WorkerRun2 workerRun2) async =>
    _workerRun0(self, (self, stream) => _workerRun1(self, stream, workerRun2));

void _workerRun0(Worker self, WorkerRun1 sworkerRun1) async {
  try {
    if (trace) print('WORKER START: $self.id');
    // notify pool
    self.sendMsg(WorkerStartedMsg.encode());
    // listen to stream
    final stream = self.receivePort.map((list) => decodeMessage(list) as Msg);
    await sworkerRun1(self, stream);
  } catch (exp, stacktrace) {
    print(exp.toString());
    print(stacktrace.toString());
    self.sendMsg(ErrorMsg.encode(exp.toString(), stacktrace.toString()));
  }
  self.receivePort.close();
}

//Future (Msg msg) async {}
Future _workerRun1(Worker self, Stream<Msg> stream, sworkerRun2) async {
  await for (final msg in stream) {
    try {
      if (trace) print('WORKER MSG: ${self.id}-$msg');
      if (msg is FinishWorker) break;
      await sworkerRun2(self, msg);
    } catch (exp, stacktrace) {
      self.sendError(exp, stacktrace);
    }
  }
}
