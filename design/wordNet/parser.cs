using EntityFramework.BulkInsert;
using EntityFramework.BulkInsert.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

/*
refresh DB:
var dbCtx = wordNet.Import.getContext(true);
wordNet.LmfStats.xmlToDBFirstPhase();
wordNet.LmfStats.xmlToDBSecondPhase();

results in 
- DB
- rewise\data\wordnet\ids.txt
- rewise\data\wordnet\root.json
*/

namespace wordNet {

  public static class Parser {

    public static void xmlToDBFirstPhase() {
      count = 0;
      var ctx = new Context(true);
      foreach (var files in xml2Objects(ctx)) { }
      Console.WriteLine("End or first phase");
    }

    // using https://www.nuget.org/packages/EntityFramework.BulkInsert-ef6-ext/
    public static void xmlToDBSecondPhase() {
      count = 0;
      var ctx = new Context(false);
      var allNodes = xml2Objects(ctx).SelectMany(f => f).ToArray();

      foreach (var node in allNodes.OfType<LexicalEntry>()) node.finish(ctx);

      var allDB = allNodes.SelectMany(n => n.createDB(ctx)).ToArray();

      using (var dbCtx = wordNetDB.Context.getContext(true)) {
        dbCtx.Ids.Add(new wordNetDB.Ids { Text = File.ReadAllText(Context.root + "ids.txt") });// ctx.ids.Values.Select(id => id.Split('=')).Select(p => p[0] + "=" + p[2]).Aggregate((r, i) => r + "\n" + i) });
        dbCtx.SaveChanges();
        Console.WriteLine("Ids inserted");
        var opt = new BulkInsertOptions() {
          BulkCopyOptions = BulkCopyOptions.TableLock,
          BatchSize = 50000,
        };
        dbCtx.BulkInsert(allDB.OfType<wordNetDB.Lang>());
        Console.WriteLine("Lang inserted");
        dbCtx.BulkInsert(allDB.OfType<wordNetDB.Entry>());
        Console.WriteLine("Entry inserted");
        dbCtx.BulkInsert(allDB.OfType<wordNetDB.Synset>());
        Console.WriteLine("Synset inserted");
        dbCtx.BulkInsert(allDB.OfType<wordNetDB.Translation>());
        Console.WriteLine("Translation inserted");
        dbCtx.BulkInsert(allDB.OfType<wordNetDB.Relation>());
        Console.WriteLine("Relation inserted");
        dbCtx.BulkInsert(allDB.OfType<wordNetDB.Sense>());
        Console.WriteLine("Sense inserted");
        dbCtx.BulkInsert(allDB.OfType<wordNetDB.Example>());
        Console.WriteLine("Example inserted");
      }
    }

      public static void dbStat() {
      using (var dbCtx = wordNetDB.Context.getContext(false)) {
        var stat = dbCtx.Langs.OrderBy(l => l.Id).Select(l => new {
          l.Id, EntriesCount = l.Entries.Count, SynsetsCount = l.Synsets.Count,
          SensesCount = l.Senses.Count, //TranslationsCount = l.Translations.Count, 
          RelationsCount = l.Relations.Count }).ToArray(); 
        File.WriteAllLines(Context.root + "dbStat.txt", stat.Select(l => string.Format(
          "Lang={0}, Entries = {1}, Synsets={2}, Senses={3}, Relations={4}",
           l.Id, l.EntriesCount, l.SynsetsCount, l.SensesCount, l.RelationsCount) ));
      }
    }

    static IEnumerable<List<Node>> xml2Objects(Context ctx) {
      var stat = new Dictionary<string, int>();
      var names = new Node[10];

      void add(int deep, string val) {
        var path = string.Join(".", names.Select(n => n.className).Take(deep + 1));
        if (enumProps.TryGetValue(path, out Dictionary<string, int> vals))
          vals[val] = vals.TryGetValue(val, out int c) ? c + 1 : 1;
        stat[path] = stat.TryGetValue(path, out int cnt) ? cnt + 1 : 1;
        count++;
        if ((count & 0xffff) == 0) Console.WriteLine(count);
      }

      var rootNode = new Root();
      XmlReaderSettings settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse };
      foreach (var fn in Directory.EnumerateFiles(Context.root, "*.xml")) {
        Console.WriteLine(fn);
        var lang = fn.Split('-')[1];
        // if (lang != "eng" && lang != "slk") continue;
        var fileNodes = new List<Node>();
        using (var sr = new StreamReader(fn))
        using (var rdr = XmlReader.Create(sr, settings)) {
          while (rdr.MoveToNextAttribute() || rdr.Read()) {
            if (rdr.NodeType == XmlNodeType.Attribute) {
              names[rdr.Depth] = Node.create(rdr.LocalName, lang);
              add(rdr.Depth, rdr.Value);
            } else if (rdr.IsStartElement() || rdr.IsEmptyElement) {
              names[rdr.Depth] = Node.create(rdr.LocalName, lang);
              add(rdr.Depth, null);
            }
            if (rdr.NodeType == XmlNodeType.EndElement || rdr.IsEmptyElement || rdr.NodeType == XmlNodeType.Attribute) {
              var done = names[rdr.Depth];
              var parent = rdr.Depth == 0 ? rootNode : names[rdr.Depth - 1];
              var node = parent.addNode(ctx, done, rdr.NodeType == XmlNodeType.Attribute ? rdr.Value : null);
              if (node != null) fileNodes.Add(node);
            }
          }
          yield return fileNodes;
        }
      }
      if (ctx.firstPhase) {
        File.WriteAllLines(Context.root + "stat.txt", stat.OrderByDescending(kv => kv.Value).Select(kv => string.Format("{0}: {1}", kv.Key, kv.Value)));
        File.WriteAllLines(Context.root + "enums.txt", enumProps.Select(kv => kv.Key + ":\n\t" + string.Join("\n\t", kv.Value.Select(kvv => kvv.Key + ": " + kvv.Value))));
        File.WriteAllLines(Context.root + "ids.txt", ctx.ids.OrderBy(kv => kv.Value).Select(kv => kv.Key + "=" + kv.Value));
      }
    }
    static int count = 0;

    static Dictionary<string, Dictionary<string, int>> enumProps = new Dictionary<string, Dictionary<string, int>>() {
      { "LexicalResource.Lexicon.LexicalEntry.Lemma.partOfSpeech", new Dictionary<string, int>() },
      { "LexicalResource.Lexicon.Synset.SynsetRelations.SynsetRelation.relType", new Dictionary<string, int>() },
      { "LexicalResource.Lexicon.Synset.baseConcept", new Dictionary<string, int>() },
      { "LexicalResource.SenseAxes.SenseAxis.relType", new Dictionary<string, int>() },
      { "LexicalResource.Lexicon.Synset.MonolingualExternalRefs.MonolingualExternalRef.relType", new Dictionary<string, int>() },
      { "LexicalResource.Lexicon.Synset.MonolingualExternalRefs.MonolingualExternalRef.externalSystem", new Dictionary<string, int>() },
      { "LexicalResource.Lexicon.language", new Dictionary<string, int>() },
      { "LexicalResource.Lexicon.languageCoding", new Dictionary<string, int>() },
      { "LexicalResource.Lexicon.label", new Dictionary<string, int>() },
      { "LexicalResource.Lexicon.version", new Dictionary<string, int>() },
      { "LexicalResource.GlobalInformation.label", new Dictionary<string, int>() },
    };

    static string path(XAttribute attr, XElement el, StringBuilder sb, bool isFirst = true) {
      if (isFirst) sb.Clear();
      if (attr != null) {
        sb.Insert(0, ":" + attr.Name.LocalName);
        return path(null, attr.Parent, sb, false);
      } else {
        sb.Insert(0, "." + el.Name.LocalName);
        return el.Parent == null ? sb.ToString().Substring(1) : path(null, el.Parent, sb, false);
      }
    }
  }
}
