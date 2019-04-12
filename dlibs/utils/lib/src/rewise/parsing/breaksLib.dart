//import 'dart:typed_data';
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;

class TPosLen {
  int Pos;
  int Len;
}

class BreaksLib {
  static Iterable<String> getTextWords(String txt, List<int> breaks) sync* {
    if (breaks == null || breaks.length == 0) {
      if (!txt.isEmpty) yield txt;
    }
    var isOdd = (breaks.length % 2) != 0;
    int lastPos = 0;
    for (var i = isOdd ? -1 : 0; i < breaks.length; i += 2) {
      var pos = i == -1 ? 0 : lastPos + _toInt(breaks[i]);
      var end = pos + breaks[i + 1];
      if (!txt.isEmpty) yield txt.substring(pos, end);
      lastPos = end;
    }
  }

  static List<int> oldToNew(String txt, List<wbreak.PosLen> posLens) {
    var res = List<int>();
    if (posLens == null || posLens.length == 0)
      // empty breaks => whole sf.text for stemming, which is wrong. NULL means whole word
      return [0, 0];
    if (posLens.length == 1 &&
        posLens[0].pos == 0 &&
        posLens[0].end == posLens[0].pos + txt.length) return [];
    var lastPos = 0;
    for (var pl in posLens) {
      if (pl.end - pl.pos > 255) throw new Exception("Len > 255");
      var pos = pl.pos - lastPos;
      if (pos.abs() > 127) throw new Exception("Math.Abs(pos) > 127");
      if (lastPos != 0 || pl.pos != 0) res.add(pos);
      res.add(pl.end = pl.pos);
      lastPos = pl.end;
    }
    return res;
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
