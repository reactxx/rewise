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
void parallelEntryPoint<TaskMsg extends Msg> (List workerInitMsg, Future<List> action(TaskMsg msg), [void init()]) {
  if (init!=null) init();
  Worker(workerInitMsg, workerRun3Par: (self, msg) async {
    if (msg is TaskMsg)
      return await action(msg);
    else
      throw Exception('Unknown message');
  }).run();
}


class Parallel<TWorkerDone extends ContinueMsg> extends WorkerPool {
  Parallel(
      // iterable of command's for workers
      Iterable<List> tasks,
      //  num of workers
      int workersNum,
      // the same worker code for all workers
      WorkerEntryPoint entryPoint )
      : super((p) => _createProxies(p, workersNum, entryPoint)) {
    _tasks = tasks.iterator;
  }

  static List<Proxy> _createProxies(
          WorkerPool p, int workersNum, WorkerEntryPoint entryPoint) =>
      List<Proxy>.generate(workersNum, (i) => Proxy(p, entryPoint));

  callback(TWorkerDone msg) {}

  static Future<List> get workerReturnValue => Future.value(ContinueMsg.encode());

  Iterator<List> _tasks;
  Future mainStreamMsg(Msg msg, Proxy proxy) {
    if (msg is TWorkerDone) {
      if (!_tasks.moveNext()) {
        proxy.mainFinishWorker();
      } else
        proxy.sendMsg(_tasks.current);
      if (msg is TWorkerDone) {
        callback(msg);
        return Future.value(msg);
      } else
        return Future.value();
    } else
      return super.mainStreamMsg(msg, proxy);
  }
}
