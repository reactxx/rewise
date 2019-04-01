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
      self.sendMsg(await action(msg));
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

  callback(ContinueMsg msg) => print('${_count++} / $taskLen');
  int taskLen;
  int _count = 1;

  static Future<List> get workerReturnFuture => Future.value(workerReturn);
  static List get workerReturn => ContinueMsg.encode();

  Iterator<List> _tasks;
  Future mainStreamMsg(Msg msg, Proxy proxy) {
    if (msg is ContinueMsg) {
      if (!_tasks.moveNext()) {
        proxy.mainFinishWorker();
      } else
        proxy.sendMsg(_tasks.current);

      if (msg is ContinueMsg) {
        callback(msg);
        return Future.value(msg);
      } else
        return futureFalse;
    } else
      return super.mainStreamMsg(msg, proxy);
  }
}
