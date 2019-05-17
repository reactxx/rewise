using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF;

public static class WiktToDB {
  public static void replaceUrlByIds() {
    foreach (var lang in WiktQueries.allLangs) replaceUrlById(lang);
  }
  public static void replaceUrlById(string lang) {
    var dir = Corpus.Dirs.wikiesDbnary + @"graphDBExport\" + lang;
    if (!Directory.Exists(dir)) return;
    var idFiles = Directory.GetFiles(dir).Where(fn => Path.GetFileNameWithoutExtension(fn).ToLower().StartsWith("id")).ToArray();
    Parallel.ForEach(idFiles, new ParallelOptions { MaxDegreeOfParallelism = 4 }, fn => {
      var destFn = Path.ChangeExtension(fn, ".txt");
      VDS.LM.Parser.parse(fn, (trip, count) => {
        var txt = trip.Object as LiteralNode;
        var txtLang = txt.Language;
      });
    });
  }
}
