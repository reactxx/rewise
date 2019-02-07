using Newtonsoft.Json;
using Sepia.Globalization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.XPath;

public static class CldrTrans {

  public static void Build (string[] roots, string[] langs, string[] scripts, string[] regions) {
    roots.Select(root => new Locale(LocaleIdentifier.Parse(root))).Select(loc => {
      var localePattern = loc.Find("//localeDisplayNames/localeDisplayPattern/localePattern").ToString();
      var localeSeparator = loc.Find("//localeDisplayNames/localeDisplayPattern/localeSeparator").ToString();
      fromCldr(loc, langs, "//localeDisplayNames/languages");
      fromCldr(loc, scripts, "//localeDisplayNames/scripts");
      fromCldr(loc, regions, "//localeDisplayNames/territories");
      return null as string;
    }).ToArray();
  }

  static Dictionary<string, string> fromCldr(Locale loc, string[] ids, string path) {
    var hash = new HashSet<string>(ids);
    var finds = loc.FindOrDefault(path);
    if (finds == null) return null;
    var res = new Dictionary<string, string>();
    foreach (var kv in finds.SelectChildren(XPathNodeType.Element).OfType<XPathNavigator>()) {
      var key = kv.SelectSingleNode("./@type").Value;
      if (!hash.Contains(key)) continue;
      res[key] = kv.SelectSingleNode("./text()").Value;
    }
    return res;
  }
}