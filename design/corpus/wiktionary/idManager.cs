using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WiktModel;

public static class WiktIdManager {

  public static Helper createLow(string tp) {
    switch (tp) {
      case WiktConsts.NodeTypeNames.Gloss: return new WiktToSQL.HelperGloss();
      case WiktConsts.NodeTypeNames.Form: return new WiktToSQL.HelperForm();
      case WiktConsts.NodeTypeNames.LexicalSense: return new Sense();
      case WiktConsts.NodeTypeNames.Page: return new Page();
      case WiktConsts.NodeTypeNames.Translation: return new Translation();
      case WiktConsts.NodeTypeNames.Statement: return new Statement();
      case WiktConsts.NodeTypeNames.LexicalÈntry: return new Entry();
      default: throw new Exception();
    }
  }

  static string dbDir = $"{Corpus.Dirs.wiktDbnary}db";
  static string classCounterFn = $"{dbDir}\\classCounter.json";

  public static void allocArrays() {
    var classCounter = Json.Deserialize<int[]>(classCounterFn);
    for (int i = 0; i < 256; i++)
      objs[i] = new Helper[classCounter[i]];
  }

  public static Helper int2Obj(int id) {
    decodeId(id, out byte langMask, out byte classMask, out int langClassId);
    var all = allObjs(langMask, classMask);
    return all[langClassId];
  }
  public static Helper[] allObjs(byte langMask, byte classMask) {
    var low = encodeLowByte(langMask, classMask);
    return objs[low];
  }
  public static Helper[] allObjs(string lang, string classUrl) {
    return allObjs(WiktConsts.AllLangsIdMask[lang], WiktConsts.ClassIdMask[classUrl]);
  }

  static Helper[][] objs = new Helper[256][];

  struct maskInfo {
    public string lang; public string classUrl;
    public byte langMask; public byte classMask;

    public string dataIdsFileName { get => $"{dbDir}\\{lang}\\dataids_{ classUrl.Replace(':', '_')}"; }
  }

  static IEnumerable<maskInfo> getAllMasks(string actLang = null) =>
    WiktConsts.AllLangs.Where(l => actLang == null || actLang == l).SelectMany(lang => WiktConsts.ClassIds.Select(classUrl => new maskInfo {
      classUrl = classUrl,
      lang = lang,
      classMask = WiktConsts.ClassIdMask[classUrl],
      langMask = WiktConsts.AllLangsIdMask[lang]
    }));

  static int encodeId(string lang, string classUrl, int langClassId) {
    return langClassId << 8 | encodeLowByte(WiktConsts.AllLangsIdMask[lang], WiktConsts.ClassIdMask[classUrl]);
  }
  static byte encodeLowByte(byte langMask, byte classMask) {
    return (byte)((classMask << 5) | langMask);
  }
  static void decodeLowByte(int id, out byte langMask, out byte classMask) {
    classMask = (byte)((id & 0xff) >> 5);
    langMask = (byte)(id & 0x1f);
  }
  static void decodeLowByte(int id, out string lang, out string classUri) {
    decodeLowByte(id, out byte langMask, out byte classMask);
    lang = WiktConsts.AllLangs[langMask]; classUri = WiktConsts.ClassIds[classMask];
  }

  static byte encodeLowByte(string lang, string classUrl) {
    return encodeLowByte(WiktConsts.AllLangsIdMask[lang], WiktConsts.ClassIdMask[classUrl]);
  }
  static void decodeId(int id, out byte langMask, out byte classMask, out int langClassId) {
    langClassId = (id & 0x7fffff00) >> 8;
    decodeLowByte(id, out langMask, out classMask);
  }
  static void decodeId(int id, out string lang, out string classUrl, out int langClassId) {
    decodeId(id, out byte langMask, out byte classMask, out langClassId);
    lang = WiktConsts.AllLangs[langMask]; classUrl = WiktConsts.ClassIds[classMask];
  }

  // ******************* design time, first run

  static List<string>[] dataIds = Enumerable.Range(0, 256).Select(i => new List<string> { "" }).ToArray();
  static HashSet<string> usedDataIds = new HashSet<string>();

  public static string registerDataId(string lang, string classUri, string dataId) {
    lock (dataIds) {
      if (usedDataIds.Contains(dataId)) return "duplicated dataId";
      usedDataIds.Add(dataId);
      dataIds[encodeLowByte(lang, classUri)].Add(dataId);
      return null;
    }
  }

  public static void designSaveDataIds2() {
    foreach (var m in getAllMasks()) {
      var low = encodeLowByte(m.langMask, m.classMask);
      var list = dataIds[low];
      if (list.Count < 2) continue;
      var fn = m.dataIdsFileName;
      var dir = Path.GetDirectoryName(fn);
      if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
      File.WriteAllLines(fn, list);
    }
  }

  public static void designLoadDataIds() {
    foreach (var m in getAllMasks()) {
      var fn = m.dataIdsFileName;
      if (!File.Exists(fn)) continue;
      using (var rdr = new StreamReader(fn)) {
        foreach (var di in rdr.ReadAllLines().Select((dataId, idx) => new { dataId, idx })) {
          if (di.idx == 0) continue;
          Helper newObj;
          stringIds.Add(di.dataId, newObj = createLow(m.classUrl));
          newObj.Id = encodeId(m.lang, m.classUrl, di.idx);
        }
      }
    }
  }
  static Dictionary<string, int> stringId2intId = new Dictionary<string, int>();

  // ******************* design time
  public static bool desingStr2Obj(string id, out Helper res) => stringIds.TryGetValue(id, out res);

  public static Helper designAssignNewId(string dataId, string lang, string classUrl, Helper obj) {
    lock (stringIds) {
      obj.Id = encodeId(lang, classUrl, getNewLangClassId(lang, classUrl));
      stringIds.Add(dataId, obj);
    }
    return obj;
  }
  public static void designSaveDataIds() {
    string[][] objs = new string[256][];
    var masks = getAllMasks().ToArray();
    // aloc dataIds arrays
    foreach (var m in masks) {
      var low = encodeLowByte(m.langMask, m.classMask);
      objs[low] = new string[langClassCounter[low]];
    }
    // assign dataId
    foreach (var kv in stringIds) {
      decodeId(kv.Value.Id, out byte langMask, out byte classMask, out int langClassId);
      var low = encodeLowByte(langMask, classMask);
      objs[low][langClassId] = kv.Key;
    }
    // write
    foreach (var m in masks) {
      var fn = m.dataIdsFileName;
      var dir = Path.GetDirectoryName(fn);
      if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
      var low = encodeLowByte(m.langMask, m.classMask);
      File.WriteAllLines(fn, objs[low]);
    }
    //langClassCounter
    Json.Serialize(classCounterFn, langClassCounter);
  }

  static int[] langClassCounter = Enumerable.Repeat(1, 256).ToArray();
  static Dictionary<string, Helper> stringIds = new Dictionary<string, Helper>();

  static int getNewLangClassId(string lang, string classUrl) {
    var low = encodeLowByte(lang, classUrl);
    return langClassCounter[low]++;
  }
  static string counterKey(string lang, string classUrl) => $"{lang}_{classUrl}";

}
