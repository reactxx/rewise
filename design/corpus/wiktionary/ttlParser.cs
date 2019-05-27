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
    }

    internal void addError(string v, string originalString) {
      if (errors.Count > 1000) return;
      errors.Add($"** {v} in {originalString}");
    }

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

  public static void parseTtls() {
    //var dumpForAcceptProp = new Dictionary<string, int[]>();
    var dumpForAcceptProp2 = new Dictionary<string, dynamic[]>();
    var dumpLastIdx = WiktQueries.allLangsIdx.Count;
    Parallel.ForEach(ttlFiles().Where(f => File.Exists(f.files[0])), new ParallelOptions { MaxDegreeOfParallelism = 4 }, f => {
      var ctx = new Context(f.lang, WiktConsts.Namespaces);
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
            var node = adjustNode(f.lang, pt.predType == WiktConsts.PredicateType.a ? pt.objDataType : null, pt.subjDataId, ctx);
            if (node != null && pt.predType != WiktConsts.PredicateType.a && pt.predType != WiktConsts.PredicateType.no) {
              //lock (dumpForAcceptProp) pt.dumpForAcceptProp(node.GetType().Name, f.lang, dumpForAcceptProp);
              lock (dumpForAcceptProp2) pt.dumpForAcceptProp2(node.GetType().Name, f.lang, dumpForAcceptProp2);
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
      // write to BSON
      var dir = Corpus.Dirs.wiktDbnary + @"toDB\" + f.lang;
      if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
      foreach (var className in new[] { "" })
        Console.WriteLine($"{ctx.lang}: END");
    });
    // save IDS to disk
    WiktIdManager.designSaveDataIds();

    //File.WriteAllLines(LowUtilsDirs.logs + "dumpForAcceptProp.txt", dumpForAcceptProp.
    //  Where(s => s.Value[dumpLastIdx] >= 1000).
    //  OrderBy(s => s.Key).Select(kv => {
    //    var ls = string.Join(",", kv.Value.
    //      Where((i, idx) => idx != dumpLastIdx && i > 0).
    //      Select((i, idx) => i.ToString() + WiktConsts.AllLangs[idx]));
    //    return $"{kv.Key} {kv.Value[dumpLastIdx]}x {ls}";
    //  }));
    File.WriteAllLines(LowUtilsDirs.logs + "dumpForAcceptPropCount.txt", dumpForAcceptProp2.OrderBy(kv => kv.Value[0]).ThenByDescending(kv => kv.Value[1]).Select(kv => kv.Key + "  #" + (int)kv.Value[1]));
    File.WriteAllLines(LowUtilsDirs.logs + "dumpForAcceptPropName.txt", dumpForAcceptProp2.OrderBy(kv => kv.Key).Select(kv => kv.Key + "  #" + (int)kv.Value[1]));

    Console.WriteLine("Done...");
    Console.ReadKey();
  }

  public static WiktModel.Helper adjustNode(string lang, string classType, string id, WiktTtlParser.Context ctx) {

    WiktModel.Helper createLow(string tp) {
      switch (tp) {
        case WiktConsts.NodeTypeNames.Gloss: return new WiktToSQL.HelperGloss();
        case WiktConsts.NodeTypeNames.Form: return new WiktToSQL.HelperForm();
        case WiktConsts.NodeTypeNames.LexicalSense: return new WiktModel.Sense();
        case WiktConsts.NodeTypeNames.Page: return new WiktModel.Page();
        case WiktConsts.NodeTypeNames.Translation: return new WiktModel.Translation();
        case WiktConsts.NodeTypeNames.Statement: return new WiktModel.Statement();
        case WiktConsts.NodeTypeNames.LexicalÈntry: return new WiktModel.Entry();
        default: throw new Exception();
      }
    }

    if (WiktIdManager.desingStr2Obj(id, out WiktModel.Helper res)) return res;

    if (classType == null) {
      ctx.addError("adjustNode", id);
      return null;
    }

    return WiktIdManager.designAssignNewId(id, lang, classType, createLow(classType));
  }


}
