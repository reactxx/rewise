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
    var frekventDirDest = Root.words;
    var allLangs = LangsLib.Metas.Items.Values.Where(it => it.StemmerClass != null).Select(it => it.lc).ToArray();
    Parallel.ForEach(allLangs, lc => {
      List<string> k64 = new List<string>();
      foreach (var wl in wordLists) {
        var source = Root.root + wl + lc.Parent.Name + ".txt";
        if (!File.Exists(source)) return;
        foreach (var word in File.ReadAllLines(source)) {
          if (fulltext.Stemming.isStemmedWord(word, lc)) {
            k64.Add(word);
            if (k64.Count > 0xffff)
              break;
          }
        }
        if (k64.Count > 0xffff)
          break;
      }
      if (k64.Count > 0)
        File.WriteAllLines(Root.words + lc.Name + ".txt", k64, EncodingEx.UTF8);
    });
  }

}

