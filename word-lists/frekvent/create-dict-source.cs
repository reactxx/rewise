using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;

public static class CreateFrekventWords {

  public static class Root {
    public static string root = AppDomain.CurrentDomain.BaseDirectory[0] + @":\rewise\word-lists\frekvent\appdata\";
  }


  public static void run() {
    var frekventDirSource = Root.root + @"source\";
    var frekventDirDest = Root.root + @"words\";
    var allLangs = LangsLib.Metas.Items.Values.Where(it => it.StemmerClass != null).Select(it => it.lc).ToArray();
    Parallel.ForEach(allLangs, lc => {
      var frekvent = frekventDirSource + lc.Parent.Name + "_full.txt";
      if (!File.Exists(frekvent)) return;

      File.WriteAllLines(frekventDirDest + lc.Name + ".txt", wordBreak(
        File.ReadAllText(frekvent),
        StemmerBreaker.Services.getService((LangsLib.langs)lc.LCID)
      ));

    });
  }

  static IEnumerable<string> wordBreak(string content, StemmerBreaker.Service service) {
    var buff = new List<string>();
    foreach (var chunk in StemmerBreaker.SplitLines.Run(File.ReadAllText(content), 5000)) {
      buff.Clear();
      service.wordBreak(chunk, word => buff.Add(chunk));
      foreach (var w in buff) yield return w;
    }
  }

}

