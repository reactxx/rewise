import 'dart:typed_data';
import 'package:tuple/tuple.dart';
import 'package:convert/convert.dart' as convert;
import 'package:rw_utils/toBinary.dart' as binary;
import 'reader.dart';

class MemoryReader extends Reader implements binary.IReaders {
  // CONSTRUCTORS
  MemoryReader(Uint8List data_) {
    _data = data_;
    _len = _data.lengthInBytes;
  }

  // ABSTRACT
  int readByte({int pos}) {
    setPos(pos);
    assert(_pos <= _len - 1);
    return _data[_pos++];
  }

  List<int> readBytesLow(int len) {
    assert(_pos + len <= _len);
    final oldPos = _pos;
    _pos += len;
    return Uint8List.view(_data.buffer, oldPos, len);
  }

  // ByteBuffer readToBuffer(int len) {
  //   final oldPos = _pos;
  //   setPos(pos);
  //   return Uint8List.view(_data.buffer, oldPos, len).buffer;
  // }

  MemoryReader setPos(int newPos) {
    if (newPos == null) return this;
    _pos = _start + newPos;
    assert(_pos >= _start && _pos < _data.length);
    return this;
  }

  // OTHERS
  int _start = 0;
  int _len = 0;
  Uint8List _data;
  int _pos = 0;

  MemoryReader get reader => this;

  int tryReadByte() {
    if (_pos == _len) return null;
    return _data[_pos++];
  }

  // shifts this._pos by len
  // resulted _pos is equal to this._pos
  // resulted _len is relative to new pos
  MemoryReader createSubReader([int len = -1]) {
    final res = MemoryReader(_data);
    res._pos = res._start = _pos;
    res._len = len == -1 ? _data.lengthInBytes : res._pos + len;
    assert(res._len <= _data.lengthInBytes, 'res._len <= _data.lengthInBytes');
    _pos = res._len;
    return res;
  }

  // don't shift this._pos
  // resulted _pos is this._start + pos
  // resulted _len is relative to new pos
  MemoryReader createSubReaderFromPos(int pos, [int len = -1]) {
    final res = MemoryReader(_data);
    res._pos = res._start = _start + pos;
    res._len = len == -1 ? _data.lengthInBytes : res._pos + len;
    assert(res._len <= _data.lengthInBytes);
    return res;
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
      setPos(mid * numSize);
      final element = binary.readInt(this, numSize);
      if (element == key) return Tuple2(mid, element);
      if (element < key)
        min = mid + 1;
      else
        max = mid;
    }
    return Tuple2(-min - 1, -1);
  }
}
