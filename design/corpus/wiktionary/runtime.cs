using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WiktModel;
using static WiktConsts;
using static WiktIdManager;

public static class WiktDB {

  public static Dictionary<string, Type> urlToType = new Dictionary<string, Type> {
    { NodeTypeNames.Gloss, typeof(Gloss)},
    { NodeTypeNames.Page, typeof(Page)},
    { NodeTypeNames.Translation, typeof(Translation)},
    { NodeTypeNames.Form, typeof(Form)},
    { NodeTypeNames.Statement, typeof(Statement)},
    { NodeTypeNames.LexicalSense, typeof(Sense)},
    { NodeTypeNames.LexicalÈntry, typeof(Entry)},
  };

  public static Dictionary<byte, Type> classMaskToType = urlToType.ToDictionary(c => WiktConsts.ClassIdMask[c.Key], c => c.Value);

  public static void loadData() {
    var objs = new List<Helper>();
    var maxIdx = Enumerable.Repeat(0, 255).ToArray();
    Parallel.ForEach(getAllMasks(), new ParallelOptions { MaxDegreeOfParallelism = 4 }, m => {
      var fn = m.dataFileName + ".bson";
      if (!File.Exists(fn)) return;
      var type = urlToType[m.classUrl];
      using (var rdr = new BsonStreamReader(fn))
        foreach (var obj in rdr.Deserialize(type).Cast<Helper>()) {
          decodeId(obj.id, out byte lowByte, out int dataIdId);
          lock (objs) {
            maxIdx[lowByte] = Math.Max(maxIdx[lowByte], dataIdId);
            objs.Add(obj);
            var ent = obj as Page;
            if (ent == null || ent.entries == null) continue;
            foreach (var en in ent.entries) {
              decodeId(en.id, out byte elowByte, out int edataIdId);
              maxIdx[elowByte] = Math.Max(maxIdx[elowByte], edataIdId);
              objs.Add(en);
            }
            if (objs.Count() % 151000 == 0) Console.Write("\r>> {0}%  ", Convert.ToInt32(objs.Count() / 151000));
          }
        }
    });
    dir = Enumerable.Range(0, 255).Select(i => new Helper[maxIdx[i] + 1]).ToArray();
    foreach (var obj in objs) {
      decodeId(obj.id, out byte lowByte, out int dataIdId);
      dir[lowByte][dataIdId] = obj;
    }

    // dump count
    var dumpDir = dir.Select((list, low) => new { list, low }).Where(li => li.list.Count() > 1).ToDictionary(li => li.low, li => li.list.Length);
    var lines = dumpDir.Select(kv => {
      decodeLowByte(kv.Key, out string lang, out string classUri);
      return new { lang, classUri, kv.Value };
    }).
    GroupBy(lcv => lcv.classUri).
    SelectMany(g => {
      return g.Select(lcv => $"{lcv.lang}={lcv.classUri}={lcv.Value}").
      Concat(Linq.Items($"**={g.Key}={g.Sum(lcv => lcv.Value)}"));
    }).
    OrderBy(s => s);
    File.WriteAllLines(LowUtilsDirs.logs + "dump-objects-count.txt", lines);

    // dump page tree counts
    IEnumerable<string[]> pageDump(Page p) {
      yield return new[] { "p" };
      if (p.entries == null) { yield return new[] { "p", "noentry" }; yield break; }
      var ens = p.entries.Length == 1 ? "entry" : "entries";
      yield return new[] { "p", ens };
      foreach (var en in p.entries) {
        if (en.otherForm == null) { yield return new[] { "p", ens, "noform" }; yield break; }
        var fms = en.otherForm.Length == 1 ? "form" : "forms";
        yield return new[] { "p", ens, fms };
      }
    }
    var pageParts = new Dictionary<string, int>();
    foreach (var s in getObjs<Page>().
      SelectMany(p => pageDump(p)).
      Select(arr => string.Join("=", arr))) 
      pageParts[s] = pageParts.TryGetValue(s, out int c) ? c + 1 : 1;
    
    File.WriteAllLines(LowUtilsDirs.logs + "dump-page-parts.txt", pageParts.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key} {kv.Value}"));
  }

  public static Helper getObj(int? id) => getObj<Helper>(id);

  public static IEnumerable<T> getObjs<T>(byte? lang = null) where T : Helper {
    return dir.Where((list, low) => {
      if (list.Count() <= 1) return false;
      decodeId(low, out byte l, out byte c, out int id);
      if (lang != null && lang != l) return false;
      if (typeof(T) != classMaskToType[c]) return false;
      return true;
    }).
    SelectMany(l => l.Where(ll => ll!=null)).
    Cast<T>();
  }

  public static T getObj<T>(int? id) where T : Helper {
    if (id == null) return null;
    decodeId((int)id, out byte lowByte, out int dataIdId);
    return (T)dir[lowByte][dataIdId];
  }
  static Helper[][] dir;
}


