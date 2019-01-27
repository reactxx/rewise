import 'dart:typed_data';
import 'package:convert/convert.dart';

class BytesWriter {
  static int getNumberSizeMask(int number) {
    // returns 0,1,2 or 3
    if (number == null) return 0;
    if (number > 0xffffff || number < 0) throw ArgumentError();
    return number == 0 ? 0 : (number <= 0xff ? 1 : (number <= 0xffff ? 2 : 3));
  }

  int len = 0;
  List<Uint8List> _bytes = List<Uint8List>();

  void addNumber(int number, int size /*0,1,2,3*/) {
    if (size == 0) return;
    addBytes(Uint8List.fromList(size == 1
        ? [number]
        : (size == 2
            ? [number & 0xFF, (number >> 8) & 0xFF]
            : [number & 0xFF, (number >> 8) & 0xFF, (number >> 16) & 0xFF])));
  }

  String hexDump() {
    return hex.encode(toBytes());
  }

  void clear() {
    _bytes = List<Uint8List>();
    len = 0;
  }

  void addBytes(Uint8List data) {
    if (data == null) return;
    len += data.length;
    _bytes.add(data);
  }

  void addList(List<int> data) {
    addBytes(Uint8List.fromList(data));
  }

  void addWriter(BytesWriter data) {
    if (data == null) return;
    len += data.len;
    _bytes.addAll(data._bytes);
  }

  Uint8List toBytes() {
    final res = Uint8List(len);
    int pos = 0;
    for (final bs in _bytes) {
      res.setAll(pos, bs);
      pos += bs.length;
    }
    if (pos != len) throw Exception("pos != len");
    return res;
  }
}