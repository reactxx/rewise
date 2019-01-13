using System;
using System.Collections.Generic;
using System.Linq;
using Hunspell = Lucene.Net.Analysis.Hunspell;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Text;

namespace fulltext {

  public static class Root {
    public static string root= AppDomain.CurrentDomain.BaseDirectory[0] + @":\rewise\";
  }

  public static class HunspellLib {

    // ********************** MAIN PROC FOR GETTING WORDS from .DIC
    // extract words from .DIC file and convert it to UTF8
    public static void extractWordLists() {
      var validLangs = LangsLib.Metas.Items.Select(it => it.Value.Id).ToDictionary(it => it, it => true);
      foreach (var data in files()) {
        var id_ = data.Item3.ToLower();
        var id = id_.Replace('_','-');
        if (hunspellAlias.ContainsKey(id_)) id = hunspellAlias[id_].ToLower();
        if (!validLangs.ContainsKey(id)) continue;
        var encod = encoding.getEncoding(data.Item2);
        var lines = File.ReadAllLines(data.Item1, encod).
          Skip(1).
          Where(l => !string.IsNullOrEmpty(l) && char.IsLetter(l[0])).
          Select(l => l.Split('/')[0]).
          ToArray();
        var wordsFn = Root.root + @"hunspell\hunspell\appdata\words\" + id;
        File.WriteAllLines(wordsFn + ".txt", lines, new UTF8Encoding(false));
      }
    }

    // ********************** STEMMER, not used in favour of SqlServer
    // use Hunspell stemmer. I returns or basic word from .DIC or self.
    // for czech, it does not works for some slovesa
    public static void init() {
      //var data = file("cs_cz");
      //var data = file("de");
      //var data = file("br_FR");
      foreach (var data in files()) {
        var encod = encoding.getEncoding(data.Item2);
        var lines = File.ReadAllLines(data.Item1, encod).Skip(1).Where(l => !string.IsNullOrEmpty(l) && char.IsLetter(l[0])).Select(l => l.Split('/')[0]).ToArray();
        using (var dic = File.OpenRead(data.Item1))
        using (var aff = File.OpenRead(data.Item2)) {
          try {
            Hunspell.Dictionary dict = new Hunspell.Dictionary(aff, dic);
            Hunspell.Stemmer stemmer = new Hunspell.Stemmer(dict);
            foreach (var w in lines) {
              var stems = stemmer.Stem(w);
              if (stems == null) continue;
              var stemsStr = stems.Select(s => new String(s.Chars));
            }
          } catch //(Exception exp)
            {
            Console.WriteLine(data.Item1);
            //throw new Exception(data.Item1, exp);
          }
        }
      }
      Console.WriteLine("DONE");
      Console.ReadKey();
    }

    static Tuple<string, string, string> file(string lang) {
      var dir = Root.root + lang + @"\" + getAlias(lang);
      var dic = dir + ".dic";
      var aff = dir + ".aff";
      if (!File.Exists(dic) || !File.Exists(aff)) throw new Exception(dic);
      return Tuple.Create(dic, aff, lang);
    }

    static IEnumerable<Tuple<string, string, string>> files() {
      var dir = Root.root + @"hunspell\hunspell\appdata\sources\";
      var dirs = Directory.GetDirectories(dir).Select(d => d.Substring(dir.Length)).ToArray();
      foreach (var dirName in dirs) {
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
    static string getAlias(string lang) {
      string res;
      return alias.TryGetValue(lang.ToLower(), out res) ? res : lang;
    }

    // ********************  normalizeHunspellLangs
    static Dictionary<string, string> hunspellAlias = new Dictionary<string, string>() {
      // ?? an_es, bn_bd, bs_ba, gd_gb, kmr_latn, lo_la, ne_np, ro, si_lk, sw_tz
      { "ar", "ar-sa" }, // Arabic
      { "bo", "bo-cn" }, // Tibetan
      { "de", "de-de" },
      { "en", "en-gb" },
      { "es", "es-es" },
      { "gl", "gl-es" }, // Galician
      { "gug", "gu-in" }, // Guarani, ??
      { "id", "id-id" }, // indonesia
      { "is", "is-IS" }, // iceland
      { "no", "nb-NO" }, //Norwegian
      { "sr", "sr-Cyrl-CS" }, //Serbian
      { "vi", "vi-VN" },
    };


    // ********************  helper for creating hunspellAlias above.
    public static void normalizeHunspellLangs() {
      var validLangs = LangsLib.Metas.Items.Select(it => it.Value.Id.Replace('-', '_')).ToDictionary(it => it, it => true);
      var files = File.ReadAllLines(@"D:\rewise\fulltext\hunspell\langs.txt").Select(f => f.Split('.')[0].ToLower()).ToArray();
      var OKFiles = files.Where(f => validLangs.ContainsKey(f)).ToArray();
      var WrongFiles = files.Where(f => !validLangs.ContainsKey(f)).ToArray();
      var texts = new List<string>();
      foreach (var fn in WrongFiles) {
        XDocument xdoc = XDocument.Load(Root.root + @"dictionaries\" + fn + @"\description.xml");
        var txt = xdoc.DescendantNodes().Where(n => n.NodeType == XmlNodeType.Text).FirstOrDefault() as XText;
        texts.Add(txt == null ? fn : txt.Value + "  ***" + fn);
      }
      File.WriteAllLines(Root.root + @"fulltext\hunspell\hunspellWrongs.txt", texts);
      File.WriteAllLines(Root.root + @"fulltext\hunspell\allLangs.txt",
        LangsLib.Metas.Items.Select(it => it.Value.lc).Select(lc => lc == null ? "" : string.Format("{0} | {1} | {2} | {3}", lc.Name, lc.DisplayName, lc.EnglishName, lc.NativeName))
      );
    }


  }
}



