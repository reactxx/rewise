import 'package:test/test.dart' as test;
import 'dart:typed_data';
import 'package:rw_utils/toBinary.dart' as toBinary;

main() {

  test.group("binary writer", () {
    test.test('addNumber', () {

      final wr = toBinary.MemoryWriter();

      var writeNum = (int n) {
        toBinary.writeInt(wr, n, toBinary.getIntSize(n));
      };

      String str;
      writeNum(0);
      str = wr.dump();
      test.expect(str, test.equals(''));

      writeNum(1);
      str = wr.dump();
      test.expect(str, test.equals('01'));

      writeNum(0x100);
      str = wr.dump();
      test.expect(str, test.equals('010001'));

      writeNum(0x10000);
      str = wr.dump();
      test.expect(str, test.equals('010001000001'));

      writeNum(0xffffff);
      str = wr.dump();
      test.expect(str, test.equals('010001000001ffffff'));
    });

    test.test('addList', () {
      final wr = toBinary.MemoryWriter();
      wr.writeBytes([1, 2, 4, 8, 16]);
      var str = wr.dump();
      test.expect(str, test.equals('0102040810'));
    });

    test.test('addWriter', () {
      final wr = toBinary.MemoryWriter();
      final subwr = toBinary.MemoryWriter();
      subwr.writeBytes([1, 2]);
      wr.writeWriter(subwr);
      wr.writeBytes([4, 8, 16]);
      var str = wr.dump();
      test.expect(str, test.equals('0102040810'));
    });

    test.test('add all', () {
      final wr = toBinary.MemoryWriter();
      wr.writeByte(0xff);
      wr.writeBytes([0x1,0x2,0x3,0x4]);
      final wr2 = toBinary.MemoryWriter();
      wr2.writeBytes(Uint8List.fromList([0xfe]));
      wr.writeWriter(wr2);
      var str = wr.dump();
      test.expect(str, test.equals('ff01020304fe'));
    });

  });
}
