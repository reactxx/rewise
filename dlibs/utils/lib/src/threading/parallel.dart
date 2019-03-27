import 'package:rw_utils/threading.dart';

typedef RunTask<TIn extends Msg> = Future<List> Function(TIn input);

class Parallel<TIn extends Msg, TOut extends Msg> extends WorkerPool {
  Parallel(Iterable<List> tasks /* iterable of TIn.encode() */,
      CreateProxies createProxies)
      : super(createProxies) {
    _tasks = tasks.iterator;
  }

  callback(TOut msg) {}
  Future<List<Msg>> runParallel() async => super.run();

  Parallel._(CreateProxies createProxies) : super(createProxies);
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
