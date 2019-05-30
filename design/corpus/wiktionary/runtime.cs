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

  public static void loadData() {
    var objs = new List<Helper>();
    var maxIdx = Enumerable.Repeat(0, 255).ToArray();
    Parallel.ForEach(getAllMasks(), new ParallelOptions { MaxDegreeOfParallelism = 4 }, m => {
      var fn = m.dataFileName + ".json";
      if (!File.Exists(fn)) return;
      var type = urlToType[m.classUrl];
      using (var rdr = new JsonStreamReader(fn, 10000000))
        foreach (var obj in rdr.Deserialize(type).Cast<Helper>()) {
          decodeId(obj.id, out byte lowByte, out int dataIdId);
          lock (objs) {
            if (objs.Count() % 100000 == 0)
              Console.Write("\r>> {0}%  ", Convert.ToInt32(objs.Count() / 100000));
            maxIdx[lowByte] = Math.Max(maxIdx[lowByte], dataIdId);
            objs.Add(obj);
          }
        }
    });
    dir = Enumerable.Range(0, 255).Select(i => new Helper[maxIdx[i] + 1]).ToArray();
    foreach (var obj in objs) {
      decodeId(obj.id, out byte lowByte, out int dataIdId);
      dir[lowByte][dataIdId] = obj;
    }
  }

  public static Helper getObj(int? id) => getObj<Helper>(id);

  public static T getObj<T>(int? id) where T : Helper {
    if (id == null) return null;
    decodeId((int)id, out byte lowByte, out int dataIdId);
    return (T)dir[lowByte][dataIdId];
  }
  static Helper[][] dir;
}


