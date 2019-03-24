import 'dart:typed_data';
import 'dart:math';
import 'package:rw_low/code.dart' show Linq;
import 'dart:io';
import 'dart:isolate' show Isolate, ReceivePort, SendPort;
import 'package:pool/pool.dart';

Map<String, Pool> pools; // = Map<String, Pool>.fromIterable(langs, value: (_) => Pool(1));

class Pools {
static const langs = ['cs-CZ', 'en-GB', 'sp-SP', 'de-DE', 'fr-FR'];
  final receivePort = ReceivePort();
  final files = Map<String, RandomAccessFile>.fromIterable(langs,
      value: (lang) =>
          File('c:\\temp\\$lang.txt').openSync(mode: FileMode.writeOnlyAppend));

  Future run(Iterable<int> isolatePars) async {
    for (final p in isolatePars)
      Isolate.spawn(_workingThread, receivePort.sendPort);
    await runFilerServer(receivePort, pools, files);
    files.values.forEach((f) => f.closeSync());
    doPrint('fileServer DONE');
    Future.value();
  }

  static _workingThread(par) async {
    ReceivePort receivePort = ReceivePort();
    final sendPort = par as SendPort;
    doPrint('T GetWork Start');
    sendPort.send(Msg(receivePort.sendPort, T_GetWork()));
    await for (final msg in receivePort) {
      if (msg is M_WorkFinished) {
        doPrint('T get WorkFinished');
        break;
      } else if (msg is M_StemmBook) {
        doPrint('T get StemmBook ${msg.lang}');
        await delay();
        doPrint('T WriteStemms ${msg.lang}');
        sendPort.send(Msg(receivePort.sendPort, T_WriteStemms(msg.lang, null)));
      } else if (msg is M_WriteStemmsDone) {
        doPrint('T get WriteStemmsDone ${msg.lang}');
        await delay();
        doPrint('T GetWork');
        sendPort.send(Msg(receivePort.sendPort, T_GetWork()));
      } else
        assert(false);
    }
    receivePort.close();
    doPrint('T DONE');
    return Future.value();
  }

  //-----------------------------------
  Future runFilerServer(ReceivePort receivePort, Map<String, Pool> pools,
      Map<String, RandomAccessFile> files) async {
    Future poolWrite(String lang, String text) async {
      //final resource = await pools[lang].request();
      try {
        files[lang].writeStringSync('$text');
        //files.values.forEach((f) => f.flushSync());
        // try {
        //   final file = File('c:\\temp\\$lang.txt')
        //       .openSync(mode: FileMode.writeOnlyAppend);
        //   try {
        // file.writeStringSync('$text');
        // } finally {
        //   file.closeSync();
        // }
      } finally {
        //resource.release();
      }
      Future.value();
    }

    final tasks = Linq.range(0, 10)
        .expand((i) => langs.map((l) => M_StemmBook(l, null)))
        .toList();
    int taskIdx = 0;

    final threads = List();
    //final objs = Set.identity();

    await for (Msg msg in receivePort) {
      final sendPort = msg.port;
      if (threads.indexOf(sendPort) < 0) threads.add(sendPort);
      //objs.add(sendPort);
      final par = msg.data;
      if (par is T_GetWork) {
        doPrint('M get GetWork');
        await delay();
        if (taskIdx < tasks.length) {
          doPrint('M StemmBook ${tasks[taskIdx].lang}');
          sendPort.send(tasks[taskIdx++]);
        } else {
          doPrint('M WorkFinished');
          sendPort.send(M_WorkFinished());
          threads.remove(sendPort);
          if (threads.length == 0) break;
        }
      } else if (par is T_WriteStemms) {
        doPrint('M get WriteStemms ${par.lang}');
        await poolWrite(par.lang, 'p');
        doPrint('M WriteStemmsDone ${par.lang}');
        sendPort.send(M_WriteStemmsDone(par.lang));
      } else
        assert(false);
    }
    receivePort.close();
    doPrint('M DONE');
    return Future.value();
  }
}

class Msg {
  Msg(this.port, this.data);
  final SendPort port;
  final data;
}

class T_GetWork {
  T_GetWork();
}

class M_WorkFinished {
  M_WorkFinished();
}

class T_WriteStemms {
  T_WriteStemms(this.lang, this.data);
  final String lang;
  final Uint8List data;
}

class M_WriteStemmsDone {
  M_WriteStemmsDone(this.lang);
  final String lang;
}

class M_StemmBook {
  M_StemmBook(this.lang, this.words);
  final String lang;
  final List<String> words;
}

main() async {
  await Pools().run(Linq.range(0, 10));
}

doPrint(String msg) => {}; //print('$msg at ${DateTime.now()}');

final random = Random();
delay() => Future.delayed(Duration(milliseconds: random.nextInt(100))); // Future.value();
