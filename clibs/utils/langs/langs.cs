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


    public string[] Regions; // other regions for given <id>
    public int LCID;
    public string StemmerClass;
    public string BreakerClass;
    [DefaultValue(false)]
    public bool IsEuroTalk;
    [DefaultValue(false)]
    public bool IsGoethe;
    [DefaultValue(false)]
    public bool IsLingea;
    public string GoogleTransId;
  }

  public const string invariantId = "xal-US";

  public static void save() {
    Json.Serialize(LangsDirs.dirCldrTexts, meta);
  }

  public static CldrLang[] meta { get { return _meta ?? (_meta = Json.DeserializeAssembly<CldrLang[]>(LangsDirs.resCldrTexts)); } }
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
    get {
      return _nameToMeta ?? (_nameToMeta = meta.
        ToDictionary(s => s.Id)
      );
    }
  }
  static Dictionary<string, CldrLang> _nameToMeta;

  public class Old2New {
    public string o;
    public string n;
  }

  public static string oldToNew(string old) {
    if (old == null) old = "";
    old = old.Replace('_','-').ToLower();
    var data = _oldToNew ?? (_oldToNew = Json.DeserializeAssembly<Old2New[]>(LangsDirs.resOld2New).ToDictionary(on => on.o, on => on.n));

    return data.TryGetValue(old, out string n) ? 
      n : LocaleIdentifier.TryParse(old, out LocaleIdentifier lci) && nameToMeta.TryGetValue(lci.ToString(), out CldrLang meta) ?
      meta.Id : string.Format("?{0}", old);
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

//namespace LangsLib {

  //public class Meta {
  //  public string id; // e.g. 'cs-cz'
  //  [DefaultValue(4096)]
  //  public int LCID;
  //  public string nameEng;
  //  public string name;

  //  public string wBreakerClass; // WBreakerClass from sqlserver.reg
  //  public string stemmerClass; // StemmerClass from sqlserver.reg
  //  [DefaultValue(false)]
  //  public bool SqlSupportFulltext; // is in "select * from sys.fulltext_languages ORDER BY lcid" Sql query. It seems thant it imply WBreakerClass or StemmerClass

  //  [DefaultValue(false)]
  //  public bool isLingea;
  //  [DefaultValue(false)]
  //  public bool isEuroTalk;
  //  [DefaultValue(false)]
  //  public bool isGoethe;

  //  //public string HunspellDir; // Hunspell dir, if it is different from Id

  //  //public string Alphabet;

  //  [JsonIgnore, XmlIgnore]
  //  public CultureInfo lc;
  //}

  //public class Metas {

  //  static Dictionary<langs, Meta> items;

  //  public static Meta get(langs lng) {
  //    return Items[lng];
  //  }

  //  public static Meta get(int LCID) {
  //    return Items[(langs)LCID];
  //  }

  //  public static Dictionary<langs, Meta> Items {
  //    get {
  //      if (items == null) {
  //        var its = Json.DeserializeAssembly<Meta[]>(LangsDirs.res + "dump.json");
  //        foreach (var item in its)
  //          item.lc = CultureInfo.GetCultureInfo(item.LCID);
  //        items = its.ToDictionary(it => (langs)it.LCID);
  //      }
  //      return items;
  //    }
  //  }

  //  //public static void designTimeRebuild() {
  //  //  var Items = new Dictionary<int, Meta>();
  //  //  SqlServerReg.Parse(Items, LangsDirs.root + "sqlserver.reg", LangsDirs.root + "sqlserver-clsids.reg");
  //  //  SqlServerQuery.Parse(Items, LangsDirs.root + "sqlserver.query");
  //  //  ByHand.Parse(Items, LangsDirs.root + "by-hand.xml");
  //  //  foreach (var nv in Items) {
  //  //    nv.Value.LCID = nv.Key == 0 ? CultureInfo.InvariantCulture.LCID : nv.Key;
  //  //    nv.Value.lc = CultureInfo.GetCultureInfo(nv.Value.LCID);
  //  //    nv.Value.id = nv.Value.lc.Name.ToLower();
  //  //    nv.Value.nameEng = nv.Value.lc.EnglishName;
  //  //  }

  //  //  var arr = Items.Values.OrderBy(m => m.id).ToArray();
  //  //  var fn = LangsDirs.root + "dump.json";
  //  //  if (File.Exists(fn)) File.Delete(fn);
  //  //  Json.Serialize(fn, arr);

  //  //  fn = LangsDirs.root + "empty.json";
  //  //  if (File.Exists(fn)) File.Delete(fn);
  //  //  var empty = arr.Select(a => new Meta {
  //  //    LCID = a.LCID,
  //  //    id = a.id,
  //  //    nameEng = a.nameEng,
  //  //    //Alphabet = " ",
  //  //  }).ToArray();
  //  //  Json.Serialize(fn, empty);
  //  //}

  //}

//}


