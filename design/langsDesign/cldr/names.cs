using Newtonsoft.Json;
using Sepia.Globalization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.XPath;

public class CldrTrans {

  public string localePattern;
  public string localeSeparator;
  public LangMatrix langs;
  public LangMatrix scripts;
  public LangMatrix regions;

  public static void Build() {
    var cldr = Json.Deserialize<Langs.CldrLang[]>(LangsDirs.dirCldrTexts);
    var roots = cldr.SelectMany(c => c.regions.Select(r => LocaleIdentifier.Parse(string.Format("{0}-{1}-{2}", c.lang, c.scriptId, r)))).ToArray();
    var locs = roots.Select(root => new Locale(root)).ToArray();

    var patterns = new LangMatrix(
      locs.Select(loc => {
        var localePattern = loc.FindOrDefault("//localeDisplayNames/localeDisplayPattern/localePattern").ToString();
        var localeSeparator = loc.FindOrDefault("//localeDisplayNames/localeDisplayPattern/localeSeparator").ToString();
        return new LangMatrixRow {
          lang = loc.Id.ToString(),
          row = new string[] { localePattern, localeSeparator },
          columnNames = new string[] { "pattern", "separator" },
        };
      }), null, true
    );
    Dictionary<string, Dictionary<string, string>> langsProtocol = new Dictionary<string, Dictionary<string, string>>();
    var langs = new LangMatrix(
      locs.Select(loc => fromCldr(loc, "//localeDisplayNames/languages")),
      langsProtocol, true
    );
    Dictionary<string, Dictionary<string, string>> scriptsProtocol = new Dictionary<string, Dictionary<string, string>>();
    var scripts = new LangMatrix(
      locs.Select(loc => fromCldr(loc, "//localeDisplayNames/scripts")),
      scriptsProtocol, true
    );
    Dictionary<string, Dictionary<string, string>> regionsProtocol = new Dictionary<string, Dictionary<string, string>>();
    var regions = new LangMatrix(
      locs.Select(loc => fromCldr(loc, "//localeDisplayNames/territories")),
      regionsProtocol, true
    );

    langs.save(LangsDesignDirs.cldr + "cldrNameLangs.csv", true);
    scripts.save(LangsDesignDirs.cldr + "cldrNameScripts.csv", true);
    regions.save(LangsDesignDirs.cldr + "cldrNameRegions.csv", true);
    patterns.save(LangsDesignDirs.cldr + "cldrNamePatterns.csv", true);
    //  var localePattern = loc.FindOrDefault("//localeDisplayNames/localeDisplayPattern/localePattern").ToString();
    //  var localeSeparator = loc.FindOrDefault("//localeDisplayNames/localeDisplayPattern/localeSeparator").ToString();
    //  fromCldr(loc, "//localeDisplayNames/languages");
    //  fromCldr(loc, "//localeDisplayNames/scripts");
    //  fromCldr(loc, "//localeDisplayNames/territories");
    //});
  }

  static LangMatrixRow fromCldr(Locale loc, string path) {
    var finds = loc.FindOrDefault(path);
    if (finds == null) return null;
    var res = new Dictionary<string, string>();
    foreach (var kv in finds.SelectChildren(XPathNodeType.Element).OfType<XPathNavigator>()) {
      var key = kv.SelectSingleNode("./@type").Value;
      if (res.ContainsKey(key)) continue;
      var value = kv.SelectSingleNode("./text()").Value;
      if (UnicodeBlocks.checkBlockNames(value, loc.Id.Script) != null) continue;
      res[key] = value;
    }
    return new LangMatrixRow {
      lang = loc.Id.ToString(),
      row = res.Values.ToArray(),
      columnNames = res.Keys.ToArray(),
    };
  }
}