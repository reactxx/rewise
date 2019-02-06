using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace LangsLib {

  public static class Root {
    public static string root = AppDomain.CurrentDomain.BaseDirectory[0] + @":\rewise\clibs\low-utils\langs\";
    public static string unicodeBlockNames = root + "unicodeBlockNames.json";
    public static string cldr = root + "cldr.json";
  }

  public static class UnicodeBlockNames {
    static UnicodeBlockNames() {
      var scripts = Json.DeserializeAssembly<UncBlocks>("LangsLib.langs.unicodeBlockNames.json");
      sorted = new SortedList<UncRange, UncRange>(scripts.ranges.ToDictionary(r => r, r => r, RangeComparer.equalityComparer), RangeComparer.comparer);
      blockNames = scripts.blockNames;
      ISO15924 = scripts.ISO15924;
    }
    public static SortedList<UncRange, UncRange> sorted;
    // unicode block names, see word-lists\lang_chars\unicodeBlockNames.cs, https://www.unicode.org/Public/11.0.0/ucd/Scripts.txt and https://unicode.org/Public/UNIDATA/PropertyValueAliases.txt
    public static string[] blockNames;
    public static string[] ISO15924;

    public static bool isLetter(char ch) {
      forSearch.start = forSearch.end = Convert.ToUInt16(ch);
      return sorted.IndexOfKey(forSearch) >= 0;
    }

    public static string filterChars(string data) {
      if (data == null) return null;
      return new string(data.Where(ch => isLetter(ch)).ToArray());
    }

    public static IEnumerable<int> blockIdxs(string str) {
      var res = new HashSet<int>();
      foreach (var ch in str) {
        forSearch.start = forSearch.end = Convert.ToUInt16(ch);
        if (!sorted.TryGetValue(forSearch, out UncRange found)) continue;
        res.Add(found.idx);
      }
      return res;
    }

    public static Dictionary<string, HashSet<char>> getBlockNames(string str, bool isISO15924 = false) {
      var res = new Dictionary<string, HashSet<char>>();
      foreach (var ch in str) {
        forSearch.start = forSearch.end = Convert.ToUInt16(ch);
        if (!sorted.TryGetValue(forSearch, out UncRange found)) continue;
        var name = isISO15924 ? ISO15924[found.idx] : blockNames[found.idx];
        if (!res.TryGetValue(name, out HashSet<char> hs))
          res[name] = hs = new HashSet<char>();
        hs.Add(ch);
      }
      return res;
    }

    public static Dictionary<string, string> checkBlockNames(string str, string script, bool isISO15924 = false) {
      if (string.IsNullOrEmpty(str)) return null;
      var res = new Dictionary<string, HashSet<char>>();
      foreach (var ch in str) {
        forSearch.start = forSearch.end = Convert.ToUInt16(ch);
        if (!sorted.TryGetValue(forSearch, out UncRange found)) continue;
        var name = isISO15924 ? ISO15924[found.idx] : blockNames[found.idx];
        if (script == "Jpan") {
          if (name == "Hani" || name == "Hira" || name == "Kana") continue;
        } else if (script == "Kore") {
          if (name == "Hani" || name == "Hang") continue;
        } else if (script == "Hant" || script == "Hans") {
          if (name == "Hani") continue;
        } else if (name == script)
          continue;
        if (!res.TryGetValue(name, out HashSet<char> hs))
          res[name] = hs = new HashSet<char>();
        hs.Add(ch);
      }
      return res.Count == 0 ? null : res.ToDictionary(b => b.Key, b => new string(b.Value.ToArray()));
    }

    [ThreadStatic]
    static UncRange forSearch;
  }

  public class testScriptResult {
    public int count;
    public int unicodeWrong;
    public int alphaWrong;
    public int auxilityWrong;
  }


  public class Meta {
    [DefaultValue(0)]
    public int LCID;
    public string Id; // e.g. 'cs-cz'
    public string Code;
    public string Name;

    public string WBreakerClass; // WBreakerClass from sqlserver.reg
    public string StemmerClass; // StemmerClass from sqlserver.reg
    [DefaultValue(false)]
    public bool SqlSupportFulltext; // is in "select * from sys.fulltext_languages ORDER BY lcid" Sql query. It seems thant it imply WBreakerClass or StemmerClass

    [DefaultValue(false)]
    public bool IsLingea;
    [DefaultValue(false)]
    public bool IsEuroTalk;
    [DefaultValue(false)]
    public bool IsGoethe;

    public string HunspellDir; // Hunspell dir, if it is different from Id

    public string Alphabet;

    [JsonIgnore, XmlIgnore]
    public CultureInfo lc;
  }

  public class Metas {

    static Dictionary<langs, Meta> items;

    public static Meta get(langs lng) {
      return Items[lng];
    }

    public static Meta get(int LCID) {
      return Items[(langs)LCID];
    }

    public static Dictionary<langs, Meta> Items {
      get {
        if (items == null) {
          var its = Json.DeserializeAssembly<Meta[]>("LangsLib.langs.dump.json");
          foreach (var item in its)
            item.lc = CultureInfo.GetCultureInfo(item.LCID);
          items = its.ToDictionary(it => (langs)it.LCID);
        }
        return items;
      }
    }

    public static void designTimeRebuild() {
      var Items = new Dictionary<int, Meta>();
      SqlServerReg.Parse(Items, Root.root + "sqlserver.reg", Root.root + "sqlserver-clsids.reg");
      SqlServerQuery.Parse(Items, Root.root + "sqlserver.query");
      ByHand.Parse(Items, Root.root + "by-hand.xml");
      foreach (var nv in Items) {
        nv.Value.LCID = nv.Key == 0 ? CultureInfo.InvariantCulture.LCID : nv.Key;
        nv.Value.lc = CultureInfo.GetCultureInfo(nv.Value.LCID);
        nv.Value.Id = nv.Value.lc.Name.ToLower();
        nv.Value.Name = nv.Value.lc.EnglishName;
      }

      var arr = Items.Values.OrderBy(m => m.Id).ToArray();
      var fn = Root.root + "dump.json";
      if (File.Exists(fn)) File.Delete(fn);
      Json.Serialize(fn, arr);

      fn = Root.root + "empty.json";
      if (File.Exists(fn)) File.Delete(fn);
      var empty = arr.Select(a => new Meta {
        LCID = a.LCID,
        Id = a.Id,
        Name = a.Name,
        Alphabet = " ",
      }).ToArray();
      Json.Serialize(fn, empty);
    }

  }

  public struct UncRange {
    public ushort start;
    public ushort end;
    public int idx;
  }

  public class UncBlocks {
    public string[] blockNames;
    public string[] ISO15924;
    public UncRange[] ranges;
  }

  public class RangeComparer : IEqualityComparer<UncRange>, IComparer<UncRange> {
    bool IEqualityComparer<UncRange>.Equals(UncRange x, UncRange y) {
      return x.start.Equals(y.start);
    }

    int IEqualityComparer<UncRange>.GetHashCode(UncRange obj) {
      return obj.start.GetHashCode();
    }

    int IComparer<UncRange>.Compare(UncRange x, UncRange y) {
      if (y.start > x.end) return -1;
      if (y.end < x.start) return 1;
      return 0;
    }

    public static IEqualityComparer<UncRange> equalityComparer = new RangeComparer();
    public static IComparer<UncRange> comparer = new RangeComparer();
  }

  public enum langs {
    _ = 127,
    af_za = 1078,
    ar_sa = 1025,
    as_in = 1101,
    az_latn_az = 1068,
    be_by = 1059,
    bg_bg = 1026,
    bn_in = 1093,
    bo_cn = 1105,
    br_fr = 1150,
    bs_latn_ba = 5146,
    ca_es = 1027,
    co_fr = 1155,
    cs_cz = 1029,
    da_dk = 1030,
    de_de = 1031,
    el_gr = 1032,
    en_gb = 2057,
    en_us = 1033,
    es_es = 3082,
    et_ee = 1061,
    eu_es = 1069,
    fa_ir = 1065,
    fi_fi = 1035,
    fr_fr = 1036,
    ga_ie = 2108,
    gl_es = 1110,
    gu_in = 1095,
    ha_latn_ng = 1128,
    he_il = 1037,
    hi_in = 1081,
    hr_hr = 1050,
    hu_hu = 1038,
    hy_am = 1067,
    id_id = 1057,
    ig_ng = 1136,
    is_is = 1039,
    it_it = 1040,
    ja_jp = 1041,
    ka_ge = 1079,
    km_kh = 1107,
    kn_in = 1099,
    ko_kr = 1042,
    ky_kg = 1088,
    lt_lt = 1063,
    lv_lv = 1062,
    mi_nz = 1153,
    mk_mk = 1071,
    ml_in = 1100,
    mn_mn = 1104,
    mr_in = 1102,
    ms_my = 1086,
    mt_mt = 1082,
    nb_no = 1044,
    nl_nl = 1043,
    nso_za = 1132,
    oc_fr = 1154,
    pa_in = 1094,
    pl_pl = 1045,
    ps_af = 1123,
    pt_br = 1046,
    pt_pt = 2070,
    quz_pe = 3179,
    ro_ro = 1048,
    ru_ru = 1049,
    sk_sk = 1051,
    sl_si = 1060,
    sq_al = 1052,
    sr_cyrl_cs = 3098,
    sr_latn_cs = 2074,
    sv_se = 1053,
    sw_ke = 1089,
    ta_in = 1097,
    te_in = 1098,
    th_th = 1054,
    tn_za = 1074,
    tr_tr = 1055,
    uk_ua = 1058,
    ur_pk = 1056,
    uz_latn_uz = 1091,
    vi_vn = 1066,
    xh_za = 1076,
    yo_ng = 1130,
    zh_cn = 2052,
    zh_hk = 3076,
    zh_mo = 5124,
    zh_sg = 4100,
    zh_tw = 1028,
    zu_za = 1077,
  }

}


