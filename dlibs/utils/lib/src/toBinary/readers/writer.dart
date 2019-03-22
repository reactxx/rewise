import 'dart:typed_data';
import 'package:protobuf/protobuf.dart' as proto;

final _endian = Endian.big;

abstract class Writer {
  // ABSTRACTS
  void writeByte(int byte, {int pos});
  void writeBytes(List<int> data, {int pos});
  Writer setPos(int pos);
  void writeToBuffer(int len, void fillData(ByteData data), {int pos});

  // OTHERS
  void writeUInt32(int value, {int pos}) =>
      writeToBuffer(4, (bd) => bd.setUint32(0, value, _endian), pos: pos);
  void writeUInt32s(List<int> data, {int pos}) =>
      writeToBuffer(data.length << 2, (bd) {
        for (final i in data) bd.setUint32(i << 2, i, _endian);
      }, pos: pos);
  void writeUInt16s(List<int> data, {int pos}) =>
      writeToBuffer(data.length << 1, (bd) {
        for (final i in data) bd.setUint16(i << 1, i, _endian);
      }, pos: pos);

  void writeDecodedString(String str, Map<int, int> fromCodeUnit,
      {bool writeLen = true, int pos}) {
    if (str == null || str.isEmpty) return;
    if (writeLen) writeByte(str.length, pos: pos);
    final list = str.codeUnits.map((c) => fromCodeUnit[c]);
    writeBytes(Uint8List.fromList(list));
  }

  void writeVLQ(int n) {
    if (n < 0) throw Exception();
    if (n <= _maxVLQ1) {
      writeByte(n | 0x80);
      return;
    }
    if (n <= _maxVLQ2) {
      writeByte(n >> 7);
      writeByte((n & 0x7f) | 0x80);
      return;
    }
    if (n <= _maxVLQ3) {
      writeByte(n >> 14);
      writeByte((n >> 7) & 0x7f);
      writeByte((n & 0x7f) | 0x80);
      return;
    }
    if (n <= _maxVLQ4) {
      writeByte(n >> 21);
      writeByte((n >> 14) & 0x7f);
      writeByte((n >> 7) & 0x7f);
      writeByte((n & 0x7f) | 0x80);
      return;
    }
    throw Exception(n.toString());
  }

  void writeMessages(List<proto.GeneratedMessage> data) {
    if (data == null || data.isEmpty == 0)
      writeVLQ(0);
    else
      writeBytesIterable(data.map((d) => d.writeToBuffer()), data.length);
  }

  void writeBytesIterable(Iterable<Uint8List> _data, int len) {
    if (_data == null || len == 0)
      writeVLQ(0);
    else {
      final data = _data.iterator;
      writeVLQ(len);
      while (data.moveNext()) {
        writeVLQ(data.current.length);
        writeBytes(data.current);
        len--;
      }
      assert(len == 0);
    }
  }

  void writeBytesStream(List<Uint8List> data) {
    writeBytesIterable(data, data == null ? 0 : data.length);
  }
}

const _maxVLQ1 = 0xFF >> 1; //0b0111_1111;
const _maxVLQ2 = 0xFFFF >> 2; //0b0011_1111_1111_1111;
const _maxVLQ3 = 0xFFFFFF >> 3; //0b0001_1111_1111_1111_1111_1111;
const _maxVLQ4 = (0xFFFFFFFF >> 4); //0b0000_1111_1111_1111_1111_1111_1111_1111;
