import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/env.dart' as env;
import 'package:rewise_low_utils/toBinary.dart' as binary;

main() {
  test.setUp(() => env.DEV__ = false);
  test.tearDown(() => env.DEV__ = false);

  test.group("bit and numbers", () {
    test.test('8_16', () {
      // *** ENCODE
      final wr = binary.BitWriter();
      binary.encode_8_16(255, wr); // 9bits
      wr.writeBitsList([0x55], 7); // 7bits (_bufLen=1)
      binary.encode_8_16(256, wr); // 17bits (_bufLen=0)

      // *** PREPARE TO ENCODE
      final rdr = binary.BitReader(wr.toBytes());
      var dump = binary.dumpIterableBoolBits(rdr.readAllBits());
      // '1111 1111 1*010 1010* 0000 0000 0000 0000 1*000 0000'
      test.expect(dump,
          test.equals('1111 1111 1010 1010 0000 0000 1000 0000 0000 0000'));
      rdr.reader.setPos(0);

      // *** DECODE
      var n1 = binary.decode_8_16(rdr);
      test.expect(n1, test.equals(255));

      dump = binary.dumpIterableBoolBits(rdr.readBits(7));
      test.expect(dump, test.equals('0101 010'));

      var n2 = binary.decode_8_16(rdr);
      test.expect(n2, test.equals(256));
    });

    test.test('4_8_16', () {
      // *** ENCODE
      final wr = binary.BitWriter();
      binary.encode_4_8_16(10, wr);
      binary.encode_4_8_16(176, wr);
      wr.writeBitsList([0x55], 6);
      binary.encode_4_8_16(28456, wr);

      // *** PREPARE TO ENCODE
      final rdr = binary.BitReader(wr.toBytes());
      var dump = binary.dumpIterableBoolBits(rdr.readAllBits());
      // '1111 1111 1*010 1010* 0000 0000 0000 0000 1*000 0000'
      test.expect(dump,
          test.equals('1101 0011 0110 0000 1010 1000 1101 1110 0101 0000'));
      rdr.reader.setPos(0);

      // *** DECODE
      var n1 = binary.decode_4_8_16(rdr);
      test.expect(n1, test.equals(10));

      var n2 = binary.decode_4_8_16(rdr);
      test.expect(n2, test.equals(176));

      rdr.readBits(6);
      var n3 = binary.decode_4_8_16(rdr);
      test.expect(n3, test.equals(28456));
    });

    test.test('4_8_16_24_32', () {
      // *** ENCODE
      final wr = binary.BitWriter();
      binary.encode_4_8_16_24_32(14, wr);
      binary.encode_4_8_16_24_32(214, wr);
      binary.encode_4_8_16_24_32(0xe412, wr);
      binary.encode_4_8_16_24_32(0xef1234, wr);
      binary.encode_4_8_16_24_32(0xffffffff, wr);

      // *** PREPARE TO ENCODE
      final rdr = binary.BitReader(wr.toBytes());
      var dump = binary.dumpIterableBoolBits(rdr.readAllBits());
      test.expect(dump,
          test.equals('1111 0011 1010 1100 0111 1001 0000 0100 1000 0111 1011 1100 0100 1000 1101 0000 0011 1111 1111 1111 1111 1111 1111 1111 1100 0000'));
      rdr.reader.setPos(0);

      // *** DECODE
      var n0 = binary.decode_4_8_16_24_32(rdr);
      test.expect(n0, test.equals(14));

      var n1 = binary.decode_4_8_16_24_32(rdr);
      test.expect(n1, test.equals(214));

      var n2 = binary.decode_4_8_16_24_32(rdr);
      test.expect(n2, test.equals(0xe412));

      var n3 = binary.decode_4_8_16_24_32(rdr);
      test.expect(n3, test.equals(0xef1234));

      var n4 = binary.decode_4_8_16_24_32(rdr);
      test.expect(n4, test.equals(0xffffffff));
    });

  });
}
