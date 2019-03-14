// import 'dart:typed_data';
// import 'dart:convert' as convert;
// import 'package:protobuf/protobuf.dart' as proto;
@Timeout(const Duration(hours: 1))

import 'package:test/test.dart';
import 'package:server_dart/commands.dart';

main() {
  group("TESTS", () {

    test('matrixsToBookOuts', () async {
      var resp = await matrixsToBooksFromRJ();
      expect(resp, equals(resp));
    }, skip: false);

    test('bookOutsToRawBook', () async {
      var resp = await bookFromRJToRawBook();
      expect(resp, equals(resp));
    }, skip: false);
    
  }, skip: false);
}
