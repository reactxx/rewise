using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF;

public static class WiktToDB {

  public static void replaceUrlByIds() {
    foreach (var lang in WiktQueries.allLangs) replaceUrlById(lang);
  }

  static Dictionary<string, Dictionary<string, int>> idsToMemory(string dir) {
    var res = new Dictionary<string, Dictionary<string, int>>();
    var idFiles = Directory.GetFiles(dir, "*.ttl").Where(fn => Path.GetFileNameWithoutExtension(fn).ToLower().StartsWith("ids_")).ToArray();
    Parallel.ForEach(idFiles, new ParallelOptions { MaxDegreeOfParallelism = 4 }, fn => {
      var clsName = Path.GetFileNameWithoutExtension(fn).ToLower().Split('_')[1];
      var destFn = Path.ChangeExtension(fn, ".txt");
      Dictionary<string, int> ids;
      //if (File.Exists(destFn)) {
      //  var count = 0;
      //  ids = File.ReadAllLines(destFn).ToDictionary(id => id, id => count++);
      //} else
      using (var wr = new StreamWriter(destFn)) {
        ids = new Dictionary<string, int>();
        VDS.LM.Parser.parse(fn, (trip, count) => {
          var txt = trip.Object as LiteralNode;
          wr.WriteLine(txt.Value);
          ids[txt.Value] = ids.Count;
        });
      }
      lock (res) res[clsName] = ids;
    });
    return res;
  }

  public static void replaceUrlById(string lang) {
    var dir = Corpus.Dirs.wikiesDbnary + @"graphDBExport\" + lang;
    if (!Directory.Exists(dir)) return;
    var files = Directory.GetFiles(dir).Select(fn => {
      var parts = Path.GetFileNameWithoutExtension(fn).ToLower().Split('_');
      var isProp = parts[0] == "prop" ? true : (parts[0] == "rel" ? false : (bool?)null);
      if (isProp == null) return null;
      return new ttlFile { fn = fn, isProp = (bool)isProp, fromCls = parts[1], predicate = parts[2], toCls = isProp == true ? null : parts[3] };
    }).
    Where(t => t != null).
    ToArray();

    var ids = idsToMemory(dir);

    using (var wrProp = new StreamWriter(dir + @"\props.txt"))
    using (var wrRel = new StreamWriter(dir + @"\rels.txt"))
      foreach (var file in files) {
        var wr = file.isProp ? wrProp : wrRel;
        var idsFrom = ids[file.fromCls];
        var idsTo = file.isProp ? null : ids[file.toCls];
        VDS.LM.Parser.parse(file.fn, (trip, count) => {
          wr.Write(' '); wr.Write('|');
          wr.Write(idsFrom[(trip.Subject as UriNode).Uri.LocalPath]); wr.Write('|');
          if (file.isProp) {
            var obj = trip.Object;
            obj = null;
          } else {
          }
        });
      }
  }

  static Dictionary<string, string> clsToName = new Dictionary<string, string> {
    { "translation", "trans" },
    { "gloss", "gloss" },
    { "page", "page" },
    { "lexicalentry", "entry" },
    { "lexicalsense", "sense" },
    { "form", "form" },
    { "multiwordexpression", "multi" },
  };


  class ttlFile {
    public string fn;
    public string fromCls;
    public bool isProp;
    public string predicate;
    public string toCls; // for isProp=false
  }
}
