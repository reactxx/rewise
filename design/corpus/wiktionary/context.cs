using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WiktModel;
using static WiktIdManager;
using static WiktSchema; 

public class WiktCtx {

  public WiktCtx(string l, IEnumerable<KeyValuePair<string, string>> ns, WiktLogger logger) {
    iso1Lang = l; lang = Iso1_3.convert(l);
    this.logger = logger;
    namespaces = ns.Where(kv => kv.Value.EndsWith("#")).ToDictionary(kv => kv.Value.TrimEnd('#'), kv => kv.Key);
    prefixes = ns.Where(kv => !kv.Value.EndsWith("#")).ToDictionary(kv => kv.Value, kv => kv.Key);
  }

  WiktLogger logger;

  public void log(Helper owner, WiktConsts.predicates pred, string text) =>
    logger.add(iso1Lang, owner==null ? "null" : owner.GetType().Name, pred.ToString(), text);
  
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
        //addError("decodePath", uri.OriginalString);
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

  // ******************* design time, second run, end
  public void designFinish() {
    idDir = stringId2obj.ToDictionary(kv => kv.Value.id, kv => kv.Value);
    stringId2obj = null;
    foreach (var obj in idDir.Values) obj.finish(this);
  }
  Dictionary<int, Helper> idDir = new Dictionary<int, Helper>();

  public Helper designGetObj(int id) {
    return idDir.TryGetValue(id, out Helper res) ? res : null;
  }
}


