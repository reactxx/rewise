using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Corpus {

  public class Dirs {
    public static char driver = AppDomain.CurrentDomain.BaseDirectory[0];
    public static string root = driver + @":\rewise\data\01_csv\corpus\";
    public static string frekvent = driver + @":\rewise\data\frekvent\";
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
}