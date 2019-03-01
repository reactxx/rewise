import 'dart:typed_data';
import 'byteReader.dart';

class BitReader {
  var _readData = 0;
  var _startPosition = 0;
  var _readDataEmpty = true;

  ByteReader dataStream;

  BitReader(Uint8List data) {
    dataStream = new ByteReader(data);
  }

  int get bitsInBuffer => _readDataEmpty ? 0 : 8 - _startPosition;

  bool readBit() {
    return readBits(1).first;
  }

  Iterable<bool> readAllBits() sync* {
    while (true) {
      // return the rest of the readData
      for (var b in readBits(bitsInBuffer)) yield b;
      final bb = dataStream.tryReadByte();
      if (bb == null) break;
      for (var i = 0; i < 8; i++) yield (bb & (1 << (31 - i - 1))) != 0;
    }
  }

  Iterable<bool> readBits(int bitCount) sync* {
    while (bitCount > 0) {
      bitCount--;
      if (bitsInBuffer == 0) {
        _readData = dataStream.readByte() << 24;
        _startPosition = 0;
        _readDataEmpty = false;
      }
      yield (_readData & (1 << (31 - _startPosition))) != 0;
      _startPosition++;
    }
  }

  void Align() {
    _startPosition = 0;
    _readData = 0;
  }
}
