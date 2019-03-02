import 'dart:typed_data';
import 'dart:math' as math;
import '../common.dart';
import 'byteWriter.dart';

class BitData {
  BitData(this.bits, this.bitsCount);
  Uint8List bits;
  int bitsCount;
}

class BitWriter implements IWriters {
  // first [0.._bufLen-1] bits from lower _buf's byte is not flushed
  int _buf = 0;
  // number of used bits from lower _buf's byte
  int _bufLen = 0; // values in 0..7

  int len = 0;

  final _dataStream = ByteWriter();

  BitWriter() {}
  BitWriter.fromBools(Iterable<bool> data) {
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

  void writeBitslist(List<int> list, int length) {
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
      // put to two bytes
      currentBuf = (currentBuf << 8) | ((byte << 8) >> currentLen);
      final copiedBits = math.min(length, 8); // used bits (from byte)
      length -= copiedBits;
      currentLen += copiedBits;
      len += copiedBits;
      if (currentLen >= 8) {
        // first byte is full
        // write first byte
        _dataStream.writeByte(currentBuf >> 8);
        // use second byte
        currentLen -= 8;
        currentBuf = currentBuf & validBitsMask[currentLen];
      } else {
        // use not already full first byte
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
