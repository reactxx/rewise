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
      expect(resp, equals(resp));
    }, skip: false);

    test('toBookSources', () async {
      var resp = await toBookSources();
      expect(resp, equals(resp));
    }, skip: false);
    
  }, skip: true);
}
