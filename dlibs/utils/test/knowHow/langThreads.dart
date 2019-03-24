// import 'dart:typed_data';
// import 'package:rw_low/code.dart' show Linq;
// import 'package:rw_utils/langs.dart' show Langs;
// import 'dart:io';
// import 'dart:isolate' show Isolate, ReceivePort, SendPort;
// import 'package:pool/pool.dart';


// class Msg {
//   Msg(this.port, [this.data]);
//   final SendPort port;
//   final data;
// }
// class Task {
// }

// class Pools {
//   Pools(Iterable<Task> tasks) {

//   }
//   final receivePort = ReceivePort();
//   final langs =
//       Langs.meta.where((m) => m.hasStemming).map((m) => m.id).toList();
//   final langThreads = Map<String, Isolate>();

//   Future run() async {
//     for (final l in langs)
//       langThreads[l] =
//           await Isolate.spawn(_workingThread, Msg(receivePort.sendPort, l));
//     await runFilerServer();
//     doPrint('fileServer DONE');
//     Future.value();
//   }

//   static _workingThread(Msg par) async {
//     ReceivePort receivePort = ReceivePort();
//     final sendPort = par.port;
//     final data = par.data;
//     sendPort.send(Msg(receivePort.sendPort, T_GetWork()));
//     await for (final msg in receivePort) {
//       if (msg is M_WorkFinished) {
//         break;
//       } else if (msg is M_StemmBook) {
//         await delay();
//         sendPort.send([receivePort.sendPort, T_GetWork(lang)]);
//       } else
//         assert(false);
//     }
//     receivePort.close();
//     doPrint('DONE $lang');
//     return Future.value();
//   }

//   //-----------------------------------
//   Future runFilerServer() async {

//     final threads = Map<String, SendPort>();

//     await for (final msg in receivePort) {
//       final sendPort = msg[0] as SendPort;
//       final lang = msg[1] as String;
//       threads.putIfAbsent(lang, () => sendPort);
//       final par = msg[1];
//       if (par is T_GetWork) {
//         doPrint('M get GetWork');
//         await delay();
//         if (taskIdx < tasks.length) {
//           doPrint('M StemmBook ${tasks[taskIdx].lang}');
//           sendPort.send(tasks[taskIdx++]);
//         } else {
//           doPrint('M WorkFinished');
//           sendPort.send(M_WorkFinished());
//           if (threads.indexOf(sendPort) >= 0) threads.remove(sendPort);
//           if (threads.length == 0) break;
//         }
//       } else if (par is T_WriteStemms) {
//         doPrint('M get WriteStemms ${par.lang}');
//         await poolWrite(par.lang, 'p');
//         doPrint('M WriteStemmsDone ${par.lang}');
//         sendPort.send(M_WriteStemmsDone(par.lang));
//       } else
//         assert(false);
//     }
//     receivePort.close();
//     doPrint('M DONE');
//     return Future.value();
//   }
// }

// class T_GetWork {
//   T_GetWork(this.lang);
//   String lang;
// }

// class M_WorkFinished {}

// class T_WriteStemms {
//   T_WriteStemms(this.lang, this.data);
//   final String lang;
//   final Uint8List data;
// }

// class M_WriteStemmsDone {
//   M_WriteStemmsDone(this.lang);
//   final String lang;
// }

// class M_StemmBook {
//   M_StemmBook(this.lang, this.words);
//   final String lang;
//   final List<String> words;
// }

// main() async {
//   await Future.wait(Linq.range(0, 10).map((i) => Pools().run([0, 1, 2, 3])));
//   files.values.forEach((f) => f.closeSync());
// }

// doPrint(String msg) => {}; //print('$msg at ${DateTime.now()}');

// delay() => Future.value(); //  Future.delayed(Duration(seconds: 1));
