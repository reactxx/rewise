import 'dart:typed_data';
import 'dart:math' as math;
import 'package:rewise_low_utils/toBinary.dart' as binary;
import '../common.dart';

class BitReader implements IReaders {
  var _buf = 0;
  var _bufPos = 0;
  var _bufEmpty = true;

  binary.ByteReader _dataStream;

  binary.ByteReader get reader => _dataStream;

  BitReader(Uint8List data) {
    _dataStream = new binary.ByteReader(data);
  }

  int get bitsToRead => _bufEmpty ? 0 : 8 - _bufPos;

  bool readBit() {
    _adjustBuf();
    return isBit(_buf, _bufPos++);
  }

  Iterable<bool> readAllBits() sync* {
    while (true) {
      // return the rest of the readData
      for (var b in readBits(bitsToRead)) yield b;
      final bb = _dataStream.tryReadByte();
      if (bb == null) break;
      for (var i = 0; i < 8; i++) yield isBit(bb, i);
    }
  }

  Iterable<bool> readBitStream(int bitCount) sync* {
    while (bitCount > 0) {
      bitCount--;
      yield readBit();
    }
  }

  List<bool> readBits(int bitCount) {
    final res = new List<bool>();
    while (bitCount > 0) {
      res.add(readBit());
      bitCount--;
    }
    return res;
  }

  Iterable<binary.IntChunk> readChunkStream(int bitCount) sync* {
    while (bitCount > 0) {
      _adjustBuf();
      var toRead = math.min(bitCount, bitsToRead);
      var skipped = _buf & rightBitsMask(_bufPos);
      var taked = skipped >> (8 - (toRead + _bufPos));
      yield binary.IntChunk(taked, toRead);
      bitCount -= toRead;
      _bufPos += toRead;
    }
  }

  int readInt(int bitCount) =>
      binary.IntChunk.fromChunks(readChunkStream(bitCount));

  void _adjustBuf() {
    if (bitsToRead == 0) {
      _buf = _dataStream.readByte();
      _bufPos = 0;
      _bufEmpty = false;
    }
  }

  void align() {
    _bufPos = 0;
    _buf = 0;
    _bufEmpty = true;
  }

  static dump(Iterable<bool> bools) {
    var res = '';
    var idx = 0;
    for (var b in bools) {
      if (idx == 4) {
        idx = 0;
        res += ' ';
      }
      res += b ? '1' : '0';
      idx++;
    }
    return res;
  }
}

