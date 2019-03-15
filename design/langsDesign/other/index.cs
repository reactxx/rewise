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

    Console.WriteLine("UnicodeDesignLib.getUnicodeBlockNames");
    UnicodeDesignLib.getUnicodeBlockNames();

    Console.WriteLine("CldrDesignLib.RefreshCldrDataSource");
    CldrDesignLib.RefreshCldrDataSource();
    Console.WriteLine("CldrDesignLib.RefreshNetSuportedCultures");
    CldrDesignLib.RefreshNetSuportedCultures();
    Console.WriteLine("CldrDesignLib.RefreshOldToNew");
    CldrDesignLib.RefreshOldToNew();
    Console.WriteLine("CldrDesignLib.RefreshTexts");
    CldrDesignLib.RefreshTexts();
    Console.WriteLine("CldrDesignLib.Build");
    CldrDesignLib.Build();

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
      row = new string[8],
      columnNames = new string[] { "0_breakGuid", "1_stemmGuid", "2_isEuroTalk", "3_isLingea", "4_isGoethe", "5_sqlQuery", "6_lcid", "7_GoogleTransApi" }
    });
    return row;
  }

  public static void RefreshOldVersionInfo() {
    var Items = new Dictionary<string, LangMatrixRow>();
    SqlServerReg.Parse(Items, LangsDesignDirs.other + "sqlserver.reg", LangsDesignDirs.other + "sqlserver-clsids.reg");
    SqlServerQuery.Parse(Items, LangsDesignDirs.other + "sqlserver.query");
    ByHand.Parse(Items, LangsDesignDirs.other + "by-hand.xml");
    GoogleTrans.Parse(Items);
    foreach (var kv in Items) {
      var lcid = CultureInfo.GetCultureInfo(kv.Value.lang).LCID;
      if (lcid != 4096) kv.Value.row[6] = lcid.ToString();
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
    });
    Langs.save();
  }


    //  public static void designTimeRebuild() {
    //    var Items = new Dictionary<int, Meta>();
    //    SqlServerReg.Parse(Items, LangsDesignDirs.other + "sqlserver.reg", LangsDesignDirs.other + "sqlserver-clsids.reg");
    //    SqlServerQuery.Parse(Items, LangsDesignDirs.other + "sqlserver.query");
    //    ByHand.Parse(Items, LangsDesignDirs.other + "by-hand.xml");
    //    foreach (var nv in Items) {
    //      nv.Value.LCID = nv.Key == 0 ? CultureInfo.InvariantCulture.LCID : nv.Key;
    //      nv.Value.lc = CultureInfo.GetCultureInfo(nv.Value.LCID);
    //      nv.Value.id = nv.Value.lc.Name.ToLower();
    //      nv.Value.nameEng = nv.Value.lc.EnglishName;
    //    }

    //    var arr = Items.Values.OrderBy(m => m.id).ToArray();
    //    var fn = LangsDirs.root + "dump.json";
    //    if (File.Exists(fn)) File.Delete(fn);
    //    Json.Serialize(fn, arr);

    //    fn = LangsDirs.root + "empty.json";
    //    if (File.Exists(fn)) File.Delete(fn);
    //    var empty = arr.Select(a => new Meta {
    //      LCID = a.LCID,
    //      id = a.id,
    //      nameEng = a.nameEng,
    //      //Alphabet = " ",
    //    }).ToArray();
    //    Json.Serialize(fn, empty);
    //  }

    //}

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

    //  public string HunspellDir; // Hunspell dir, if it is different from Id

    //  public string Alphabet;

    //  [JsonIgnore, XmlIgnore]
    //  public CultureInfo lc;
    //}
  }

