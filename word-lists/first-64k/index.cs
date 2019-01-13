using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Text;

public static class First_64k {

  public static string[] wordLists = new string[] {
      @"word-lists\frekvent\appdata\words\",
      @"hunspell\hunspell\appdata\words\",
    };

  public static class Root {
    public static string root = AppDomain.CurrentDomain.BaseDirectory[0] + @":\rewise\";
    public static string words = root + @"word-lists\first-64k\appdata\words\";
    public static fulltext.WordList wordLists = new fulltext.WordList {
      firstIs64k = true,
      items = new string[] {
        @"word-lists\first-64k\appdata\words\",
        @"word-lists\frekvent\appdata\words\",
        @"hunspell\hunspell\appdata\words\",
      }
    };
    static fulltext.WordList sourceLists = new fulltext.WordList {
      firstIs64k = false,
      items = new string[] {
        @"word-lists\frekvent\appdata\words\",
        @"hunspell\hunspell\appdata\words\",
      }
    };
  }

  public static void run() {
    Console.WriteLine("First_64k");
    var frekventDirDest = Root.words;
    var allLangs = LangsLib.Metas.Items.Values.Where(it => it.StemmerClass != null).Select(it => it.lc)/*.Where(lc => lc.Name=="cs-CZ")*/.ToArray();
    using (var imp = new Impersonator.Impersonator("pavel", "LANGMaster", "zvahov88_")) {
      Parallel.ForEach(allLangs, lc => {
        List<string> k64 = new List<string>();
        foreach (var wl in wordLists) {
          var source = Root.root + wl + lc.Name + ".txt";
          if (!File.Exists(source)) return;
          var words = File.ReadAllLines(source);
          foreach (var intv in Intervals.intervals(words.Length, 10)) {
            fulltext.Stemming.addStemmableWords(words, intv.start, intv.take, lc, k64);
            if (k64.Count > 0xffff)
              break;
          }
          if (k64.Count > 0xffff)
            break;
        }
        if (k64.Count > 0)
          File.WriteAllLines(Root.words + lc.Name + ".txt", k64, EncodingEx.UTF8);
      });
    }
  }

}

