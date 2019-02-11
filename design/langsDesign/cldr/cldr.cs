using Sepia.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class CldrDesignLib {

  public static void RefreshCldrData() {
    Cldr.Instance.DownloadLatestAsync().Wait();
  }

  public static void GetOldToNew() {
    Json.Serialize(LangsDirs.old2New, o2n.Select(o => new Langs.Old2New { o = o.Key, n = o.Value }));
  }

  public static void GetTexts() {
    getAllSpecific(out LocaleIdentifier[] allSpecifics, out LangMatrix.Values[] netSpecifics);

    var isNet = new HashSet<string>(netSpecifics.Select(n => n.lang));
    var cldrInfos = CldrTextLib.fromCldrLocaleIdentifiers(allSpecifics.Where(c => !isNet.Contains(c.ToString()))).ToArray();
    var all = cldrInfos.Concat(netSpecifics).ToArray();

    LangMatrix savedTexts = new LangMatrix(all);
    CldrTextLib.save(savedTexts);

    // CHECK
    var wrongTexts = savedTexts.checkTexts(0, CldrTextLib.compareCount);
    if (wrongTexts.Count > 1)
      throw new Exception();
  }

  public static void Build() {

    getAllSpecific(out LocaleIdentifier[] allSpecifics, out LangMatrix.Values[] netSpecifics);

    // TEXTS
    var savedTexts = CldrTextLib.load();
    var sb = new StringBuilder();
    var texts = savedTexts.locsDir.ToDictionary(
      kv => kv.Key,
      kv => savedTexts[kv.Key].JoinStrings(";", 0, CldrTextLib.compareCount, sb).ToLower(),
      LangMatrix.Comparer
    );

    // ISO15924 alphabet names
    var ISO15924 = UnicodeBlocks.ISO15924.ToHashSet();

    // CHECK
    // check all unicode scripts exists: missingInUnicodeScripts.Length must be 0
    var allScripts =
      allSpecifics.Select(lid => lid.Script).Distinct().OrderBy(s => s).ToArray();
    var missingInUnicodeScripts = // hans, hant, jpan, kore ise replaced in langs.cs
      allScripts.Where(n => !ISO15924.Contains(n)).Except(new string[] { "Hans", "Hant", "Jpan", "Kore" }).ToArray();
    if (missingInUnicodeScripts.Length > 0)
      throw new Exception();

    // ************* NORMALIZATION
    // specifics with more than one SCRIPT
    var moreScriptsPerLang = allSpecifics.
      GroupBy(lid => lid.Language).
      Select(glid => new { glid.Key, scriptCount = glid.Select(lid => lid.Script).Distinct().ToArray() }).
      Where(d => d.scriptCount.Length > 1).
      Select(g => g.Key).
      ToHashSet();

    // cs-Latn-CZ => cs-CZ, sr-Latn-SR => sr-Latn-SR
    var removedCZScript = allSpecifics.ToDictionary(s => s, s => {
      if (!moreScriptsPerLang.Contains(s.Language)) return LocaleIdentifier.Parse(s.Language + "-" + s.Region);
      return LocaleIdentifier.Parse(s.Language + "-" + s.Script + "-" + s.Region);
    }, LangMatrix.Comparer);

    // cs-Latn-CZ => cs-CZ, sr-Latn-SR => sr-Latn
    var removeCZScriptSRRegion = allSpecifics.ToDictionary(s => s, s => {
      if (!moreScriptsPerLang.Contains(s.Language)) return LocaleIdentifier.Parse(s.Language + "-" + s.Region);
      return LocaleIdentifier.Parse(s.Language + "-" + s.Script);
    }, LangMatrix.Comparer);

    // cs-Latn-CZ => cs, sr-Latn-SR => sr-Latn
    var removeCZAllSRRegion = allSpecifics.ToDictionary(s => s, s => {
      if (!moreScriptsPerLang.Contains(s.Language)) return LocaleIdentifier.Parse(s.Language);
      return LocaleIdentifier.Parse(s.Language + "-" + s.Script);
    }, LangMatrix.Comparer);

    // **************  ALL LANGS
    // for every cs, sr-Latn..., its MostLikelySubtags
    var allLangs = allSpecifics.
      Select(s => removeCZAllSRRegion[s].MostLikelySubtags()).
      Distinct(LangMatrix.Comparer).
      OrderBy(id => id.ToString()).
      ToArray();
    // for every allRootLangs: groups by TEXT
    var langTextGroups = allLangs.ToDictionary(
      l => l,
      l => allSpecifics.
      Where(ll => ll.Language == l.Language && (l.Script == "" || l.Script == ll.Script)).
      GroupBy(ll => texts[ll]).
      ToDictionary(g => g.Key, g => g.ToArray())
    );

    //CHECK
    var wrongs = allLangs.Except(allSpecifics, LangMatrix.Comparer).ToArray();
    if (wrongs.Length > 2) //{kr-Latn-ZZ},  {syr-Syrc-IQ}
      throw new Exception();

    //FINAL RESULT
    string[] scriptIdParts; bool hasDefault; LocaleIdentifier secondLoc; //string[] actTexts = null;

    var cldr = allLangs.
      SelectMany(specific => Linq.Items(new Langs.CldrLang {
        texts = texts.TryGetValue(specific, out string specificText) ? specificText : null,
        id = specificText == null ? null : removeCZScriptSRRegion[specific.ToString() == "ar-Arab-EG" ? LocaleIdentifier.Parse("ar-Arab-SA") : specific].ToString(), // e.g. cs-CZ
        isDefault = hasDefault = (specificText == null ? false : true),
        scriptId = specific.Script, // e.g. latn
        scriptIdParts = scriptIdParts = specific.Script == "Jpan" ? new string[] { "Hani", "Hira", "Kana" } : (specific.Script == "Kore" ? new string[] { "Hani", "Hang" } : (specific.Script == "Hant" || specific.Script == "Hans" ? new string[] { "Hani" } : null)),
        regions = specificText == null ? null : langTextGroups[specific][specificText].Select(l => l.Region).OrderBy(s => s).ToArray(), // specifics with same text
        //alpha = specificText == null ? null : (actTexts = savedTexts[specific])[CldrTextLib.alphaIdx],
        //alphaAux = actTexts==null ? null : actTexts[CldrTextLib.alphaAuxilityIdx],
      }).
      Concat(
        langTextGroups[specific].
        Where(kv => kv.Key != specificText).
        Select(kv => new Langs.CldrLang {
          scriptId = specific.Script,
          scriptIdParts = scriptIdParts,
          isDefault = !hasDefault ? (hasDefault = true) : false,
          texts = kv.Key,
          regions = kv.Value.Select(kvv => kvv.Region).OrderBy(s => s).ToArray(),
          id = removedCZScript[secondLoc = selectSecodnaryLocale(kv.Value)].ToString(),
          //alpha = (actTexts = savedTexts[secondLoc])[CldrTextLib.alphaIdx],
          //alphaAux = actTexts == null ? null : actTexts[CldrTextLib.alphaAuxilityIdx],
        }))).
      Where(c => c.id != null).
      OrderBy(c => c.id.Split('-')[0]).
      ThenByDescending(c => c.isDefault).
      ToArray();

    //CHECK
    var cldrLangsHash = new HashSet<string>(cldr.Select(c => c.id.ToLower()));
    var notInOldLangs = oldLangs.Select(o => Langs.oldToNew(o).ToLower()).Where(c => c != "-" && !cldrLangsHash.Contains(c)).ToArray();
    if (notInOldLangs.Length > 0)
      throw new Exception();

    //SAVE
    Json.Serialize(LangsDirs.dirCldrTexts, cldr);

    //CldrTrans.Build(
    //  cldr.Select(c => c.id.ToString()).Distinct().OrderBy(s => s).ToArray(),
    //  allSpecifics.Select(l => l.Language).Distinct().OrderBy(s => s).ToArray(),
    //  allSpecifics.Select(l => l.Script).Distinct().OrderBy(s => s).ToArray(),
    //  allSpecifics.Select(l => l.Region).Distinct().OrderBy(s => s).ToArray()
    // );

    //DUMP INFO
    var moreVariants = cldr.
      Select(c => LocaleIdentifier.Parse(c.id)).
      GroupBy(l => l.Language).
      Where(g => g.Count() > 1).
      Select(g => "  " + g.Key.ToString() + ": " + g.Select(gi => gi.ToString()).JoinStrings(",")).
      JoinStrings("\r\n");
    File.WriteAllText(LangsDesignDirs.cldr + "cldrStatistics.txt", string.Format(@"
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

    //CHECK
    // all langs has to have default flag
    if (cldr.Count(l => l.isDefault) != allLangs.Length)
      throw new Exception();

  }

  static void getAllSpecific(out LocaleIdentifier[] allSpecifics, out LangMatrix.Values[] netSpecifics) {

    // get raw source from all CLDR file names
    var allCldrFiles = allLangIdsFromCldrFiles();

    // get all specific langs (e.g. cs-Latn-CZ)
    allSpecifics = allCldrFiles.
      Select(l => LocaleIdentifier.Parse(l).MostLikelySubtags()).
      Where(l => l.Script != "Cakm" && l.ToString().IndexOf("valencia") < 0).
      Distinct(LangMatrix.Comparer).
      OrderBy(s => s.ToString()).
      ToArray();

    // add .NET
    netSpecifics = CldrTextLib.fromNetCultureInfos(allSpecifics).ToArray();

    allSpecifics = allSpecifics.
      Concat(netSpecifics.Select(ns => LocaleIdentifier.Parse(ns.lang))).
      OrderBy(ns => ns, LangMatrix.Comparer).
      ToArray();
  }

  static LocaleIdentifier selectSecodnaryLocale(LocaleIdentifier[] langs) {
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

  static HashSet<string> oldLangs = new HashSet<string>(Enum.GetNames(typeof(LangsLib.langs)).Select(n => n.Replace('_', '-')));

  static string[] allLangIdsFromCldrFiles() {
    return allDirs.
      SelectMany(dir => Directory.GetFiles(LangsDesignDirs.cldrRepo + dir)).
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

