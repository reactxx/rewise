import 'dart:typed_data';
import 'dart:math' as math;
import 'package:rewise_low_utils/toBinary.dart' as binary;

class BitReader implements binary.IReaders {
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
    return binary.isBit(_buf, _bufPos++);
  }

  Iterable<bool> readAllBits() sync* {
    while (true) {
      // return the rest of the readData
      for (var b in readBitStream(bitsToRead)) yield b;
      final bb = _dataStream.tryReadByte();
      if (bb == null) break;
      yield* binary.byteToBools(bb);
      //for (var i = 0; i < 8; i++) yield binary.isBit(bb, i);
    }
  }

  Iterable<bool> readBitStream([int bitCount]) sync* {
    if (bitCount == null)
      while (true) yield readBit();
    else
      while (bitCount > 0) {
        bitCount--;
        yield readBit();
      }
  }

  void doAsert(int flag) {
    assert((() => readInt(8) == flag)());
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
      var skipped = _buf & binary.rightBitsMask(_bufPos);
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
}
