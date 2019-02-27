using System;
using System.Collections.Generic;
using System.Linq;

public static class UnicodeBlocksDirs {
  public static string root = LowUtilsDirs.root + @"unicode\";
  public static string res = LowUtilsDirs.res + "unicode.";
  public static string dirUnicodeBlocks = root + "unicodeBlocks.json";
  public static string resUnicodeBlocks = res + "unicodeBlocks.json";

}
public static class UnicodeBlocks {

  static UnicodeBlocks() {
    var scripts = Json.DeserializeAssembly<UncBlocks>(UnicodeBlocksDirs.resUnicodeBlocks);
    sorted = new SortedList<UncRange, UncRange>(scripts.ranges.ToDictionary(r => r, r => r, RangeComparer.equalityComparer), RangeComparer.comparer);
    blockNames = scripts.blockNames;
    ISO15924 = scripts.ISO15924;
  }
  public static SortedList<UncRange, UncRange> sorted;
  // unicode block names, see word-lists\lang_chars\unicodeBlockNames.cs, https://www.unicode.org/Public/11.0.0/ucd/Scripts.txt and https://unicode.org/Public/UNIDATA/PropertyValueAliases.txt
  public static string[] blockNames;
  public static string[] ISO15924;

  public static bool isLetter(char ch) {
    forSearch.start = forSearch.end = Convert.ToUInt16(ch);
    return sorted.IndexOfKey(forSearch) >= 0;
  }

  public static string filterChars(string data) {
    if (data == null) return null;
    return new string(data.Where(ch => isLetter(ch)).ToArray());
  }

  public static IEnumerable<int> blockIdxs(string str) {
    var res = new HashSet<int>();
    foreach (var ch in str) {
      forSearch.start = forSearch.end = Convert.ToUInt16(ch);
      if (!sorted.TryGetValue(forSearch, out UncRange found)) continue;
      res.Add(found.idx);
    }
    return res;
  }

  public static Dictionary<string, HashSet<char>> getBlockNames(string str) {
    return getBlockNames(Linq.Items(str));
  }

  public static Dictionary<string, HashSet<char>> getBlockNames(IEnumerable<string> texts) {
    var res = new Dictionary<string, HashSet<char>>();
    foreach (var str in texts)
      if (str != null) foreach (var ch in str) {
          forSearch.start = forSearch.end = Convert.ToUInt16(ch);
          if (!sorted.TryGetValue(forSearch, out UncRange found)) continue;
          var name = ISO15924[found.idx];
          if (!res.TryGetValue(name, out HashSet<char> hs))
            res[name] = hs = new HashSet<char>();
          hs.Add(ch);
        }
    return res;
  }

  public static Dictionary<string, string> checkBlockNames(IEnumerable<string> texts, string script) {
    if (texts == null) return null;
    var res = new Dictionary<string, HashSet<char>>();
    foreach (var str in texts)
      if (str != null)
        foreach (var ch in str) {
          forSearch.start = forSearch.end = Convert.ToUInt16(ch);
          if (!sorted.TryGetValue(forSearch, out UncRange found)) continue;
          var name = ISO15924[found.idx];
          if (script == "Jpan") {
            if (name == "Hani" || name == "Hira" || name == "Kana") continue;
          } else if (script == "Kore") {
            if (name == "Hani" || name == "Hang") continue;
          } else if (script == "Hant" || script == "Hans") {
            if (name == "Hani") continue;
          } else if (name == script)
            continue;
          if (!res.TryGetValue(name, out HashSet<char> hs))
            res[name] = hs = new HashSet<char>();
          hs.Add(ch);
        }
    return res.Count == 0 ? null : res.ToDictionary(b => b.Key, b => new string(b.Value.ToArray()));
  }

  public static Dictionary<string, string> checkBlockNames(string str, string script) {
    return str == null ? null : checkBlockNames(Linq.Items(str), script);
  }

  [ThreadStatic]
  static UncRange forSearch;

  public struct UncRange {
    public ushort start;
    public ushort end;
    public int idx;
  }

  public class UncBlocks {
    public string[] blockNames;
    public string[] ISO15924;
    public UncRange[] ranges;
  }

  public class RangeComparer : IEqualityComparer<UncRange>, IComparer<UncRange> {
    bool IEqualityComparer<UncRange>.Equals(UncRange x, UncRange y) {
      return x.start.Equals(y.start);
    }

    int IEqualityComparer<UncRange>.GetHashCode(UncRange obj) {
      return obj.start.GetHashCode();
    }

    int IComparer<UncRange>.Compare(UncRange x, UncRange y) {
      if (y.start > x.end) return -1;
      if (y.end < x.start) return 1;
      return 0;
    }

    public static IEqualityComparer<UncRange> equalityComparer = new RangeComparer();
    public static IComparer<UncRange> comparer = new RangeComparer();
  }

}

