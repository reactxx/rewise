using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

public static class CreateFrekventWords {

  public static class Root {
    public static string root = AppDomain.CurrentDomain.BaseDirectory[0] + @":\rewise\word-lists\frekvent\appdata\";
  }


  public static void run() {
    var frekventDirSource = Root.root + @"source\";
    var frekventDirDest = Root.root + @"words\";
    foreach (var lc in LangsLib.Metas.Items.Values.Where(it => it.StemmerClass != null).Select(it => it.lc)) {
      var frekvent = frekventDirSource + lc.Parent.Name + "_full.txt";
      if (!File.Exists(frekvent)) continue;
      var fr = File.ReadAllLines(frekvent)
          .Select(w => w.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
          .Where(p => p.Length == 2)
          .Select(p => p[0]); //p[lc.TextInfo.IsRightToLeft ? 1 : 0])
      File.WriteAllLines(frekventDirDest + lc.Name + ".txt", fr);
    }
  }

}