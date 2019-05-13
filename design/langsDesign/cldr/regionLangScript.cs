using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

public static class CldrLangRegionScript {

  static XElement supplements = XElement.Load(LangsDesignDirs.cldrRepo + @"common\supplemental\supplementalData.xml");
  static XElement engTrans = XElement.Load(LangsDesignDirs.cldrRepo + @"common\main\en.xml");

  public class EnglishNames {
    public Dictionary<string, string> Teritories;
    public Dictionary<string, string> Langs;
    public Dictionary<string, string> Scripts;
  }
  public class RegionLow {
    public string id;
    public string name;
  }
  public class LangOrRegion {
    public string id;
    public string name;
    public string[] scripts;
    public int offSum;
    public int nonOffSum;
    public string off;
    public string nonOff;
  }
  internal class regLang {
    internal string region;
    internal string lang;
    internal int population;
    internal bool isOfficial;
  }

  public class Teritory : RegionLow {
    public List<Teritory> childs;
    public List<RegionLow> regions;
    internal Teritory(EnglishNames names, string id, Dictionary<string, string[]> dir) {
      this.id = id;
      name = names.Teritories[id];
      var values = dir[id];
      foreach (var v in values) {
        if (char.IsDigit(v[0])) {
          if (childs == null) childs = new List<Teritory>();
          childs.Add(new Teritory(names, v, dir));
        } else {
          if (regions == null) regions = new List<RegionLow>();
          regions.Add(new RegionLow { id = v, name = names.Teritories[id] });
        }
      }
    }
    internal IEnumerable<string> regs() {
      if (childs != null) foreach (var r in childs) foreach (var rr in r.regs()) yield return rr;
      if (regions != null) foreach (var r in regions) yield return r.id;
    }
  }

  static EnglishNames computeLangRegionEnglish() {

    // ************************** 
    // ***** array of <region,lang,population,isOfficial>
    var regionLang = supplements.Descendants("territoryInfo").
      Single().
      Elements("territory").
      SelectMany(el => {
        var allPop = int.Parse(el.Attribute("population").Value);
        var region = el.Attribute("type").Value;
        return el.Elements("languagePopulation").
            Select(ee => {
              return new regLang {
                region = region,
                lang = ee.Attribute("type").Value,
                population = (int)Math.Round((double)allPop * float.Parse(ee.Attribute("populationPercent")?.Value ?? "0", CultureInfo.InvariantCulture) / 100),
                isOfficial = ee.Attribute("officialStatus") != null,
              };
            });
      }).
      ToArray();

    // ************************** 
    // ***** english translation

    // function for getting english translation
    Dictionary<string, string> trans(string tag) {
      var r = engTrans.
        Descendants(tag).
        Single().
        Elements().
        Where(el => el.Attribute("alt") == null).
        ToDictionary(el => el.Attribute("type").Value, el => el.Value);
      //if (hand != null) foreach (var kv in hand) r[kv.Key] = kv.Value;
      return r;
    }

    var Langs = trans("languages");

    // missing language names: get it from comment in supplementalData.xml, e.g. from
    // <languagePopulation type="syl" literacyPercent="35" populationPercent="5"/>	<!--Sylheti-->
    //var supplements = XElement.Load(LangsDesignDirs.cldrRepo + @"common\supplemental\supplementalData.xml");
    var missing = regionLang.Select(rl => rl.lang).Except(Langs.Keys).ToHashSet();
    var els = supplements.
      Descendants("languagePopulation").
      Select(el => new { el, lang = el.Attribute("type").Value }).Where(el => missing.Contains(el.lang)).Select(el => new {
        el.lang, name = (el.el.NextNode as XComment).Value
      }).
      ToArray();
    foreach (var kv in els) Langs[kv.lang] = kv.name;

    var english = new EnglishNames {
      Teritories = trans("territories"),
      Langs = Langs,
      Scripts = trans("scripts"),
    };
    Json.Serialize(LangsDesignDirs.root + @"patches\cldrEnglish.json", english);

    // ************************** 
    // ***** population grouping

    // group and sum by <region> or <lang>
    void compute(regLang[] data, bool byReg, bool noOffOnly = false) {
      var by = byReg ? data.GroupBy(d => d.region) : data.GroupBy(d => d.lang);
      string getName(regLang d, bool isKey) {
        var actNames = byReg == isKey ? english.Teritories : english.Langs;
        var actId = byReg == isKey ? d.region : d.lang;
        return actNames[actId];
      }
      var res = by.Select(g => new LangOrRegion {
        name = getName(g.First(), true),
        id = g.Key,
        offSum = g.Where(l => l.isOfficial).Sum(l => l.population),
        nonOffSum = g.Where(l => !l.isOfficial).Sum(l => l.population),
        off = string.Join(", ", g.Where(l => l.isOfficial && l.population > 0).OrderByDescending(l => l.population).Select(l => $"{getName(l, false)}:{l.population}")),
        nonOff = string.Join(", ", g.Where(l => !l.isOfficial && l.population > 0).OrderByDescending(l => l.population).Select(l => $"{getName(l, false)}:{l.population}")),
      }).
      Where(r => r.id != "und" && (noOffOnly ? r.offSum == 0 : r.offSum + r.nonOffSum > 0)).
      OrderByDescending(r => r.offSum + r.nonOffSum).
      ToArray();

      LangOrRegion[] fillScripts(LangOrRegion[] langs) {

        var validScrits = UnicodeBlocks.ISO15924.ToHashSet();

        var scriptDir = supplements.
          Elements("languageData").
          Elements("language").
          Where(e => e.Attribute("scripts") != null).
          Select(e => new { id = e.Attribute("type").Value, vals = e.Attribute("scripts").Value.Split(' ') }).
          Where(e => english.Langs.ContainsKey(e.id)).
          GroupBy(e => e.id).
          ToDictionary(
            g => g.Key,
            g => g.SelectMany(v => v.vals.Where(s => validScrits.Contains(s))).ToArray()
           );
        var noScript = new List<LangOrRegion>();
        foreach (var lang in langs) {
          if (scriptDir.TryGetValue(lang.id, out string[] scripts)) lang.scripts = scripts;
          else noScript.Add(lang);
          if (lang.nonOff == "") lang.nonOff = null;
          if (lang.off == "") lang.off = null;
        }
        Json.Serialize(LangsDesignDirs.root + @"patches\cldrLangsUnknownScript.json", noScript);
        return langs.Where(l => l.scripts != null).ToArray();
      }


      if (!byReg && !noOffOnly) res = fillScripts(res);

      var nm = byReg ? "Regions" : (noOffOnly ? "LangsNotOfficial" : "Langs");
      Json.Serialize(LangsDesignDirs.root + $"patches\\cldr{nm}.json", res);
    }

    compute(regionLang, true);
    compute(regionLang, false, true);
    compute(regionLang, false);

    return english;
  }

  static void createTeritoryTree(EnglishNames names) {
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

  public static void CldrPatch() {
    var names = computeLangRegionEnglish();
    createTeritoryTree(names);
  }
}

