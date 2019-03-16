import 'dart:typed_data';
import 'package:convert/convert.dart' as convert;
import 'package:rw_utils/toBinary.dart' as binary;
import 'package:protobuf/protobuf.dart' as proto;

class ByteWriter implements binary.IWriters {
  final _byteList = new List<int>();

  String dump() => convert.hex.encode(toBytes());
  Uint8List toBytes() => Uint8List.fromList(_byteList);
  ByteWriter get writer => this;

  void writeByte(int byte) {
    _byteList.add(byte);
  }

  void writeBytes(Uint8List data) {
    if (data == null) return;
    _byteList.addAll(data);
  }

  void writeList(List<int> data) {
    writeBytes(Uint8List.fromList(data));
  }

  void writeWriter(binary.IWriters data) {
    if (data == null) return;
    _byteList.addAll(data.toBytes());
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
    if (_data==null || len == 0)
      writeVLQ(0);
    else {
      final data = _data.iterator;
      writeVLQ(len);
      while (data.moveNext()) {
        writeVLQ(data.current.length);
        writeBytes(data.current);
        len--;
      }
      assert(len==0);
    }
  }
  void writeBytesStream(List<Uint8List> data) {
    writeBytesIterable(data, data==null ? 0 : data.length);
  }

}

const _maxVLQ1 = 0xFF >> 1; //0b0111_1111;
const _maxVLQ2 = 0xFFFF >> 2; //0b0011_1111_1111_1111;
const _maxVLQ3 = 0xFFFFFF >> 3; //0b0001_1111_1111_1111_1111_1111;
const _maxVLQ4 = (0xFFFFFFFF >> 4); //0b0000_1111_1111_1111_1111_1111_1111_1111;
