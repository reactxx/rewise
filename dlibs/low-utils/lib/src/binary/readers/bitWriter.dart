part of binary.readers;

class BitWriter implements IWriteDataHolder {
  int _data = 0; // max 15 hight bits valid
  int _dataLength = 0; // valus in 0..7

  final _dataStream = ByteWriter();

  List<int> get byteList => _dataStream.byteList;

  //List<byte> data { get { return _dataStream.writerData; } }

  //int get bitsToAligment => (32 - _dataLength) % 8;

  void writeBit(bool value) {
    writeBits(value ? _trueBit : _falseBit, 1);
  }

  Uint8List _trueBit = Uint8List.fromList([0x80]);
  Uint8List _falseBit = Uint8List.fromList([0]);

  void writeBools(Iterable<bool> values) {
    for (var b in values) writeBit(b);
  }

  // bits are at the begining of the value, first bit is in (value[0] & 0x80, ..., value[n] & 0x01)
  void writeBits(Uint8List value, int length) {
    if (length <= 0) return;
    var valueIdx = 0;
    var currentData = _data;
    var currentLength = _dataLength;
    while (length > 0) {
      var byte = value[valueIdx];
      var byteLen = math.min(length, 8);
      currentData = _data | (byte << (32 - _dataLength));
      currentLength = _dataLength + byteLen;
      valueIdx++;
      length -= 8;
      if (currentLength >= 8) {
        _dataStream.writeByte(currentData >> 24);
        currentData <<= 8;
        currentLength -= 8;
      }
    }
    _data = currentData;
    _dataLength = currentLength;
  }

  void align() {
    if (_dataLength == 0) return;
    _dataStream.writeByte(_data >> 24);
    _data = 0;
    _dataLength = 0;
  }
}
