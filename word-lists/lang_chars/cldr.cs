using Sepia.Globalization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

public static class CldrLib {

  public static void Build(bool refreshCldrInfo = false, bool withInstallation = false) {

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
    var netSpecifics = CldrTextInfoLib.getNetCultureInfos(allSpecifics);
    var fromNet = netSpecifics.ToDictionary(n => n.id, LocaleIdentifierEqualityComparer.Instance);

    allSpecifics = allSpecifics.
      Concat(netSpecifics.Select(ns => ns.id)).
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

    // ************* NORMALIZATION
    // cs-Latn-CZ => cs-CZ, sr-Latn-SR => sr-Latn-SR
    var removedCZScript = allSpecifics.ToDictionary(s => s, s => {
      if (!moreScriptsPerLang.Contains(s.Language)) return LocaleIdentifier.Parse(s.Language + "-" + s.Region);
      return LocaleIdentifier.Parse(s.Language + "-" + s.Script + "-" + s.Region);
    }, LocaleIdentifierEqualityComparer.Instance);

    // cs-Latn-CZ => cs-CZ, sr-Latn-SR => sr-Latn
    var removeCZScriptSRRegion = allSpecifics.ToDictionary(s => s, s => {
      if (!moreScriptsPerLang.Contains(s.Language)) return LocaleIdentifier.Parse(s.Language + "-" + s.Region);
      return LocaleIdentifier.Parse(s.Language + "-" + s.Script);
    }, LocaleIdentifierEqualityComparer.Instance);

    // TEXTS
    CldrTextInfo[] savedTexts;
    if (refreshCldrInfo) {
      var srcTexts = allSpecifics.Select(s => fromNet.TryGetValue(s, out CldrTextInfo netText) ? netText : new CldrTextInfo(s)).ToArray();
      savedTexts = srcTexts.GroupBy(s => s.texts).Select(sg => {
        var res = sg.First();
        res.ids = sg.Select(ss => ss.id).OrderBy(ss => ss, LocaleIdentifierEqualityComparer.Instance).ToArray();
        return res;
      }).
      OrderBy(s => s.idsStr.Aggregate((r,i) => r + " " + i)).
      ToArray();
      CldrTextInfoLib.save(savedTexts);
      var wrongTexts = CldrTextInfoLib.checkTexts(savedTexts);
      if (wrongTexts.Count > 1)
        throw new Exception();
    } else
      savedTexts = CldrTextInfoLib.load();

    var texts = savedTexts.SelectMany(s => s.ids.Select(id => new { id, s.texts })).ToDictionary(
        s => s.id,
        s => s.texts,
        LocaleIdentifierEqualityComparer.Instance
      );

    // EVERY LANGS
    // for every Language, its MostLikelySubtags
    var allLangs = allSpecifics.
      Select(s => LocaleIdentifier.Parse(s.Language).MostLikelySubtags()).
      Distinct(LocaleIdentifierEqualityComparer.Instance).
      ToArray();

    var wrongs = allLangs.Except(allSpecifics, LocaleIdentifierEqualityComparer.Instance).ToArray();
    if (wrongs.Length > 2) //{kr-Latn-ZZ},  {syr-Syrc-IQ}
      throw new Exception();

    // for every allRootLangs: groups by TEXT
    var langTextGroups = allLangs.ToDictionary(
      l => l,
      l => allSpecifics.
      Where(ll => ll.Language == l.Language && (l.Script == "" || l.Script == ll.Script)).
      GroupBy(ll => texts[ll]).
      ToDictionary(g => g.Key, g => g.ToArray())
    );

    string[] scriptIdParts; LocaleIdentifier specificSubLang;

    var cldr = allLangs.
      SelectMany(specific => new CldrLang[] { new CldrLang {
          texts = texts.TryGetValue(specific, out string specificText) ? specificText : null,
          id = specificText == null ? null : removeCZScriptSRRegion[specific].ToString(), // e.g. cs-CZ
          isDefault = true,
          specific = specific.ToString(),
          scriptId = specific.Script, // e.g. latn
          scriptIdParts = scriptIdParts = specific.Script == "Jpan" ? new string[] { "Hani", "Hira", "Kana" } : (specific.Script == "Kore" ? new string[] { "Hani", "Hang" } : (specific.Script == "Hant" || specific.Script == "Hans" ? new string[] { "Hani" } : null)),
          theSame = specificText == null ? null : langTextGroups[specific][specificText].Select(l => removedCZScript[l].ToString()).OrderBy(s => s).ToArray(), // specifics with same text
        } }.
      Concat(
          langTextGroups[specific].
          Where(kv => kv.Key != specificText). // ... different from rootLang text ...
          Select(kv => new CldrLang {
            scriptId = specific.Script,
            scriptIdParts = scriptIdParts,
            texts = kv.Key,
            theSame = kv.Value.Select(kvv => removedCZScript[kvv].ToString()).OrderBy(s => s).ToArray(),
            id = removedCZScript[specificSubLang = selectSecodnaryLocale(kv.Value)].ToString(),
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
    if (notInOldLangs.Length > 17)
      throw new Exception();

    var allLangsCount = allSpecifics.Select(l => l.Language).Distinct().Count();
    var allScriptsCount = allSpecifics.Select(l => l.Script).Distinct().Count();
    var moreVariants = cldr.
      Select(c => LocaleIdentifier.Parse(c.id)).
      GroupBy(l => l.Language).
      Where(g => g.Count() > 1).
      Select(g => "  " + g.Key.ToString() + ": " + g.Select(gi => gi.ToString()).Aggregate((r, i) => r + "," + i)).
      Aggregate((r,i) => r + "\r\n" + i);
    var allVariantsCount = cldr.Count();
    var allCount = allSpecifics.Count();
    File.WriteAllText(Root.unicode + "cldrStatistics.txt", string.Format(@"
languages: {0}
alphabets: {1}
languages x alphabets x language variants: {3}
languages x alphabets x language variants x countries: {4}
more variants: 
{2}
", allLangsCount, allScriptsCount, moreVariants, allVariantsCount, allCount));

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

  static string[] allLangIdsFromCldrFiles() {
    return allDirs.
      SelectMany(dir => Directory.GetFiles(Root.cldrRepo + dir)).
      Select(f => Path.GetFileNameWithoutExtension(f)).
      Select(f => f.ToLower()).
      Where(f => f.IndexOf("posix") < 0).
      Distinct().
      OrderBy(f => f).
      ToArray();
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

