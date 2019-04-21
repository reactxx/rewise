using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using W = Microsoft.Office.Interop.Word;

public static class SpellCheck {

  //SOURCE: rewise\design\langsDesign\appdata\other\spellCheckSupport.xml from https://support.office.com/en-us/article/language-accessory-pack-for-office-82ee1236-0f9a-45ee-9c72-05b026ee809f
  //DOWNLOADS: rewise\design\langsDesign\appdata\other\spellCheckSupportDownload.html
  public static void ParseMSoftHTMLPage() {
    var doc = XElement.Load(LangsDesignDirs.msword + "spellCheckSupport.xml");
    var bodies = doc.
      Descendants("tbody").
      Select(b => b.Elements("tr").ToArray()).
      Where(trs => trs.Length == 2).
      Select(trs => trs[1]).
      Select(tr => tr.Elements("td").ToArray()).
      Where(tds => tds.Length == 7).
      Select(tds => new {
        lang = tds[1].Value, info = tds[3].Value, detailed = tds[4].Value.Replace("\n", " "),
        download = tds[5].Elements("p").First().Element("a").Attribute("href").Value
      }).
      ToArray();
    bodies = bodies.Where(
      b =>
    b.info == "Full" ||
    b.info == "Proofing Tools only" ||
    b.detailed.IndexOf("and includes proofing") > 0 ||
    b.lang == "hi-in").
    ToArray();

    // download .EXE's, for d:\rewise\dlibs\utils\test\langs\downloadMSWord_test.dart
    var res = bodies.ToDictionary(b => b.lang, b => b.download);
    Json.Serialize(LangsDesignDirs.msword + "spellCheckSupportDownload.json", res);

    // run .EXE's and install languages to msword
    File.WriteAllLines(
      LangsDesignDirs.msword + "spellCheckSupport.cmd",
      res.OrderByDescending(kv => kv.Key).SelectMany(kv => new string[] {
      string.Format(@"call d:\rewise\design\langsDesign\appdata\msword\{0}.exe", kv.Key),
      "ping 192.0.2.2 -n 1 -w 6000 > nul"
      //"pause"
      })
    );
    bodies = null;

  }

  // add info to LangsDesignDirs.otherappdata + "oldVersionInfo.csv. Then (after MergeOldToCldr) to rewise\clibs\utils\langs\cldrTexts.json
  public static void Parse(Dictionary<string, LangMatrixRow> res) {
    var msWordLangs = Json.Deserialize<Dictionary<string, string>>(LangsDesignDirs.msword + "spellCheckSupportDownload.json").
      Keys.
      Select(k => k.ToLower()).
      Where(k => k != "ca-es-valencia").
      Select(k => new { ms = k, lm = msWordLangToMetaLang.TryGetValue(k, out string val) ? val : k }).
      ToArray();
    foreach (var meta in msWordLangs.Select(l => new { l.ms, meta = Langs.meta.First(m => String.Compare(m.Id, l.lm, true) == 0) })) {
      var row = LangsDesignLib.adjustNewfulltextDataRow(res, meta.meta.Id);
      row.row[8] = CultureInfo.GetCultureInfo(meta.ms).LCID.ToString();
    }
    addMSWordConst(res, "en-GB", W.WdLanguageID.wdEnglishUK);
    addMSWordConst(res, "zh-Hant-HK", W.WdLanguageID.wdChineseHongKongSAR);
    //addMSWordConst(res, "km-KH", W.WdLanguageID.wdKhmer);
  }
  static void addMSWordConst(Dictionary<string, LangMatrixRow> res, string lang, W.WdLanguageID wid) {
    LangsDesignLib.adjustNewfulltextDataRow(res, lang).row[8] = ((int)wid).ToString();
  }
  static Dictionary<string, string> msWordLangToMetaLang = new Dictionary<string, string>() {
    {"az-latn-az","az-Latn"},
    {"bn-in","bn-BD"},
    {"bs-latn-ba","bs-Latn"},
    {"zh-cn","zh-Hans"},
    {"zh-tw","zh-Hant"},
    {"ha-latn-ng","ha-NG"},
    {"sw-ke","sw-TZ"},
    {"pa-in","pa-Guru"},
    {"sr-cyrl-rs","sr-Cyrl"},
    {"sr-latn-rs","sr-Latn"},
    {"uz-latn-uz","uz-Latn"},
  };
  public static void msWordLangNotInMeta() {
    var msWordLangs = Json.Deserialize<Dictionary<string, string>>(LangsDesignDirs.msword + "spellCheckSupportDownload.json").Keys.Select(k => k.ToLower()).ToArray();
    var langs = new HashSet<String>(Langs.nameToMeta.Keys.Select(k => k.ToLower()));
    var missing = msWordLangs.Where(l => !langs.Contains(l)).ToArray();
    File.WriteAllLines(@"c:\temp\pom.txt", missing);
  }

  public static void withoutSpellChecker() {
    File.WriteAllLines(@"c:\temp\pom.txt", Langs.meta.Where(m => m.WordSpellCheckLCID==0 && (m.IsEuroTalk || m.IsLingea)).OrderBy(m => m.Id).Select(m => m.Name + " - " +  m.Id));
  }
}
