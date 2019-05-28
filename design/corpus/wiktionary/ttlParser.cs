using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static WiktSchema;

// design time extension
public static class WiktTtlParser {

  public static IEnumerable<TtlFile> ttlFiles() => WiktConsts.AllLangs.Where(l => true).Select(lang => new TtlFile {
    lang = lang,
    files = ttlFileParts.Select(fp => $"{ttlRoot}{lang}\\{lang}_dbnary_{fp}.ttl").ToArray()
  });
  const string ttlRoot = @"c:\Users\pavel\graphdb-import\dbnary\";
  static string[] ttlFileParts = new[] { "ontolex", "morpho" };
  public class TtlFile { public string lang; public string[] files; }

  public static void parseTtlsFirstRun() {

    Parallel.ForEach(ttlFiles().Where(f => File.Exists(f.files[0])), new ParallelOptions { MaxDegreeOfParallelism = 4 }, f => {
      var ctx = new WiktCtx(f.lang, WiktConsts.Namespaces);
      Console.WriteLine($"{f.lang}: START");
      foreach (var fn in f.files) {
        VDS.LM.Parser.parse(fn, (t, c) => {
          var pt = ParsedTriple.firstRun(ctx, t);
          if (pt == null)
            return;
          var err = ctx.registerDataId(pt.objDataType, pt.subjDataId);
          if (err != null) ctx.addError(err, pt.subjDataId);
        });
        ctx.writeErrors(Path.GetFileName(fn) + ".1err");
      }
      Console.WriteLine($"{f.lang}: END");
      // save IDs to disk
      ctx.designSaveDataIds();
    });

    Console.WriteLine("Done...");
    Console.ReadKey();
  }

  public static void parseTtlsSecondRun() {


    var dumpAllProps = new Dictionary<string, dynamic[]>();
    Parallel.ForEach(ttlFiles().Where(f => File.Exists(f.files[0])), new ParallelOptions { MaxDegreeOfParallelism = 4 }, f => {
      Console.WriteLine($"{f.lang}: START");
      var ctx = new WiktCtx(f.lang, WiktConsts.Namespaces);
      // load IDs from disk
      ctx.designLoadDataIds();
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
            var node = ctx.designGetObj(pt.subjDataId);// (f.lang, pt.predType == WiktConsts.PredicateType.a ? pt.objDataType : null, pt.subjDataId, ctx);
            if (node != null) { // && pt.predType != WiktConsts.PredicateType.a) {// && pt.predType != WiktConsts.PredicateType.no) {
              node.acceptProp(pt, ctx);
            }
          } else if (pt.subjBlankId != null) {
            if (pt.objValue == null) ctx.addError("blank subj", pt.subjBlankId);
            ctx.blankValues[pt.subjBlankId] = pt.objValue;
          }
        });
        ctx.writeErrors(Path.GetFileName(fn) + ".2err");
      }
      // save objects to disk
      ctx.designSaveData();
      Console.WriteLine($"{f.lang}: END");
    });
    //File.WriteAllLines(LowUtilsDirs.logs + "dumpAllProps-Count.txt", dumpAllProps.OrderBy(kv => kv.Value[0]).ThenByDescending(kv => kv.Value[1]).Select(kv => kv.Key + "  #" + (int)kv.Value[1]));
    //File.WriteAllLines(LowUtilsDirs.logs + "dumpAllProps-PropName.txt", dumpAllProps.OrderBy(kv => kv.Key).Select(kv => kv.Key + "  #" + (int)kv.Value[1]));

    WiktCtx.dumpLog(LowUtilsDirs.logs + "secondRun");
    Console.WriteLine("Done...");
    Console.ReadKey();
  }

}
