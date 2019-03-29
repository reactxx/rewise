import 'package:rw_utils/threading.dart';

typedef RunTask<TIn extends Msg> = Future<List> Function(TIn input);

typedef Worker CreateWorker(WorkerPool pool);

class Parallel<TIn extends Msg, TOut extends Msg> extends WorkerPool {
  Parallel(Iterable<List> tasks /* iterable of TIn.encode() */,
      CreateWorker workerCreator, int workersNum)
      : super((p) => _createProxies(p, workerCreator, workersNum)) {
    _tasks = tasks.iterator;
  }

  static List<Worker> _createProxies(
          WorkerPool p, CreateWorker workerCreator, int workersNum) =>
      List<Worker>.generate(workersNum, (i) => workerCreator(p));


  callback(TOut msg) {}
  Future<List<Msg>> runParallel() async => super.run();

  //Parallel._(CreateProxies createProxies) : super(createProxies);
  Iterator<List> _tasks;
  Future mainStreamMsg(Msg msg, Worker proxy) {
    if (msg is TOut || msg is WorkerStartedMsg) {
      if (!_tasks.moveNext()) {
        proxy.mainFinishWorker();
      } else
        proxy.sendMsg(_tasks.current);
      if (msg is TOut) {
        callback(msg);
        return Future.value(msg);
      } else
        return Future.value();
    } else
      return super.mainStreamMsg(msg, proxy);
  }
}
