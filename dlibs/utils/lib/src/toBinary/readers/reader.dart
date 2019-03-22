import 'dart:typed_data';
import 'package:protobuf/protobuf.dart' as proto;
import 'package:rw_low/code.dart' show Linq;

final _endian = Endian.big;

abstract class Reader  {
  // ABSTRACTS
  int readByte({int pos});
  List<int> readBytes(int len, {int pos});
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

  String readEncodedString(Uint16List toCodeUnit, {int len, int pos}) {
    if (len == null) len = readByte(pos: pos);
    return String.fromCharCodes(readBytes(len).map((b) => toCodeUnit[b]));
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

  Iterable<T> readMessages<T extends proto.GeneratedMessage>(T create(Uint8List data)) sync* {
    yield* readBytesStream().map((b) => b==null ? null : create(b));
  }

  Iterable<Uint8List> readBytesStream() sync* {
    final len =readVLQ();
    if (len==0) return;
    for(var i=0; i<len; i++) {
      final bl = readVLQ();
      yield bl==0 ? null : readBytes(bl);
    }
  }
}

