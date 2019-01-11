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
  }

  public class Meta {
    [DefaultValue(0)]
    public int LCID;
    public string Id; // e.g. 'cs-cz'
    public string Code;

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

    [XmlIgnore]
    public CultureInfo lc;
  }

  public class Metas {

    static Dictionary<langs, Meta> items;
    public static Dictionary<langs, Meta> Items {
      get {
        if (items == null) {
          var assembly = Assembly.GetExecutingAssembly();
          var resourceName = "LangsLib.langs.dump.xml";

          var ser = new XmlSerializer(typeof(Meta[]));
          using (Stream stream = assembly.GetManifestResourceStream(resourceName)) {
            var its = ser.Deserialize(stream) as Meta[];
            foreach (var item in its)
              item.lc = CultureInfo.GetCultureInfo(item.LCID);
            items = its.ToDictionary(it => (langs)it.LCID);
          }
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
      }

      var arr = Items.Values.OrderBy(m => m.Id).ToArray();
      var fn = Root.root + "dump.xml";
      if (File.Exists(fn)) File.Delete(fn);
      var ser = new XmlSerializer(typeof(Meta[]));
      using (var fs = File.OpenWrite(fn))
        ser.Serialize(fs, arr);
    }

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


