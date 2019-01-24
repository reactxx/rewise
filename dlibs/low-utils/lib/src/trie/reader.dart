import 'dart:typed_data';
import 'package:tuple/tuple.dart';
import 'package:convert/convert.dart' as convert;

class BytesReader {
  int _start = 0;
  int _len = 0;
  Uint8List _data;
  int _pos = 0;

  BytesReader(Uint8List data_) {
    _data = data_;
    _len = _data.lengthInBytes;
  }

  // BytesReader.parented(BytesReader parent, [int len = 0]) {
  //   _data = parent._data;
  //   _pos = _start = parent._pos;
  //   _len = len == 0 ? _data.lengthInBytes : _start + len;
  //   if (_len > _data.lengthInBytes) throw ArgumentError();
  // }

  BytesReader innerReader({int pos: 0, int len: 0}) {
    final res = BytesReader(_data);
    res._pos = res._start = _pos + pos;
    res._len = len == 0 ? _data.lengthInBytes : res._pos + len;
    if (_len > _data.lengthInBytes) throw ArgumentError();

    // if (len_ == 0) len_ = _len - _pos;
    // final parentLen = len_ == 0 ? _data.lengthInBytes : _pos + len_;
    // if (parentLen > _data.lengthInBytes) return null;
    // final res = BytesReader.parented(this, len_);
    _pos = len == 0 ? _data.lengthInBytes : _pos + len;

    return res;
  }

  BytesReader setPos(int newPos) {
    _pos = _start + newPos;
    if (_pos < _start || _pos >= _data.length) throw ArgumentError();
    return this;
  }

  int readNum(int size) {
    switch (size) {
      case 0:
        return 0;
      case 1:
        return _data[_pos++];
      case 2:
        return _data[_pos++] | _data[_pos++] << 8;
      case 3:
        return _data[_pos++] | _data[_pos++] << 8 | _data[_pos++] << 16;
      default:
        throw UnimplementedError();
    }
  }

  String hexDump() {
    return convert.hex
        .encode(Uint8List.view(_data.buffer, _start, _len - _start));
  }

  Tuple2<int, int> BinarySearch(int size, int key) {
    int min = 0;
    int max = ((_len - _pos) ~/ size);
    while (min < max) {
      int mid = min + ((max - min) >> 1);
      final element = setPos(mid * size).readNum(size);
      if (element == key) return Tuple2(mid, element);
      if (element < key)
        min = mid + 1;
      else
        max = mid;
    }
    return Tuple2(-min - 1, -1);
  }
}
