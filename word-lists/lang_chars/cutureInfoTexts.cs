using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Linq;
using System.ComponentModel;

//specific culture = language: ISO 639-1 (https://en.wikipedia.org/wiki/ISO_3166-1) + country: ISO_3166-1 (https://en.wikipedia.org/wiki/ISO_3166-1). 
// Culture.ThreeLetterISOLanguageName: ISO 639-2 code (https://en.wikipedia.org/wiki/List_of_ISO_639-2_codes)
// alphabets (scripts): https://en.wikipedia.org/wiki/List_of_languages_by_writing_system, https://en.wikipedia.org/wiki/List_of_writing_systems
public static class CultureInfoTexts {

  // for every LCID: samples of used characters
  //public static Dictionary<string, char[]> charsSample {
  //  get {
  //    if (texts == null) {
  //      var fn = Root.unicode + "cultureInfoTexts.xml";
  //      var ser = new XmlSerializer(typeof(textInfo[]));
  //      textInfo[] infos;
  //      texts = new Dictionary<string, char[]>();
  //      using (var fs = File.OpenRead(fn))
  //        infos = ser.Deserialize(fs) as textInfo[];
  //      foreach (var t in infos)
  //        foreach (var name in t.names)
  //          texts[name] = t.dateTexts./*Normalize().*/ToCharArray().Where(ch => LangsLib.UnicodeBlockNames.isLetter(ch)).Distinct().OrderBy(ch => ch).ToArray();

  //    }
  //    return texts;
  //  }
  //}
  //static Dictionary<string, char[]> texts = null;

  public class textInfo {
    public string id;
    public string name;
    public string dateTexts;
    public string netUnicodeGroups;
    [DefaultValue(false)]
    public bool isSupplementalRoot;
    [DefaultValue(false)]
    public bool isEmptyMain;
    //public bool isInCldrMain;
  }

  public static IEnumerable<CultureInfo> validLangs() {
    //c:\rewise\word-lists\lang_chars\appdata\cldr\common\main\
    var mainDir = Directory.GetFiles(Root.cldr + @"common\main\").Select(f => Path.GetFileNameWithoutExtension(f)).ToDictionary(l => l.Replace('_', '-'), l => true);
    return CultureInfo.GetCultures(CultureTypes.AllCultures).Where(lc => mainDir.ContainsKey(lc.Name));
  }

  // ********** CULTURE INFO TEXTS
  public static void getCultureInfoTexts() {

    var allLangs = validLangs().ToArray();

    //C:\rewise\word-lists\lang_chars\appdata\cldr\cldr\common\supplemental\supplementalData.xml
    var supplementalData = XElement.Load(Root.cldr + @"common\supplemental\supplementalData.xml");
    var parentLocales_root = supplementalData.Descendants("parentLocales").Descendants().First(nd => nd.Attribute("parent").Value == "root").Attribute("locales").Value;
    var isRootDir = parentLocales_root.Split(' ').ToDictionary(l => l.Replace('_', '-'), l => true);
    // single dateTexts
    var dateTextsDir = new Dictionary<string, string>();
    // empty Main files
    var emptyMains = allLangs.
      Select(lc => new { lc, xml = XElement.Load(Root.cldr + @"common\main\" + lc.Name.Replace('-', '_') + ".xml") }).
      Where(lc => lc.xml.Nodes().Count() == 1).
      ToDictionary(lc => lc.lc.Name, lc => true);

    var texts = allLangs.
      Select(lc => {
        var dateTexts = getDateTextx(lc);
        if (!dateTextsDir.TryGetValue(dateTexts, out string source)) {
          dateTextsDir[dateTexts] = "@" + lc.Name;
        } else
          dateTexts = source;
        return new { lc, dateTexts };
      }). // only single dateTexts
      Select(lc => new textInfo {
        dateTexts = lc.dateTexts,
        netUnicodeGroups = LangsLib.UnicodeBlockNames.getBlockNames(lc.dateTexts).Select(kv => kv.Key).Aggregate((r, i) => r + " * " + i),
        id = lc.lc.Name,
        isSupplementalRoot = isRootDir.ContainsKey(lc.lc.Name),
        name = lc.lc.EnglishName,
        isEmptyMain = emptyMains.ContainsKey(lc.lc.Name),
      }).
      OrderBy(g => g.id).
      ToArray();
    var wrongs = texts.Where(t => t.isEmptyMain && t.dateTexts[0] != '@').Select(t => t.id).ToArray();
    var ser = new XmlSerializer(typeof(textInfo[]));
    var fn = Root.unicode + "cultureInfoTexts.xml";
    if (File.Exists(fn)) File.Delete(fn);
    using (var fs = File.OpenWrite(fn))
      ser.Serialize(fs, texts);
  }

  static string getDateTextx(CultureInfo lc) {
    var texts = new string[3];
    var fmt = lc.DateTimeFormat;
    texts[0] = fmt.MonthNames.Take(12).Aggregate((r, i) => r + "*" + i);
    texts[1] = fmt.MonthGenitiveNames.Take(12).Aggregate((r, i) => r + "*" + i);
    texts[2] = fmt.DayNames.Take(7).Aggregate((r, i) => r + "*" + i);
    return texts.Aggregate((r, i) => r + "\n" + i).ToLower();
  }

  // from c:\rewise\word-lists\lang_chars\appdata\cldr\common\rbnf\
  public static void cldrRbnf() {
  }

  // DUMP types from c:\rewise\word-lists\lang_chars\appdata\cldr\common\rbnf\
  public static void dumpDummyMains() {
    var allMains = validLangs().
      Select(lc => new { lc, xml = XElement.Load(Root.cldr + @"common\main\" + lc.Name.Replace('-', '_') + ".xml") }).
      Where(lc => lc.xml.Nodes().Count() == 1).
      Select(lc => lc.lc.Name);
  }

  // DUMP types from c:\rewise\word-lists\lang_chars\appdata\cldr\common\rbnf\
  public static void dumpCldrRbnfTypes() {
    var texts = validLangs().Select(l => l.Name.Replace('-', '_')).ToDictionary(l => l, l => true);
    var mainDir = Directory.GetFiles(Root.cldr + @"common\rbnf\").Select(f => Path.GetFileNameWithoutExtension(f)).Where(l => texts.ContainsKey(l)).ToArray();
    var rulesetTypes = new Dictionary<string, string>();
    foreach (var fn in mainDir) {
      var xml = XElement.Load(Root.cldr + @"common\rbnf\" + fn + ".xml");
      xml.XPathSelectElements("");
      foreach (var rule in xml.Descendants("ruleset")) {
        var value = rule.Attribute("type").Value;
        if (!value.StartsWith("spellout-")) continue;
        value = value.Substring("spellout-".Length);
        if (rulesetTypes.TryGetValue(value, out string count))
          rulesetTypes[value] = count + "," + fn;
        else
          rulesetTypes[value] = fn;
      }
    }
    File.WriteAllLines(@"c:\temp\rulesetTypes.txt", rulesetTypes.OrderByDescending(kv => kv.Value.Length).Select(rv => rv.Key + ":" + rv.Value));
  }

}