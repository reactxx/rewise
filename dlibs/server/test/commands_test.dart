// import 'dart:typed_data';
// import 'dart:convert' as convert;
// import 'package:protobuf/protobuf.dart' as proto;
@Timeout(const Duration(hours: 1))

import 'package:test/test.dart';
import 'package:server_dart/commands.dart';
import 'package:server_dart/utils.dart';

main() {
  group("TESTS", () {

    test('REFRESH MESSAGE DART', () {
      // **** 1. step:
      //refreshMessagesCmd();
      // **** 2. step by hand: copy C:\rewise\protobuf\compiler\include\rewise\fragment.cmd to gen.cmd
      // **** 3. step by hand: run C:\rewise\protobuf\compiler\include\rewise\gen.cmd
      // **** 4. step
      refreshServicesCSharp();
      refreshMessagesDart();
      expect(0, equals(0));
    }, skip: false);

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
