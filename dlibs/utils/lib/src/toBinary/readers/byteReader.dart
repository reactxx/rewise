import 'dart:typed_data';
import 'package:tuple/tuple.dart';
import 'package:convert/convert.dart' as convert;
import 'package:rw_utils/toBinary.dart' as binary;
import 'package:protobuf/protobuf.dart' as proto;

class ByteReader implements binary.IReaders {
  int _start = 0;
  int _len = 0;
  Uint8List _data;
  int _pos = 0;

  ByteReader get reader => this;

  ByteReader(Uint8List data_) {
    _data = data_;
    _len = _data.lengthInBytes;
  }

  int readByte() {
    assert(_pos <= _len - 1);
    return _data[_pos++];
  }

  int tryReadByte() {
    if (_pos == _len) return null;
    return _data[_pos++];
  }

  Uint8List readBytes(int len) {
    assert(_pos + len <= _len);
    final oldPos = _pos;
    _pos+=len;
    return Uint8List.view(_data.buffer, oldPos, len);
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
