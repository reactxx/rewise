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
      return Future.value();
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
        1, (idx) => TThread.proxy(p, InitPar.encode(['en-GB', 'cs-CZ'])));
    return ThreadPool(createThreads, TThread.mainStreamMsg).run();
  }

  // message decoder
  static MsgLow msgDecoder(List list) {
    switch (list[0]) {
      case InitPar.id:
        return InitPar.decode(list);
      default:
        return decodeMessage(list);
    }
  }

  // message dispatcher on client side
  static final MainStreamMsg mainStreamMsg = (pool, msg, proxy) {
    //proxy.pxFinish();
    return pool.mainStreamMsg(msg, proxy);
  };

  // message dispatcher on worker side
  @override
  Future workerStream(Stream<Msg> stream) async {
    print(initPar);
    final par = InitPar.decode(initPar);
    //await Future.delayed(Duration(seconds: 1));
    return Future
        .value(); // don't start queue => addOnExitListener is in action
    await for (final msg in stream) {
      //await Future.delayed(Duration(seconds: 1));
      if (msg is FinishWorker) break;
    }
  }

  //*****************************************
  //  must-be code
  //*****************************************
  TThread.proxy(ThreadPool pool, List list) : super.proxy(pool, msgDecoder, list);
  TThread.worker(List list) : super.worker(list, msgDecoder);

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

main_() async {
  await TThread.START();
  print('****** SUCCESS **********');
  return Future.value();
}
