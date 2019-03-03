import 'dart:typed_data';
import 'dart:math' as math;
import '../common.dart';
import 'byteWriter.dart';

class BitData {
  BitData(this.bits, this.bitsCount /*high bit first*/);
  Uint8List bits;
  int bitsCount;
}

// inspiration in https://github.com/matanlurey/binary/blob/master/lib/binary.dart
class BitWriter implements IWriters {
  // first [0.._bufLen-1] bits from lower _buf's byte is not flushed
  int _buf = 0;
  // number of used bits from lower _buf's byte
  int _bufLen = 0; // values in 0..7

  int len = 0;

  ByteWriter _dataStream;

  BitWriter.fromByteWriter(this._dataStream);
  BitWriter() {
    _dataStream = ByteWriter();
  }
  BitWriter.fromBools(Iterable<bool> data) {
    _dataStream = ByteWriter();
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

  ByteWriter get writer => _dataStream;

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

  // bits are at the begining of the lower byte, first bit is in (value[0] & 0x80, ..., value[n] & 0x01)
  void writeBits(Uint8List value, int length) {
    if (length <= 0) return;
    var valueIdx = 0;
    var currentBuf = _buf;
    var currentLen = _bufLen;
    while (length > 0) {
      final byte = value[valueIdx++];
      // put <currentBuf> + part of <byte> to two bytes
      currentBuf = (currentBuf << 8) | ((byte << 8) >> currentLen);
      final copiedBits = math.min(length, 8);
      length -= copiedBits;
      currentLen += copiedBits;
      len += copiedBits;
      if (currentLen >= 8) {
        // high byte is full
        // write high byte
        _dataStream.writeByte(currentBuf >> 8);
        // low byte remains in buf
        currentLen -= 8;
        currentBuf = currentBuf & validBitsMask[currentLen];
      } else {
        // shift high byte to low position as a new buf
        currentBuf = (currentBuf >> 8) & validBitsMask[currentLen];
      }
    }
    _buf = currentBuf;
    _bufLen = currentLen;
  }

  void align() {
    if (_bufLen == 0) return;
    _dataStream.writeByte(_buf);
    _buf = 0;
    _bufLen = 0;
  }
}

const validBitsMask = [
  0,
  (0xff << 7) & 0xff,
  (0xff << 6) & 0xff,
  (0xff << 5) & 0xff,
  (0xff << 4) & 0xff,
  (0xff << 3) & 0xff,
  (0xff << 2) & 0xff,
  (0xff << 1) & 0xff,
];
