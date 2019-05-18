using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF;

public static class WiktToSQL {

  public static void runs() {
    foreach (var lang in WiktQueries.allLangs) run(lang);
  }

  public static void run(string lang) {
    var dir = Corpus.Dirs.wiktDbnary + @"toDB\" + lang;
    var infos = WiktReplaceUrlByIds.run(lang);
    File.WriteAllLines(dir + @"\ep.txt", 
      infos.OfType<WiktReplaceUrlByIds.InfoProp>().Where(inf => inf.subjType == "e" && inf.propId == "ep").Select(inf => inf.value).OrderBy(s => s).ToHashSet());
    File.WriteAllLines(dir + @"\el.txt",
      infos.OfType<WiktReplaceUrlByIds.InfoProp>().Where(inf => inf.subjType == "e" && inf.propId == "el").Select(inf => inf.value).OrderBy(s => s).ToHashSet());
  }

}
