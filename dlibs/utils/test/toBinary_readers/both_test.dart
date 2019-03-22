import 'package:test/test.dart' as test;
import 'dart:typed_data';
import 'package:rw_utils/toBinary.dart' as bin;
import 'package:rw_low/code.dart' show Dir;
import 'package:convert/convert.dart' as convert;

main() {
  test.group("writer x reader, memory x stream ", () {
    test.test('addNumber', () {
      final dir = Dir(r'\temp');
      List<bin.Writer> getWriters() => List<bin.Writer>.from([
            bin.StreamWriter.fromPath(dir.absolute('both_test.bin')),
            bin.MemoryWriter()
          ]);

      List<bin.Reader> getReaders(bin.MemoryWriter memBytes) =>
          List<bin.Reader>.from([
            bin.StreamReader.fromPath(dir.absolute('both_test.bin')),
            bin.MemoryReader(memBytes.toBytes())
          ]);

      String dump(List<int> data) =>
          data == null ? null : convert.hex.encode(Uint8List.fromList(data));

      final wrs = getWriters();
      wrs.forEach((wr) {
        try {
          wr.writeByte(77);
          wr.writeBytesLow([1, 2, 3, 4, 5]);

          wr.writeBytes(null);
          wr.writeBytes([]);
          wr.writeBytes([0xff, 0xfe]);

          wr.writeString(null);
          wr.writeString('');
          wr.writeString('Čao');

          wr.writeSizedInt(0x74f3f2f1, 0);
          wr.writeSizedInt(0x74f3f2f1, 1);
          wr.writeSizedInt(0x74f3f2f1, 2);
          wr.writeSizedInt(0x74f3f2f1, 3);
          wr.writeSizedInt(0x74f3f2f1, 4);

          wr.writeBytess(null);
          wr.writeBytess([]);
          wr.writeBytess([null]);
          wr.writeBytess([[]]);
          wr.writeBytess([
            [1]
          ]);
          wr.writeBytess([
            [1],
            [0xff]
          ]);

          wr.writeSizedInts(null, 2);
          wr.writeSizedInts([], 2);
          wr.writeSizedInts([1], 2);
        } finally {
          if (wr is bin.StreamWriter) wr.close();
        }
      });

      final rdrs = getReaders(wrs[1]);
      rdrs.forEach((rdr) {
        try {
          test.expect(rdr.readByte(), test.equals(77));
          test.expect(dump(rdr.readBytesLow(5)), test.equals('0102030405'));

          test.expect(dump(rdr.readBytes()), test.equals(null));
          test.expect(dump(rdr.readBytes()), test.equals(null));
          test.expect(dump(rdr.readBytes()), test.equals('fffe'));

          test.expect(rdr.readString(), test.equals(null));
          test.expect(rdr.readString(), test.equals(null));
          test.expect(rdr.readString(), test.equals('Čao'));

          test.expect(rdr.readSizedInt(0), test.equals(0));
          test.expect(rdr.readSizedInt(1), test.equals(0xf1));
          test.expect(rdr.readSizedInt(2), test.equals(0xf2f1));
          test.expect(rdr.readSizedInt(3), test.equals(0xf3f2f1));
          test.expect(rdr.readSizedInt(4), test.equals(0x74f3f2f1));

          test.expect(rdr.readBytess(), test.equals(null));
          test.expect(rdr.readBytess(), test.equals(null));
          test.expect(rdr.readBytess(), test.equals([null]));
          test.expect(rdr.readBytess(), test.equals([null]));
          test.expect(
              rdr.readBytess(),
              test.equals([
                [1]
              ]));
          test.expect(
              rdr.readBytess(),
              test.equals([
                [1],
                [0xff]
              ]));

          test.expect(rdr.readSizedInts(2), test.equals(null));
          test.expect(rdr.readSizedInts(2), test.equals(null));
          test.expect(rdr.readSizedInts(2), test.equals([1]));
        } finally {
          if (rdr is bin.StreamReader) rdr.close();
        }
      });
    });

  });
}
