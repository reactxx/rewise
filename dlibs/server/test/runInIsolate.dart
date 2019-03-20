@Timeout(const Duration(hours: 1))

import 'dart:isolate' show Isolate, ReceivePort, SendPort;
import 'package:test/test.dart';
import 'package:server_dart/commands.dart';
//import 'package:rw_low/code.dart' show Linq;
//import 'package:async/async.dart';
//import 'dart:async/stream_controller.dart' show StreamController; // implements StreamSink interface
//import 'package:isolate/isolate.dart' show LoadBalancer, IsolateRunner;
// import 'package:stream_channel/stream_channel.dart'
//     show IsolateChannel, StreamChannel;

main() async {
  print('*' + DateTime.now().toString());
  //await parallelToParse(10, 4);
  //await Future.wait(Linq.range(0, 100).map((idx) => runToParsedAsync(idx)));
  await _runParallel(2, 4);
  print('*' + DateTime.now().toString());
}

//https://stackoverflow.com/questions/11363676/will-dart-execute-isolates-in-parallel-in-a-multi-core-environment/11364519#11364519
//https://news.dartlang.org/2016/04/unboxing-packages-streamchannel.html
//https://www.dartlang.org/tutorials/language/streams
//https://stackoverflow.com/questions/52259889/bidirectional-communication-with-isolates-in-dart-2
//https://github.com/dart-lang/stream_channel/tree/master/test
//https://itnext.io/how-to-use-streams-in-dart-part-1-4503fec0cdd7
Future _runParallel(int workingThreadCount, int tasksCount) async {
  ReceivePort receivePort = ReceivePort();

  try {
    for (var i = 0; i < workingThreadCount; i++)
      Isolate.spawn(_runInIsolate, receivePort.sendPort);
    var closedCount = workingThreadCount;

    await for (final msg in receivePort) {
      if (msg is SendPort) {
        if (tasksCount > 0)
          msg.send(tasksCount--);
        else {
          msg.send(-1);
          if (--closedCount <= 0) receivePort.close();
        }
      }
    }
  } finally {
    receivePort.close();
  }
  return Future.value();
}

_runInIsolate(SendPort sendPort) async {
  ReceivePort receivePort = ReceivePort();
  sendPort.send(receivePort.sendPort);

  await for (final msg in receivePort) {
    if (msg < 0) {
      receivePort.close();
    } else {
      await _runToParsed(msg);
      sendPort.send(receivePort.sendPort);
    }
  }
}

Future<int> _runToParsed(int idx) async {
  print('- START $idx at ${DateTime.now()}');
  await toParsed();
  print('- END $idx at ${DateTime.now()}');
  return Future.value(idx);
}

// Future<List<int>> parallelToParse(int limit, int parallelity) {
//   return LoadBalancer.create(parallelity, IsolateRunner.spawn)
//       .then((LoadBalancer pool) {
//     var tasks = Linq.range(0, limit).map((idx) => pool.run(run, idx)).toList();
//     return Future.wait(tasks).whenComplete(pool.close);
//   });
// }

// Future runToParsedAsync(int idx) async {
//   ReceivePort receivePort =
//       ReceivePort(); //port for this main isolate to receive messages.
//   await Isolate.spawn(runToParsed, receivePort.sendPort);
//   receivePort.listen((data) => print('- $data'));
// }

// void runToParsed(SendPort sendPort) async {
//   await Future.delayed(Duration(seconds: 2));
//   await toParsed();
//   sendPort.send(DateTime.now().toString());
// }
