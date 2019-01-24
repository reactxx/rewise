//import 'dart:convert';
//import 'dart:typed_data';

import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/index.dart';

main() {
  BytesWriter wr;
  test.setUp(() {
    wr = BytesWriter();
  });

  test.group("trie writer", () {
    test.test('addNumber', () {
      var writeNum = (int n) {
        wr.addNumber(n, getNumberSizeMask(n));
      };

      String str;
      writeNum(0);
      str = wr.hexDump();
      test.expect(str, test.equals(''));

      writeNum(1);
      str = wr.hexDump();
      test.expect(str, test.equals('01'));

      writeNum(0x100);
      str = wr.hexDump();
      test.expect(str, test.equals('010001'));

      writeNum(0x10000);
      str = wr.hexDump();
      test.expect(str, test.equals('010001000001'));

      writeNum(0xffffff);
      str = wr.hexDump();
      test.expect(str, test.equals('010001000001ffffff'));
    });

    test.test('addList', () {
      wr.addList([1, 2, 4, 8, 16]);
      var str = wr.hexDump();
      test.expect(str, test.equals('0102040810'));
    });

    test.test('addWriter', () {
      BytesWriter subwr = BytesWriter();
      subwr.addList([1,2]);
      wr.addWriter(subwr);
      wr.addList([4, 8, 16]);
      var str = wr.hexDump();
      test.expect(str, test.equals('0102040810'));
    });
  });
}
