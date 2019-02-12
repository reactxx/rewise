using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public static class SqlServerReg {
  static IEnumerable<Tuple<int, string, string>> Parse(string regFile, string clsIds) {
    var ciItems = File.ReadAllText(clsIds).Split(new string[] { "}]\r\n@=\"" }, StringSplitOptions.RemoveEmptyEntries);
    var guidToFile = new Dictionary<string, string>();
    for (var i = 0; i < ciItems.Length; i++) {
      var line = ciItems[i];
      var idx = line.LastIndexOf('{');
      if (line.Length - idx == 37) {
        var guid = line.Substring(idx) + '}';
        idx = ciItems[i + 1].IndexOf('"');
        var fileName = ciItems[i + 1].Substring(0, idx);
        guidToFile[guid] = fileName;
      }
    }

    var items = File.ReadAllText(regFile).Split(new string[] { @"""Locale""=dword:" }, StringSplitOptions.RemoveEmptyEntries);
    foreach (var item in items) {
      var lines = item.Split(new string[] { "\r\n" }, 2, StringSplitOptions.RemoveEmptyEntries);
      int lcid; string br = null; string st = null;
      if (!int.TryParse(lines[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out lcid)) continue;
      var l = lines[1].Split(new string[] { "\"WBreakerClass\"=\"{" }, 2, StringSplitOptions.None);
      if (l.Length == 2) {
        br = '{' + l[1].Split('}')[0] + '}';
        br += '/' + guidToFile[br];
      }
      l = lines[1].Split(new string[] { "\"StemmerClass\"=\"{" }, 2, StringSplitOptions.None);
      if (l.Length == 2) {
        st = '{' + l[1].Split('}')[0] + '}';
        st += '/' + guidToFile[st];
      }
      yield return Tuple.Create(lcid, br, st);
    }
  }
  public static void Parse(Dictionary<int, Meta> res, string regFile, string clsIds) {
    foreach (var t in Parse(regFile, clsIds)) {
      Meta meta;
      if (!res.TryGetValue(t.Item1, out meta)) res.Add(t.Item1, meta = new Meta());
      meta.wBreakerClass = t.Item2;
      meta.stemmerClass = t.Item3;
    }
  }
  public static void Parse(Dictionary<string, LangMatrixRow> res, string regFile, string clsIds) {
    foreach (var t in Parse(regFile, clsIds)) {
      var lcid = t.Item1 == 0 ? 127 : t.Item1;
      var id = CultureInfo.GetCultureInfo(lcid).Name;
      var row = LangsDesignLib.adjustNewfulltextDataRow(res, Langs.oldToNew(id));
      row.row[0] = t.Item2;
      row.row[1] = t.Item3;
    }
  }
}
