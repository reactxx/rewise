import 'dart:math' as math;
import 'package:rw_utils/toBinary.dart' as binary;

class IntBytes {
  IntBytes(this.byte, this.bitsCount);
  //!!! bitsCount is 0..32, low bit first !!!*/
  static Iterable<IntBytes> fromInt(int data, int bitsCount) {
    final res = List<IntBytes>();
    if (bitsCount == 0) return res;
    while (true) {
      final toRead = math.min(bitsCount, 8);
      res.add(IntBytes(data & binary.rightBitsMask(8-toRead), toRead));
      bitsCount -= toRead;
      if (bitsCount <= 0) break;
      data = data >>= 8;
    }
    return res.reversed;
  }

  /*chunks.lenght is max 5, sum(chunks.bitsCount)<=32*/
  static int fromChunks(Iterable<IntBytes> chunks) {
    //var chunks = _chunks.toList();
    var res = 0;
    var checkCount = 0;
    for (var ch in chunks) {
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
