// @test.Timeout(const Duration(hours: 1))

// import 'package:test/test.dart' as test;
// import 'dart:isolate' show Isolate, ReceivePort;
// import 'package:rw_utils/threading.dart';

// main() {
//   test.group('isolate', () {
//     test.test('addOnExitListener', () async {
//       final receivePort = ReceivePort();
//       var ds = _delayThread;
//       var iso = await Isolate.spawn(ds, receivePort.sendPort);
//       iso.addOnExitListener(receivePort.sendPort, response: 'done');
//       await for (var msg in receivePort) {
//         test.expect(msg, test.equals('done'));
//         break;
//       }
//       return Future.value();
//     });

//     test.test('addErrorListener', () async {
//       final receivePort = ReceivePort();
//       var iso = await Isolate.spawn(_errorThread, receivePort.sendPort);
//       iso.setErrorsFatal(false);
//       iso.addErrorListener(receivePort.sendPort);
//       await for (var msg in receivePort) {
//         test.expect(msg[0], test.equals('Exception: ERROR'));
//         break;
//       }
//       return Future.value();
//     });

//     // test.test('threading', () async {
//     //   final res = await TThread.START(10);
//     //   test.expect(res.length, test.equals(0));
//     // });

//     test.test('parallel', () async {
//       final tasks = 10;
//       final res = await parallelSTART(tasks, 5);
//       test.expect(res.length, test.equals(0));
//     });
//   });
// }

// _delayThread(par) => Future.delayed(Duration(seconds: 1));

// _errorThread(par) {
//   Future.delayed(Duration(seconds: 1));
//   throw Exception('ERROR');
// }

// Future<List> parallelSTART(int taskNum, num parallels) {
//   final tasks = List.generate(taskNum, (idx) => TestMsg.encode());
//   return Parallel(tasks, parallels, _parallelTestEntryPoint).run();
// }

// void _parallelTestEntryPoint(List workerInitMsg) =>
//     parallelEntryPoint<TestMsg>(workerInitMsg, (msg) async {
//       await Future.delayed(Duration(milliseconds: 100));
//       return Parallel.workerReturnFuture;
//     }, initThreadingTest);

// const _namespace = 'th.test.';

// void initThreadingTest() {
//   if (_called) return;
//   initMessages();
//   messageDecoders.addAll(<String, DecodeProc>{
//     InitPar.id: (list) => InitPar.fromIter(list),
//     TestMsg.id: (list) => TestMsg.decode(list),
//   });
//   _called = true;
// }

// bool _called = false;

// class InitPar extends Msg {
//   static const id = _namespace + 'InitPar';
//   List<String> langs;
//   static List encode(List<String> langs) => [id].followedBy(langs).toList();
//   InitPar.fromIter(Iterator list) : super.fromIter(list) {
//     langs = list.skip(1).cast<String>().toList();
//   }
// }

// class TestMsg extends ContinueMsg {
//   static const id = _namespace + 'TestMsg';
//   static List encode() => [id];
//   TestMsg.decode(List list) : super.decode(list);
// }

// main_() async {
//   await parallelSTART(10, 3);
//   return Future.value();
// }
