using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    foreach (var m in getAllMasks()) {
      var fn = m.dataIdsFileName;
      if (!File.Exists(fn)) continue;
      var type = urlToType[m.classUrl];
      var maxIdx = Enumerable.Repeat(0, 255).ToArray();
      var objs = new List<Helper>();
      using (var rdr = new BsonStreamReader(fn))
        foreach (var obj in rdr.Deserialize(type).Cast<Helper>()) {
          decodeId(obj.id, out byte lowByte, out int dataIdId);
          maxIdx[lowByte] = Math.Max(maxIdx[lowByte], lowByte);
          objs.Add(obj);
        }
      dir = Enumerable.Range(0, 255).Select(i => new Helper[maxIdx[i]]).ToArray() ;
      foreach (var obj in objs) {
        decodeId(obj.id, out byte lowByte, out int dataIdId);
        dir[lowByte][dataIdId] = obj;
      }
    }
  }

  public static Helper getObj(int? id) => getObj<Helper>(id);

  public static T getObj<T>(int? id) where T : Helper {
    if (id == null) return null;
    decodeId((int)id, out byte lowByte, out int dataIdId);
    return dir[lowByte][dataIdId];
  }
  static Helper[][] dir;
}


