using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace LangsLib
{

  public static class SqlServerReg
  {
    public static void Parse(Dictionary<int, Meta> res, string regFile, string clsIds)
    {
      var ciItems = File.ReadAllText(clsIds).Split(new string[] { "}]\r\n@=\"" }, StringSplitOptions.RemoveEmptyEntries);

      var items = File.ReadAllText(regFile).Split(new string[] { @"""Locale""=dword:" }, StringSplitOptions.RemoveEmptyEntries);
      foreach (var item in items)
      {
        var lines = item.Split(new string[] { "\r\n" }, 2, StringSplitOptions.RemoveEmptyEntries);
        int lcid; string br = null; string st = null;
        if (!int.TryParse(lines[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out lcid)) continue;
        var l = lines[1].Split(new string[] { "\"WBreakerClass\"=\"{" }, 2, StringSplitOptions.None);
        if (l.Length == 2)
          br = l[1].Split('}')[0];
        l = lines[1].Split(new string[] { "\"StemmerClass\"=\"{" }, 2, StringSplitOptions.None);
        if (l.Length == 2)
          st = l[1].Split('}')[0];
        Meta meta;
        if (!res.TryGetValue(lcid, out meta)) res.Add(lcid, meta = new Meta());
        meta.WBreakerClass = br;
        meta.StemmerClass = st;
      }
    }
  }

}