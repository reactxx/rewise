using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WiktModel;
using static WiktConsts;
using static WiktIdManager;
using static WiktSchema;

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
      using (var rdr = new BsonStreamReader(fn))
        foreach (var obj in rdr.Deserialize(type).Cast<Helper>())
          idDir.Add(obj.id, obj);
    }
  }

  public static Helper getObj(int? id) => getObj<Helper>(id);
  public static T getObj<T>(int? id) where T : Helper =>
    id == null ? null : (T)(idDir.TryGetValue((int)id, out Helper res) ? res : null);
  static Dictionary<int, Helper> idDir = new Dictionary<int, Helper>();
}


