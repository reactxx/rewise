using System.IO;
using System.Xml.Linq;
using System.Linq;

public static class Wiki {

  public class WikiStat {
    public string lang;
    public string name;
    public int articles;
    public int users;
    public static WikiStat[] load() => Json.Deserialize<WikiStat[]>(Directory.GetCurrentDirectory() + @"\patches\wikiStat.json");
  }
  public static void CldrPatch() {
    var rows = XElement.Load(Directory.GetCurrentDirectory() + @"\wiki\stat.xml").Descendants("tr").Select(tr => tr.Elements().ToArray()).ToArray();
    var langs = CldrLangRegionScript.LangOrRegion.loadLangs().ToDictionary(l =>l.id);
    var res = rows.Select(r => new WikiStat {
      name = r[0].Value,
      lang = langs.ContainsKey(r[2].Value) ? langs[r[2].Value].id : "?" + r[2].Value,
      articles = int.Parse(r[3].Value.Replace(",", "")),
      users = int.Parse(r[7].Value.Replace(",", "")),
    }).OrderBy(r => r.lang).ToArray();
    Json.Serialize(LangsDesignDirs.root + @"patches\wikiStat.json", res);
  }
}
