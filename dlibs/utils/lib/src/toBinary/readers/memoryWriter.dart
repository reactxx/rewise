import 'dart:typed_data';
import 'package:convert/convert.dart' as convert;
import 'package:rw_utils/toBinary.dart' as binary;
import 'writer.dart';

class MemoryWriter extends Writer implements binary.IWriters {
  // ABSTRACTS
  void writeByte(int byte, {int pos}) {
    setPos(pos);
    _byteList.add(byte);
  }

  void writeBytesLow(List<int> data, {int pos}) {
    setPos(pos);
    if (data == null) return;
    _byteList.addAll(data);
  }

  MemoryWriter setPos(int pos) {
    if (pos == null) return this;
    throw Exception('No random access write to MemoryWriter');
  }

  void writeToBuffer(int len, void fillData(ByteData data), {int pos}) {
    setPos(pos);
    final toWrite = Uint8List(len);
    fillData(ByteData.view(toWrite.buffer));
    writeBytesLow(toWrite);
  }

  // OTHERS
  final _byteList = new List<int>();

  String dump() => convert.hex.encode(toBytes());
  Uint8List toBytes() => Uint8List.fromList(_byteList);
  List<int> get byteList => _byteList;
  MemoryWriter get writer => this;

  void writeWriter(binary.IWriters data) {
    if (data == null) return;
    _byteList.addAll(data.toBytes());
  }
}
