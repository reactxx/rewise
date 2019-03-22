import 'dart:typed_data';
import 'package:protobuf/protobuf.dart' as proto;
import 'dart:convert' as conv;
import 'package:rw_utils/utils.dart' as utils;

final _endian = Endian.big;

abstract class Writer {
  // ABSTRACTS
  void writeByte(int byte, {int pos});
  void writeBytesLow(List<int> data, {int pos});
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

  void writeSizedIntsLow(List<int> ints, int size /*1,2,3,4*/) {
      if (ints==null || ints.isEmpty) return;
      for (final i in ints) writeSizedInt(i, size);
  }

  void writeSizedInts(List<int> ints, int size /*1,2,3,4*/) {
    assert(size >= 1 && size <= 4);
    if (ints == null || ints.isEmpty)
      writeVLQ(0);
    else {
      writeVLQ(ints.length);
      writeSizedIntsLow(ints, size);
    }
  }

  void writeSizedInt(int number, int size /*0,1,2,3,4*/) {
    assert(number!=null && number <= utils.maxInt);
    assert(size >= 0 && size <= 4);
    switch (size) {
      case 0:
        return;
      case 1:
        return writeBytesLow([number]);
      case 2:
        return writeBytesLow([number & 0xff, (number >> 8) & 0xff]);
      case 3:
        return writeBytesLow(
            [number & 0xff, (number >> 8) & 0xff, (number >> 16) & 0xff]);
      case 4:
        assert(number <= 0x7fffffff);
        return writeBytesLow([
          number & 0xff,
          (number >> 8) & 0xff,
          (number >> 16) & 0xff,
          (number >> 24) & 0xff
        ]);
      default:
        throw UnimplementedError();
    }
  }

  void writeString(String str, {int pos}) {
    setPos(pos);
    if (str == null || str.isEmpty) {
      writeVLQ(0);
    } else {
      final bytes = conv.utf8.encode(str);
      writeVLQ(bytes.length);
      writeBytesLow(bytes);
    }
  }

  void writeBytes(List<int> data, {int pos}) {
    setPos(pos);
    if (data == null || data.isEmpty) {
      writeVLQ(0);
    } else {
      writeVLQ(data.length);
      writeBytesLow(data);
    }
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

  void writeMessages(List<proto.GeneratedMessage> data) =>
      writeBytesIterable(data?.map((d) => d.writeToBuffer()), data?.length);

  void writeBytesIterable(Iterable<List<int>> _data, int len) {
    if (_data == null || len == 0 || len == null)
      writeVLQ(0);
    else {
      writeVLQ(len);
      final data = _data.iterator;
      while (data.moveNext()) {
        writeBytes(data.current);
        len--;
      }
      assert(len == 0);
    }
  }

  void writeBytess(List<List<int>> data) =>
      writeBytesIterable(data, data?.length);
}

const _maxVLQ1 = 0xFF >> 1; //0b0111_1111;
const _maxVLQ2 = 0xFFFF >> 2; //0b0011_1111_1111_1111;
const _maxVLQ3 = 0xFFFFFF >> 3; //0b0001_1111_1111_1111_1111_1111;
const _maxVLQ4 = (0xFFFFFFFF >> 4); //0b0000_1111_1111_1111_1111_1111_1111_1111;
