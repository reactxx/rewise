import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/env.dart' as env;
import 'package:rewise_low_utils/toBinary.dart' as binary;

main() {
  test.setUp(() => env.DEV__ = false);
  test.tearDown(() => env.DEV__ = false);

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
      var dump = binary.BitReader.dump(rdr.readAllBits());
      test.expect(dump, test.equals('0000 0001 0000 0010 0101 0100'));

      rdr.reader.setPos(0);
      rdr.readBits(5);
      dump = binary.BitReader.dump(rdr.readBits(3));
      test.expect(dump, test.equals('001'));
    });
    test.test('reader.readByte', () {
      final wr = binary.BitWriter();
      wr.writeBitsList([0x55, 0x55, 0x55], 24);
      final rdr = binary.BitReader(wr.toBytes());
      var dump = binary.BitReader.dump(rdr.readAllBits());
      test.expect(dump, test.equals('0101 0101 0101 0101 0101 0101'));
      rdr.reader.setPos(0);

      var n = rdr.readByte(); // 01010101
      test.expect(n, test.equals(85));

      rdr.readBits(3);
      n = rdr.readByte(); // 10101010
      test.expect(n, test.equals(170));
    });

    test.test('reader.IntChunk', () {
      final base = 0xffffffff;
      
      var list = binary.IntChunk.fromInt(base, 0).toList();
      var n = binary.IntChunk.fromChunks(list);
      test.expect(n, test.equals(0));

      list = binary.IntChunk.fromInt(base, 8).toList();
      n = binary.IntChunk.fromChunks(list);
      test.expect(n, test.equals(0xff));

      list = binary.IntChunk.fromInt(base, 9).toList();
      n = binary.IntChunk.fromChunks(list);
      test.expect(n, test.equals(0x1ff));

      list = binary.IntChunk.fromInt(base, 32).toList();
      n = binary.IntChunk.fromChunks(list);
      test.expect(n, test.equals(base));

      list = binary.IntChunk.fromInt(5, 3).toList();
      n = binary.IntChunk.fromChunks(list);
      test.expect(n, test.equals(5));

      list = binary.IntChunk.fromInt(256, 9).toList();
      n = binary.IntChunk.fromChunks(list);
      test.expect(n, test.equals(256));

      list = binary.IntChunk.fromInt(28361927, 29).toList();
      n = binary.IntChunk.fromChunks(list);
      test.expect(n, test.equals(28361927));

    });
  });
}
