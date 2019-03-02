import 'dart:typed_data';
import 'package:convert/convert.dart' as convert;
import '../common.dart';

class ByteWriter implements IWriters {

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

  void writeWriter(IWriters data) {
    if (data == null) return;
    _byteList.addAll(data.toBytes());
  }

  void writeNumber(int number, int size /*0,1,2,3*/) {
    if (size == 0) return;
    writeList(size == 1
        ? [number]
        : (size == 2
            ? [number & 0xFF, (number >> 8) & 0xFF]
            : [number & 0xFF, (number >> 8) & 0xFF, (number >> 16) & 0xFF]));
  }

  //*** STATIC
  static int getNumberSizeMask(int number) {
    // returns 0,1,2 or 3
    if (number == null) return 0;
    if (number > 0xffffff || number < 0) throw ArgumentError();
    return number == 0 ? 0 : (number <= 0xff ? 1 : (number <= 0xffff ? 2 : 3));
  }

}
