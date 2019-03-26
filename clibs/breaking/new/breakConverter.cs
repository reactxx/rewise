using StemmerBreakerNew;
using System.Linq;
using System;
using System.Collections.Generic;

public static class BreaksConverter {

  public static List<string> getStemms(string txt, byte[] data) {
    var res = new List<string>();
    if (data == null || data.Length == 0) {
      res.Add(txt);
      return res;
    }
    var isOdd = (data.Length % 2) != 0;
    int lastPos = 0;
    for (var i = isOdd ? -1 : 0; i < data.Length; i += 2) {
      var pos = i == -1 ? 0 : lastPos + toInt(data[i]);
      var end = pos + data[i + 1];
      res.Add(txt.Substring(pos, end - pos));
      lastPos = end;
    }
    return res;
  }

  public static byte[] oldToNew(string txt, List<TPosLen> posLens) {
    var res = new List<byte>();
    if (posLens == null || posLens.Count == 0) return null;
    if (posLens.Count == 1 && posLens[0].Pos == 0 && posLens[0].Len == txt.Length) return null;
    var lastPos = 0;
    foreach (var pl in posLens) {
      if (pl.Len > 255) throw new Exception("Len > 255");
      var pos = pl.Pos - lastPos;
      if (Math.Abs(pos) > 127) throw new Exception("Math.Abs(pos) > 127");
      if (lastPos != 0 || pl.Pos != 0)
        res.Add((byte)pos);
      res.Add((byte)pl.Len);
      lastPos = pl.Pos + pl.Len;
    }
    return res.ToArray();
  }

  public static void Tests() {
    var txt = "01234567890123456789";
    Test(txt, 0, 3, 0, 2, 0, 1);
    Test(txt, 0, 1, 2, 1, 4, 1);
    Test(txt, 1, 3, 1, 2, 1, 1);
    Test(txt, 1, 1, 3, 1, 5, 1);
  }
  static int toInt(int x) {
    var abs = x & 0x7f;
    if (abs == x) return x;
    return abs - 128;
  }

  static List<string> getStemmsOld(string txt, List<TPosLen> pl) {
    return pl.Select(p => txt.Substring(p.Pos, p.Len)).ToList();
  }

  static void Test(String txt, params int[] bytes) {
    var posLens = help(bytes).ToList();
    var breaks = oldToNew(txt, posLens);
    var breaksDump = breaks.Select(b => b.ToString()).JoinStrings(",");
    var stemms = getStemms(txt, breaks.ToArray()).JoinStrings(",");
    var stemmsOld = getStemmsOld(txt, posLens).JoinStrings(",");
    if (stemms != stemmsOld) throw new Exception();
  }
  static IEnumerable<TPosLen> help(params int[] bytes) {
    for (var i = 0; i < bytes.Length; i += 2)
      yield return new TPosLen { Pos = bytes[i], Len = bytes[i + 1] };
  }

}
