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

    var loggerWrong = new WiktLogger();

    Parallel.ForEach(ttlFiles().Where(f => File.Exists(f.files[0])), new ParallelOptions { MaxDegreeOfParallelism = 4 }, f => {
      var ctx = new WiktCtx(f.lang, WiktConsts.Namespaces, loggerWrong);
      Console.WriteLine($"{f.lang}: START");
      foreach (var fn in f.files) {
        VDS.LM.Parser.parse(fn, (t, c) => {
          var pt = ParsedTriple.firstRun(ctx, t);
          if (pt == null)
            return;
          var err = ctx.registerDataId(pt.objDataType, pt.subjDataId);
          if (err != null) ctx.log(null, WiktConsts.predicates.no, err + pt.subjDataId);
        });
      }
      Console.WriteLine($"{f.lang}: END");
      // save IDs to disk
      ctx.designSaveDataIds();
    });
    loggerWrong.save(LowUtilsDirs.logs + "dump-errors1");
    Console.WriteLine("Done...");
    Console.ReadKey();
  }

  public static void parseTtlsSecondRun() {


    var loggerAll = new WiktLogger();
    var loggerWrong = new WiktLogger();
    var dumpAllProps = new Dictionary<string, dynamic[]>();
    Parallel.ForEach(ttlFiles().Where(f => File.Exists(f.files[0])), new ParallelOptions { MaxDegreeOfParallelism = 4 }, f => {
      Console.WriteLine($"{f.lang}: START");
      var ctx = new WiktCtx(f.lang, WiktConsts.Namespaces, loggerWrong);
      // load IDs from disk
      ctx.designLoadDataIds();

      foreach (var fn in f.files) {
        VDS.LM.Parser.parse(fn, (t, c) => {
          var pt = new ParsedTriple(ctx, t);
          if (pt.predType == WiktConsts.PredicateType.Ignore)
            return;
          if (pt.subjDataId != null) {
            if (pt.objBlankId != null) {
              if (!ctx.blankValues.TryGetValue(pt.objBlankId, out string value)) ctx.log(null, WiktConsts.predicates.no, "blank obj" + pt.subjBlankId);
              else { ctx.blankValues.Remove(pt.objBlankId); pt.objBlankId = null; pt.objValue = value; }
            }
            var node = ctx.designGetObj(pt.subjDataId);
            if (node != null) {
              loggerAll.add(ctx.iso1Lang, node.GetType().Name, pt.predicateUri, pt.objUri);
              node.acceptProp(pt, ctx);
            }
          } else if (pt.subjBlankId != null) {
            if (pt.objValue == null) ctx.log(null, WiktConsts.predicates.no, "blank subj" + pt.subjBlankId);
            ctx.blankValues[pt.subjBlankId] = pt.objValue;
          }
        });
      }
      // save objects to disk
      ctx.designSaveData();
      Console.WriteLine($"{f.lang}: END");
    });

    loggerWrong.save(LowUtilsDirs.logs + "dump-errors2");
    loggerAll.save(LowUtilsDirs.logs + "dump-all");

    Console.WriteLine("Done...");
    Console.ReadKey();
  }

}
