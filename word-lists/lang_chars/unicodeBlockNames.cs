using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml.Linq;

public static class Unicode {

  // ********** CULTURE INFO TEXTS
  public static void getCultureInfoTexts() {

    var diffLc = CultureInfo.GetCultures(CultureTypes.SpecificCultures).
      Select(lc => new { lc, text = getCultureInfoText(lc) }).
      GroupBy(lt => lt.text).
      ToDictionary(ltg => ltg.Key, ltg => ltg.Select(l => l.lc.Name).OrderBy(s => s).Aggregate((r, i) => r + " " + i));
    var diffXml = new XElement("root",
      diffLc.OrderBy(kv => kv.Value).Select(kv => new XElement("culture",
      new XElement("text", kv.Key),
      new XElement("lcids", kv.Value)
      ))
      );
    diffXml.Save(Root.unicode + "cultureInfoTexts.xml");

  }

  static string getCultureInfoText(CultureInfo lc) {
    var texts = new List<string>();
    //texts.Add(lc.NativeName);
    var fmt = lc.DateTimeFormat;
    if (fmt == null) return "";
    if (fmt.AbbreviatedDayNames != null) texts.AddRange(fmt.AbbreviatedDayNames);
    if (fmt.AbbreviatedMonthGenitiveNames != null) texts.AddRange(fmt.AbbreviatedMonthGenitiveNames);
    if (fmt.AbbreviatedMonthNames != null) texts.AddRange(fmt.AbbreviatedMonthNames);
    if (fmt.DayNames != null) texts.AddRange(fmt.DayNames);
    if (fmt.MonthGenitiveNames != null) texts.AddRange(fmt.MonthGenitiveNames);
    if (fmt.MonthNames != null) texts.AddRange(fmt.MonthNames);
    //if (fmt.NativeCalendarName != null) texts.Add(fmt.NativeCalendarName);
    if (fmt.ShortestDayNames != null) texts.AddRange(fmt.ShortestDayNames);
    return texts.Where(t => t != null).Distinct().OrderBy(s => s).DefaultIfEmpty().Aggregate((r, i) => r + " " + i);
  }

  // ********** GET UNICODE CHAR BLOCK NAME

  // parse https://www.unicode.org/Public/11.0.0/ucd/Scripts.txt
  public static void parseUnicodeSripts() {
    // IGNORES:
    var aliases = File.ReadAllLines(Root.unicode + "PropertyValueAliases.txt").
      Where(l => !string.IsNullOrEmpty(l) && l[0] != '#').
      Select(l => l.Split(';').Select(w => w.Trim()).ToArray()).Where(arr => arr[0] == "sc").ToDictionary(arr => arr[1], arr => arr[2]);
    // IGNORES:
    var extensions = File.ReadAllLines(Root.unicode + "ScriptExtensions.txt").
      Where(l => !string.IsNullOrEmpty(l) && l[0] != '#').
      Select(l => {
        var parts = l.Split(';').Select(w => w.Trim()).ToArray();
        var second = parts[1].Split('#');
        if (second[1].Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries)[0][0] != 'L') return null;
        return UncRange.fromString(parts[0], second[0].Split(' '));
      }).
      Where(r => r != null).
      ToArray();
    // USED:
    var scripts = File.ReadAllLines(Root.unicode + "Scripts.txt").
      Where(l => !string.IsNullOrEmpty(l) && l[0] != '#').
      Select(l => {
        // e.g.
        //0041..005A    ; Latin # L&  [26] LATIN CAPITAL LETTER A..LATIN CAPITAL LETTER Z
        var parts = l.Split(';').Select(w => w.Trim()).ToArray();
        var second = parts[1].Split('#');
        if (second[1].Trim().Split(' ')[0][0] != 'L') return null; // L... letter
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
    var fn = LangsLib.Root.unicodeBlockNames;
    if (File.Exists(fn)) File.Delete(fn);
    var blocks = new UncBlocks {
      ranges = scripts,
      blockNames = aliasIdxs.OrderBy(kv => kv.Value).Select(kv => kv.Key).ToArray()
    };
    var ser = new XmlSerializer(typeof(UncBlocks));
    using (var fs = File.OpenWrite(fn))
      ser.Serialize(fs, blocks);
  }

  // check diff among own and .net letter test
  public static void getNetUncLettersDiff() {

    var l = LangsLib.UnicodeBlockNames.sorted;

    // all letters in .NET and UNC
    var netLetters = Enumerable.Range(0, 0xffff).Where(i => char.IsLetter(Convert.ToChar(i))).ToArray();
    var uncLetters = Enumerable.Range(0, 0xffff).Where(i => LangsLib.UnicodeBlockNames.isLetter(Convert.ToChar(i))).ToArray();
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
    public UncRange[] ranges;
  }

}