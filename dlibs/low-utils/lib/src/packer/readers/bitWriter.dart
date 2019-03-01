import 'dart:typed_data';
import 'dart:math' as math;
import '../writerHolder.dart';
import 'byteWriter.dart';

class BitWriter implements IWriteDataHolder {
  // first [0.._bufLen-1] bits from lower _buf's byte is not flushed
  int _buf = 0;
  // number of used bits from lower _buf's byte
  int _bufLen = 0; // values in 0..7

  final _dataStream = ByteWriter();

  List<int> get byteList => _dataStream.byteList;

  void writeBit(bool value) {
    writeBits(value ? _trueBit : _falseBit, 1);
  }

  Uint8List _trueBit = Uint8List.fromList([0x80]);
  Uint8List _falseBit = Uint8List.fromList([0]);

  void writeBools(Iterable<bool> values) {
    for (var b in values) writeBit(b);
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
      if (currentLen >= 8) { // first byte is full
        // write first byte
        _dataStream.writeByte(currentBuf >> 8);
        // use second byte
        currentLen -= 8;
        currentBuf = currentBuf & validBitsMask[currentLen];
      } else { // use not already full first byte
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
