using Sepia.Globalization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public static class Langs {

  public class CldrLang {
    public string Id; // e.g. cs-CZ, sr-Latn, 1 for invariant locale
    public string Lang;
    public string ScriptId; // unicode script, e.g. Latn, Arab etc.
    public string DefaultRegion;
    public bool HasMoreScripts;
    public bool HasStemming; // for DART
    public string Alphabet;
    public string AlphabetUpper;

    public string Name;
    public string[] Regions; // other regions for given <id>
    public int LCID;
    public string StemmerClass;
    public string BreakerClass;
    //[DefaultValue(false)]
    //public bool IsEuroTalk;
    //[DefaultValue(false)]
    //public bool IsGoethe;
    //[DefaultValue(false)]
    //public bool IsLingea;
    public string GoogleTransId;
    public int WordSpellCheckLCID;
    public string iso3 { get => _iso3 ?? (_iso3 = Iso1_3.encode(Lang)); }
    string _iso3;
  }

  public const string invariantId = "xal-US";

  public static void save() {
    Json.Serialize(LangsDirs.dirCldrTexts, meta);
  }

  public static CldrLang[] meta {
    get {
      return _meta ?? (_meta = Json.DeserializeAssembly<CldrLang[]>(LangsDirs.resCldrTexts));
    }
  }
  static CldrLang[] _meta;

  static public IEnumerable<LocaleIdentifier> getFullNames(CldrLang c) {
    return c.Regions.Select(r => LocaleIdentifier.Parse(string.Format("{0}-{1}-{2}", c.Lang, c.ScriptId, r)));
  }

  public static Dictionary<string, CldrLang> fullNameToMeta {
    get {
      return _fullNameToMeta ?? (_fullNameToMeta = meta.
        SelectMany(c => getFullNames(c).Select(loc => new { c, loc })).
        ToDictionary(s => s.loc.ToString(), s => s.c)
      );
    }
  }
  static Dictionary<string, CldrLang> _fullNameToMeta;

  public static Dictionary<string, CldrLang> nameToMeta {
    get => _nameToMeta ?? (_nameToMeta = meta.ToDictionary(s => s.Id));
  }
  static Dictionary<string, CldrLang> _nameToMeta;

  public static Dictionary<string, CldrLang> iso3ToMeta {
    get => _iso3ToMeta ?? (_iso3ToMeta = meta.ToDictionary(s => s.iso3));
  }
  static Dictionary<string, CldrLang> _iso3ToMeta;

  public class Old2New {
    public string o;
    public string n;
  }

  public static string oldToNew(string old) {
    if (old == null) old = "";
    old = old.Replace('_', '-').ToLower();
    var data = _oldToNew ?? (_oldToNew = Json.DeserializeAssembly<Old2New[]>(LangsDirs.resOld2New).ToDictionary(on => on.o, on => on.n));

    var res = data.TryGetValue(old, out string n) ?
      n : LocaleIdentifier.TryParse(old, out LocaleIdentifier lci) && nameToMeta.TryGetValue(lci.ToString(), out CldrLang meta) ?
      meta.Id : string.Format("?{0}", old);
    return res;
  }
  static Dictionary<string, string> _oldToNew;

}

public static class LangsDirs {
  public static string root = LowUtilsDirs.root + @"langs\";
  public static string res = LowUtilsDirs.res + "langs.";
  public static string dirCldrTexts = root + "cldrTexts.json";
  public static string resCldrTexts = res + "cldrTexts.json";
  public static string dirOld2New = root + "old2New.json";
  public static string resOld2New = res + "old2New.json";

  public static string dartRoot = AppDomain.CurrentDomain.BaseDirectory[0] + @":\rewise\dlibs\utils\lib\src\langs\";
  public static string dartRootOld2New = dartRoot + "data_oldToNew.dart";
  public static string dartLangsData = dartRoot + "data_langsData.dart";
  public static string dartUnicodeBlocks = dartRoot + "data_unicodeData.dart";
}