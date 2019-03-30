import 'package:rw_utils/threading.dart';

void parallelStringEntryPoint(
        List workerInitMsg, Future<List> action(StringMsg msg)) =>
    parallelEntryPoint<StringMsg>(workerInitMsg, action, initRwParallel);

class ParallelString extends Parallel {
  ParallelString(Iterable<List> tasks, this._taskLen,
      WorkerEntryPoint entryPoint, int workersNum)
      : super(tasks, workersNum, entryPoint);

  int _taskLen;
  int _count = 1;
  @override
  callback(ContinueMsg msg) => print('${_count++} / $_taskLen');
}

class StringMsg extends Msg {
  StringMsg(this.relPath) : super();
  static const id = _namespace + 'ParseBook';
  String relPath;
  static List encode(String relPath) => [id, relPath];
  StringMsg.decode(List list) : super.decode(list) {
    relPath = list[3];
  }
}

const _namespace = 'rw.parallel.';
bool _called = false;
void initRwParallel() {
  if (!_called) {
    initMessages();
    messageDecoders.addAll(<String, DecodeProc>{
      StringMsg.id: (list) => StringMsg.decode(list),
    });
    _called = true;
  }
}
