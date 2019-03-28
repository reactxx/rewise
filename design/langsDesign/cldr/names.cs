using Newtonsoft.Json;
using Sepia.Globalization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.XPath;

public class CldrTrans {

  public string localePattern;
  public string localeSeparator;
  public LangMatrix langs;
  public LangMatrix scripts;
  public LangMatrix regions;

  public static void Build() {
    var cldr = Json.Deserialize<Langs.CldrLang[]>(LangsDirs.dirCldrTexts);
    var roots = cldr.SelectMany(c => c.Regions.Select(r => LocaleIdentifier.Parse(string.Format("{0}-{1}-{2}", c.Lang, c.ScriptId, r)))).ToArray();
    var locs = roots.Select(root => new Locale(root)).ToArray();

    // ALPHABETS
    var rx = new Regex("[{} ]");
    var alphas = locs.Select(loc => {
      var localePattern = loc.Find("//characters");
      var data = localePattern.SelectChildren(XPathNodeType.Element).OfType<XPathNavigator>().Where(al => al.Name == "exemplarCharacters").Select(al => {
        var value = CldrUtils.decodeUnicodeLiteral(al.Value.Normalize().Trim('[', ']'));
        value = rx.Replace(value, "");
        var keyNode = al.SelectSingleNode("./@type");
        var key = keyNode == null ? "root" : keyNode.Value;
        //if (key == "numbers" || key == "punctuation" || string.IsNullOrEmpty(value)) return null;
        return new { key, value };
      }).Where(n => n != null).ToArray();
      return new {
        lang = loc.Id.ToString(),
        data
      };
    }).Where(d => d.data.Length > 0).OrderBy(l => l.lang).ToArray();

    Func<string, IEnumerable<LangMatrixRow>> alphasRes = (string key) => alphas.SelectMany(a => a.data.Where(aa => aa.key == key).Select(aa =>
        new LangMatrixRow {
          lang = a.lang, columnNames = Linq.Items(aa.key).ToArray(), row = Linq.Items(aa.value).ToArray()
        }));
    var alphaRoot = new LangMatrix(alphasRes("root"), null, true);
    var alphaAuxlity = new LangMatrix(alphasRes("auxiliary"), null, true);
    var alphaIndex = new LangMatrix(alphasRes("index"), null, true);
    var alphaNumbers= new LangMatrix(alphasRes("numbers"), null, true);
    var alphaPunctuation = new LangMatrix(alphasRes("punctuation"), null, true);

    var patterns = new LangMatrix(locs.Select(loc => {
      var localePattern = loc.FindOrDefault("//localeDisplayNames/localeDisplayPattern/localePattern").ToString();
      var localeSeparator = loc.FindOrDefault("//localeDisplayNames/localeDisplayPattern/localeSeparator").ToString();
      return new LangMatrixRow {
        lang = loc.Id.ToString(),
        row = new string[] { localePattern, localeSeparator },
        columnNames = new string[] { "pattern", "separator" },
      };
    }), null, true);
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
    alphaRoot.save(LangsDesignDirs.cldr + "alphaRoot.csv", true);
    alphaAuxlity.save(LangsDesignDirs.cldr + "alphaAuxlity.csv", true);
    alphaIndex.save(LangsDesignDirs.cldr + "alphaIndex.csv", true);
    alphaNumbers.save(LangsDesignDirs.cldr + "alphaNumbers.csv", true);
    alphaPunctuation.save(LangsDesignDirs.cldr + "alphaPunctuation.csv", true);

    // save to DART messages
    File.WriteAllBytes(LangsDesignDirs.data + @"langsDesign\cldrNameLangs.msg", Protobuf.ToBytes(matrixToDart(langs)));
    File.WriteAllBytes(LangsDesignDirs.data + @"langsDesign\cldrNameScripts.msg", Protobuf.ToBytes(matrixToDart(scripts)));
    File.WriteAllBytes(LangsDesignDirs.data + @"langsDesign\cldrNameRegions.msg", Protobuf.ToBytes(matrixToDart(regions)));
    File.WriteAllBytes(LangsDesignDirs.data + @"langsDesign\cldrNamePatterns.msg", Protobuf.ToBytes(matrixToDart(patterns)));
    File.WriteAllBytes(LangsDesignDirs.data + @"langsDesign\alphaRoot.msg", Protobuf.ToBytes(matrixToDart(alphaRoot)));
    File.WriteAllBytes(LangsDesignDirs.data + @"langsDesign\alphaAuxlity.msg", Protobuf.ToBytes(matrixToDart(alphaAuxlity)));
    File.WriteAllBytes(LangsDesignDirs.data + @"langsDesign\alphaIndex.msg", Protobuf.ToBytes(matrixToDart(alphaIndex)));
    File.WriteAllBytes(LangsDesignDirs.data + @"langsDesign\alphaNumbers.msg", Protobuf.ToBytes(matrixToDart(alphaNumbers)));
    File.WriteAllBytes(LangsDesignDirs.data + @"langsDesign\alphaPunctuation.msg", Protobuf.ToBytes(matrixToDart(alphaPunctuation)));

    //  var localePattern = loc.FindOrDefault("//localeDisplayNames/localeDisplayPattern/localePattern").ToString();
    //  var localeSeparator = loc.FindOrDefault("//localeDisplayNames/localeDisplayPattern/localeSeparator").ToString();
    //  fromCldr(loc, "//localeDisplayNames/languages");
    //  fromCldr(loc, "//localeDisplayNames/scripts");
    //  fromCldr(loc, "//localeDisplayNames/territories");
    //});
  }

  static Rw.Common.Matrix matrixToDart(LangMatrix matrix) {
    var res = new Rw.Common.Matrix();
    res.Cols.AddRange(matrix.colNames);
    res.Rows.AddRange(matrix.langs.Select((val, idx) => {
      var row = new Rw.Common.Row();
      row.Langs.AddRange(val.Split(','));
      row.Values.AddRange(matrix.data[idx].Select(cell => cell ?? ""));
      return row;
    }));
    return res;
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