using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Corpus {

  public class Dirs {
    public static char driver = AppDomain.CurrentDomain.BaseDirectory[0];
    public static string root = driver + @":\rewise\data\01_csv\corpus\";
    public static string frekvent = driver + @":\rewise\data\frekvent\";
    public static string wikies = driver + @":\rewise\data\wikies\";
  }

  public static class Files {
    public static string fileToLang(string fn) => Langs.oldToNew(Path.GetFileNameWithoutExtension(fn).Split('.')[0]);
    public static IEnumerable<string> files { get => Directory.GetFiles(Dirs.root, "*.*", SearchOption.AllDirectories); }
    public static IEnumerable<string> langs { get => files.Select(f => fileToLang(f)).Distinct(); }
    public static IEnumerable<string> getLangFiles(string lang) => files.Where(f => fileToLang(f).ToLower() == lang.ToLower());
    public static IEnumerable<string> getLangPars(string lang) => getLangFiles(lang).SelectMany(fn => {
      Console.WriteLine(fn);
      var lines = File.ReadLines(fn);
      if (Path.GetExtension(fn) == ".csv") lines = lines.Skip(1).Select(l => l.Split(';')[0]);
      return lines;
    });
    public static IEnumerable<string> getLangWords(string lang) => StemmerBreakerNew.Service.wordBreak(lang, getLangPars(lang));
  }

  public static class CountIntervals {
    static readonly double b = Math.Exp(Math.Log(10) / 10);
    static readonly int[] ints = new int[10];
    static List<int> bounds = new List<int>() { 1, 2, 3, 4, 5, 6, 8 };
    static CountIntervals() {
      ints[0] = 10; double bb = 1;
      for (var i = 1; i < 10; i++) {
        bb = bb * b;
        ints[i] = (int)Math.Round(bb * 10);
      }
      var act = 1;
      while (true) {
        for (var i = 0; i < 10; i++) {
          bounds.Add(act * ints[i]);
          if (bounds.Last() > 10000000) { act = -1; break; }
        }
        if (act < 0) break;
        act = act * 10;
      }
    }
    static int getBoundIdx(int actCount, bool first = false) {
      for (var i = bounds.Count - 1; i >=0; i--)
        if (actCount >= bounds[i]) return i;
      throw new Exception();
    }
    public class BoundStatus {
      public int actIdx;
      public int[] boundsCount;
      public IEnumerable<string> dump() {
        for (var i = boundsCount.Length - 1; i >= 0; i--)
          if (boundsCount[i] > 0) yield return string.Format(">={0}: {1}", bounds[i], boundsCount[i]);
      } 
      
    }
    public static IEnumerable<string> writeBound(string word, int actCount, BoundStatus st) {
      yield return word;
      if (st.boundsCount == null) st.boundsCount = bounds.Select(i => 0).ToArray();
      var idx = getBoundIdx(actCount);
      st.boundsCount[idx]++;
      if (idx == st.actIdx) yield break;
      st.actIdx = idx;
      yield return "#####        >=" + bounds[idx].ToString();
    }
  }
}