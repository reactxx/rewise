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

  public static void designTimeRebuild() {
    var Items = new Dictionary<int, Meta>();
    SqlServerReg.Parse(Items, LangsDesignDirs.other + "sqlserver.reg", LangsDesignDirs.other + "sqlserver-clsids.reg");
    SqlServerQuery.Parse(Items, LangsDesignDirs.other + "sqlserver.query");
    ByHand.Parse(Items, LangsDesignDirs.other + "by-hand.xml");
    foreach (var nv in Items) {
      nv.Value.LCID = nv.Key == 0 ? CultureInfo.InvariantCulture.LCID : nv.Key;
      nv.Value.lc = CultureInfo.GetCultureInfo(nv.Value.LCID);
      nv.Value.id = nv.Value.lc.Name.ToLower();
      nv.Value.nameEng = nv.Value.lc.EnglishName;
    }

    var arr = Items.Values.OrderBy(m => m.id).ToArray();
    var fn = LangsDirs.root + "dump.json";
    if (File.Exists(fn)) File.Delete(fn);
    Json.Serialize(fn, arr);

    fn = LangsDirs.root + "empty.json";
    if (File.Exists(fn)) File.Delete(fn);
    var empty = arr.Select(a => new Meta {
      LCID = a.LCID,
      id = a.id,
      nameEng = a.nameEng,
      //Alphabet = " ",
    }).ToArray();
    Json.Serialize(fn, empty);
  }

}

public class Meta {
  public string id; // e.g. 'cs-cz'
  [DefaultValue(4096)]
  public int LCID;
  public string nameEng;
  public string name;

  public string wBreakerClass; // WBreakerClass from sqlserver.reg
  public string stemmerClass; // StemmerClass from sqlserver.reg
  [DefaultValue(false)]
  public bool SqlSupportFulltext; // is in "select * from sys.fulltext_languages ORDER BY lcid" Sql query. It seems thant it imply WBreakerClass or StemmerClass

  [DefaultValue(false)]
  public bool isLingea;
  [DefaultValue(false)]
  public bool isEuroTalk;
  [DefaultValue(false)]
  public bool isGoethe;

  //public string HunspellDir; // Hunspell dir, if it is different from Id

  //public string Alphabet;

  [JsonIgnore, XmlIgnore]
  public CultureInfo lc;
}

