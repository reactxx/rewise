using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WiktModel;

public static class WiktToSQL {

  public static void runs() {
    var task = new Task(() => run("en"));
    task.Start();
    Parallel.ForEach(WiktQueries.allLangs.Where(l => l != "en"), new ParallelOptions { MaxDegreeOfParallelism = 3 }, lang => run(lang));
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

    var objs = new Dictionary<string, IObj[]> {
      { "e",  Enumerable.Range(0, infos.idCounts["entry"]).Select(id => new Entry { Id = id + 1 }).ToArray() },
      { "t",  Enumerable.Range(0, infos.idCounts["trans"]).Select(id => new Trans { Id = id + 1 }).ToArray() },
      { "s",  Enumerable.Range(0, infos.idCounts["sense"]).Select(id => new Sense { Id = id + 1 }).ToArray() },
      { "f",  Enumerable.Range(0, infos.idCounts["form"]).Select(id => new form { Id = id + 1 }).ToArray() },
      { "g",  Enumerable.Range(0, infos.idCounts["gloss"]).Select(id => new gloss { Id = id + 1 }).ToArray() },
      { "m",  Enumerable.Range(0, infos.idCounts["multi"]).Select(id => new multi { Id = id + 1 }).ToArray() },
      { "p",  Enumerable.Range(0, infos.idCounts["page"]).Select(id => new Page { Id = id + 1 }).ToArray() },
    };

    foreach (var info in infos.rels) {
      if (info.subjId == 0 || info.objId == 0) continue;
      var left = objs[info.subjType][info.subjId - 1];
      var right = objs[info.objType][info.objId - 1];
      right = null;
    }
    foreach (var info in infos.props) {
      if (info.subjId == 0) continue;
      var left = objs[info.subjType][info.subjId - 1];
    }
    foreach (var className in infos.idCounts.Keys.Where(k => k != "page")) {
      var code = className[0].ToString();
      using (var wr = new JsonStreamWriter(dir + className + ".json")) {
        foreach (var obj in objs[code])
          wr.Serialize(obj);
      }
    }

    objs = null;

    //File.WriteAllLines(dir + @"\ep.txt",
    //  infos.OfType<WiktReplaceUrlByIds.InfoProp>().Where(inf => inf.subjType == "e" && inf.propId == "ep").Select(inf => inf.value).OrderBy(s => s).ToHashSet());
    //File.WriteAllLines(dir + @"\el.txt",
    //  infos.OfType<WiktReplaceUrlByIds.InfoProp>().Where(inf => inf.subjType == "e" && inf.propId == "el").Select(inf => inf.value).OrderBy(s => s).ToHashSet());
  }

  class form: IObj {
    public int Id { get; set; }
    public string Note;
    public string WrittenRep;
    public string PhoneticRep;
  }
  class gloss: IObj {
    public int Id { get; set; }
    public int Rank;
    public int SenseNumber;
    public string Value;
  }
  class multi: IObj {
    public int Id { get; set; }
  }
}


