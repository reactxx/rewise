using Sepia.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class CaseFolding {

  //# "caseFolding.txt" is clipboard copy from https://www.ibm.com/support/knowledgecenter/en/ssw_ibm_i_72/nls/rbagsuppertolowermaptable.htm

  public static void Parse() {

    var codes = Enumerable.Range(32, 0xffff - 31).Select(i => (UInt16)i).ToArray();
    var strUpper = new String(codes.Select(i => Convert.ToChar(i)).ToArray());
    var str = strUpper.ToLower();
    var strNet = new String(codes.Select(c => Convert.ToChar(NETFolding.toLower(c))).ToArray());
    var strNet2 = new String(codes.Select(c => Convert.ToChar(NET2Folding.toLower(c))).ToArray());
    var strIBM = new String(codes.Select(c => Convert.ToChar(IBMFolding.toLower(c))).ToArray());

    if (strIBM != strNet)
      throw new Exception();
    if (str != strNet2)
      throw new Exception();

    var strDiffInt = Enumerable.Range(0, codes.Length).Where(i => str[i] != strNet2[i]).ToArray();
    var strDiff = new string(strDiffInt.Select(c => Convert.ToChar(c)).ToArray());
    var len = strDiff.Length;


    var codes2 = Enumerable.Range(32, 0xffff - 31).Select(i => (UInt16)i).ToArray();
    var strUpper2 = new String(codes2.Select(i => Convert.ToChar(i)).ToArray());
    var str2 = strUpper2.ToLower();
    //var strDiff2 = Enumerable.Range(0, codes2.Length).Where(i => str2[i] != strUpper2[i]).ToArray();
    var netLetters2 = strUpper2.Zip(str2, (u, l) => new { u, l }).Where(ul => ul.u != ul.l).ToDictionary(ul => Convert.ToInt32(ul.u), ul => Convert.ToInt32(ul.l));
    var allUppers = new String(netLetters2.Select(kv => Convert.ToChar(kv.Key)).ToArray());
    var allLowers = new String(netLetters2.Select(kv => Convert.ToChar(kv.Value)).ToArray());

    var netLetters = new Dictionary<int, int>();
    for (var i = 32; i <= 0xffff; i++) {
      var ch = Convert.ToChar(i);
      if (!char.IsUpper(ch)) continue;
      var lowi = Convert.ToInt32(char.ToLower(ch));
      if (i == lowi) continue;
      netLetters.Add(i, lowi);
    }

    var ibmLetters = File.ReadAllLines(LangsDesignDirs.otherappdata + "caseFolding.txt")
      .Select(t => t.Split(new char[] { '\t' }, 3))
      .ToDictionary(
      parts => int.Parse(parts[0], System.Globalization.NumberStyles.HexNumber),
      parts => int.Parse(parts[1], System.Globalization.NumberStyles.HexNumber)
    );

    File.WriteAllText(@"C:\rewise\design\langsDesign\other\foldingNET.cs", GenerateCode(netLetters, "NET"));
    File.WriteAllText(@"C:\rewise\design\langsDesign\other\foldingIBM.cs", GenerateCode(netLetters, "IBM"));
    File.WriteAllText(@"C:\rewise\design\langsDesign\other\foldingNET2.cs", GenerateCode(netLetters2, "NET2"));
    File.WriteAllText(@"C:\rewise\dlibs\utils\lib\src\langs\netToLower.dart", GenerateCode(netLetters2, null, true));
    File.WriteAllText(@"C:\rewise\dlibs\utils\lib\src\langs\netToLower2.dart", GenerateCode(netLetters2, null, null));

  }

  public static string GenerateCode(Dictionary<int, int> pairs, string name, bool? isDart = false) {

    if (isDart == null) {
      var maskDart = @"
import 'dart:collection';

final _p1 = HashMap.of(
 const<int, int>{{{0}}}
);
int netToLowerChar2(int ch) => _p1[ch] ?? ch;";
      return string.Format(maskDart, pairs.Select(kv => string.Format("{0}:{1}", kv.Key, kv.Value)).JoinStrings(","));
    }


    var parsed = pairs
      .Select(kv => new Ints { up = kv.Key, upLast = kv.Key, low = kv.Value, diff = kv.Value - kv.Key })
      .ToArray();
    var ints = new List<Ints>();
    var last = parsed[0];
    ints.Add(last);
    foreach (var p in parsed.Skip(1)) {
      if (last.hasNext(p))
        last.upLast = p.up;
      else {
        last = p;
        ints.Add(last);
      }
    }
    var more = ints.Where(i => i.upLast > i.up).ToArray();

    var moreG = more.GroupBy(m => m.diff).ToArray();
    var morsSum = more.Sum(i => i.upLast - i.up + 1);
    var plus1 = ints.Where(i => i.upLast == i.up && i.diff == 1).ToArray();
    var plus1Sum = plus1.Count();
    var other = ints.Where(i => !(i.upLast > i.up) && !(i.upLast == i.up && i.diff == 1)).ToArray();
    var otherSum = other.Count();
    if (morsSum + plus1Sum + otherSum != parsed.Length)
      throw new Exception();

    // generace
    var mask = Tuple.Create(@"
using System;
using System.Collections.Generic;

public static class {3}Folding {{
  public static int toLower(UInt16 ch) {{
{0}
    if (p1.Contains(ch)) return ch + 1;
    switch (ch) {{
{1}
      default: return ch;
    }}
  }}
  static HashSet<UInt16> p1 = new HashSet<UInt16>() {{ {2} }};
}}
", @"
import 'dart:collection';
final _p1 = HashSet<int>.from([ {2} ]);

int netToLowerChar(int ch) {{
{0}
    if (_p1.contains(ch)) return ch + 1;
    switch (ch) {{
{1}
      default: return ch;
    }}
}}
");
    var maskMore = Tuple.Create("    if (ch >= {0} && ch <= {1}) return ch + {2};", "    if (ch >= {0} && ch <= {1}) return ch + {2};");
    var maskMoreG = "    if ({0}) return ch + {1};";
    var maskMoreGitem = "ch >= {0} && ch <= {1}";
    var maskOther = Tuple.Create("      case {0}: return {1};", "      case {0}: return {1};");

    var genMore = more.Select(m => string.Format(isDart==true ? maskMore.Item2 : maskMore.Item1, m.up, m.upLast, m.diff)).JoinStrings("\n");
    var genMoreG = moreG.Select(mg => string.Format(
        maskMoreG,
        mg.Select(m => string.Format(maskMoreGitem, m.up, m.upLast)).JoinStrings(" || "),
        mg.Key)).JoinStrings("\n");
    var genPlus1 = plus1.Select(m => m.up.ToString()).JoinStrings(", ");
    var genOther = other.Select(m => string.Format(isDart==true ? maskOther.Item2 : maskOther.Item1, m.up, m.low)).JoinStrings("\n");

    return string.Format(isDart==true ? mask.Item2 : mask.Item1, genMoreG, genOther, genPlus1, name);
  }

  public class Ints {
    public bool hasNext(Ints n) {
      return n.upLast == upLast + 1 && diff == n.diff;
    }
    public int up;
    public int upLast;
    public int low;
    public int diff;
  }
}

