import 'dart:typed_data';
import 'dart:math' as math;
import 'byteReader.dart';
import '../common.dart';

class IntChunk {
  IntChunk(this.byte, this.bitsCount);
  //!!! bitsCount is 0..32, low bit first !!!*/
  static Iterable<IntChunk> fromInt(int data, int bitsCount) sync* {
    if (bitsCount == 0) return;
    while (true) {
      final toRead = math.min(bitsCount, 8);
      yield IntChunk(data & (0xff >> (8 - toRead)), toRead);
      bitsCount -= toRead;
      if (bitsCount <= 0) break;
      data = data >>= 8;
    }
  }

  /*chunks.lenght is max 5, sum(chunks.bitsCount)<=32*/
  static int fromChunks(List<IntChunk> chunks) {
    var res = 0;
    var checkCount = 0;
    for (var ch in chunks.reversed) {
      checkCount += ch.bitsCount;
      if (checkCount > 32) throw Exception();
      res = res << ch.bitsCount; // get space for chunk
      res = res | ch.byte; // copy chunk data
    }
    return res;
  }

  int byte; // only low byte is valid
  int bitsCount; // how much LOW bits from low byte is valid
}

class BitReader implements IReaders {
  var _buf = 0;
  var _bufPos = 0;
  var _bufEmpty = true;

  ByteReader _dataStream;

  ByteReader get reader => _dataStream;

  BitReader(Uint8List data) {
    _dataStream = new ByteReader(data);
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

  Iterable<IntChunk> readChunkStream(int bitCount) sync* {
    while (bitCount > 0) {
      _adjustBuf();
      var readed = math.min(bitCount, bitsToRead);
      yield IntChunk(_buf & (0xff >> readed), readed);
      bitCount -= readed;
      _bufPos += readed;
    }
  }

  int readByte() {
    var btr = bitsToRead;
    final toRead = 8 - btr;
    int res = 0;
    if (btr > 0) {
      res = (_buf << (8 - btr)) & 0xff;
      _bufPos += btr;
      if (toRead == 0) return res;
    }
    _adjustBuf();
    res = res | (_buf >> btr);
    _bufPos += toRead;
    return res;
  }

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

// isBit(0x80, 0) == true == isBit(0x01, 7)
bool isBit(int byte, int idx /*0..7*/) => (byte & idxBitsMask[idx]) != 0;

const idxBitsMask = [
  0x80,
  0x40,
  0x20,
  0x10,
  0x8,
  0x4,
  0x2,
  0x1,
];
