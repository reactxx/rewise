import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/toBinary.dart' as binary;

main() {

  test.group("bit reader and writer", () {
    test.test('writer.writeBits', () {
      var wr = binary.BitWriter();
      wr.writeBitsList([0x01, 0x02, 0x55], 23);
      wr.writeBitsList([0xaa], 2);
      wr.writeBitsList([0xaa], 7);
      wr.writeBitsList([0x55], 2);
      wr.writeBitsList([0x55], 7);
      wr.writeBitsList([0xaa], 7);
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
      wr.writeBitsList([0x01, 0x02, 0x55], 23);
      final rdr = binary.BitReader(wr.toBytes());
      var dump = binary.dumpIterableBoolBits(rdr.readAllBits());
      test.expect(dump, test.equals('0000 0001 0000 0010 0101 0100'));

      rdr.reader.setPos(0);
      rdr.readBits(5);
      dump = binary.dumpIterableBoolBits(rdr.readBits(3));
      test.expect(dump, test.equals('001'));
    });
    test.test('reader.readByte', () {
      final wr = binary.BitWriter();
      wr.writeBitsList([0x55, 0x55, 0x55], 24);
      final rdr = binary.BitReader(wr.toBytes());
      var dump = binary.dumpIterableBoolBits(rdr.readAllBits());
      test.expect(dump, test.equals('0101 0101 0101 0101 0101 0101'));
      rdr.reader.setPos(0);

      var n = rdr.readInt(8); // 01010101
      test.expect(n, test.equals(85));

      rdr.readBits(3);
      n = rdr.readInt(8); // 10101010
      test.expect(n, test.equals(170));
    });

    test.test('reader.IntChunk', () {
      final base = 0xffffffff;

      var n = binary.IntBytes.fromChunks(binary.IntBytes.fromInt(base, 0));
      test.expect(n, test.equals(0));

      n = binary.IntBytes.fromChunks(binary.IntBytes.fromInt(base, 8));
      test.expect(n, test.equals(0xff));

      n = binary.IntBytes.fromChunks(binary.IntBytes.fromInt(base, 9));
      test.expect(n, test.equals(0x1ff));

      n = binary.IntBytes.fromChunks(binary.IntBytes.fromInt(base, 32));
      test.expect(n, test.equals(base));

      n = binary.IntBytes.fromChunks(binary.IntBytes.fromInt(5, 3));
      test.expect(n, test.equals(5));

      n = binary.IntBytes.fromChunks(binary.IntBytes.fromInt(256, 9));
      test.expect(n, test.equals(256));

      n = binary.IntBytes.fromChunks(binary.IntBytes.fromInt(28361927, 29));
      test.expect(n, test.equals(28361927));
    });
    test.test('read and write INT', () {
      var wr = binary.BitWriter();
      final maxint = 0xffffffff;
      wr.writeInt(maxint, 0, false);
      wr.writeInt(maxint, 1, false);
      wr.writeInt(maxint, 8, false);
      wr.writeInt(maxint, 9, false);
      wr.writeInt(maxint, 32, false);
      wr.writeInt(maxint, 6, false);
      var rdr = binary.BitReader(wr.toBytes());
      var n = rdr.readInt(0);
      test.expect(n, test.equals(0));
      n = rdr.readInt(1);
      test.expect(n, test.equals(binary.maxIntBits(1)));
      n = rdr.readInt(8);
      test.expect(n, test.equals(binary.maxIntBits(8)));
      n = rdr.readInt(9);
      test.expect(n, test.equals(binary.maxIntBits(9)));
      n = rdr.readInt(32);
      test.expect(n, test.equals(binary.maxIntBits(32)));
      n = rdr.readInt(6);
      test.expect(n, test.equals(binary.maxIntBits(6)));
    });

    test.test('read and write INT 2', () {
      var wr = binary.BitWriter();
      wr.writeInt(0, 0);
      wr.writeInt(1, 1);
      wr.writeInt(63485,16);
      wr.writeInt(0x55555555, 31);
      wr.writeInt(15, 4);
      wr.writeInt(255, 8);
      var rdr = binary.BitReader(wr.toBytes());
      var n = rdr.readInt(0);
      test.expect(n, test.equals(0));
      n = rdr.readInt(1);
      test.expect(n, test.equals(1));
      n = rdr.readInt(16);
      test.expect(n, test.equals(63485));
      n = rdr.readInt(31);
      test.expect(n, test.equals(0x55555555));
      n = rdr.readInt(4);
      test.expect(n, test.equals(15));
      n = rdr.readInt(8);
      test.expect(n, test.equals(255));
    });

    test.test('read and write INT 3', () {
      var wr = binary.BitWriter();
      wr.writeInt(3, 2);
      wr.writeInt(0x531, 11); // 10100110001
      var rdr = binary.BitReader(wr.toBytes());
      var dump = binary.dumpIterableBoolBits(rdr.readAllBits());
      // '10 1001 1000 1'= '10100110001'
      test.expect(dump, test.equals('1110 1001 1000 1000'));
      rdr.reader.setPos(0);
      var n = rdr.readInt(2);
      test.expect(n, test.equals(3));
      n = rdr.readInt(11);
      // 6-58-111010 (ma byt 101001), 5-17-10001
      test.expect(n, test.equals(0x531));
    });


  });
}
