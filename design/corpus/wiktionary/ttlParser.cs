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

    internal void addError(string v, string originalString) {
      if (errors.Count > 200) return;
      errors.Add($"** {v} in {originalString}");
    }

    public string iso1Lang;
    public string lang;
    public Dictionary<string, WiktModel.Helper> idToObj = new Dictionary<string, WiktModel.Helper>();
    //public Dictionary<string, Action<ParsedItem>> setBlankValue = new Dictionary<string, Action<ParsedItem>>();
    public TripleItem decodePath(Uri uri) {
      var dbnary = uri.Host == "kaiko.getalp.org";
      if (dbnary && uri.LocalPath.StartsWith(dataPrefix)) {
        return new TripleItem { Scheme = lang, Path = uri.LocalPath.Substring(dataPrefix.Length) };
      } else {
        var parts = uri.OriginalString.Split('#');
        if (parts.Length == 2) {
          return new TripleItem { Scheme = namespaces[parts[0]], Path = parts[1] };
        } else if (!dbnary) {
          var r = prefixes.First(kv => uri.OriginalString.StartsWith(kv.Key));
          return new TripleItem { Scheme = r.Value, Path = uri.OriginalString.Substring(r.Key.Length) };
        } else {
          addError("decodePath", uri.OriginalString);
          return new TripleItem { Scheme = "", Path = uri.OriginalString };
        }
      }

    }
    Dictionary<string, string> namespaces;
    Dictionary<string, string> prefixes;
    string dataPrefix;

    public List<string> errors = new List<string>();
  }

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
          if (pt.subjDataId != null) {
            var node = WiktToSQL.adjustNode(pt.propType ? pt.classType : null, pt.subjDataId, ctx);
            if (node!=null && !pt.propType) node.acceptProp(pt, ctx);
          } else if (pt.subjBlankId != null) {

          }
        });
        if (ctx.errors.Count > 6) File.WriteAllLines(LowUtilsDirs.logs + Path.GetFileName(fn) + ".err", ctx.errors);
        ctx.errors.Clear();
      }
      Console.WriteLine($"{ctx.lang}: END");
    });
    Console.WriteLine("Done...");
    Console.ReadKey();
  }
}
