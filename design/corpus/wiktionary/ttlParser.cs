using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static WiktSchema;

// design time extension
public static class WiktTtlParser {

  public class Context {
    public Context(string l, bool firstRun, IEnumerable<KeyValuePair<string, string>> ns) {
      this.firstRun = firstRun;
      iso1Lang = l; lang = Iso1_3.convert(l);
      namespaces = ns.Where(kv => kv.Value.EndsWith("#")).ToDictionary(kv => kv.Value.TrimEnd('#'), kv => kv.Key);
      prefixes = ns.Where(kv => !kv.Value.EndsWith("#")).ToDictionary(kv => kv.Value, kv => kv.Key);
    }

    internal void addError(string v, string originalString) {
      if (!firstRun || errors.Count > 1000) return;
      errors.Add($"** {v} in {originalString}");
    }
    public bool firstRun;
    public string iso1Lang;
    public string lang;
    public Dictionary<string, WiktModel.Helper> idToObj = new Dictionary<string, WiktModel.Helper>();

    public TripleItem decodePath(Uri uri) {
      var dbnary = uri.Host == "kaiko.getalp.org";
      var dataPrefix = $"/dbnary/{lang}/";

      if (dbnary && uri.LocalPath.StartsWith(dataPrefix, StringComparison.Ordinal)) {
        return new TripleItem { Scheme = lang, Path = uri.LocalPath.Substring(dataPrefix.Length) };
      } else {
        var parts = uri.OriginalString.Split('#');
        if (parts.Length == 2) {
          return new TripleItem { Scheme = namespaces[parts[0]], Path = parts[1] };
        } else if (!dbnary) {
          var r = prefixes.First(kv => uri.OriginalString.StartsWith(kv.Key, StringComparison.Ordinal));
          return new TripleItem { Scheme = r.Value, Path = uri.OriginalString.Substring(r.Key.Length) };
        } else {
          addError("decodePath", uri.OriginalString);
          return new TripleItem { Scheme = "", Path = uri.OriginalString };
        }
      }
    }

    Dictionary<string, string> namespaces;
    Dictionary<string, string> prefixes;

    public Dictionary<string, string> blankValues = new Dictionary<string, string>();

    public HashSet<string> errors = new HashSet<string>();
  }

  public static IEnumerable<TtlFile> ttlFiles() => WiktConsts.AllLangs.Where(l => true).Select(lang => new TtlFile {
    lang = lang,
    files = ttlFileParts.Select(fp => $"{ttlRoot}{lang}\\{lang}_dbnary_{fp}.ttl").ToArray()
  });
  const string ttlRoot = @"c:\Users\pavel\graphdb-import\dbnary\";
  static string[] ttlFileParts = new[] { "ontolex", "morpho" };
  public class TtlFile { public string lang; public string[] files; }

  public static void parseTtlsFirstRun() {

    Parallel.ForEach(ttlFiles().Where(f => File.Exists(f.files[0])), new ParallelOptions { MaxDegreeOfParallelism = 4 }, f => {
      var ctx = new Context(f.lang, true, WiktConsts.Namespaces);
      Console.WriteLine($"{ctx.lang}: START");
      foreach (var fn in f.files) {
        VDS.LM.Parser.parse(fn, (t, c) => {
          var pt = ParsedTriple.firstRun(ctx, t);
          if (pt == null)
            return;
          var err = WiktIdManager.registerDataId(f.lang, pt.objDataType, pt.subjDataId);
          if (err != null) ctx.addError(err, pt.subjDataId);
        });
        if (ctx.errors.Count > 1) File.WriteAllLines(LowUtilsDirs.logs + Path.GetFileName(fn) + ".1err", ctx.errors);
        ctx.errors.Clear();
      }
      Console.WriteLine($"{ctx.lang}: END");
    });
    // save IDs to disk
    WiktIdManager.designSaveDataIds();

    Console.WriteLine("Done...");
    Console.ReadKey();
  }

  public static void parseTtlsSecondRun() {

    WiktIdManager.designLoadDataIds();

    var dumpAllProps = new Dictionary<string, dynamic[]>();
    Parallel.ForEach(ttlFiles().Where(f => File.Exists(f.files[0])), new ParallelOptions { MaxDegreeOfParallelism = 4 }, f => {
      var ctx = new Context(f.lang, false, WiktConsts.Namespaces);
      Console.WriteLine($"{ctx.lang}: START");
      foreach (var fn in f.files) {
        VDS.LM.Parser.parse(fn, (t, c) => {
          var pt = new ParsedTriple(ctx, t);
          if (pt.predType == WiktConsts.PredicateType.Ignore)
            return;
          if (pt.subjDataId != null) {
            if (pt.objBlankId != null) {
              if (!ctx.blankValues.TryGetValue(pt.objBlankId, out string value)) ctx.addError("blank obj", pt.subjBlankId);
              else { ctx.blankValues.Remove(pt.objBlankId); pt.objBlankId = null; pt.objValue = value; }
            }
            var node = WiktIdManager.designGetObj(pt.subjDataId);// (f.lang, pt.predType == WiktConsts.PredicateType.a ? pt.objDataType : null, pt.subjDataId, ctx);
            if (node != null && pt.predType != WiktConsts.PredicateType.a && pt.predType != WiktConsts.PredicateType.no) {
              node.acceptProp(pt, ctx);
            }
          } else if (pt.subjBlankId != null) {
            if (pt.objValue == null) ctx.addError("blank subj", pt.subjBlankId);
            ctx.blankValues[pt.subjBlankId] = pt.objValue;
          }
        });
        if (ctx.errors.Count > 1) File.WriteAllLines(LowUtilsDirs.logs + Path.GetFileName(fn) + ".err", ctx.errors);
        ctx.errors.Clear();
      }
      Console.WriteLine($"{ctx.lang}: END");
    });
    WiktIdManager.designSaveData();
    File.WriteAllLines(LowUtilsDirs.logs + "dumpAllProps-Count.txt", dumpAllProps.OrderBy(kv => kv.Value[0]).ThenByDescending(kv => kv.Value[1]).Select(kv => kv.Key + "  #" + (int)kv.Value[1]));
    File.WriteAllLines(LowUtilsDirs.logs + "dumpAllProps-PropName.txt", dumpAllProps.OrderBy(kv => kv.Key).Select(kv => kv.Key + "  #" + (int)kv.Value[1]));

    Console.WriteLine("Done...");
    Console.ReadKey();
  }

}
