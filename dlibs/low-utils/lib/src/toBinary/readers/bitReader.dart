import 'dart:typed_data';
import 'byteReader.dart';
import '../common.dart';

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
    return readBits(1).toList()[0];
  }

  Iterable<bool> readAllBits() sync* {
    while (true) {
      // return the rest of the readData
      for (var b in readBits(bitsToRead)) yield b;
      final bb = _dataStream.tryReadByte();
      if (bb == null) break;
      for (var i = 0; i < 8; i++) yield isBit(bb, i);
    }
    align();
  }

  void skipBits(int bitCount) {
    for (var b in readBits(bitCount));
  }

  Iterable<bool> readBits(int bitCount) sync* {
    while (bitCount > 0) {
      _adjustBuf();
      yield isBit(_buf, _bufPos);
      bitCount--;
      _bufPos++;
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
