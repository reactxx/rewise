using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public static class SqlServerQuery {

  public static void Parse(Dictionary<int, Meta> res, string queryFile) {
    var items = File.ReadAllText(queryFile).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    foreach (var item in items) {
      var lines = item.Split(new string[] { "\t" }, 2, StringSplitOptions.RemoveEmptyEntries);
      if (lines.Length != 2) continue;
      var lcid = int.Parse(lines[0]);
      Meta meta;
      if (!res.TryGetValue(lcid, out meta)) res.Add(lcid, meta = new Meta());
      meta.SqlSupportFulltext = true;
    }
  }

  public static void Parse(Dictionary<string, LangMatrixRow> res, string queryFile) {
    var items = File.ReadAllText(queryFile).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    foreach (var item in items) {
      var lines = item.Split(new string[] { "\t" }, 2, StringSplitOptions.RemoveEmptyEntries);
      if (lines.Length != 2) continue;
      var lcid = int.Parse(lines[0]);
      var id = Langs.oldToNew(CultureInfo.GetCultureInfo(lcid == 0 ? 127 : lcid).Name);
      LangsDesignLib.adjustNewfulltextDataRow(res, id).row[5] = "1";
    }
  }

}
