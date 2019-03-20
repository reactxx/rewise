// import 'dart:typed_data';
// import 'dart:convert' as convert;
// import 'package:protobuf/protobuf.dart' as proto;
@Timeout(const Duration(hours: 1))

import 'dart:isolate';
import 'package:test/test.dart';
import 'package:isolate/isolate.dart' show LoadBalancer, IsolateRunner;
import 'package:server_dart/commands.dart';
import 'package:rw_low/code.dart' show Linq;

main() {
  group("commands", () {
    test('toRaw', () async {
      var resp = await toRaw();
      expectLater(resp.isEmpty, equals(true), reason: resp);
    }, skip: true);

    test('toParsed', () async {
      print(DateTime.now().toString());
      var res = await toParsed();
      print(DateTime.now().toString());
      expectLater(res != null, equals(true), reason: '');
    }, skip: true);

    test('toParsed', () async {
      print('*' + DateTime.now().toString());
      await parallelToParse(100, 4);
      //await Future.wait(Linq.range(0, 100).map((idx) => runToParsedAsync(idx)));
      print('*' + DateTime.now().toString());
    }, skip: false);
  }, skip: true);
}

Future<List<int>> parallelToParse(int limit, int parallelity) {
  return LoadBalancer.create(parallelity, IsolateRunner.spawn)
      .then((LoadBalancer pool) {
    var tasks =
        Linq.range(0, limit).map((idx) => pool.run((arg) => run(arg), idx)).toList();
    return Future.wait(tasks).whenComplete(pool.close);
  });
}

Future<int> run(int idx) async {
  await toParsed();
  print('- $idx at ${DateTime.now()}');
  return Future.value(idx);
}

Future runToParsedAsync(int idx) async {
  ReceivePort receivePort =
      ReceivePort(); //port for this main isolate to receive messages.
  await Isolate.spawn(runToParsed, receivePort.sendPort);
  receivePort.listen((data) => print('- $data'));
}

void runToParsed(SendPort sendPort) async {
  await toParsed();
  sendPort.send(DateTime.now().toString());
}

