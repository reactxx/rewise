using Newtonsoft.Json;
using Sepia.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using W = Microsoft.Office.Interop.Word;

public static class SpellCheck {
  //public static void getSpellCheckIds() {
  //  var lcids = new HashSet<int>(Langs.meta.Select(m => m.LCID).Where(lcid => lcid != 0));
  //  var spells = Enum.GetValues(typeof(W.WdLanguageID)).Cast<int>().Where(l => lcids.Contains(l)).ToArray();
  //  var infos = spells.Select(s => {
  //    var ci = CultureInfo.GetCultureInfo(s);
  //    return new { name = ci.Name, spell = Enum.GetName(typeof(W.WdLanguageID), s), id = ci.LCID, msSupport = true   };
  //  }).OrderBy(l => l.name).ToArray();
  //  Json.Serialize(LangsDesignDirs.otherappdata + "spellCheckSupport.json", infos);
  //}
  //SOURCE: rewise\design\langsDesign\appdata\other\spellCheckSupport.xml from https://support.office.com/en-us/article/language-accessory-pack-for-office-82ee1236-0f9a-45ee-9c72-05b026ee809f
  //DOWNLOADS: rewise\design\langsDesign\appdata\other\spellCheckSupportDownload.html
  public static void ParseMSoftHTMLPage() {
    var doc = XElement.Load(LangsDesignDirs.otherappdata + "spellCheckSupport.xml");
    var bodies = doc.
      Descendants("tbody").
      Select(b => b.Elements("tr").ToArray()).
      Where(t => t.Length == 2).
      Select(t => t[1]).
      Select(t => t.Elements("td").ToArray()).
      Where(t => t.Length == 7).
      Select(t => new { lang = t[1].Value, info = t[3].Value, detailed = t[4].Value.Replace("\n", " "),
        download = t[5].Elements("p").First().Element("a").Attribute("href").Value }).
      ToArray();
    bodies = bodies.Where(
      b => 
    b.info == "Full" ||
    b.info == "Proofing Tools only" ||
    b.detailed.IndexOf("and includes proofing") > 0 ||
    b.lang == "hi-in").
    ToArray();

    var res = bodies.ToDictionary(b => b.lang, b => b.download); // .OrderBy(b => b.lang).Select(b => new Dictionary<string, string>() { { b.lang, b.download } });
    Json.Serialize(LangsDesignDirs.otherappdata + "spellCheckSupportDownload.json", res);
    //var html = new XElement("body", bodies.Select(b => new XElement("p", new XElement("a", new XAttribute("href", b.download), new XText(b.lang)))));
    //html.Save(LangsDesignDirs.otherappdata + "spellCheckSupportDownload.html");
    bodies = null;

  }
  public static void Parse(Dictionary<string, LangMatrixRow> res) {
    //var infos = Newtonsoft.Json.JsonConvert.DeserializeObject(LangsDesignDirs.otherappdata + "spellCheckSupport.json");
    //oks.ForEach((item, idx) => {
    //  var row = LangsDesignLib.adjustNewfulltextDataRow(res, item.Id.ToString());
    //  row.row[8] = googleLocsCodes[idx];
    //});

  }
}
