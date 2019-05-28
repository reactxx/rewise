using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WiktModel;
using static WiktIdManager;
using static WiktSchema;

public class WiktCtx {

  public WiktCtx(string l, IEnumerable<KeyValuePair<string, string>> ns) {
    iso1Lang = l; lang = Iso1_3.convert(l);
    namespaces = ns.Where(kv => kv.Value.EndsWith("#")).ToDictionary(kv => kv.Value.TrimEnd('#'), kv => kv.Key);
    prefixes = ns.Where(kv => !kv.Value.EndsWith("#")).ToDictionary(kv => kv.Value, kv => kv.Key);
  }

  internal void addError(string v, string originalString) {
    if (errors.Count > 1000) return;
    var msg = $"** {v} in {originalString}";
    errors[msg] = errors.TryGetValue(msg, out int c) ? c + 1 : 1;
  }

  public class Log {
    public string lang;
    public int count;
    public string text;
    public static Dictionary<string, Log> logs = new Dictionary<string, Log>();
  }

  public static void log(WiktCtx ctx, string text) {
    lock (Log.logs) Log.logs.AddEx($"{ctx.lang}: {text}", l => { l.count++; return l; }, () => new Log { lang = ctx.iso1Lang, count = 1, text = text });
  }
  public static void dumpLog(string fn) {
    File.WriteAllLines($"{fn}.1.log", Log.logs.OrderBy(kv => kv.Value.lang).ThenByDescending(kv => kv.Value.count).Select(kv => $"{kv.Key} {kv.Value.count}"));
    File.WriteAllLines($"{fn}.2.log", Log.logs.OrderBy(kv => kv.Value.text).Select(kv => $"{kv.Value.text} {kv.Value.count}"));
    File.WriteAllLines($"{fn}.3.log", Log.logs.OrderByDescending(kv => kv.Value.count).Select(kv => $"{kv.Value.text} {kv.Value.count}"));
  }

  public void writeErrors(string fn) {
    if (errors.Count > 0) File.WriteAllLines(LowUtilsDirs.logs + fn, errors.OrderBy(kv => kv.Value).Select(kv => $"{kv.Value}x {kv.Key}"));
    errors.Clear();
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

  public Dictionary<string, int> errors = new Dictionary<string, int>();

  //********************* ID MANAGER
  // ******************* design time, first run

  List<string>[] dataIds = Enumerable.Range(0, 256).Select(i => new List<string> { "" }).ToArray();
  HashSet<string> usedDataIds = new HashSet<string>();

  public string registerDataId(string classUri, string dataId) {
    if (usedDataIds.Contains(dataId)) return "duplicated dataId";
    usedDataIds.Add(dataId);
    dataIds[encodeLowByte(iso1Lang, classUri)].Add(dataId);
    return null;
  }

  public void designSaveDataIds() {
    foreach (var m in getAllMasks(iso1Lang)) {
      var low = encodeLowByte(m.langMask, m.classMask);
      var list = dataIds[low];
      if (list.Count < 2) continue;
      var fn = m.dataIdsFileName;
      var dir = Path.GetDirectoryName(fn);
      if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
      File.WriteAllLines(fn, list);
    }
  }

  // ******************* design time, second run

  public void designLoadDataIds() {
    foreach (var m in getAllMasks(iso1Lang)) {
      var fn = m.dataIdsFileName;
      if (!File.Exists(fn)) continue;
      using (var rdr = new StreamReader(fn)) {
        foreach (var di in rdr.ReadAllLines().Select((dataId, idx) => new { dataId, idx })) {
          if (di.idx == 0) continue;
          Helper newObj = createLow(m.classUrl);
          newObj.id = encodeId(m.lang, m.classUrl, di.idx);
          stringId2obj.Add(di.dataId, newObj);
          var maskId = encodeLowByte(m.lang, m.classUrl);
          data[maskId].Add(newObj);
        }
      }
    }
  }
  Dictionary<string, Helper> stringId2obj = new Dictionary<string, Helper>();
  List<Helper>[] data = Enumerable.Range(0, 256).Select(i => new List<Helper>()).ToArray();

  public Helper designGetObj(string dataId) {
    return stringId2obj.TryGetValue(dataId, out Helper res) ? res : null;
  }

  public void designSaveData() {
    foreach (var m in getAllMasks(iso1Lang)) {
      var fn = m.dataFileName;
      var maskId = encodeLowByte(m.lang, m.classUrl);
      var list = data[maskId];
      if (list.Count == 0) continue;
      using (var wr = new JsonStreamWriter(fn + ".json")) {
        //using (var wr = new BsonStreamWriter(fn + ".bson")) {
        foreach (var obj in list) wr.Serialize(obj);
      }
    }
  }


}


