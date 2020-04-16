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

  public static void loadData2() {
    var objs = new List<Helper>();
    var maxIdx = Enumerable.Repeat(0, 255).ToArray();

    Parallel.ForEach(getAllMasks(), new ParallelOptions { MaxDegreeOfParallelism = 22 }, m => {
      var fn = m.dataFileName + ".json";
      if (!File.Exists(fn)) return;
      var res = Json.Deserialize<WiktModel.Page[]>(fn);
      void finishObj(Helper obj) {
        obj.lang = m.lang;
        decodeId(obj.id, out byte lowByte, out int dataIdId);
        maxIdx[lowByte] = Math.Max(maxIdx[lowByte], dataIdId);
        objs.Add(obj);
        foreach (var trans in obj.getTrans()) { trans.lang = m.lang; trans.owner = obj; }
      }
      lock (objs) {
        foreach (var page in res) {
          finishObj(page);
          if (page.entries != null) foreach (var en in page.entries) {
              en.page = page;
              finishObj(en);
              foreach (var form in en.getFormData()) { form.lang = m.lang; form.entry = en; form.isOther = form != en.canonicalForm; }
              if (en.senses != null) foreach (var sens in en.senses) {
                  sens.entry = en;
                  finishObj(sens);
                }
            }
          if (objs.Count % 140000 == 0) Console.Write("\r>> {0}%  ", Convert.ToInt32(objs.Count / 140000));
        }
      }
    });

    database = Enumerable.Range(0, 255).Select(i => new Helper[maxIdx[i] + 1]).ToArray();
    foreach (var obj in objs) {
      decodeId(obj.id, out byte lowByte, out int dataIdId);
      database[lowByte][dataIdId] = obj;
    }
    objs = null;
    Console.WriteLine("Done");
  }

  public static void loadData() {
    var objs = new List<Helper>();
    var maxIdx = Enumerable.Repeat(0, 255).ToArray();
    Parallel.ForEach(getAllMasks(), new ParallelOptions { MaxDegreeOfParallelism = 1 }, m => {
      var fn = m.dataFileName + ".json";
      if (!File.Exists(fn)) return;
      var type = urlToType[m.classUrl];
      /*<<<<<<< HEAD
            using (var rdr = new JsonStreamReader(fn, 5000000))
              foreach (var obj in rdr.Deserialize(type).Cast<Helper>()) {
                decodeId(obj.id, out byte lowByte, out int dataIdId);
                lock (objs) {
                  objs.Add(obj);
                  maxIdx[lowByte] = Math.Max(maxIdx[lowByte], dataIdId);
                  var page = obj as Page;
                  if (page == null || page.entries == null) continue;
                  foreach (var en in page.entries) {
                    en.page = page;
                    decodeId(en.id, out byte elowByte, out int edataIdId);
                    maxIdx[elowByte] = Math.Max(maxIdx[elowByte], edataIdId);
                    objs.Add(en);
                    if (en.senses == null) continue;
                    foreach (var sens in en.senses) {
                      sens.entry = en;
                      decodeId(sens.id, out byte slowByte, out int sdataIdId);
                      maxIdx[slowByte] = Math.Max(maxIdx[slowByte], sdataIdId);
                      objs.Add(sens);
                    }
      =======*/
      Json.DeserializeEnum(type, fn, o => {
        var obj = o as Helper;
        decodeId(obj.id, out byte lowByte, out int dataIdId);
        lock (objs) {
          objs.Add(obj);
          maxIdx[lowByte] = Math.Max(maxIdx[lowByte], dataIdId);
          var page = obj as Page;
          if (page == null || page.entries == null) return;
          foreach (var en in page.entries) {
            en.page = page;
            decodeId(en.id, out byte elowByte, out int edataIdId);
            maxIdx[elowByte] = Math.Max(maxIdx[elowByte], edataIdId);
            objs.Add(en);
            if (en.senses == null) continue;
            foreach (var sens in en.senses) {
              sens.entry = en;
              decodeId(sens.id, out byte slowByte, out int sdataIdId);
              maxIdx[slowByte] = Math.Max(maxIdx[slowByte], sdataIdId);
              objs.Add(sens);
              //>>>>>>> 0358a53d895981ab2fee97d6839a4f4178e7a2de
            }
          }
          if (objs.Count % 140000 == 0) Console.Write("\r>> {0}%  ", Convert.ToInt32(objs.Count / 140000));
        }
      });
      //using (var rdr = new JsonStreamReader(fn, 1000000))
      //  foreach (var obj in rdr.Deserialize(type).Cast<Helper>()) {
      //  }
    });
    database = Enumerable.Range(0, 255).Select(i => new Helper[maxIdx[i] + 1]).ToArray();
    foreach (var obj in objs) {
      decodeId(obj.id, out byte lowByte, out int dataIdId);
      database[lowByte][dataIdId] = obj;
    }
    objs = null;
    Console.WriteLine("Done");
  }


  public static T getObj<T>(int? id) where T : Helper {
    if (id == null) return null;
    decodeId((int)id, out byte lowByte, out int dataIdId);
    return (T)database[lowByte][dataIdId];
  }
  public static Helper getObj(int? id) => getObj<Helper>(id);

  public static IEnumerable<T> getObjs<T>(byte? lang = null) where T : Helper {
    return database.Where((list, low) => {
      if (list.Count() <= 1) return false;
      decodeId(low, out byte l, out byte c, out int id);
      if (lang != null && lang != l) return false;
      if (typeof(T) != classMaskToType[c]) return false;
      return true;
    }).
    SelectMany(l => l.Where(ll => ll != null)).
    Cast<T>();
  }
  public static IEnumerable<T> getObjsStr<T>(string lang = null) where T : Helper => getObjs<T>(lang == null ? (byte?)null : AllLangsIdMask[lang]);

  public static Helper[][] database;
}


