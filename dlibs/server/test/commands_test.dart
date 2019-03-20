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
      var res = await Future.wait(Linq.range(0,100).map((idx) => toParsed()));
      expectLater(res!=null, equals(true), reason: '');
    }, skip: false);
    
  }, skip: true);
}
