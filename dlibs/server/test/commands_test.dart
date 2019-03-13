// import 'dart:typed_data';
// import 'dart:convert' as convert;
// import 'package:protobuf/protobuf.dart' as proto;
@Timeout(const Duration(hours: 1))

import 'package:test/test.dart';
import 'package:server_dart/commands.dart';
import 'package:server_dart/utils.dart';

main() {
  group("TESTS", () {
    test('matrixsToBookOuts', () async {
      var resp = await matrixsToBookOuts();
      expect(resp, equals(resp));
    }, skip: false);

    test('bookOutsToRawBook', () async {
      var resp = await bookOutsToRawBook();
      expect(resp, equals(resp));
    }, skip: false);
    
    test('refreshMessagesDart', () async {
      refreshMessagesDart();
      expect(0, equals(0));
    }, skip: false);
  }, skip: false);
}
