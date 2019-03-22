import 'dart:typed_data';
import 'package:protobuf/protobuf.dart' as proto;
import 'package:rw_low/code.dart' show Linq;
import 'dart:convert' as conv;

final _endian = Endian.big;

abstract class Reader {
  // ABSTRACTS
  int readByte({int pos});
  List<int> readBytesLow(int len, {int pos});
  ByteBuffer readToBuffer(int len, {int pos});
  Reader setPos(int pos);

  // OTHERS
  int readUInt32({int pos}) =>
      ByteData.view(readToBuffer(4, pos: pos)).getUint32(0, _endian);

  List<int> readUInt32s(int length, {int pos}) {
    final bd = ByteData.view(readToBuffer(length << 2, pos: pos));
    return List.from(
        Linq.range(0, length).map((i) => bd.getUint32(i << 2, _endian)));
  }

  List<int> readUInt16s(int length, {int pos}) {
    final bd = ByteData.view(readToBuffer(length << 1, pos: pos));
    return List.from(
        Linq.range(0, length).map((i) => bd.getUint16(i << 1, _endian)));
  }

  List<int> readSizedIntsLow(int len, int size /*1,2,3,4*/) {
    if (len <= 0) return null;
    final res = List<int>(len);
    for (var i = 0; i < len; i++) res[i] = readSizedInt(size);
    return res;
  }

  List<int> readSizedInts(int size /*1,2,3,4*/) {
    assert(size >= 1 && size <= 4);
    return readSizedIntsLow(readVLQ(), size);
  }

  int readSizedInt(int size /*0,1,2,3,4*/) {
    assert(size >= 0 && size <= 4);
    switch (size) {
      case 0:
        return 0;
      case 1:
        return readByte();
      case 2:
        return readByte() | readByte() << 8;
      case 3:
        return readByte() | readByte() << 8 | readByte() << 16;
      case 4:
        return readByte() |
            readByte() << 8 |
            readByte() << 16 |
            readByte() << 24;
      default:
        throw UnimplementedError();
    }
  }

  String readString({int pos}) {
    setPos(pos);
    final len = readVLQ();
    return len == 0 ? null : conv.utf8.decode(readBytesLow(len));
  }

  List<int> readBytes({int pos}) {
    setPos(pos);
    final len = readVLQ();
    return len == 0 ? null : readBytesLow(len);
  }

  int readVLQ() {
    var b1 = readByte();
    if ((b1 & 0x80) != 0) return b1 & 0x7f;
    var b2 = readByte();
    if ((b2 & 0x80) != 0) return (b1 << 7) | (b2 & 0x7f);
    var b3 = readByte();
    if ((b3 & 0x80) != 0) return (b1 << 14) | (b2 << 7) | (b3 & 0x7f);
    var b4 = readByte();
    if ((b4 & 0x80) != 0)
      return (b1 << 21) | (b2 << 14) | (b3 << 7) + (b4 & 0x7f);
    throw Exception();
  }

  Iterable<T> readMessages<T extends proto.GeneratedMessage>(
      T create(Uint8List data)) sync* {
    yield* readBytess().map((b) => b == null ? null : create(b));
  }

  List<List<int>> readBytess() {
    final len = readVLQ();
    if (len == 0) return null;
    final res = List<List<int>>(len);
    for (var i = 0; i < len; i++) res[i] = readBytes();
    return res;
  }
}
