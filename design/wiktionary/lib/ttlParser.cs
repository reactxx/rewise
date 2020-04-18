using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;
using static WiktConsts;
using static WiktTriple;

// design time extension

namespace VDS.LM {
  public static class Parser {
    public static void parse(string fn, Action<Triple, int> onTriple) {
      var ttlparser = new TurtleParser();
      Options.InternUris = false;
      using (var graph = new MyGraph(onTriple))
      using (var rdr = new StreamReader(fn))
        ttlparser.Load(graph, rdr);
    }
  }
  class MyGraph : NonIndexedGraph {
    public MyGraph(Action<Triple, int> onTriple) : base() {
      this.onTriple = onTriple;
    }
    Action<Triple, int> onTriple;
    int count = 0;
    public override bool Assert(Triple t) {
      onTriple(t, count++);
      return true;
    }

  }

}
public static class WiktTtlParser {

  public static IEnumerable<TtlFile> ttlFiles() => WiktConsts.AllLangs.Where(l => true).Select(lang => new TtlFile {
    lang = lang,
    files = ttlFileParts.Select(fp => $"{ttlRoot}{lang}\\{lang}_dbnary_{fp}.ttl").ToArray()
  });
  //const string ttlRoot = @"c:\Users\pavel\graphdb-import\dbnary\";
  const string ttlRoot = @"d:\rewise\data\wiktionary\dbnary\source-ttls\";
  static string[] ttlFileParts = new[] { "ontolex", "morpho", "enhancement" };
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
    Parallel.ForEach(ttlFiles().Where(f => /*f.lang=="la" &&*/ File.Exists(f.files[0])), new ParallelOptions { MaxDegreeOfParallelism = 4 }, f => {
      Console.WriteLine($"{f.lang}: START");
      var ctx = new WiktCtx(f.lang, WiktConsts.Namespaces, loggerWrong); 
      // load IDs from disk
      ctx.designLoadDataIds();

      foreach (var fn in f.files.Where(ff => File.Exists(ff))) {
        // if (fn.IndexOf("en_dbnary_morpho.ttl") < 0) continue;
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
              //if (pt.subjDataId == "eng:__wf_-ZSElg--_lie_to__Verb__1" && pt.predicate == predicates.olia_hasTense) {
              //  if (pt == null) return;
              //}
              node.acceptProp(pt, ctx);
            }
          } else if (pt.subjBlankId != null) {
            if (pt.objValue == null) ctx.log(null, WiktConsts.predicates.no, "blank subj" + pt.subjBlankId);
            ctx.blankValues[pt.subjBlankId] = pt.objValue;
          }
        });
        if (ctx.blankValues.Count > 0) Console.WriteLine($"{f.lang} blank {ctx.blankValues.First().Key}");
      }
      // finish objects
      ctx.designFinish();

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
