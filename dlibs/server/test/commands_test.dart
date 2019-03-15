// import 'dart:typed_data';
// import 'dart:convert' as convert;
// import 'package:protobuf/protobuf.dart' as proto;
@Timeout(const Duration(hours: 1))

import 'package:test/test.dart';
import 'package:server_dart/commands.dart';

main() {
  group("commands", () {

    test('toRaw', () async {
      var resp = await toRaw();
      expectLater(resp.isEmpty, equals(true), reason: resp);
    }, skip: false);

    test('toParsed', () async {
      var resp = await toParsed();
      expectLater(resp.isEmpty, equals(true), reason: resp);
    }, skip: false);
    
  }, skip: true);
}
