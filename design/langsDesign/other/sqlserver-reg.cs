using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public static class SqlServerReg {
  public static void Parse(Dictionary<int, Meta> res, string regFile, string clsIds) {
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
      Meta meta;
      if (!res.TryGetValue(lcid, out meta)) res.Add(lcid, meta = new Meta());
      meta.wBreakerClass = br;
      meta.stemmerClass = st;
    }
  }
}
