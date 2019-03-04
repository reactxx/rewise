import 'dart:typed_data';
import 'package:convert/convert.dart' as convert;
import 'package:rewise_low_utils/toBinary.dart' as binary;

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

}
