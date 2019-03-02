import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/env.dart' as env;
import 'package:rewise_low_utils/toBinary.dart' as binary;

main() {
  test.setUp(() => env.DEV__ = false);
  test.tearDown(() => env.DEV__ = false);

  test.group("bit reader and writer", () {
    test.test('writer.writeBits', () {
      var wr = binary.BitWriter();
      wr.writeBitslist([0x01, 0x02, 0x55], 23);
      wr.writeBitslist([0xaa], 2);
      wr.writeBitslist([0xaa], 7);
      wr.writeBitslist([0x55], 2);
      wr.writeBitslist([0x55], 7);
      wr.writeBitslist([0xaa], 7);
      var dump = wr.dump();
      test.expect(dump, test.equals('010255555555'));
      test.expect((23 + 2 + 7 + 2 + 7 + 7) / 8, test.equals(dump.length / 2));
    });

    test.test('writer.writeBools', () {
      final wr = binary.BitWriter();
      wr.writeBool(true);
      wr.writeBools([true, true, true, true, true, true, true, true]);
      final dump = wr.dump();
      test.expect(dump, test.equals('ff80'));
    });

    test.test('reader', () {
      final wr = binary.BitWriter();
      wr.writeBitslist([0x01, 0x02, 0x55], 23);
      final rdr = binary.BitReader(wr.toBytes());
      var dump = binary.BitReader.dump(rdr.readAllBits());
      test.expect(dump, test.equals('0000 0001 0000 0010 0101 0100'));

      rdr.reader.setPos(0);
      rdr.skipBits(5);
      dump = binary.BitReader.dump(rdr.readBits(3));
      test.expect(dump, test.equals('001'));
    });
    test.test('reader.readByte', () {
      final wr = binary.BitWriter();
      wr.writeBitslist([0x55, 0x55, 0x55], 24);
      final rdr = binary.BitReader(wr.toBytes());
      var dump = binary.BitReader.dump(rdr.readAllBits());
      test.expect(dump, test.equals('0101 0101 0101 0101 0101 0101'));
      rdr.reader.setPos(0);

      var n = rdr.readByte(); // 01010101
      test.expect(n, test.equals(85));

      rdr.skipBits(3);
      n = rdr.readByte(); // 10101010
      test.expect(n, test.equals(170));
    });
  });
}
