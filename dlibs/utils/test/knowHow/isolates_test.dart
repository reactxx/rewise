@test.Timeout(const Duration(hours: 1))

import 'package:test/test.dart' as test;
import 'dart:isolate' show Isolate, ReceivePort;
import 'package:rw_utils/threading.dart';

main() {
  test.group('isolate', () {
    test.test('addOnExitListener', () async {
      _delayThread(par) {
        Future.delayed(Duration(seconds: 1));
      }

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
      _errorThread(par) {
        Future.delayed(Duration(seconds: 1));
        throw Exception('ERROR');
      }

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
      final errs = await TThread.START();
      test.expect(errs, test.equals(null));
    });
  });
}

class TThread extends Thread {
  //*****************************************
  //  MAIN CODE
  //*****************************************

  // main proc on client side
  static Future<List<ErrorMsg>> START() async {
    final CreateProxies createThreads = (ThreadPool p) => List.generate(
        10, (idx) => TThread.proxy(p, InitPar.encode(['en-GB', 'cs-CZ'])));
    return ThreadPool(createThreads, TThread.mainStreamMsg).run(_decode);
  }

  // message decoder
  static MsgLow _decode(List list) {
    switch (list[0]) {
      case InitPar.id:
        return InitPar.decode(list);
      case TestMsg.id:
        return TestMsg.decode(list);
      default:
        return decodeMessage(list);
    }
  }

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
  Future workerStream(Stream<Msg> stream) async {
    var testMsgCount = 3;
    final par = InitPar.decode(initPar);
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
  TThread.proxy(ThreadPool pool, List list) : super.proxy(pool, _decode, list);
  TThread.worker(List list) : super.worker(list, _decode);

  @override
  EntryPoint get entryPoint => wkCode;
  static void wkCode(List l) => TThread.worker(l).workerRun();
}

class InitPar extends MsgLow {
  static const id = 'th.test.InitPar';
  List<String> langs;
  static List encode(List<String> langs) => [id].followedBy(langs).toList();
  InitPar.decode(List list) : super.decode(list) {
    langs = list.skip(1).cast<String>().toList();
  }
}

class TestMsg extends Msg {
  static const id = 'th.test.TestMsg';
  static List encode() => [id];
  TestMsg.decode(List list) : super.decode(list);
}

main_() async {
  final err = await TThread.START();
  print(err==null ? '****** SUCCESS **********' : '****** ERROR **********');
  return Future.value();
}
