import 'dart:typed_data';
import 'dart:math' as math;
import '../writerHolder.dart';
import 'byteWriter.dart';

class BitWriter implements IWriteDataHolder {
  // first [0.._startPosition-1] bits from lower byte is not flushed
  int _buf = 0;
  // first bit in lower _buf byte for writing
  int _bufLen = 0; // valus in 0..7

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
      final byte = value[valueIdx];
      final freeSpace = 8 - currentLen;
      final copiedBits = math.min(length, freeSpace);
      final toIgnore = freeSpace - copiedBits;
      currentBuf |= toIgnore>0 ? ((byte >> (currentLen + toIgnore)) << toIgnore) : (byte >> currentLen);
      currentLen += copiedBits;
      length -= copiedBits;
      final toCopy = length>0 ? math.min(8-copiedBits,length) : 0;
      assert(currentLen <= 8);
      if (currentLen == 8) {
        _dataStream.writeByte(currentBuf);
        currentBuf = 0;
        currentLen = 0;
        if (toCopy>0) {
          currentBuf = (byte << (8-copiedBits)) & firstByteMask;
          currentLen = toCopy;
        }
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
