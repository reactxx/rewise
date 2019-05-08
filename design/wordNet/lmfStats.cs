using EntityFramework.BulkInsert;
using EntityFramework.BulkInsert.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace wordNet {

  public static class LmfStats {

    public static string driver = AppDomain.CurrentDomain.BaseDirectory[0].ToString();
    static string root = driver + @":\rewise\data\wordnet\";

    // using https://www.nuget.org/packages/EntityFramework.BulkInsert-ef6-ext/
    public static void xmlToDBSecondPhase() {
      foreach (var files in xml2Objects(false)) { }
      return;
      var opt = new BulkInsertOptions() {
        BulkCopyOptions = BulkCopyOptions.TableLock,
        BatchSize = 5000,
      };
      foreach (var files in xml2Objects(false)) {
        using (var ctx = Import.getContext(false)) {
          //ctx.BulkInsert(objects2entities(files.Where(n => n != null).OfType<wordNetDB.LexicalEntry>()));
          //ctx.BulkInsert(objects2entities(files.Where(n => n != null).OfType<wordNetDB.LexicalEntry>()));
          //ctx.BulkInsert(objects2entities(files.Where(n => n != null).OfType<wordNetDB.LexicalEntry>()));
        }
      }
    }

    public static void xmlToDBFirstPhase() {
      foreach (var files in xml2Objects(true)) { }
    }

    static IEnumerable<List<Node>> xml2Objects(bool firstPhase) {
      var ctx = new Context { firstPhase = firstPhase };
      if (!firstPhase)
       ctx.ids = File.ReadAllLines(root + "ids.txt").Select(l => l.Split(new char[] { '=' }, 2)).ToDictionary(parts => parts[0], parts => parts[1]);
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
      XmlReaderSettings settings = new XmlReaderSettings();
      settings.DtdProcessing = DtdProcessing.Parse;
      foreach (var fn in Directory.EnumerateFiles(root, "*.xml")) {
        Console.WriteLine(fn);
        var fileNodes = new List<Node>();
        using (var tr = new StreamReader(fn))
        using (var rdr = XmlReader.Create(tr, settings)) {
          while (rdr.MoveToNextAttribute() || rdr.Read()) {
            if (rdr.NodeType == XmlNodeType.Attribute) {
              names[rdr.Depth] = Node.create(rdr.LocalName);
              add(rdr.Depth, rdr.Value);
            } else if (rdr.IsStartElement() || rdr.IsEmptyElement) {
              names[rdr.Depth] = Node.create(rdr.LocalName);
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
        //break;
      }
      if (firstPhase) {
        File.WriteAllLines(root + "stat.txt", stat.OrderByDescending(kv => kv.Value).Select(kv => string.Format("{0}: {1}", kv.Key, kv.Value)));
        File.WriteAllLines(root + "enums.txt", enumProps.Select(kv => kv.Key + ":\n\t" + string.Join("\n\t", kv.Value.Select(kvv => kvv.Key + ": " + kvv.Value))));
        File.WriteAllText(root + "root.json", JsonConvert.SerializeObject(rootNode, Newtonsoft.Json.Formatting.Indented));
        File.WriteAllLines(root + "ids.txt", ctx.ids.OrderBy(kv => kv.Value).Select(kv => kv.Key + "=" + kv.Value));
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
