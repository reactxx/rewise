using Sepia.Globalization;
using Sepia.Globalization.Numbers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

public static class CldrLib {

  public class LocaleIdentifierEqualityComparer : IEqualityComparer<LocaleIdentifier>, IComparer<LocaleIdentifier> {
    public bool Equals(LocaleIdentifier x, LocaleIdentifier y) {
      return x.ToString().Equals(y.ToString());
    }

    public int GetHashCode(LocaleIdentifier obj) {
      return obj.ToString().GetHashCode();
    }

    public int Compare(LocaleIdentifier x, LocaleIdentifier y) {
      return x.ToString().CompareTo(y.ToString());
    }

    public static LocaleIdentifierEqualityComparer Instance = new LocaleIdentifierEqualityComparer();
  }

  static NetCultureInfo[] getNetCultureInfos(LocaleIdentifier[] cldrSpecifics) {
    //var cldrs = new HashSet<string>();
    var cldrs = new HashSet<string>(cldrSpecifics.Select(c => c.Language));
    var nets = new HashSet<LocaleIdentifier>(LocaleIdentifierEqualityComparer.Instance);
    var texts = CultureInfo.GetCultures(CultureTypes.AllCultures).
      Select(cu => {
        LocaleIdentifier lid = null;
        { try { lid = LocaleIdentifier.Parse(cu.Name); } catch { } }
        if (lid == null || string.IsNullOrEmpty(lid.Region) || char.IsDigit(lid.Region[0])) return null;
        if (cldrs.Contains(lid.Language)) return null;
        var text = getDateTextx(cu);
        var scr = LangsLib.UnicodeBlockNames.getBlockNames(text, true).Select(kv => kv.Key).Single();
        lid = LocaleIdentifier.Parse(string.Format("{0}-{1}-{2}", lid.Language, scr, lid.Region));
        return new NetCultureInfo { lid = lid, text = text };
      }).
      Where(lt => lt != null).
      ToArray();
    //var ser = new XmlSerializer(typeof(NetCultureInfo[]));
    //var fn = Root.unicode + "netCultureInfos.xml";
    //if (File.Exists(fn)) File.Delete(fn);
    //using (var fs = File.OpenWrite(fn))
    //  ser.Serialize(fs, texts);
    return texts;
  }

  static string getDateTextx(CultureInfo lc) {
    var texts = new string[3];
    var fmt = lc.DateTimeFormat;
    texts[0] = fmt.MonthNames.Take(12).Aggregate((r, i) => r + "*" + i);
    texts[1] = fmt.MonthGenitiveNames.Take(12).Aggregate((r, i) => r + "*" + i);
    if (texts[0] == texts[1]) texts[1] = null;
    texts[2] = fmt.DayNames.Take(7).Aggregate((r, i) => r + "*" + i);
    return texts.Aggregate((r, i) => r + "\n" + i).ToLower();
  }

  public class NetCultureInfo {
    public string text;
    [XmlIgnore]
    public LocaleIdentifier lid;
    public string loc { get { return lid.ToString(); } set { } }
  }



  public static void Install(bool refreshCldrInfo = false, bool withInstallation = false) {

    if (withInstallation) Cldr.Instance.DownloadLatestAsync().Wait();

    //var lt = LangTexts(new Locale(LocaleIdentifier.Parse("cs").MostLikelySubtags()));
    //return;

    // get raw source from all CLDR file names
    var allCldrFiles = allLangIdsFromCldrFiles();

    // ISO15924 alphabet names
    var ISO15924 = LangsLib.UnicodeBlockNames.ISO15924.ToHashSet();

    // get all specific langs (e.g. cs-Latn-CZ)
    var allSpecifics = allCldrFiles.
      Select(l => LocaleIdentifier.Parse(l).MostLikelySubtags()).
      Where(l => l.Script != "Cakm" && l.ToString().IndexOf("valencia") < 0).
      Distinct(LocaleIdentifierEqualityComparer.Instance).
      OrderBy(s => s.ToString()).
      ToArray();

    // add .NET
    var netSpecifics = getNetCultureInfos(allSpecifics);
    var fromNet = netSpecifics.ToDictionary(n => n.lid, n => n.text, LocaleIdentifierEqualityComparer.Instance);
    allSpecifics = allSpecifics.
      Concat(netSpecifics.Select(ns => ns.lid)).
      OrderBy(ns => ns, LocaleIdentifierEqualityComparer.Instance).
      ToArray();

    // check all unicode scripts exists: missingInUnicodeScripts.Length must be 0
    var allScripts =
      allSpecifics.Select(lid => lid.Script).Distinct().OrderBy(s => s).ToArray();

    var missingInUnicodeScripts = // hans, hant, jpan, kore ise replaced in langs.cs
      allScripts.Where(n => !ISO15924.Contains(n)).Except(new string[] { "Hans", "Hant", "Jpan", "Kore" }).ToArray();

    if (missingInUnicodeScripts.Length > 0)
      throw new Exception();

    // **************  final result

    // specifics with more than one SCRIPT
    var moreScriptsPerLang = allSpecifics.
      GroupBy(lid => lid.Language).
      Select(glid => new { glid.Key, scriptCount = glid.Select(lid => lid.Script).Distinct().ToArray() }).
      Where(d => d.scriptCount.Length > 1).
      Select(g => g.Key).
      ToHashSet();

    // ************* NORMALIATION
    // cs-Latn-CZ => cs-CZ, sr-Latn-SR => sr-Latn-SR
    var removedCZScript = allSpecifics.ToDictionary(s => s, s => {
      if (!moreScriptsPerLang.Contains(s.Language)) return LocaleIdentifier.Parse(s.Language + "-" + s.Region);
      return LocaleIdentifier.Parse(s.Language + "-" + s.Script + "-" + s.Region);
    }, LocaleIdentifierEqualityComparer.Instance);

    // cs-Latn-CZ => cs-CZ, sr-Latn-SR => sr-Latn
    var removeCZScriptSRregion = allSpecifics.ToDictionary(s => s, s => {
      if (!moreScriptsPerLang.Contains(s.Language)) return LocaleIdentifier.Parse(s.Language + "-" + s.Region);
      return LocaleIdentifier.Parse(s.Language + "-" + s.Script);
    }, LocaleIdentifierEqualityComparer.Instance);

    // cs-Latn-CZ => cs, sr-Latn-SR => sr-Latn
    var removeRegionCZScript = allSpecifics. //Where(l => l.Language == "kr").
      Select(s => {
        if (!moreScriptsPerLang.Contains(s.Language)) return LocaleIdentifier.Parse(s.Language);
        return LocaleIdentifier.Parse(s.Language + "-" + s.Script);
      }).
      Distinct(LocaleIdentifierEqualityComparer.Instance).
      ToArray();

    var wrongs = removeRegionCZScript.Select(l => l.MostLikelySubtags()).Except(allSpecifics, LocaleIdentifierEqualityComparer.Instance).ToArray();
    if (wrongs.Length>2) //{kr-Latn-ZZ},  {syr-Syrc-IQ}
      throw new Exception();

    // TEXTS
    Dictionary<LocaleIdentifier, string> texts;
    if (refreshCldrInfo) {
      texts = allSpecifics.ToDictionary(
        s => s,
        s => fromNet.TryGetValue(s, out string netText) ? netText : getLangTexts(new Locale(s)),
        LocaleIdentifierEqualityComparer.Instance
      );
      saveCldrInfo(texts);
    } else
      texts = loadCldrInfo();

    // for every allRootLangs: groups by TEXT
    var langTextGroups = removeRegionCZScript.ToDictionary(
      l => l,
      l => allSpecifics.
      Where(ll => ll.Language == l.Language && (l.Script == "" || l.Script == ll.Script)).
      GroupBy(ll => texts[ll]).
      ToDictionary(g => g.Key, g => g.ToArray())
    );

    string[] scriptIdParts; LocaleIdentifier specific; LocaleIdentifier[] subLangs; LocaleIdentifier specificSubLang;

    var cldr = removeRegionCZScript.
      SelectMany(rootLang => new CldrLang[] { new CldrLang {
          texts = texts.TryGetValue(specific = rootLang.MostLikelySubtags(), out string specificText) ? specificText : null,
          id = specificText == null ? null : removeCZScriptSRregion[specific].ToString(), // e.g. cs-CZ
          isDefault = true,
          specific = specific.ToString(),
          scriptId = specific.Script, // e.g. latn
          scriptIdParts = scriptIdParts = specific.Script == "Jpan" ? new string[] { "Hani", "Hira", "Kana" } : (specific.Script == "Kore" ? new string[] { "Hani", "Hang" } : (specific.Script == "Hant" || specific.Script == "Hans" ? new string[] { "Hani" } : null)),
          theSame = specificText == null ? null : langTextGroups[rootLang][specificText].Select(l => removedCZScript[l].ToString()).OrderBy(s => s).ToArray(), // specifics with same text
        } }.
      Concat(
          langTextGroups[rootLang].
          Where(kv => kv.Key != specificText). // ... different from rootLang text ...
          Select(kv => new CldrLang {
            scriptId = specific.Script,
            scriptIdParts = scriptIdParts,
            texts = kv.Key,
            theSame = (subLangs = kv.Value).Select(kvv => removedCZScript[kvv].ToString()).OrderBy(s => s).ToArray(),
            id = removedCZScript[specificSubLang = selectSecodnaryLocale(subLangs)].ToString(),
            specific = specificSubLang.ToString(),
          }))).
      Where(c => c.id != null).
      OrderBy(c => c.id.Split('-')[0]).
      ThenByDescending(c => c.isDefault).
      ToArray();

    var ser = new XmlSerializer(typeof(CldrLang[]));
    if (File.Exists(LangsLib.Root.cldr)) File.Delete(LangsLib.Root.cldr);
    using (var fs = File.OpenWrite(LangsLib.Root.cldr))
      ser.Serialize(fs, cldr);

    var moreSame = cldr.Where(c => c.theSame.Length > 1).Select(c => c.theSame).ToArray();
    var moreSameSer = new XmlSerializer(typeof(string[][]));
    var fn = Root.unicode + "moreSame.xml";
    using (var fs = File.OpenWrite(fn))
      moreSameSer.Serialize(fs, moreSame);

    var cldrLangsHash = new HashSet<string>(cldr.Select(c => c.id.ToLower()));
    var notInOldLangs = oldLangs.Where(c => !cldrLangsHash.Contains(c)).ToArray();
    File.WriteAllLines(Root.unicode + "notInOldLangs.xml", notInOldLangs);
    if (notInOldLangs.Length > 16)
      throw new Exception();

    // comparu CLDR with NET
    //var allNetLangs = allSpecifics.Select(cu => CultureInfo.GetCultureInfo(cu.ToString())).Select(cu => cu.Name).Distinct().OrderBy(s => s).ToArray();

    var allLangsCount = allSpecifics.Select(l => l.Language).Distinct().Count();
    var allScriptsCount = allSpecifics.Select(l => l.Script).Distinct().Count();
    var allDefaultsCount = cldr.Where(c => c.isDefault).Count();
    var allVariantsCount = cldr.Count();
    var allCount = allSpecifics.Count();
    File.WriteAllText(Root.unicode + "cldrStatistics.txt", string.Format(@"
languages: {0}
alphabets: {1}
languages x alphabets: {2}
languages x alphabets x language variants: {3}
languages x alphabets x language variants x countries: {4}
", allLangsCount, allScriptsCount, allDefaultsCount, allVariantsCount, allCount));

  }



  static Dictionary<LocaleIdentifier, string> loadCldrInfo() {
    var serInfo = new XmlSerializer(typeof(CldrInfo[]));
    CldrInfo[] infos;
    using (var fs = File.OpenRead(Root.unicode + "cldrInfos.xml"))
      infos = serInfo.Deserialize(fs) as CldrInfo[];
    return infos.
      SelectMany(inf => inf.ids.Select(id => new { inf.texts, id })).
      ToDictionary(inf => LocaleIdentifier.Parse(inf.id), inf => inf.texts, LocaleIdentifierEqualityComparer.Instance);
  }
  static void saveCldrInfo(Dictionary<LocaleIdentifier, string> texts) {
    var infos = texts.GroupBy(kv => kv.Value).Select(g => new CldrInfo { texts = "\n" + g.Key + "\n", ids = g.Select(l => l.Key.ToString()).ToArray() }).ToArray();
    var serInfo = new XmlSerializer(typeof(CldrInfo[]));
    var fn = Root.unicode + "cldrInfos.xml";
    if (File.Exists(fn)) File.Delete(fn);
    using (var fs = File.OpenWrite(Root.unicode + "cldrInfos.xml"))
      serInfo.Serialize(fs, infos);
  }

  static LocaleIdentifier selectSecodnaryLocale(LocaleIdentifier[] langs) {
    var ll = langs.FirstOrDefault(l => prioLangs.Contains(l.ToString()));

    var allOld = langs.Where(l => oldLangs.Contains(l.ToString().ToLower())).ToArray();
    if (allOld.Length == 1) return allOld[0];

    ll = langs.FirstOrDefault(l => l.Language == l.Region.ToLower());
    if (ll != null) return ll;
    return langs.First();
  }
  static HashSet<string> prioLangs = new HashSet<string>() {
    "ar-SA",
    //"sw-KE"
  };

  static HashSet<string> oldLangs = new HashSet<string>(Enum.GetNames(typeof(LangsLib.langs)).Select(n => n.Replace('_', '-')));

  public class CldrLang {
    public string id;
    [DefaultValue(false)]
    public bool isDefault;
    public string scriptId;
    public string specific;
    public string[] theSame;
    public string[] scriptIdParts;
    [XmlIgnore]
    public string texts;
  }

  public class CldrInfo {
    public string[] ids;
    public string texts;
  }

  static string[] allLangIdsFromCldrFiles() {
    return allDirs.SelectMany(dir => Directory.GetFiles(Root.cldrRepo + dir)).Select(f => Path.GetFileNameWithoutExtension(f)).Select(f => f.ToLower()).Where(f => f.IndexOf("posix") < 0).Distinct().OrderBy(f => f).ToArray();
  }

  static string[] allDirs = new string[] {
@"common\annotations\",
@"common\annotationsDerived\",
@"common\casing\",
@"common\collation\",
@"common\main\",
@"common\rbnf\",
@"common\segments\",
@"common\subdivisions\",
    };

  public static string getLangTexts(Locale loc) {

    var res = new string[11];

    //if (loc.ToString() != "af-Latn-NA" && loc.ToString() != "af-Latn-ZA") return "";

    // **** GREGORIAN
    XPathNavigator gregorian;

    try {
      gregorian = loc.Find("//calendar[@type=\"gregorian\"]//monthContext[@type=\"stand-alone\"]/monthWidth[@type=\"wide\"]");
      res[0] = agregate(gregorian.Select("./*/text()").Cast<object>().Select(o => o.ToString()));
    } catch { }
    try {
      gregorian = loc.Find("//calendar[@type=\"gregorian\"]//monthContext[@type=\"format\"]/monthWidth[@type=\"wide\"]");
      res[1] = agregate(gregorian.Select("./*/text()").Cast<object>().Select(o => o.ToString()));
      if (res[0] == res[1]) res[1] = null;
    } catch { }

    try {
      gregorian = loc.Find("//calendar[@type=\"gregorian\"]//dayContext[@type=\"stand-alone\"]/dayWidth[@type=\"wide\"]");
      if (loc.ToString() != "lrc-Arab-IQ" && loc.ToString() != "lrc-Arab-IR" && loc.ToString() != "mzn-Arab-IR")
        res[2] = agregate(gregorian.Select("./*/text()").Cast<object>().Select(o => o.ToString()));
    } catch { }
    try {
      gregorian = loc.Find("//calendar[@type=\"gregorian\"]//dayContext[@type=\"format\"]/dayWidth[@type=\"wide\"]");
      if (loc.ToString() != "lrc-Arab-IQ" && loc.ToString() != "lrc-Arab-IR" && loc.ToString() != "mzn-Arab-IR")
        res[3] = agregate(gregorian.Select("./*/text()").Cast<object>().Select(o => o.ToString()));
      if (res[2] == res[3]) res[3] = null;
    } catch { }


    // **** SPELL NUMBERS
    try {
      var spell = SpellingFormatter.Create(loc, new SpellingOptions { Style = SpellingStyle.Cardinal });
      res[4] = doCatch(() => agregate(Enumerable.Range(0, 21).Select(n => spell.Format(n))));
    } catch { }
    try {
      var spell = SpellingFormatter.Create(loc, new SpellingOptions { Style = SpellingStyle.Ordinal });
      res[5] = doCatch(() => agregate(Enumerable.Range(0, 21).Select(n => spell.Format(n))));
      if (res[4] == res[5]) res[5] = null;
    } catch { }

    // ALPHABETS
    try {
      res[6] = loc.Find("//characters/exemplarCharacters[not(@type)]/text()").ToString();
    } catch { }
    try {
      res[7] = loc.Find("//characters/exemplarCharacters[@type=\"auxiliary\"]/text()").ToString();
    } catch { }
    try {
      if (Array.IndexOf(new string[] { "zh-Hans-CN", "zh-Hans-HK", "zh-Hans-MO", "zh-Hans-SG", "yue-Hans-CN" }, loc.ToString()) < 0)
        res[8] = loc.Find("//characters/exemplarCharacters[@type=\"index\"]/text()").ToString();
    } catch { }
    try {
      res[9] = loc.Find("//characters/exemplarCharacters[@type=\"numbers\"]/text()").ToString();
    } catch { }

    // force lang distinction
    switch (loc.ToString()) {
      case "en-Latn-GB":
        res[10] = "en-Latn-GB"; break;
    }

    // RETURN
    var str = res.Aggregate((r, i) => r + "\n" + i).ToLower();

    if (loc.ToString() == "be-Cyrl-BY")
      str = str.Replace('i', 'і');

    str = expandUnicodeCodes(str);

    var wrongs = LangsLib.UnicodeBlockNames.checkBlockNames(str, loc.Id.Script, true);
    if (wrongs != null) {
      wrongData[loc.ToString()] = wrongs;
      //ERROR: lo-Laoo-LA some thai chars
      //be-Cyrl-BY 'i' latin char
      //ja-Jpan-JP //unicode hani, hira, kana, script Jpan 
      //ko-Kore-KP //unicode hani, hang script Kore 
      //ko-Kore-KR //unicode hani, hang script Kore 
      //*** res[2,3] in english:
      //mzn-Arab-IR
      //lrc-Arab-IQ
      //lrc-Arab-IR 
      //*** index chars in english
      //yue-Hans-CN 
      //zh-Hans-CN 
      //zh-Hans-HK 
      //zh-Hans-MO 
      //zh-Hans-SG 
      //*** unicode hani, script Hans
      //yue-Hant-HK  
      //zh-Hant-HK 
      //zh-Hant-MO 
      //zh-Hant-TW 
      return "**** ERROR: WRONG UNICODE CHARS";
    }
    return str;
  }

  // DEBUG: langs with wrong UNICODE alphabet texts
  static Dictionary<string, Dictionary<string, string>> wrongData = new Dictionary<string, Dictionary<string, string>>();

  static string doCatch(Func<string> fnc) {
    try { return fnc(); } catch { return null; }
  }

  static string agregate(IEnumerable<string> data) {
    return data.DefaultIfEmpty().Aggregate((r, i) => r + "*" + i);
  }

  // expands \u00FF part of CLDR string
  static string expandUnicodeCodes(string str) {
    var parts = str.Split(new string[] { "\\u" }, StringSplitOptions.None);
    if (parts.Length == 1) return str;
    return parts.Aggregate((r, i) => {
      var hex = i.Substring(0, 4);
      return r + Convert.ToChar(int.Parse(hex, System.Globalization.NumberStyles.HexNumber)) + i.Substring(4);
    });
  }

}

/*
????
pa-in => ? pa-Arab, pa-Guru ?
zh-hk => ? zh-Hans-HK or zh-Hant-HK
zh-mo => ? zh-Hans-MO or zh-Hant-MO
quz-pe => ? qu-PE ?

REPLACE
sw-ke => sw-TZ is default lang
bn-in => bn-BD is default lang.
ha-latn-ng => ha-NG is defaul lang

zh-tw => zh-Hant
az-latn-az => az-Latn
uz-latn-uz => uz-latn
zh-sg => zh-Hans-SG
zh-cn => zh-Hans (zh-Hans-CN)
sr-latn-cs => sr-Latn
sr-cyrl-cs => sr-cyrl
bs-latn-ba => bs-Latn
*/

