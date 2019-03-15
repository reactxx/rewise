using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Text;

public static class CreateFrekventWords {

  public static class Root {
    public static string root = AppDomain.CurrentDomain.BaseDirectory[0] + @":\rewise\design\frekventWordList\appdata\";
  }

  // Source from RJ: r:\radek\RWData\FreqLists\
  public static void run() {
    Console.WriteLine("CreateFrekventWords");
    var frekventDirSource = Root.root + @"source\";
    var frekventDirDest = Root.root + @"words\";
    var allLangs = LangsLib.Metas.Items.Values.Where(it => it.stemmerClass != null).Select(it => it.lc).ToArray();
    Parallel.ForEach(allLangs, lc => {
      var frekvent = frekventDirSource + lc.Parent.Name + "_full.txt";
      if (!File.Exists(frekvent)) return;

      File.WriteAllLines(frekventDirDest + lc.Name + ".txt",
        StemmerBreaker.Services.getService(lc.Name).wordBreakLargeWordList(File.ReadAllText(frekvent)/*.Normalize()*/),
        EncodingEx.UTF8
      );

    });
  }

}
