@test.Timeout(const Duration(hours: 1))

import 'package:test/test.dart' as test;
import 'dart:isolate' show Isolate, ReceivePort, SendPort;
import 'package:rw_utils/utils.dart';
import 'package:rw_low/code.dart';

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
      await TThread.START();
      return Future.value();
    });
  });
}

class TThread extends Thread {
  //*****************************************
  //  MAIN CODE
  //*****************************************

  // main proc on client side
  static Future START() async {
    final GetThreads createThreads =
        (ThreadPool p) => List.generate(5, (idx) => TThread.px(p));
    return ThreadPool(createThreads, TThread.pxOnStreamMsg).run();
  }

  // message decoder
  static Msg msgDecoder(List list) {
    switch (list[0]) {
      default:
        return ThreadPool.decodeMessage(list);
    }
  }

  // message dispatcher on client side
  static final PXOnMsg pxOnStreamMsg = (pool, msg, proxy) {
    proxy.pxFinish();
    return pool.pxOnMsg(msg, proxy);
  };

  // message dispatcher on worker side
  @override
  Future wkOnStream(Stream<Msg> stream) async {
    //await Future.delayed(Duration(seconds: 1));
    //return Future.value(); // don't start queue => addOnExitListener is in action
    await for (final msg in stream) {
      //await Future.delayed(Duration(seconds: 1));
      if (msg is FinishWorker) break;
    }
  }

  //*****************************************
  //  must-be code
  //*****************************************
  TThread.px(ThreadPool pool) : super.px(pool);
  TThread.wk(MsgDecode msgDecoder, List list) : super.wk(msgDecoder, list);

  @override
  EntryPoint get pxWkCode => wkCode;
  static void wkCode(List l) => TThread.wk(msgDecoder, l).wkRun();
}

main_() async {
  await TThread.START();
  print('****** SUCCESS **********');
  return Future.value();
}
