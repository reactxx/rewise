using Sepia.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

public static class CldrLangRegionScript2 {

  public static void CldrPatch() {
    var names = englishNames();
    computeLangRegionEnglish(names);
    Teritory.createTeritoryTree(names);
  }

  // CLDR sources
  static XElement supplements = XElement.Load(LangsDesignDirs.cldrRepo + @"common\supplemental\supplementalData.xml");
  static XElement engTrans = XElement.Load(LangsDesignDirs.cldrRepo + @"common\main\en.xml");
  static HashSet<string> allLangs => supplements.
    Descendants("languagePopulation").
    Select(el => el.Attribute("type").Value).
    ToHashSet();
  static Dictionary<string, string> langTrans = trans("languages").Where(kv => allLangs.Contains(kv.Key)).ToDictionary(kv => kv.Key, kv => kv.Value);

  // Display english friendly names
  public class EnglishNames {
    public Dictionary<string, string> Teritories;
    public Dictionary<string, string> Langs;
    public Dictionary<string, string> Scripts;
  }
  public class Region {
    public string id;
    public string name;
  }
  public class LangScripts {
    public string lang;
    public string name;
    public string script;
    public List<string> other = new List<string>();
  }
  public class LangOrRegion {
    public string id;
    public string name;
    public int offSum;
    public int nonOffSum;
    public string off;
    public string nonOff;
    public string likely;
    public string scripts;
  }

  public class Teritory : Region {
    public List<Teritory> childs;
    public List<Region> regions;

    internal Teritory(EnglishNames names, string id, Dictionary<string, string[]> dir) {
      this.id = id;
      name = names.Teritories[id];
      var values = dir[id];
      foreach (var v in values) {
        if (char.IsDigit(v[0])) {
          if (childs == null) childs = new List<Teritory>();
          childs.Add(new Teritory(names, v, dir));
        } else {
          if (regions == null) regions = new List<Region>();
          regions.Add(new Region { id = v, name = names.Teritories[id] });
        }
      }
    }
    internal static void createTeritoryTree(EnglishNames names) {
      var ters = supplements.
        Descendants("territoryContainment").
        Single().Elements("group").
        Where(g => g.Attribute("status") == null && g.Attribute("grouping") == null).
        ToDictionary(el => el.Attribute("type").Value, el => el.Attribute("contains").Value.Split(' '));

      // testUniq is empty => every region is unique in tree
      var testUniq = ters.SelectMany(kv => kv.Value.Select(Value => new { kv.Key, Value })).GroupBy(v => v.Value).Where(g => g.Count() > 1);
      if (testUniq.Count() > 0) throw new Exception();

      // build tree
      var root = new Teritory(names, "001", ters);
      Json.Serialize(LangsDesignDirs.root + @"patches\cldrTeritoryTree.json", root);
    }
  }

  static Dictionary<string, string> trans(string tag) => engTrans.
      Descendants(tag).
      Single().
      Elements().
      Where(el => el.Attribute("alt") == null).
      ToDictionary(el => el.Attribute("type").Value, el => el.Value);

  static EnglishNames englishNames() {

    // missing trans from comments in supplements, e.g.
    // <languagePopulation type="wbp" populationPercent="0.011"/>	<!--Warlpiri-->
    var missing = allLangs.Except(langTrans.Keys).ToHashSet();
    var els = supplements.
      Descendants("languagePopulation").
      Select(el => new { el, lang = el.Attribute("type").Value }).Where(el => missing.Contains(el.lang)).Select(el => new {
        el.lang, name = (el.el.NextNode as XComment).Value
      }).
      ToArray();
    foreach (var kv in els) langTrans[kv.lang] = kv.name;

    var english = new EnglishNames {
      Teritories = trans("territories"),
      Langs = langTrans,
      Scripts = trans("scripts"),
    };
    Json.Serialize(LangsDesignDirs.root + @"patches\cldrEnglish.json", english);

    return english;
  }

  static List<LangScripts> extractScript() {

    var validScrits = UnicodeBlocks.ISO15924.ToHashSet();
    validScrits.Add("Jpan"); validScrits.Add("Kore"); validScrits.Add("Hans"); validScrits.Add("Hant");

    var res = new List<LangScripts>();

    // see C:\Users\pavel\AppData\Local\UnicodeCLDR\core\common\supplemental\supplementalData.xml
    foreach (var row in XElement.Load(Directory.GetCurrentDirectory() + @"\cldr\langScripts.xml").Descendants("tr")) {
      // last TD with value.length==4 is SCRIPT
      var script = row.Elements().Select(el => el.Value.Trim()).Where(s => s.Length == 4).Last();
      
      // first TR with seven TD child defines LANG
      if (row.Elements().Count() == 7) {
        var s = new LangScripts { lang = row.Descendants("a").Single().Attribute("name").Value };
        res.Add(s);
      }
      // obsolete script? (info in last node)
      var lastNodeValue = (row.LastNode as XElement).Value.Trim();
      var obsolete = !validScrits.Contains(script) || lastNodeValue == "N" || lastNodeValue == "n/a";
      // 
      if (!obsolete) res.Last().other.Add(script);
    }

    res = res.Where(l => allLangs.Contains(l.lang)).ToList();

    foreach (var s in res) {
      // no script found => add "2?" + likely
      var likely = LocaleIdentifier.Parse(s.lang).MostLikelySubtags();
      if (s.other.Count == 0) {
        s.script = "2?" + likely.Script;
        s.other = null;
        continue;
      }

      // likely is not in found script => add "1?" + likely
      s.script = s.other.Contains(likely.Script) ? likely.Script : "1?" + likely.Script;
      s.other.Remove(likely.Script);
      if (s.other.Count == 0) s.other = null;
    }

    // add missing langs: add script from lang (e.g. Arab for uz_Arab) OR "3?" + likely
    foreach (var lang in allLangs.Except(res.Select(l => l.lang)))
      res.Add(new LangScripts { lang = lang, script = lang.Contains('_') ? lang.Split('_')[1] : "3?" + LocaleIdentifier.Parse(lang).MostLikelySubtags().Script, other = null });

    foreach (var lang in res) lang.name = langTrans[lang.lang];

    // sort, write and return
    res = res.OrderByDescending(s => s.other==null ? 0 : s.other.Count).ThenBy(s => s.lang).ToList();
    Json.Serialize(LangsDesignDirs.root + $"patches\\cldrScript.json", res);
    return res;
  }

  static void computeLangRegionEnglish(EnglishNames english) {

    var scripts = extractScript().ToDictionary(
      s => s.lang, 
      s => s.script + (s.other == null ? "" : "|" + string.Join(",", s.other)));

    // ************************** 
    // ***** REGION-LANG table: <region,lang>,<population,isOfficial,script>
    var regionLang = supplements.Descendants("territoryInfo").
      Single().
      Elements("territory").
      SelectMany(el => {
        var allPop = int.Parse(el.Attribute("population").Value);
        var region = el.Attribute("type").Value;
        return el.Elements("languagePopulation").
            Select(ee => {
              string lang = ee.Attribute("type").Value, key1 = $"{lang}|{region}", key2 = $"{lang}|";
              return new regLang {
                region = region,
                lang = lang,
                population = (int)Math.Round((double)allPop * float.Parse(ee.Attribute("populationPercent")?.Value ?? "0", CultureInfo.InvariantCulture) / 100),
                isOfficial = ee.Attribute("officialStatus") != null,
              };
            });
      }).
      ToArray();

    // ************************** 
    // ***** population grouping

    // group and sum by <region> or <lang>
    void groupAndSum(regLang[] data, bool byReg, bool noOffOnly = false) {

      string friendlyName(regLang d, bool isKey, bool sh = false) {
        var actId = byReg == isKey ? d.region : d.lang;
        if (sh) return actId;
        var actNames = byReg == isKey ? english.Teritories : english.Langs;
        return actNames[actId];
      }
      string stringDetail(IGrouping<string, regLang> g, bool officialOnly) {
        var sd = string.Join(", ", g.Where(l => officialOnly == l.isOfficial && l.population > 0).
          OrderByDescending(l => l.population).
          Select(l => $"{friendlyName(l, false, false)}:{friendlyName(l, false, true)}:{l.population}"));
        return sd == "" ? null : sd;
      }

      var by = byReg ? data.GroupBy(d => d.region) : data.GroupBy(d => d.lang);

      var res = by.Select(g => new LangOrRegion {
        name = friendlyName(g.First(), true),
        id = g.Key,
        offSum = g.Where(l => l.isOfficial).Sum(l => l.population),
        nonOffSum = g.Where(l => !l.isOfficial).Sum(l => l.population),
        off = stringDetail(g, true),
        nonOff = stringDetail(g, false),
        likely = byReg ? null : LocaleIdentifier.Parse(g.Key).MostLikelySubtags().ToString(),
        scripts = byReg ? null : scripts[g.Key],
      }).
      Where(r => r.id != "und" && (noOffOnly ? r.offSum == 0 : r.offSum + r.nonOffSum > 0)).
      OrderByDescending(r => r.offSum + r.nonOffSum).
      ToArray();

      var nm = byReg ? "Regions" : (noOffOnly ? "LangsNotOfficial" : "Langs");
      Json.Serialize(LangsDesignDirs.root + $"patches\\cldr{nm}.json", res);
    }

    groupAndSum(regionLang, true);
    groupAndSum(regionLang, false, true);
    groupAndSum(regionLang, false);
  }

  class regLang {
    // ids
    internal string region;
    internal string lang;
    // data
    internal int population;
    internal bool isOfficial;
  }

  // select language script from "languageData"
  //static void fillScripts(EnglishNames english) {
  //  var validScrits = UnicodeBlocks.ISO15924.ToHashSet();
  //  var scriptDir = supplements.
  //    Elements("languageData").
  //    Elements("language").
  //    Where(e => e.Attribute("scripts") != null).
  //    Select(e => new {
  //      lang = e.Attribute("type").Value,
  //      scripts = e.Attribute("scripts").Value.Split(' '),
  //      territories = e.Attribute("territories") == null ? null : e.Attribute("territories").Value.Split(' '),
  //    }).
  //    Where(e => english.Langs.ContainsKey(e.lang)).
  //    GroupBy(e => e.lang).
  //    Select(g => new Script {
  //      id = g.Key,
  //      scripts = g.SelectMany(v => {
  //        var res = v.scripts.Where(s => validScrits.Contains(s));
  //        if (v.territories == null) return res;
  //        return v.territories.SelectMany(t => res.Select(s => $"{s}-{t}"));
  //      }).ToArray(),
  //      name = english.Langs[g.Key]
  //    }).
  //    Where(s => s.scripts.Length > 0).
  //    ToArray();

  //  Json.Serialize(LangsDesignDirs.root + @"patches\cldrScript.json", scriptDir);
  //}


}

