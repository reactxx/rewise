using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

public static class CreateDictSource {

  public static void run(string root) {
    var metas = new LangsLib.Metas();
    var hunspellDir = root + @"dictionariesWordLists\";
    var frekventDir = root + @"dicts_full\";
    var dump = new XElement("root");
    foreach (var lc in metas.Items.Values.Where(it => it.StemmerClass != null).Select(it => it.lc)) {
      var frekvent = frekventDir + lc.Parent.Name + "_full.txt";
      var fr = File.Exists(frekvent) ? File.ReadAllLines(frekvent) : null;
      var frCount = fr==null ? 0 : fr.Length;
      if (fr != null) {
        fr = fr
          .Select(w => w.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
          .Where(p => p.Length == 2)
          .Select(p => p[0]) //p[lc.TextInfo.IsRightToLeft ? 1 : 0])
          .ToArray();
        if (frCount - fr.Length > 10)
          throw new Exception("frCount - fr.Length > 10");
      }
      var hunspell = hunspellDir + lc.Name + ".txt";
      var h = File.Exists(hunspell) ? File.ReadAllLines(hunspell) : null;
      dump.Add(new XElement(lc.Name.Replace('-', '_'),
          new XAttribute ("name", lc.EnglishName),
          new XAttribute("id", lc.Name),
          new XAttribute("lcid", lc.LCID),
          new XAttribute("frekvent", fr==null ? 0 : fr.Length),
          new XAttribute("hunspell", h == null ? 0 : h.Length)
        ));

      var all = fr != null && h != null ? fr.Concat(h).ToArray() : fr ?? h;
      if (all != null)
        File.WriteAllLines(root + @"dicts_source\" + lc.Name + ".txt", all);
    }
    dump.Save(root + @"fulltext\sqlserver\create-dict-source.xml");
  }

}