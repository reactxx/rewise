﻿using Newtonsoft.Json;
using Sepia.Globalization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

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

    // cs-Latn-CZ => cs-CZ, sr-Latn-SR => sr-Latn
    var removeCZAllSRRegion = allSpecifics.ToDictionary(s => s, s => {
      if (!moreScriptsPerLang.Contains(s.Language)) return LocaleIdentifier.Parse(s.Language);
      return LocaleIdentifier.Parse(s.Language + "-" + s.Script);
    }, LocaleIdentifierEqualityComparer.Instance);

    // for every Language, its MostLikelySubtags
    var allLangs = allSpecifics.
      Select(s => removeCZAllSRRegion[s].MostLikelySubtags()).
      Distinct(LocaleIdentifierEqualityComparer.Instance).
      ToArray();

    // TEXTS
    CldrTextInfo[] savedTexts;
    if (refreshCldrInfo) {
      var srcTexts = allSpecifics.Select(s => fromNet.TryGetValue(s, out CldrTextInfo netText) ? netText : new CldrTextInfo(s)).ToArray();
      savedTexts = srcTexts.GroupBy(s => s.texts).Select(sg => {
        var res = sg.First();
        res.ids = sg.Select(ss => ss.id).OrderBy(ss => ss, LocaleIdentifierEqualityComparer.Instance).ToArray();
        return res;
      }).
      OrderBy(s => s.idsStr.JoinStrings(" ")).
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

    // Compare locale words:
    var textsDetails = savedTexts.SelectMany(s => s.ids.Select(id => new { id, s })).ToDictionary(
      s => s.id, 
      s => s.s,
      LocaleIdentifierEqualityComparer.Instance
    );
    var diff = textsDetails[LocaleIdentifier.Parse("es-Latn-ES")].diff(textsDetails[LocaleIdentifier.Parse("es-Latn-PE")]).ToArray();

    // EVERY LANGS
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

    string[] scriptIdParts; bool hasDefault; CldrTextInfo actTexts = null; LocaleIdentifier secondLoc;

    var cldr = allLangs.
      SelectMany(specific => new Langs.CldrLang[] { new Langs.CldrLang {
          texts = texts.TryGetValue(specific, out string specificText) ? specificText : null,
          id = specificText == null ? null : removeCZScriptSRRegion[specific.ToString()=="ar-Arab-EG" ? LocaleIdentifier.Parse("ar-Arab-SA") : specific].ToString(), // e.g. cs-CZ
          isDefault = hasDefault = (specificText==null ? false : true),
          scriptId = specific.Script, // e.g. latn
          lang = specific.Language,
          scriptIdParts = scriptIdParts = specific.Script == "Jpan" ? new string[] { "Hani", "Hira", "Kana" } : (specific.Script == "Kore" ? new string[] { "Hani", "Hang" } : (specific.Script == "Hant" || specific.Script == "Hans" ? new string[] { "Hani" } : null)),
          theSame = specificText == null ? null : langTextGroups[specific][specificText].Select(l => removedCZScript[l].ToString()).OrderBy(s => s).ToArray(), // specifics with same text
          alpha = specificText == null ? null : (actTexts = textsDetails[specific]).alpha,
          alphaAux = actTexts==null ? null : actTexts.alphaAuxility,
        } }.
      Concat(
          langTextGroups[specific].
          Where(kv => kv.Key != specificText).
          Select(kv => new Langs.CldrLang {
            scriptId = specific.Script,
            lang = specific.Language,
            scriptIdParts = scriptIdParts,
            isDefault = !hasDefault ? (hasDefault = true) : false,
            texts = kv.Key,
            theSame = kv.Value.Select(kvv => removedCZScript[kvv].ToString()).OrderBy(s => s).ToArray(),
            id = removedCZScript[secondLoc = selectSecodnaryLocale(kv.Value)].ToString(),
            alpha = (actTexts = textsDetails[secondLoc]).alpha,
            alphaAux = actTexts.alphaAuxility,
          }))).
      Where(c => c.id != null).
      OrderBy(c => c.id.Split('-')[0]).
      ThenByDescending(c => c.isDefault).
      ToArray();

    var cldrLangsHash = new HashSet<string>(cldr.Select(c => c.id.ToLower()));
    var notInOldLangs = oldLangs.Select(o => Langs.o2n(o).ToLower()).Where(c => c != "-" && !cldrLangsHash.Contains(c)).ToArray();
    //File.WriteAllLines(Root.unicode + "notInOldLangs.txt", notInOldLangs);
    if (notInOldLangs.Length > 0)
      throw new Exception();

    Json.Serialize(LangsLib.Root.cldr, cldr);
    Json.Serialize(LangsLib.Root.o2n, o2n.Select(o => new Langs.Old2New { o = o.Key, n = o.Value }));

    CldrTrans.Build(
      cldr.Select(c => c.id.ToString()).Distinct().OrderBy(s => s).ToArray(),
      allSpecifics.Select(l => l.Language).Distinct().OrderBy(s => s).ToArray(),
      allSpecifics.Select(l => l.Script).Distinct().OrderBy(s => s).ToArray(),
      allSpecifics.Select(l => l.Region).Distinct().OrderBy(s => s).ToArray()
     ;

    var moreVariants = cldr.
      Select(c => LocaleIdentifier.Parse(c.id)).
      GroupBy(l => l.Language).
      Where(g => g.Count() > 1).
      Select(g => "  " + g.Key.ToString() + ": " + g.Select(gi => gi.ToString()).JoinStrings(",")).
      JoinStrings("\r\n");
    File.WriteAllText(Root.unicode + "cldrStatistics.txt", string.Format(@"
alphabets: {0}
languages: {1}
languages x alphabets: {2}={3}
languages x alphabets x language-variants: {4}
languages x alphabets x language-variants x countries: {5}
more language-variants: 
{6}
",
allSpecifics.Select(l => l.Script).Distinct().Count(),

allSpecifics.Select(l => l.Language).Distinct().Count(),
cldr.Count(l => l.isDefault),
allLangs.Length,

cldr.Count(),
allSpecifics.Count(),

moreVariants
));

    // all langs has to have default flag
    if (cldr.Count(l => l.isDefault) != allLangs.Length)
      throw new Exception();

  }

  static LocaleIdentifier selectSecodnaryLocale(LocaleIdentifier[] langs) {
    //if (langs[0].Script == "Arab")
    //  if (langs[0].Script != "Arab") return null;
    //var ll = langs.FirstOrDefault(l => prioLangs.Contains(l.ToString()));
    //if (ll != null)
    //  return ll;

    if (langs.Length == 1)
      return langs[0];

    var allOld = langs.Where(l => oldLangs.Contains(l.ToString().ToLower())).ToArray();
    if (allOld.Length == 1) // not used
      return allOld[0];

    var ll = langs.FirstOrDefault(l => l.Language == l.Region.ToLower()); // used for pt-PT
    if (ll != null)
      return ll;

    return langs.First();
  }
  //static HashSet<string> prioLangs = new HashSet<string>() {
  //"ar-Arab-SA",
  //"sw-KE"
  //};

  static HashSet<string> oldLangs = new HashSet<string>(Enum.GetNames(typeof(LangsLib.langs)).Select(n => n.Replace('_', '-')));

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

  static Dictionary<string, string> o2n = new Dictionary<string, string> {
    {"pa-in","pa-Guru" },
{"quz-pe","qu-PE" },
{"sw-ke","sw-TZ" },
{"bn-in","bn-BD" },
{"ha-latn-ng","ha-NG" },

{"az-latn-az","az-Latn" },
{"uz-latn-uz","uz-Latn" },
{"bs-latn-ba","bs-Latn" },

{"zh-cn","zh-Hans" },
{"zh-sg","zh-Hans-SG" },

{"zh-tw","zh-Hant" },
{"zh-hk","zh-Hant-HK" },
{"zh-mo","zh-Hant-MO" },

{"sr-latn-cs","sr-Latn" },
{"sr-cyrl-cs","sr-Cyrl" },
  };

}



/*
????
pa-in => ? pa-Arab, pa-Guru ?
quz-pe => ? qu-PE ?

REPLACE
sw-ke => sw-TZ is default lang
bn-in => bn-BD is default lang.
ha-latn-ng => ha-NG is defaul lang

az-latn-az => az-Latn
uz-latn-uz => uz-Latn
bs-latn-ba => bs-Latn

zh-cn => zh-Hans (zh-Hans-CN) // China is simplified
zh-sg => zh-Hans-SG // Singapore is simplified (Hans)

zh-tw => zh-Hant // Taiwan is traditional
zh-hk => zh-Hant-HK // Hong Kong is traditional (Hant)
zh-mo => zh-Hant-MO // Macau is traditional (Hant)

sr-latn-cs => sr-Latn
sr-cyrl-cs => sr-Cyrl

*/

