//https://webdev.dartlang.org/articles/performance/event-loop
import 'package:test/test.dart' as test;
import 'dart:isolate' show Isolate, ReceivePort, SendPort;
import 'package:rw_utils/utils.dart';

main_() {
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
      final pool = TestPool();
      await pool.run();
      return Future.value();
    });
  });
}

main() async {
  final pool = TestPool();
  await pool.run();
  return Future.value();
}

class TestPool extends ThreadPool {
  TestPool() : super((p) => [TestThreadProxy(p), TestThreadProxy(p), TestThreadProxy(p)]);
  //TestPool() : super((p) => [TestThreadProxy(p)]);
  Future<bool> onMsg(Msg msg, ThreadProxy proxy) async {
    proxy.finish();
    return super.onMsg(msg, proxy);
  }

  static Msg decodeMessage(List list) {
    switch (list[0]) {
      default:
        return ThreadPool.decodeMessage(list);
    }
  }
}

class TestWorker extends Worker {
  TestWorker(DecodeMessage decodeMessage, List list) : super(decodeMessage, list);
  Future<bool> onMsg(Msg msg) {
    return futureFalse;
  }
}

class TestThreadProxy extends ThreadProxy {
  TestThreadProxy(ThreadPool pool) : super(pool);
  EntryPoint get entryPoint => _workerEntryPoint;
  static _workerEntryPoint(List list) async =>
      TestWorker(TestPool.decodeMessage, list).doRun();
}

_delayThread(par) {
  Future.delayed(Duration(seconds: 1));
}

_errorThread(par) {
  Future.delayed(Duration(seconds: 1));
  throw Exception('ERROR');
}
