using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF;
using WiktModel;

public static class WiktToSQL {

  public static void runs() {
    var task = new Task(() => run("en"));
    task.Start();
    Parallel.ForEach(WiktQueries.allLangs.Where(l => l!="en"), new ParallelOptions { MaxDegreeOfParallelism = 4 }, lang => run(lang));
    task.Wait();
  }

  public static void run(string lang) {
    var dir = Corpus.Dirs.wiktDbnary + @"toDB\" + lang + @"\";
    var infos = WiktReplaceUrlByIds.run(lang);
    // Pages
    using (var wr = new JsonStreamWriter(dir + "page.json")) { 
      var cnt = 1;
      foreach (var line in File.ReadLines(dir + "ids_page.txt"))
        wr.Serialize(new Page { Id = cnt++, Title = line.Replace("_", " ") });
    }
    var entry = Enumerable.Range(0, infos.idCounts["entry"]).Select(id => new Entry { Id = id }).ToArray();
    var trans = Enumerable.Range(0, infos.idCounts["trans"]).Select(id => new Entry { Id = id }).ToArray();
    var sense = Enumerable.Range(0, infos.idCounts["sense"]).Select(id => new Entry { Id = id }).ToArray();
    sense = null;
    // 
    //foreach (var info in infos) {
    //}
    //File.WriteAllLines(dir + @"\ep.txt",
    //  infos.OfType<WiktReplaceUrlByIds.InfoProp>().Where(inf => inf.subjType == "e" && inf.propId == "ep").Select(inf => inf.value).OrderBy(s => s).ToHashSet());
    //File.WriteAllLines(dir + @"\el.txt",
    //  infos.OfType<WiktReplaceUrlByIds.InfoProp>().Where(inf => inf.subjType == "e" && inf.propId == "el").Select(inf => inf.value).OrderBy(s => s).ToHashSet());
  }

}
