import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/env.dart' as env;
import 'package:rewise_low_utils/binary.dart' as binary;
import 'dart:typed_data';

main() {
  test.setUp(() => env.DEV__ = false);
  test.tearDown(() => env.DEV__ = false);

  test.group("binary writer", () {
    test.test('addNumber', () {

      final wr = binary.ByteWriter();

      var writeNum = (int n) {
        wr.writeNumber(n, binary.ByteWriter.getNumberSizeMask(n));
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
      final wr = binary.ByteWriter();
      wr.writeList([1, 2, 4, 8, 16]);
      var str = wr.hexDump();
      test.expect(str, test.equals('0102040810'));
    });

    test.test('addWriter', () {
      final wr = binary.ByteWriter();
      final subwr = binary.ByteWriter();
      subwr.writeList([1, 2]);
      wr.writeWriter(subwr);
      wr.writeList([4, 8, 16]);
      var str = wr.hexDump();
      test.expect(str, test.equals('0102040810'));
    });

    test.test('add all', () {
      final wr = binary.ByteWriter();
      wr.writeByte(0xff);
      wr.writeList([0x1,0x2,0x3,0x4]);
      final wr2 = binary.ByteWriter();
      wr2.writeBytes(Uint8List.fromList([0xfe]));
      wr.writeWriter(wr2);
      var str = wr.hexDump();
      test.expect(str, test.equals('ff01020304fe'));
    });

  });
}
