using System;
using System.Collections.Generic;
using System.Linq;
using Hunspell = Lucene.Net.Analysis.Hunspell;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace fulltext
{
  public static class vlq
  {

    public static void init()
    {
      //var data = file("cs_cz");
      //var data = file("de");
      //var data = file("br_FR");
      foreach (var data in files())
      {
        var encod = encoding.getEncoding(data.Item2);
        var lines = File.ReadAllLines(data.Item1, encod).Skip(1).Where(l => !string.IsNullOrEmpty(l) && char.IsLetter(l[0])).Select(l => l.Split('/')[0]).ToArray();
        var wordsFn = rootDir(@"dictionariesWordLists\") + data.Item3 + ".txt";
        File.WriteAllLines(wordsFn, lines);
        continue;
        using (var dic = File.OpenRead(data.Item1))
        using (var aff = File.OpenRead(data.Item2))
        {
          try
          {
            Hunspell.Dictionary dict = new Hunspell.Dictionary(aff, dic);
            Hunspell.Stemmer stemmer = new Hunspell.Stemmer(dict);
            foreach (var w in lines)
            {
              var stems = stemmer.Stem("Aerosolteilchenkonzentration");
              if (stems == null) continue;
              var stemsStr = stems.Select(s => new String(s.Chars));
            }
          }
          catch //(Exception exp)
          {
            Console.WriteLine(data.Item1);
            //throw new Exception(data.Item1, exp);
          }
        }
      }
      Console.WriteLine("DONE");
      Console.ReadKey();
    }
    static string rootDir(string subDir = @"dictionaries\")
    {
      return Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\" + (subDir ?? ""));
    }

    static Tuple<string, string, string> file(string lang)
    {
      var dir = rootDir() + lang + @"\" + getAlias(lang);
      var dic = dir + ".dic";
      var aff = dir + ".aff";
      if (!File.Exists(dic) || !File.Exists(aff)) throw new Exception(dic);
      return Tuple.Create(dic, aff, lang);
    }

    static IEnumerable<Tuple<string, string, string>> files()
    {
      var dir = rootDir();
      var dirs = Directory.GetDirectories(dir).Select(d => d.Substring(dir.Length)).ToArray();
      foreach (var dirName in dirs)
      {
        var path = dir + dirName + @"\" + getAlias(dirName);
        var dic = path + ".dic";
        var aff = path + ".aff";
        if (!File.Exists(dic) || !File.Exists(aff)) continue;
        yield return Tuple.Create(dic, aff, dirName);
      }
    }

    static Dictionary<string, string> alias = new Dictionary<string, string>() {
      { "de", "de_DE_frami" },
      { "en", "en_GB" },
      { "es", "es_ANY" },
      { "fr_fr", "fr" },
      { "gl", "gl_ES" },
      { "id", "id_ID" },
      { "lt_lt", "lt" },
      { "no", "nb_NO" },
      { "ro", "ro_RO" },
      { "vi", "vi_VN" },
    };
    static string getAlias(string lang)
    {
      string res;
      return alias.TryGetValue(lang.ToLower(), out res) ? res : lang;
    }
  }
}

