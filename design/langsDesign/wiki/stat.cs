using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

public static class Wiki {

  public class WikiStat {
    public string lang;
    public string wikiLang;
    public string wikiName;
    public string name;
    public int articles;
    public int users;
    public static WikiStat[] load() => Json.Deserialize<WikiStat[]>(Directory.GetCurrentDirectory() + @"\patches\wikiStat.json");
  }
  public static void CldrPatch() {
    var rows = XElement.Load(Directory.GetCurrentDirectory() + @"\wiki\stat.xml").Descendants("tr").Select(tr => tr.Elements().ToArray()).ToArray();
    var langs = CldrLangRegionScript.LangOrRegion.loadLangs().ToDictionary(l => l.id);
    var names = CldrLangRegionScript.EnglishNames.load().Langs;
    var res = rows.Select(row => {
      var ws = new WikiStat {
        wikiName = row[0].Value,
        articles = int.Parse(row[3].Value.Replace(",", "")),
        users = int.Parse(row[7].Value.Replace(",", "")),
      };
      var lang = row[2].Value;
      if (langs.TryGetValue(lang, out CldrLangRegionScript.LangOrRegion l)) ws.lang = l.id;
      else if (wrongLangs.TryGetValue(lang, out string ll)) { ws.lang = ll; ws.wikiLang = lang; }
      else ws.wikiLang = lang;
      var name = ws.lang != null ? names[ws.lang] : null;
      if (name != null && name != ws.wikiName) ws.name = name;
      return ws;
    }).
    //Where(ws => ws.lang!=null).
    OrderBy(r => r.lang ?? "").
    ThenBy(r => r.wikiLang ?? "").
    ToArray();
    Json.Serialize(LangsDesignDirs.root + @"patches\wikiStat.json", res);
  }

  static Dictionary<string, string> wrongLangs = new Dictionary<string, string> {
    { "bh","bho" },
    { "bxr","bua" },
    { "diq","zza" },
    { "eml","egl" },
    { "fiu-vro","vro" },
    { "no","nb" },
    { "tl","fil" },
    { "zh-yue","yue" },
    { "als","gsw" },
  };

}