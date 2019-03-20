// import 'dart:typed_data';
// import 'dart:convert' as convert;
// import 'package:protobuf/protobuf.dart' as proto;
@Timeout(const Duration(hours: 1))

import 'package:test/test.dart';
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
      await Future.wait(Linq.range(0, 100).map((idx) => run(idx)));
      print('*' + DateTime.now().toString());
    }, skip: false);
  }, skip: true);
}

Future run(int idx) async {
  await Future.delayed(Duration(seconds: idx * 3));
  await toParsed();
  print('- ' + DateTime.now().toString());
  Future.value(null);
}
