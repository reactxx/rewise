import 'dart:typed_data';
import 'dart:math' as math;
import 'package:rw_utils/toBinary.dart' as binary;

class BitData {
  BitData(this.bits, this.bitsCount /*high bit first*/);
  Uint8List bits;
  int bitsCount;
}

// inspiration in https://github.com/matanlurey/binary/blob/master/lib/binary.dart
class BitWriter implements binary.IWriters {
  // first [0.._bufLen-1] bits from lower _buf's byte is not flushed
  int _buf = 0;
  // number of used bits from lower _buf's byte
  int _bufLen = 0; // values in 0..7

  int len = 0;

  binary.ByteWriter _dataStream;

  BitWriter.fromByteWriter(this._dataStream);
  BitWriter() {
    _dataStream = binary.ByteWriter();
  }
  BitWriter.fromBools(Iterable<bool> data) {
    _dataStream = binary.ByteWriter();
    writeBools(data);
  }

  String dump() {
    align();
    return _dataStream.dump();
  }

  Uint8List toBytes() {
    align();
    return _dataStream.toBytes();
  }

  binary.ByteWriter get writer => _dataStream;

  void writeBool(bool value) {
    writeBits(value ? _trueBit : _falseBit, 1);
  }

  Uint8List _trueBit = Uint8List.fromList([0x80]);
  Uint8List _falseBit = Uint8List.fromList([0]);

  void writeBools(Iterable<bool> values) {
    for (var b in values) writeBool(b);
  }

  void writeBitsList(List<int> list, int length) {
    writeBits(Uint8List.fromList(list), length);
  }

  void writeDatas(Iterable<BitData> datas) {
    for (var d in datas) writeData(d);
  }

  void writeData(BitData data) {
    writeBits(data.bits, data.bitsCount);
  }

  void writeChunk(int skipStart, int value, int length) {
    if (length <= 0 || skipStart + length == 0) return;
    assert(skipStart + length <= 8);
    final byte = value << skipStart;
    // put <currentBuf> + part of <byte> to two bytes
    _buf = (_buf << 8) | ((byte << 8) >> _bufLen);
    final copiedBits = math.min(length, 8);
    _bufLen += copiedBits;
    len += copiedBits;
    if (_bufLen >= 8) {
      // high byte is full
      // write high byte
      _dataStream.writeByte(_buf >> 8);
      // low byte remains in buf
      _bufLen -= 8;
      _buf = _buf & binary.leftBitsMask(_bufLen);
    } else {
      // shift high byte to low position as a new buf
      _buf = (_buf >> 8) & binary.leftBitsMask(_bufLen);
    }
  }

  void writeInt(int value, int bitCount, [bool checkOverflow = true]) {
    if (bitCount == 0) return;
    if (checkOverflow && (value > binary.maxIntBits(bitCount)))
      throw Exception();
    for (var ch in binary.IntBytes.fromInt(value, bitCount))
      writeChunk(8 - ch.bitsCount, ch.byte, ch.bitsCount);
  }

  void doAsert(int flag) {
    assert(() {
      writeInt(flag, 8, true);
      return true;
    }());
  }

  // bits are at the begining of the lower byte, first bit is in (value[0] & 0x80, ..., value[n] & 0x01)
  void writeBits(Uint8List value, int length) {
    var valueIdx = 0;
    while (length > 0) {
      final copiedBits = math.min(length, 8);
      writeChunk(0, value[valueIdx++], math.min(length, 8));
      length -= copiedBits;
    }
  }

  void align() {
    if (_bufLen == 0) return;
    _dataStream.writeByte(_buf);
    _buf = 0;
    _bufLen = 0;
  }
}

//0..0, 1..10000000, 2..11000000, ..., 7..11111110, 8..11111111
//leftBitsMask(int bufPos) => (0xff << (8 - bufPos)) & 0xff;
