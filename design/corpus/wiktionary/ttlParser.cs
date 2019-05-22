using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static WiktSchema;

// design time extension
public static class WiktTtlParser {

  public class Context {
    public Context(string l, IEnumerable<KeyValuePair<string, string>> ns) {
      iso1Lang = l; lang = Iso1_3.convert(l);
      namespaces = ns.Where(kv => kv.Value.EndsWith("#")).ToDictionary(kv => kv.Value.TrimEnd('#'), kv => kv.Key);
      prefixes = ns.Where(kv => !kv.Value.EndsWith("#")).ToDictionary(kv => kv.Value, kv => kv.Key);
      dataPrefix = $"/dbnary/{lang}/";
    }
    public string iso1Lang;
    public string lang;
    public Dictionary<string, WiktModel.Helper> dirs = new Dictionary<string, WiktModel.Helper>();
    //public Dictionary<string, Action<ParsedItem>> setBlankValue = new Dictionary<string, Action<ParsedItem>>();
    public SchemePath decodePath(Uri uri) {
      if (uri.Host == "kaiko.getalp.org" && uri.LocalPath.StartsWith(dataPrefix)) {
        return new SchemePath { Scheme = lang, Path = uri.LocalPath.Substring(dataPrefix.Length) };
      } else {
        var parts = uri.OriginalString.Split('#');
        if (parts.Length == 2) {
          return new SchemePath { Scheme = namespaces[parts[0]], Path = parts[1] };
        } else {
          var r = prefixes.First(kv => uri.OriginalString.StartsWith(kv.Key));
          return new SchemePath { Scheme = r.Value, Path = uri.OriginalString.Substring(r.Key.Length) };
        }
      }
      
    }
    Dictionary<string, string> namespaces;
    Dictionary<string, string> prefixes;
    string dataPrefix;

    public int devWrongClassNames;
  }
  public struct SchemePath { public string Scheme; public string Path; }

  public static IEnumerable<TtlFile> ttlFiles() => WiktQueries.allLangs.Select(lang => new TtlFile {
    lang = lang,
    files = ttlFileParts.Select(fp => $"{ttlRoot}{lang}\\{lang}_dbnary_{fp}.ttl").ToArray()
  });
  const string ttlRoot = @"c:\Users\pavel\graphdb-import\dbnary\";
  static string[] ttlFileParts = new[] { "ontolex", "morpho" };
  public class TtlFile { public string lang; public string[] files; }

  public static void parseTtls() {
    Parallel.ForEach(ttlFiles().Where(f => File.Exists(f.files[0])), new ParallelOptions { MaxDegreeOfParallelism = 4 }, f => {
      //foreach (var f in ttlFiles().Where(f => File.Exists(f.files[0]))) {
      var ctx = new Context(f.lang, WiktSchema.Namespaces);
      Console.WriteLine($"{ctx.lang}: START");
      foreach (var fn in f.files) {
        VDS.LM.Parser.parse(fn, (t, c) => {
          var pt = new ParsedTriple(ctx, t);
          if (pt.subjClassId != null) {
            WiktToSQL.adjustNode(pt.propType ? pt.classType : null, pt.subjClassId, ctx);
          }
          //if (pt.subject.blankId != null) {
          //} else if (pt.subject.dataId != null) {
          //} else throw new NotImplementedException();
        });
      }
      Console.WriteLine($"{ctx.lang}: {ctx.devWrongClassNames}");
    });

  }
}
