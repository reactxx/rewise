import 'dart:typed_data';
import 'package:test/test.dart' as test;
import 'dart:io';
import 'dart:isolate' show Isolate, ReceivePort, SendPort;
import 'package:pool/pool.dart';

class Pools {
  final pools = <String, Pool>{'cs-CZ': Pool(1), 'en-GB': Pool(1)};
  final receivePort = ReceivePort();

  Future run(Iterable<int> isolatePars) async {
    final fileServer = runFilerServer();
    await Future.wait(isolatePars.map(
        (i) => Isolate.spawn(_workingThread, [receivePort.sendPort, 'T$i'])));
    receivePort.close();
    await fileServer;
    Future.value();
  }

  static _workingThread(par) async {
    final sendPort = par[0] as SendPort;
    final receivePort = ReceivePort();
    sendPort.send(receivePort.sendPort);

    await for (final msg in receivePort) {
    if (msg < 0) {
      receivePort.close();
    } else {
      await _runToParsed(msg);
      sendPort.send(receivePort.sendPort);
    }
  }


    for (var i = 0; i < 100; i++)
      (par[0] as SendPort).send([
        i % 2 == 1 ? 'cs-CZ' : 'en-GB',
        '${par[1]}:$i',
        Uint8List.fromList([0, 1, 2, 3, 4])
      ]);
  }

  //-----------------------------------
  Future runFilerServer() async {
    Future poolWrite(String lang, String text, par) async {
      final resource = await pools[lang].request();
      try {
        final file = File('c:\\temp\\$lang.txt')
            .openSync(mode: FileMode.writeOnlyAppend);
        try {
          file.writeStringSync('$text${par.length}, ');
        } finally {
          file.closeSync();
        }
      } finally {
        resource.release();
      }
      Future.value();
    }

    await for (final msg in receivePort) {
      if (msg is SendPort) {
        if (tasksCount > 0)
          msg.send(tasksCount--);
        else {
          msg.send(-1);
          if (--closedCount <= 0) receivePort.close();
        }
      }
      poolWrite(msg[0], msg[1], msg[2]);
    }

    return Future.value();
  }
}

main() {
  test.test('mutex', () async {
    await Pools().run([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
    test.expect(true, test.equals(true));
  });
}
