import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_utils/threading.dart';

Future processTasks(WorkerEntryPoint entryPoint,
    Future<Msg> action(DataMsg msg), List<DataMsg> tasks,
    {bool emptyPrint = true,
    String printDetail(DataMsg msg),
    bool doParallel}) async {
  //proc
  void doPrint(int c, m) {
    if (emptyPrint) return;
    print('$c/${tasks.length} ${printDetail == null ? '' : printDetail(m)}');
  }

  if (doParallel == null ? fileSystem.desktop : doParallel == true) {
    await Parallel(tasks, 4, entryPoint).run(traceMsg: doPrint);
  } else {
    var count = 0;
    for (final msg in tasks) {
      doPrint(++count, msg);
      await action(msg);
    }
  }
}

void parallelEntryPoint(List workerInitMsg, Future<Msg> action(DataMsg msg)) {
  Worker(workerInitMsg, workerRun3Par: (self, msg) async {
    if (msg is DataMsg) {
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
      List<DataMsg> tasks,
      //  num of workers
      int workersNum,
      // the same worker code for all workers
      WorkerEntryPoint entryPoint)
      : super((p) => _createProxies(p, workersNum, entryPoint)) {
    _tasks = tasks.iterator;
  }

  static List<Proxy> _createProxies(
      WorkersPool p, int workersNum, WorkerEntryPoint entryPoint) {
    return List<Proxy>.generate(workersNum, (i) => Proxy(p, entryPoint));
  }

  int taskLen;
  int _count = 1;
  _callback(int count, msg) => print('${count} / $taskLen (${msg.toString()})');

  static Future<Msg> get workerReturnFuture => Future.value(workerReturn);
  static ContinueMsg get workerReturn => ContinueMsg();

  Iterator<DataMsg> _tasks;
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
