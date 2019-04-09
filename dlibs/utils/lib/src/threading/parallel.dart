import 'package:rw_utils/threading.dart';

/*
void _exampleEntryPoint(List workerInitMsg) {
  // register messages...
  // run worker
  Worker(workerInitMsg, workerRun3Par: (self, msg) => null).run();
}

Future _exampleRun() async {
  return Parallel(null, 3, _exampleEntryPoint).run();
}
*/
void parallelEntryPoint<TaskMsg extends Msg>(
    List workerInitMsg, Future<List> action(TaskMsg msg),
    [void init()]) {
  if (init != null) init();
  Worker(workerInitMsg, workerRun3Par: (self, msg) async {
    if (msg is TaskMsg) {
      if (trace) print('Parallel worker ACTION: ${msg.threadId}-$msg');
      final respMsg = await action(msg);
      assert(respMsg != null);
      self.sendMsg(respMsg);
    } else {
      if (trace) print('Parallel worker CONTINUE: ${msg.threadId}-$msg');
      self.sendMsg(Parallel.workerReturn);
    }
    return Future.value();
  }).run();
}

class Parallel extends WorkersPool {
  Parallel(
      // iterable of command's for workers
      Iterable<List> tasks,
      //  num of workers
      int workersNum,
      // the same worker code for all workers
      WorkerEntryPoint entryPoint,
      {this.taskLen})
      : super((p) => _createProxies(p, workersNum, entryPoint)) {
    _tasks = tasks.iterator;
  }

  static List<Proxy> _createProxies(
      WorkersPool p, int workersNum, WorkerEntryPoint entryPoint) {
    return List<Proxy>.generate(workersNum, (i) => Proxy(p, entryPoint));
  }

  int taskLen;
  int _count = 1;
  _callback(int count, msg) =>
      print('${count} / $taskLen (${msg.toString()})');

  static Future<List> get workerReturnFuture => Future.value(workerReturn);
  static List get workerReturn => ContinueMsg.encode();

  Iterator<List> _tasks;
  Future mainStreamMsg(Msg msg, Proxy proxy, {void traceMsg(int count, msg)}) {
    if (msg is ContinueMsg) {
      if (!_tasks.moveNext()) {
        proxy.mainFinishWorker();
      } else {
        _count++;
        (traceMsg ?? _callback)(_count, _tasks.current);
        proxy.sendMsg(_tasks.current);
      }

      if (msg is ContinueMsg) {
        return Future.value(msg);
      } else
        return futureFalse;
    } else
      return super.mainStreamMsg(msg, proxy);
  }
}
