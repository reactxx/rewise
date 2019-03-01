import 'dart:typed_data';
import 'package:tuple/tuple.dart';
import 'package:convert/convert.dart' as convert;

class ByteReader {
  int _start = 0;
  int _len = 0;
  Uint8List _data;
  int _pos = 0;

  ByteReader(Uint8List data_) {
    _data = data_;
    _len = _data.lengthInBytes;
  }

  int readByte() {
    assert(_pos <= _len - 1);
    return _data[_pos++];
  }

  int tryReadByte() {
    if (_pos==_len) return null;
    return _data[_pos++];
  }
  // shifts this._pos by len
  // resulted _pos is equal to this._pos
  // resulted _len is relative to new pos
  ByteReader createSubReader([int len = -1]) {
    final res = ByteReader(_data);
    res._pos = res._start = _pos;
    res._len = len == -1 ? _data.lengthInBytes : res._pos + len;
    assert(res._len <= _data.lengthInBytes, 'res._len <= _data.lengthInBytes');
    _pos = res._len;
    return res;
  }

  // don't shift this._pos
  // resulted _pos is this._start + pos
  // resulted _len is relative to new pos
  ByteReader createSubReaderFromPos(int pos, [int len = -1]) {
    final res = ByteReader(_data);
    res._pos = res._start = _start + pos;
    res._len = len == -1 ? _data.lengthInBytes : res._pos + len;
    assert(res._len <= _data.lengthInBytes);
    return res;
  }

  ByteReader setPos(int newPos) {
    _pos = _start + newPos;
    assert(_pos >= _start && _pos < _data.length);
    return this;
  }

  int readNum(int size) {
    switch (size) {
      case 0:
        return 0;
      case 1:
        return readByte();
      case 2:
        assert(_pos <= _len - 2);
        return _data[_pos++] | _data[_pos++] << 8;
      case 3:
        assert(_pos <= _len - 3);
        return _data[_pos++] | _data[_pos++] << 8 | _data[_pos++] << 16;
      default:
        throw UnimplementedError();
    }
  }

  String hexDump() {
    final view = Uint8List.view(_data.buffer, _start, _len - _start);
    return convert.hex.encode(view);
  }

  // search on _data[_start, _len]
  Tuple2<int, int> BinarySearch(int numSize, int key) {
    int min = 0;
    int max = (_len - _start) ~/ numSize;
    while (min < max) {
      int mid = min + ((max - min) >> 1);
      final element = setPos(mid * numSize).readNum(numSize);
      if (element == key) return Tuple2(mid, element);
      if (element < key)
        min = mid + 1;
      else
        max = mid;
    }
    return Tuple2(-min - 1, -1);
  }
}
