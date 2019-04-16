import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_utils/threading.dart';

Future processTasks(WorkerEntryPoint entryPoint,
    Future<Msg> action(DataMsg msg, InitMsg initPar), List<DataMsg> tasks,
    {bool emptyPrint = true,
    InitMsg initPar,
    String printDetail(DataMsg msg),
    bool doParallel}) async {
  //proc
  void doPrint(int c, m) {
    if (emptyPrint) return;
    print('$c/${tasks.length} ${printDetail == null ? '' : printDetail(m)}');
  }

  if (doParallel == null ? fileSystem.desktop : doParallel == true) {
    await Parallel(tasks, 4, entryPoint, initPar: initPar).run(traceMsg: doPrint);
  } else {
    var count = 0;
    for (final msg in tasks) {
      doPrint(++count, msg);
      await action(msg, initPar);
    }
  }
}

void parallelEntryPoint(List workerInitMsg, Future<Msg> action(DataMsg msg, InitMsg initMsg)) {
  Worker(workerInitMsg, workerRun3Par: (self, msg) async {
    if (msg is DataMsg) {
      if (trace) print('Parallel worker ACTION: ${msg.threadId}-$msg');
      final respMsg = await action(msg, self.initMessage);
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
      WorkerEntryPoint entryPoint,
      {InitMsg initPar})
      : super((p) => _createProxies(p, workersNum, entryPoint, initPar: initPar)) {
    _tasks = tasks.iterator;
  }

  static List<Proxy> _createProxies(
      WorkersPool p, int workersNum, WorkerEntryPoint entryPoint, {InitMsg initPar}) {
    return List<Proxy>.generate(workersNum, (i) => Proxy(p, entryPoint, initPar: initPar));
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
