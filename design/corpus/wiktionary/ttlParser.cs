using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static WiktSchema;

// design time extension
public static class WiktTtlParser {

  public class Context {
    public Context(string lang) { iso1Lang = lang;  this.lang = Iso1_3.convert(lang); }
    public string iso1Lang;
    public string lang;
    public Dictionary<string, WiktModel.Helper> dirs = new Dictionary<string, WiktModel.Helper>();
    //public Dictionary<string, Action<ParsedItem>> setBlankValue = new Dictionary<string, Action<ParsedItem>>();
  }

  public static IEnumerable<TtlFile> ttlFiles() => WiktQueries.allLangs.Select(lang => new TtlFile {
    lang = lang,
    files = ttlFileParts.Select(fp => $"{ttlRoot}{lang}\\{lang}_dbnary_{fp}.ttl").ToArray()
  });
  const string ttlRoot = @"c:\Users\pavel\graphdb-import\dbnary\";
  static string[] ttlFileParts = new[] { "ontolex", "morpho" };
  public class TtlFile { public string lang; public string[] files; }

  public static void parseTtls() {
    var namespaces = WiktSchema.Namespaces.ToDictionary(kv => kv.Key, kv => kv.Key + ":");
    Parallel.ForEach(ttlFiles().Where(f => File.Exists(f.files[0])), new ParallelOptions { MaxDegreeOfParallelism = 4 }, f => {
      //foreach (var f in ttlFiles().Where(f => File.Exists(f.files[0]))) {

      var ctx = new Context(f.lang);
      foreach (var fn in f.files) {
        VDS.LM.Parser.parse(fn, (t, c) => {
          var pt = new ParsedTriple(ctx, t);
          if (pt.subjClassId != null) {
            WiktToSQL.adjustNode(pt.propType ? pt.classType : null, pt.subjClassId, ctx);
          }
          //if (pt.subject.blankId != null) {
          //} else if (pt.subject.dataId != null) {
          //} else throw new NotImplementedException();
        }, namespaces.Keys.Concat(Linq.Items(Iso1_3.convert(f.lang))));
      }
    });

  }
}
