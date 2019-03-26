import 'dart:typed_data';

class TPosLen {
  int Pos;
  int Len;
}

class BreakConverter {
  static Iterable<String> getStemms(String txt, List<int> data) sync* {
    if (data == null || data.length == 0) {
      if (!txt.isEmpty) yield txt;
    }
    var isOdd = (data.length % 2) != 0;
    int lastPos = 0;
    for (var i = isOdd ? -1 : 0; i < data.length; i += 2) {
      var pos = i == -1 ? 0 : lastPos + _toInt(data[i]);
      var end = pos + data[i + 1];
      if (!txt.isEmpty) yield txt.substring(pos, end);
      lastPos = end;
    }
  }

  static Uint8List oldToNew(String txt, List<TPosLen> posLens) {
    var res = List<int>();
    if (posLens == null || posLens.length == 0) return null;
    if (posLens.length == 1 &&
        posLens[0].Pos == 0 &&
        posLens[0].Len == txt.length) return null;
    var lastPos = 0;
    for (var pl in posLens) {
      if (pl.Len > 255) throw new Exception("Len > 255");
      var pos = pl.Pos - lastPos;
      if (pos.abs() > 127) throw new Exception("Math.Abs(pos) > 127");
      if (lastPos != 0 || pl.Pos != 0) res.add(pos);
      res.add(pl.Len);
      lastPos = pl.Pos + pl.Len;
    }
    return Uint8List.fromList(res);
  }

  static int _toInt(int x) {
    var abs = x & 0x7f;
    if (abs == x) return x;
    return abs - 128;
  }
}
// void Tests() {
//   var txt = "01234567890123456789";
//   Test(txt, 0, 3, 0, 2, 0, 1);
//   Test(txt, 0, 1, 2, 1, 4, 1);
//   Test(txt, 1, 3, 1, 2, 1, 1);
//   Test(txt, 1, 1, 3, 1, 5, 1);
// }

// List<String> getStemmsOld(String txt, List<TPosLen> pl) {
//   return pl.Select(p => txt.Substring(p.Pos, p.Len)).ToList();
// }

// void Test(String txt, params int[] bytes) {
//   var posLens = help(bytes).ToList();
//   var breaks = oldToNew(txt, posLens);
//   var breaksDump = breaks.Select(b => b.ToString()).JoinStrings(",");
//   var stemms = getStemms(txt, breaks.ToArray()).JoinStrings(",");
//   var stemmsOld = getStemmsOld(txt, posLens).JoinStrings(",");
//   if (stemms != stemmsOld) throw new Exception();
// }
// Iterable<TPosLen> help(params int[] bytes) {
//   for (var i = 0; i < bytes.length; i += 2)
//     yield return new TPosLen { Pos = bytes[i], Len = bytes[i + 1] };
// }
