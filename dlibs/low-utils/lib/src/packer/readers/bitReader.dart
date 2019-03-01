import 'dart:typed_data';
import 'byteReader.dart';

class BitReader {
  var _buf = 0;
  var _bufPos = 0;
  var _bufEmpty = true;

  ByteReader dataStream;

  BitReader(Uint8List data) {
    dataStream = new ByteReader(data);
  }

  int get bitsToRead => _bufEmpty ? 0 : 8 - _bufPos;

  bool readBit() {
    return readBits(1).first;
  }

  Iterable<bool> readAllBits() sync* {
    while (true) {
      // return the rest of the readData
      for (var b in readBits(bitsToRead)) yield b;
      final bb = dataStream.tryReadByte();
      if (bb == null) break;
      for (var i = 0; i < 8; i++) yield isBit(bb, i);
    }
  }

  Iterable<bool> readBits(int bitCount) sync* {
    while (bitCount > 0) {
      bitCount--;
      if (bitsToRead == 0) {
        _buf = dataStream.readByte();
        _bufPos = 0;
        _bufEmpty = false;
      }
      yield isBit(_buf, _bufPos);
      _bufPos++;
    }
  }

  void align() {
    _bufPos = 0;
    _buf = 0;
    _bufEmpty = true;
  }
}

bool isBit(int byte, int idx /*0..7*/) => (byte & idxBitsMask[idx]) == 1;

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
