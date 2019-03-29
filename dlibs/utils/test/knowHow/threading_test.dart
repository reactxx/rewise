@test.Timeout(const Duration(hours: 1))

import 'package:test/test.dart' as test;
import 'dart:isolate' show Isolate, ReceivePort;
import 'package:rw_utils/threading.dart';

main() {
  test.group('isolate', () {
    test.test('addOnExitListener', () async {
      final receivePort = ReceivePort();
      var ds = _delayThread;
      var iso = await Isolate.spawn(ds, receivePort.sendPort);
      iso.addOnExitListener(receivePort.sendPort, response: 'done');
      await for (var msg in receivePort) {
        test.expect(msg, test.equals('done'));
        break;
      }
      return Future.value();
    });

    test.test('addErrorListener', () async {
      final receivePort = ReceivePort();
      var iso = await Isolate.spawn(_errorThread, receivePort.sendPort);
      iso.setErrorsFatal(false);
      iso.addErrorListener(receivePort.sendPort);
      await for (var msg in receivePort) {
        test.expect(msg[0], test.equals('Exception: ERROR'));
        break;
      }
      return Future.value();
    });

    test.test('threading', () async {
      final res = await TThread.START(10);
      test.expect(res.length, test.equals(0));
    });

    test.test('parallel', () async {
      final tasks = 10;
      final res = await TParallel.START(tasks, 5);
      test.expect(res.length, test.equals(tasks));
    });
  });
}

_delayThread(par) => Future.delayed(Duration(seconds: 1));

_errorThread(par) {
  Future.delayed(Duration(seconds: 1));
  throw Exception('ERROR');
}

class TParallel extends Parallel<TestMsg, TestMsg> {
  TParallel(Iterable<List> tasks, int workersNum)
      : super(tasks, (p) => TPWorker.proxy(p), workersNum) {
    initThreadingTest();
  }

  static Future<List> START(int taskNum, num parallels) async {
    final tasks = List.generate(taskNum, (idx) => TestMsg.encode());
    final parallel = TParallel(tasks, parallels);
    return await parallel.runParallel();
  }
}

class TPWorker extends Worker {
  TPWorker.proxy(pool, {List initPar}) : super.proxy(pool);
  TPWorker.worker(List list) : super.worker(list) {
    initThreadingTest();
  }
  @override
  Future workerRun2(Msg input) async {
    if (input is TestMsg) {
      await Future.delayed(Duration(milliseconds: 500));
      sendMsg(TestMsg.encode());
    } else
      return super.workerRun2(input);
  }

  @override
  EntryPoint get entryPoint => workerCode;
  static void workerCode(List l) {
    TPWorker.worker(l).workerRun0();
  }
}

class TThread extends Worker {
  //*****************************************
  //  MAIN CODE
  //*****************************************

  // main proc on client side
  static Future<List<Msg>> START(int workersNum) async {
    final CreateProxies createThreads = (WorkerPool p) => List.generate(
        workersNum,
        (idx) => TThread.proxy(p, InitPar.encode(['en-GB', 'cs-CZ'])));
    initThreadingTest();
    return WorkerPool(createThreads, TThread.mainStreamMsg).run();
  }

  // message decoder
  // message dispatcher on main thread
  static final MainStreamMsg mainStreamMsg = (pool, msg, proxy) {
    if (msg is WorkerStartedMsg || msg is TestMsg) {
      proxy.sendMsg(TestMsg.encode());
      return futureFalse;
    }
    return pool.mainStreamMsg(msg, proxy);
  };

  // message dispatcher on worker thread
  @override
  Future workerRun1(Stream<Msg> stream) async {
    var testMsgCount = 3;
    final par = InitPar.decode(initPar);
    if (par == null) return;
    //await Future.delayed(Duration(seconds: 1));
    // don't start queue => addOnExitListener is in action
    //return Future.value();
    await for (final msg in stream) {
      //await Future.delayed(Duration(seconds: 1));
      if (msg is FinishWorker) break;
      if (msg is TestMsg) {
        if (testMsgCount-- == 0) break;
        sendMsg(TestMsg.encode());
        //workerFinishedSelf();
      }
    }
  }

  //*****************************************
  //  must-be code
  //*****************************************
  TThread.proxy(WorkerPool pool, List initPar)
      : super.proxy(pool, initPar: initPar);
  TThread.worker(List list) : super.worker(list) {
    initThreadingTest();
  }

  @override
  EntryPoint get entryPoint => workerCode;
  static void workerCode(List l) => TThread.worker(l).workerRun0();
}

const _namespace = 'th.test.';

void initThreadingTest() {
  if (_called) return;
  initMessages();
  messageDecoders.addAll(<String, DecodeProc>{
    InitPar.id: (list) => InitPar.decode(list),
    TestMsg.id: (list) => TestMsg.decode(list),
  });
  _called = true;
}

bool _called = false;

class InitPar extends MsgLow {
  static const id = _namespace + 'InitPar';
  List<String> langs;
  static List encode(List<String> langs) => [id].followedBy(langs).toList();
  InitPar.decode(List list) : super.decode(list) {
    langs = list.skip(1).cast<String>().toList();
  }
}

class TestMsg extends Msg {
  static const id = _namespace + 'TestMsg';
  static List encode() => [id];
  TestMsg.decode(List list) : super.decode(list);
}

main_() async {
  //final res = await TThread.START(1);
  final res = await TParallel.START(10, 5);
  print(res.length == 10
      ? '****** SUCCESS **********'
      : '****** ERROR **********');
  return Future.value();
}
