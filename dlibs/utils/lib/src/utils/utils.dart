import 'dart:typed_data';

const int maxInt = 0x7fffffff;
const int minInt = -maxInt;

int maxIntBits(int bits) => bits==0 ? maxInt : 0xffffffff >> (32 - bits);

// MASK: 0..10000000, 1..01000000, 2..00100000, 7..00000001
bool isBit(int byte, int idx /*0..7*/) => (byte & (1 << (7 - idx))) != 0;

//0..0, 1..10000000, 2..11000000, ..., 7..11111110, 8..11111111
int leftBitsMask(int bufPos) => (0xff << (8 - bufPos)) & 0xff;

//0..11111111, 1..01111111, 7..00000001, 8..00000000
int rightBitsMask(int bufPos) => 0xff >> bufPos;

String dumpIterableBoolBits(Iterable<bool> bools) {
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

Iterable<bool> byteToBools(int byte) sync* {
  if (byte == null) return;
  for (var i = 0; i < 8; i++) yield isBit(byte, i);
}

Iterable<bool> bytesToBools(Uint8List bytes) sync* {
  yield* bytes.expand((b) => byteToBools(b));
}

String dumpBytesBits(Uint8List bytes) => dumpIterableBoolBits(bytesToBools(bytes));

class Interval {
  Interval(this.start, this.end);
  int start;
  int end;
  int get skip => start;
  int get take => end - start;

  static Iterable<Interval> intervals(int count, int intLen) sync* {
    var n = (count / intLen).floor();
    if (n * intLen < count) n++;
    var st = 0;
    for (int i = 0; i < n; i++) {
      //int st = i * intLen;
      var en = st + intLen;
      if (en > count) en = count;
      yield Interval(st, en);
      st += intLen;
    }
  }
}
