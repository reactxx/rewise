using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

public static class LangsDesignLib {

  public static void Build() {

    Console.WriteLine("CldrDesignLib.RefreshCldrDataSource");
    CldrDesignLib.RefreshCldrDataSource();

    Console.WriteLine("UnicodeDesignLib.getUnicodeBlockNames");
    UnicodeDesignLib.getUnicodeBlockNames();

    Console.WriteLine("CldrDesignLib.RefreshNetSuportedCultures");
    CldrDesignLib.RefreshNetSuportedCultures();
    Console.WriteLine("CldrDesignLib.RefreshOldToNew");
    CldrDesignLib.RefreshOldToNew();
    Console.WriteLine("CldrDesignLib.RefreshTexts");
    CldrDesignLib.RefreshTexts();
    Console.WriteLine("CldrDesignLib.Build");
    CldrDesignLib.Build();

    Console.WriteLine("CldrTrans.Build");
    CldrTrans.Build();

    Console.WriteLine("LangsDesignLib.RefreshOldVersionInfo");
    LangsDesignLib.RefreshOldVersionInfo();
    Console.WriteLine("LangsDesignLib.MergeOldToCldr");
    LangsDesignLib.MergeOldToCldr();

    Console.WriteLine("CldrDesignLib.RefreshCldrStatistics");
    CldrDesignLib.RefreshCldrStatistics();

    Console.WriteLine("CldrDesignLib.BuildDart");
    CldrDesignLib.BuildDart();
    Console.WriteLine("CldrDesignLib.UnicodeDart");
    CldrDesignLib.UnicodeDart();

  }

  public static LangMatrixRow adjustNewfulltextDataRow(Dictionary<string, LangMatrixRow> res, string lang) {
    if (!res.TryGetValue(lang, out LangMatrixRow row)) res.Add(lang, row = new LangMatrixRow {
      lang = lang,
      row = new string[10],
      columnNames = new string[] { "0_breakGuid", "1_stemmGuid", "2_isEuroTalk", "3_isLingea", "4_isGoethe", "5_sqlQuery", "6_lcid", "7_GoogleTransApi", "8_MSWordCheck", "9_English" }
    });
    return row;
  }

  public static void RefreshOldVersionInfo() {
    var Items = new Dictionary<string, LangMatrixRow>();
    SqlServerReg.Parse(Items, LangsDesignDirs.other + "sqlserver.reg", LangsDesignDirs.other + "sqlserver-clsids.reg");
    SqlServerQuery.Parse(Items, LangsDesignDirs.other + "sqlserver.query");
    ByHand.Parse(Items, LangsDesignDirs.other + "by-hand.xml");
    GoogleTrans.Parse(Items);
    SpellCheck.Parse(Items);
    foreach (var kv in Items) {
      var ci = CultureInfo.GetCultureInfo(kv.Value.lang);
      var lcid = ci.LCID;
      if (lcid != 4096) kv.Value.row[6] = lcid.ToString();
      kv.Value.row[9] = ci.EnglishName;
    }
    var fullText = new LangMatrix(
      Items.Select(kv => kv.Value)
    );
    fullText.save(LangsDesignDirs.otherappdata + "oldVersionInfo.csv");

    var wrongs = LangMatrix.readLangs(LangsDesignDirs.otherappdata + "oldVersionInfo.csv").Where(l => !Langs.nameToMeta.ContainsKey(l)).ToArray();
    if (wrongs.Length > 1) // "", LCID 127
      throw new Exception();
  }

  public static void MergeOldToCldr() {
    var olds = new LangMatrix(LangsDesignDirs.otherappdata + "oldVersionInfo.csv");
    olds.langs.ForEach((l, idx) => {
      var cldr = Langs.nameToMeta[Langs.oldToNew(l)];
      var old = olds.data[idx];
      cldr.BreakerClass = old[0];
      cldr.StemmerClass = old[1];
      cldr.IsEuroTalk = old[2] != null;
      cldr.IsLingea = old[3] != null;
      cldr.IsGoethe = old[4] != null;
      int.TryParse(old[6], out cldr.LCID);
      cldr.GoogleTransId = old[7];
      cldr.WordSpellCheckLCID = old[8] != null ? int.Parse(old[8]) : 0;
      cldr.Name = old[9];
    });
    // prepare langGuids
    var withGuid = Langs.meta.
      Where(m => (m.BreakerClass != null) && m.Lang != "zh" && m.Id != "en-US" && m.Lang != "pt" && m.Lang != "sr");
    var dupls2 = withGuid.GroupBy(m => m.Lang).Where(g => g.Count() > 1).ToArray();
    var dupls = withGuid.GroupBy(m => m.Lang).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
    var langGuids = withGuid.Where(m => !dupls.Contains(m.Lang)).ToDictionary(m => m.Lang, m => new { m.StemmerClass, m.BreakerClass });
    // prepare scriptGuids
    withGuid = Langs.meta.
      Where(m => (m.BreakerClass != null) && m.Lang != "zh" && m.Id != "en-US" && m.Lang != "pt" && m.Lang != "sr" && m.Id != "ur-PK");
    dupls2 = withGuid.GroupBy(m => m.ScriptId).Where(g => g.Count() > 1).ToArray();
    dupls = withGuid.GroupBy(m => m.ScriptId).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
    var scriptGuids = withGuid.Where(m => !dupls.Contains(m.ScriptId)).ToDictionary(m => m.ScriptId, m => m.BreakerClass);

    //expand breaking and stemming GUID to other langs
    foreach (var m in Langs.meta.Where(m => m.StemmerClass == null && m.BreakerClass == null && langGuids.ContainsKey(m.Lang))) {
      var lg = langGuids[m.Lang];
      m.StemmerClass = lg.StemmerClass;
      m.BreakerClass = lg.BreakerClass;
    }

    foreach (var m in Langs.meta.Where(m => m.BreakerClass == null && scriptGuids.ContainsKey(m.ScriptId))) {
      m.BreakerClass = scriptGuids[m.ScriptId];
    }
    // alphabets
    var alphs = new LangMatrix(LangsDesignDirs.cldr + "alphaRoot.csv");
    var ignAlphas = new HashSet<string>() { "Hant", "Hans", "Jpan", "Kore" };
    Langs.meta.ForEach(m => {
      m.Alphabet = "";
      m.AlphabetUpper = null;
      if (!ignAlphas.Contains(m.ScriptId))
        Langs.getFullNames(m).ForEach(n => m.Alphabet += alphs[n.ToString(), 0]);
      finishAlphabet(m);
      if (m.Alphabet.Length == 0)
        m.Alphabet = null;
      else {
        m.Alphabet = new String(m.Alphabet.Distinct().OrderBy(ch => ch).ToArray());
        m.AlphabetUpper = m.Alphabet.ToUpper();
      }
    });

    Langs.save();
  }

  static void finishAlphabet(Langs.CldrLang meta) {
    switch (meta.Id) {
      case "ar-SA": meta.Alphabet += "\x64e\x652\x650\x64f\x651\x64d\x64c\x640\x64b"; break;
      case "bg-BG": meta.Alphabet += "-"; break;
      case "dk-DK": meta.Alphabet += "é"; break;
      case "en-GB": meta.Alphabet += "'"; break;
      case "fr-FR": meta.Alphabet += "'"; break;
      case "he-IL": meta.Alphabet += "\x5b8\x5b4\x5bc\x5b7\x5b0\x5b9\x5b6\x5b5\x5b2\x5c1\x5c2\x5b1"; break;
      case "hi-IN": meta.Alphabet += "\x93e\x94d\x93f\x947\x940\x902\x94b\x941\x942\x93c\x948\x943\x94c\x200d\x949\x90e\x945"; break;
      case "hu-HU": meta.Alphabet += "x"; break;
      case "id-ID": meta.Alphabet += "-"; break;
      case "ms-MY": meta.Alphabet += "-"; break;
      case "pt-BR": meta.Alphabet += "-"; break;
      case "th-TH": meta.Alphabet += "\xe48\xe49\xe31\xe35\xe34\xe37\xe39\xe38\xe47\xe36\xe4c\xe46\xe4a\xe4b"; break;
      case "tr-TR": meta.Alphabet += "\xe2\xee\xfb"; break;
      case "uk-UA": meta.Alphabet += "'"; break;
      case "ur-PK": meta.Alphabet += "\x64a\x200d\x643\x649\x651\x6d3\x64f\x650\x64b"; break;
      case "ro-RO": meta.Alphabet += "şţ"; break;
      case "fa-IR": meta.Alphabet += "\x64a\x643\x6c0\x649\x200c"; break;
      case "pt-PT": meta.Alphabet = "-abcdefghijklmnopqrstuvwxyzàáâãçéêíóôõú"; break;
      case "ja-JP": meta.Alphabet = "\x30fc\x3005"; break;
        //case "lt-LT": meta.Alphabet += "á"; break;
        //case "pt-PT": meta.Alphabet += "-"; break;
        //case "fa-IR": meta.Alphabet = "كي٩٨٧٦٥٤٣٢١٠ءآأؤئابةتثجحخدذرزسشصضطظعغفقلمنهؤًٌٍّپچژکگی"; break;
    }
    if (!string.IsNullOrEmpty(meta.Alphabet) && meta.ScriptId == "Latn") meta.Alphabet += "'";
  }
}

