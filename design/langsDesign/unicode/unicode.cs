using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

//unicode blocks: https://en.wikipedia.org/wiki/Unicode_block
//.net - 4.0: C:\rewise\design\langsDesign\appdata\unicode\Blocks-net.txt from https://docs.microsoft.com/en-us/dotnet/standard/base-types/character-classes-in-regular-expressions#supported-named-blocks
// 4.0: c:\rewise\design\langsDesign\appdata\unicode\Blocks-4.0.0.txt from ftp://www.unicode.org/Public/4.0-Update/Blocks-4.0.0.txt
//11.0: c:\rewise\design\langsDesign\appdata\unicode\Blocks-11.0.0.txt from ftp://www.unicode.org/Public/11.0.0/ucd/Blocks.txt

public static class UnicodeDesignLib {



  public static void CJK() {
    // info from: https://en.wikipedia.org/wiki/Template:ISO_15924_script_codes_and_related_Unicode_data
    // japan: Jpan = alias for Han + Hiragana + Katakana, https://www.key-shortcut.com/en/writing-systems/%E3%81%B2%E3%82%89%E3%81%8C%E3%81%AA-japanese/
    // korea: Kore = alias for Han + Hangul, https://en.wikipedia.org/wiki/Hangul
    // chinesse: Hans + Hant = alias for Han (Hani)
    // CJK: https://en.wikipedia.org/wiki/CJK_Unified_Ideographs
  }

  // ********** GET UNICODE CHAR BLOCK NAME
  // parse https://www.unicode.org/Public/11.0.0/ucd/Scripts.txt and https://unicode.org/Public/UNIDATA/PropertyValueAliases.txt, sc
  public static void getUnicodeBlockNames() {
    // ALIAS:
    var aliases = File.ReadAllLines(LangsDesignDirs.unicode + "PropertyValueAliases.txt").
      Where(l => !string.IsNullOrEmpty(l) && l[0] != '#').
      Select(l => l.Split(';').Select(w => w.Trim()).ToArray()).Where(arr => arr[0] == "sc").ToDictionary(arr => arr[2], arr => arr[1]);
    // IGNORES:
    //var extensions = File.ReadAllLines(Root.unicode + "ScriptExtensions.txt").
    //  Where(l => !string.IsNullOrEmpty(l) && l[0] != '#').
    //  Select(l => {
    //    var parts = l.Split(';').Select(w => w.Trim()).ToArray();
    //    var second = parts[1].Split('#');
    //    if (second[1].Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries)[0][0] != 'L') return null;
    //    return UncRange.fromString(parts[0], second[0].Split(' '));
    //  }).
    //  Where(r => r != null).
    //  ToArray();
    // USED:
    var scripts = File.ReadAllLines(LangsDesignDirs.unicode + "Scripts.txt").
      Where(l => !string.IsNullOrEmpty(l) && l[0] != '#').
      Select(l => {
        // e.g.
        //0041..005A    ; Latin # L&  [26] LATIN CAPITAL LETTER A..LATIN CAPITAL LETTER Z
        var parts = l.Split(';').Select(w => w.Trim()).ToArray();
        var second = parts[1].Split('#');
        var category = second[1].Trim().Split(' ')[0];
        if (category[0] != 'L' || category == "Lm" /*letter's delimiter*/) return null; // L... letter
        return UncRange.fromString(parts[0], second[0].Trim());
      }).
      Where(r => r != null).
      ToArray().
      OrderBy(s => s.start).
      ToArray();
    // check insreasing seq and concat
    var i = 1;
    var last = scripts[0];
    var aliasIdxs = new Dictionary<string, int>();
    while (i < scripts.Length) {
      var act = scripts[i];
      if (last.end >= act.start)
        throw new Exception();
      if (last.end != act.start - 1 || last.alias != act.alias) {
        last = act;
        if (!aliasIdxs.TryGetValue(last.alias, out int idx)) idx = aliasIdxs[last.alias] = aliasIdxs.Count;
        last.idx = idx;
        last.alias = null;
        i++;
        continue;
      }
      last.end = act.end;
      scripts[i] = null;
      i++;
    }
    scripts[0].alias = null;
    scripts = scripts.Where(s => s != null).ToArray();
    // save
    // num of chars
    var charsNum = scripts.Sum(s => s.end - s.start + 1);
    //string[] bn;
    var blocks = new RewiseDom.UncBlocks();
    blocks.ISO15924.AddRange(aliasIdxs.
      OrderBy(kv => kv.Value).
      Select(kv => kv.Key).
      ToArray().
      Select(b => aliases[b]).
      ToArray()
      );
    blocks.Ranges.AddRange(
      scripts.Select(s => new RewiseDom.UncRange {
        Idx = s.idx, Start = s.start, End = s.end
      })
    );
    File.WriteAllText(UnicodeBlocksDirs.dirUnicodeBlocks, blocks.ToString());
    //Json.Serialize(UnicodeBlocksDirs.dirUnicodeBlocks, blocks);
  }

  // check diff among own and .net letter test
  public static void dumpNetUncLettersDiff() {

    var l = UnicodeBlocks.sorted;

    // all letters in .NET and UNC
    var netLetters = Enumerable.Range(0, 0xffff).Where(i => char.IsLetter(Convert.ToChar(i))).ToArray();
    var uncLetters = Enumerable.Range(0, 0xffff).Where(i => UnicodeBlocks.isLetter(Convert.ToChar(i))).ToArray();
    //var uncLetters = new List<int>();
    //foreach (var r in l) uncLetters.AddRange(Enumerable.Range(r.Key.start, r.Key.end - r.Key.start + 1));
    //var uncLetters = uncCodes.Select(i => Convert.ToChar(i)).ToArray();

    // .NET x UNC diff
    var notNet = uncLetters.Except(netLetters).OrderBy(ch => ch).ToArray(); // 116 missing
    var notUnc = netLetters.Except(uncLetters).OrderBy(ch => ch).ToArray(); // 2 missing
  }

  public class UncRange {
    public static UncRange fromString(string src, string[] aliases) {
      var ints = src.Split(new string[] { ".." }, StringSplitOptions.None).Select(hex => int.Parse(hex, NumberStyles.HexNumber)).ToArray();
      if (ints.Length == 1) ints = new int[] { ints[0], ints[0] };
      if (ints[0] > 0xffff || ints[1] > 0xffff) return null;
      return new UncRange { start = (ushort)ints[0], end = (ushort)ints[1], aliases = aliases };
    }
    public static UncRange fromString(string src, string alias) {
      var ints = src.Split(new string[] { ".." }, StringSplitOptions.None).Select(hex => int.Parse(hex, NumberStyles.HexNumber)).ToArray();
      if (ints[0] == 1600)
        ints[0] = 1600;
      if (ints.Length == 1) ints = new int[] { ints[0], ints[0] };
      if (ints[0] > 0xffff || ints[1] > 0xffff) return null;
      return new UncRange { start = (ushort)ints[0], end = (ushort)ints[1], alias = alias };
    }
    public ushort start;
    public ushort end;
    public string[] aliases;
    public string alias;
    public int idx; // UncBlocks.blockNames[idx] == alias
  }

  public class UncBlocks {
    public string[] blockNames;
    public string[] ISO15924;
    public UncRange[] ranges;
  }

}