import 'package:rw_utils/threading.dart';

typedef Worker CreateWorker(WorkerPool pool);

class ParallelString extends Parallel<StringMsg, ContinueMsg> {
  ParallelString(
      Iterable<List> tasks, this._taskLen, CreateWorker workerCreator, int workersNum)
      : super(tasks, workerCreator, workersNum) {
    _initThreadingTest();
    _taskLen = tasks.length;
  }

  static Future<List> START(
      tasks, int taskLen, CreateWorker workerCreator, int workersNum) async {
    final parallel = ParallelString(tasks, taskLen, workerCreator, workersNum);
    return await parallel.runParallel();
  }

  int _taskLen;
  int _count = 1;
  @override
  callback(ContinueMsg msg) => print('${_count++} / $_taskLen');

}

class ParallelStringWorker extends Worker {
  ParallelStringWorker.proxy(pool) : super.proxy(pool) {}
  ParallelStringWorker.worker(List list) : super.worker(list) {
    _initThreadingTest();
  }

  Future workerRun3(String par) => throw Exception('Unknown workerProc');
  @override
  Future workerRun2(Msg input) async {
    if (input is StringMsg) {
      await workerRun3(input.relPath);
      sendMsg(ContinueMsg.encode());
    } else
      return super.workerRun2(input);
  }

  @override
  EntryPoint get entryPoint => workerCode;
  static void workerCode(List l) => ParallelStringWorker.worker(l).workerRun0();
}

class StringMsg extends Msg {
  static const id = _namespace + 'ParseBook';
  String relPath;
  static List encode(String relPath) => [id, relPath];
  StringMsg.decode(List list) : super.decode(list) {
    relPath = list[3];
  }
}

const _namespace = 'rw.parsing.';
bool _called = false;
void _initThreadingTest() {
  if (!_called) {
    initMessages();
    messageDecoders.addAll(<String, DecodeProc>{
      StringMsg.id: (list) => StringMsg.decode(list),
    });
    _called = true;
  }
}
