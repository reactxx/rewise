using System.Collections.Generic;
using System.IO;
using System.Linq;
using WiktModel;

public static class WiktIdManager {

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
      var dir = $"{dbDir}\\{m.lang}";
      if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
      var low = encodeLowByte(m.langMask, m.classMask);
      File.WriteAllLines($"{dir}\\dataids_{m.classUrl.Replace(':', '_')}", objs[low]);
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
