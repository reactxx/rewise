using Sepia.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public static class CldrDesignLib {

  public static void exportForWikibulary() {
    var fromMain = Directory.GetFiles(@"d:\wikibulary\data\cldr\common\main", "*.xml", SearchOption.TopDirectoryOnly)
      .Select(fn => Path.GetFileNameWithoutExtension(fn).Replace('_', '-'))
      .Select(l => {
        try {
          var li = LocaleIdentifier.Parse(l);
          return new { lang = l, likely = LocaleIdentifier.Parse(l).MostLikelySubtags().ToString() };
        } catch { return null; }
      });
    var fromMeta = Langs.meta.Select(m => new { lang = m.Id, likely = LocaleIdentifier.Parse(m.Id).MostLikelySubtags().ToString() });

    var res = fromMain.Concat(fromMeta).Where(l => l!=null).GroupBy(l => l.lang).Select(g => new { lang = g.Key, likely = g.First().likely }).ToArray();
    Json.Serialize(@"d:\wikibulary\cs\libs\utils\langs\design\fromRewise.xml", res);
  }

  public static void RefreshCldrDataSource() {
    Cldr.Instance.DownloadLatestAsync().Wait();
  }

  public static void RefreshOldToNew() {
    Json.Serialize(LangsDirs.dirOld2New, o2n.Select(o => new Langs.Old2New { o = o.Key, n = o.Value }));
  }

  public static void BuildDart() {
    var cldr = Json.Deserialize<Langs.CldrLang[]>(LangsDirs.dirCldrTexts);
    var msgs = new RewiseDom.CldrLangs();
    foreach (var cl in cldr) {
      var msg = new RewiseDom.CldrLang {
        HasStemming = cl.StemmerClass != null,
        HasMoreScripts = cl.HasMoreScripts,
        Id = cl.Id,
        Lang = cl.Lang,
        ScriptId = cl.ScriptId,
        Alphabet = cl.Alphabet ?? "",
        AlphabetUpper = cl.AlphabetUpper ?? "",
        GoogleTransId = cl.GoogleTransId ?? "",
        WordSpellCheckLcid = cl.WordSpellCheckLCID,
      };
      msgs.Langs.Add(msg);
      if (cl.DefaultRegion != null) msg.DefaultRegion = cl.DefaultRegion;
    }
    var str = Protobuf.ToBase64(msgs);
    //var str = msgs.ToString();// Json.SerializeStr(cldr, true);

    using (var wr = new StreamWriter(LangsDirs.dartLangsData)) {
      wr.Write(@"
import 'dart:convert';
import 'package:rw_utils/dom/utils.dart';

CldrLangs getLangsData() {
  if (_langsData == null) {
    const res = '");

      wr.Write(str);
      wr.Write(@"';
    _langsData = CldrLangs.fromBuffer(base64.decode(res));
  }
  return _langsData;
}

CldrLangs _langsData;
");
    }
  }

  public static void UnicodeDart() {
    var uniBlocks = Protobuf.FromJson(File.ReadAllText(UnicodeBlocksDirs.dirUnicodeBlocks), () => new RewiseDom.UncBlocks());
    var str = Protobuf.ToBase64(uniBlocks);

    using (var wr = new StreamWriter(LangsDirs.dartUnicodeBlocks)) {
      wr.Write(@"
import 'dart:convert';
import 'package:rw_utils/dom/utils.dart';

UncBlocks getUnicodeData() {
  if (_unicodeData==null) {
    const res = '");

      wr.Write(str);
      wr.Write(@"';
    _unicodeData = UncBlocks.fromBuffer(base64.decode(res));
  }
  return _unicodeData;
}
UncBlocks _unicodeData;
");
    }
  }


  public static void RefreshTexts() {
    getAllSpecific(out LocaleIdentifier[] allSpecifics, out LangMatrixRow[] netSpecifics);

    var isNet = new HashSet<string>(netSpecifics.Select(n => n.lang));
    var cldrInfos = CldrUtils.fromCldrLocaleIdentifiers(allSpecifics.Where(c => !isNet.Contains(c.ToString()))).ToArray();
    var all = cldrInfos.Concat(netSpecifics).ToArray();

    Dictionary<string, Dictionary<string, string>> protocol = new Dictionary<string, Dictionary<string, string>>();
    LangMatrix textMatrix = new LangMatrix(all, protocol);
    if (protocol.Count > 1)
      throw new Exception();

    textMatrix.save(LangsDesignDirs.cldr + "cldrInfos.csv", true);
  }

  //https://msdn.microsoft.com/en-us/globalization/mt778914.aspx
  public static void RefreshNetSuportedCultures() {
    // get .NETsupported cultures (where it has unique non 4096 LCID):
    var wrongLcids = CultureInfo.GetCultures(CultureTypes.AllCultures).
      Select(c => new { c.Name, c.LCID }).
      GroupBy(ni => ni.LCID).
      Where(g => g.Count() > 1).
      Select(g => new { g.Key, dupls = g.Select(gg => gg.Name).ToArray() }).
      ToArray();
    if (wrongLcids.Length > 3) // 4096 (hundreds of items), 4 (2), 31748 (2)
      throw new Exception();
    var lcids = CultureInfo.GetCultures(CultureTypes.AllCultures).
      Select(c => new { c.Name, c.LCID }).
      Where(c => wrongLcids.All(cc => cc.Key != c.LCID)).
      OrderBy(c => c.Name).
      ToArray();
    Json.Serialize(LangsDesignDirs.cldr + @"netLCID.json", lcids);

    File.WriteAllLines(LangsDesignDirs.cldr + @"allNetCultureNames.txt", CultureInfo.GetCultures(CultureTypes.AllCultures).Select(c => c.Name).Distinct().OrderBy(s => s));

  }

  public static void RefreshCldrStatistics() {

    var cldr = Json.Deserialize<Langs.CldrLang[]>(LangsDirs.dirCldrTexts);

    //CHECK missinfOldLangs
    var cldrLangsHash = new HashSet<string>(cldr.Select(c => c.Id.ToLower()));
    var oldLangs = new HashSet<string>(Enum.GetNames(typeof(oldLangs)).Select(n => n.Replace('_', '-')));
    var missinfOldLangs = oldLangs.Select(o => Langs.oldToNew(o).ToLower()).Where(c => !cldrLangsHash.Contains(c)).ToArray();
    if (missinfOldLangs.Length > 0)
      throw new Exception();

    //DUMP INFO
    var moreVariants = cldr.
      Select(c => LocaleIdentifier.Parse(c.Id)).
      GroupBy(l => l.Language).
      Where(g => g.Count() > 1).
      Select(g => "  " + g.Key.ToString() + ": " + g.Select(gi => gi.ToString()).JoinStrings(",")).
      JoinStrings("\r\n");
    File.WriteAllText(LangsDesignDirs.cldr + "cldrStatistics.txt", string.Format(@"
alphabets: {0}
languages: {1}
regions: {3}
languages x alphabets: {2}
languages x alphabets x language-variants: {4}
languages x alphabets x language-variants x regions: {5}
google translation: {7}
stemming: {8}
wordBreaking: {9}
euroTalk: {10}
goethe: {11}
lingea: {12}
more language-variants: 
{6}
",
cldr.Select(l => l.ScriptId).Distinct().Count(),

cldr.Select(l => l.Id.Split('-')[0]).Distinct().Count(),
cldr.Count(l => l.DefaultRegion != null),
cldr.SelectMany(c => c.Regions).Distinct().Count(),

cldr.Count(),
cldr.Select(c => c.Regions.Length).Sum(),

moreVariants,

cldr.Where(c => c.GoogleTransId != null).Count(),
cldr.Where(c => c.StemmerClass != null).Count(),
cldr.Where(c => c.BreakerClass != null).Count()
//cldr.Where(c => c.IsEuroTalk).Count(),
//cldr.Where(c => c.IsGoethe).Count(),
//cldr.Where(c => c.IsLingea).Count()
));

  }

  public static void Build() {

    // missing langs
    var missing = Json.DeserializeStr<Langs.CldrLang[]>(missingLocsJson).Concat(GoogleTrans.getMissingLangs()).ToArray();
    var missingLocs = missing.Select(m => LocaleIdentifier.Parse(m.Id).MostLikelySubtags().ToString()).ToHashSet();

    // parse first matrix column
    var langs = LangMatrix.readLangs(LangsDesignDirs.cldr + "cldrInfos.csv").
      Select(lv => lv.Split(',')).
      SelectMany(arr => {
        var arrRemoved = arr.Where(l => !missingLocs.Contains(l)).ToArray();
        if (arrRemoved.Length == 0) return null;
        var locs = arrRemoved.Select(l => LocaleIdentifier.Parse(l)).ToArray();
        return locs.GroupBy(l => l.Language).Select(grp => {
          var regions = grp.Select(l => l.Region).ToList();
          var langScript = LocaleIdentifier.Parse(grp.First().Language + "-" + grp.First().Script);
          return new { langScript, regions, info = new BuildInfo() };
        });
      }).NotNulls().ToArray();

    // langs with more than single scripts
    var moreScripts = langs.
      Select(l => l.langScript).
      Distinct(LangMatrixComparer.Comparer).
      GroupBy(l => l.Language).
      Where(g => g.Count() > 1).
      Select(g => g.Key).
      ToHashSet();

    // process <lang>-<script> groups
    langs.GroupBy(l => l.langScript, LangMatrixComparer.Comparer).ForEach(grp => {

      // lang has more scripts
      if (moreScripts.Contains(grp.Key.Language))
        grp.ForEach(ll => ll.info.hasMoreScripts = true);

      // adjust default region
      var defaultReg = grp.Key.MostLikelySubtags().Region;
      var lang = grp.FirstOrDefault(it => it.regions.IndexOf(defaultReg) >= 0);
      if (lang == null) {
        if (grp.Count() > 1) throw new Exception();
        grp.First().regions.Add(defaultReg);
      } else {
        if (grp.Key.ToString() == "ar-Arab" && defaultReg == "EG")
          defaultReg = "SA";
        lang.info.defaultRegion = defaultReg;
      }

      // compute id
      grp.ForEach(l => {
        // lang
        var id = l.langScript.Language;
        // script
        if (l.info.hasMoreScripts)
          id += "-" + l.langScript.Script;
        // default region
        if (l.info.defaultRegion != null && !l.info.hasMoreScripts)
          id += "-" + l.info.defaultRegion;
        // select region from regions
        if (l.info.defaultRegion == null) {
          if (l.regions.Count == 1)
            id += "-" + l.regions[0];
          else {
            var reg = l.regions.FirstOrDefault(r => l.langScript.Language == r.ToLower()); // used for pt-PT
            id += "-" + (reg == null ? l.regions.First() : reg);
          }
        }
        l.info.id = id;
      });
    });

    var cldr = langs.Select(l => new Langs.CldrLang {
      Id = l.info.id,
      DefaultRegion = l.info.defaultRegion,
      Regions = l.regions.ToArray(),
      ScriptId = l.langScript.Script,
      Lang = l.langScript.Language,
      HasMoreScripts = l.info.hasMoreScripts,
    }).
    Concat(missing).
    OrderBy(c => c.Id.Split('-')[0]).
    ThenByDescending(c => c.DefaultRegion != null).
    ToArray();

    Json.Serialize(LangsDirs.dirCldrTexts, cldr);

  }
  class BuildInfo {
    public string id;
    public string defaultRegion;
    public bool hasMoreScripts;
  }

  static string missingLocsJson = @"
[
  {
    'id': 'en-GB',
    'lang': 'en',
    'scriptId': 'Latn',
    'regions': [
      'GB'
    ]
  },
   {
    'id': 'zh-Hans-SG',
    'lang': 'zh',
    'scriptId': 'Hans',
    'hasMoreScripts': true,
    'regions': [
      'SG'
    ]
  },
  {
    'id': 'zh-Hant-HK',
    'lang': 'zh',
    'scriptId': 'Hant',
    'hasMoreScripts': true,
    'regions': [
      'HK'
    ]
  },
  {
    'id': 'zh-Hant-MO',
    'lang': 'zh',
    'scriptId': 'Hant',
    'hasMoreScripts': true,
    'regions': [
      'MO'
    ]
  }
]";

  static void getAllSpecific(out LocaleIdentifier[] allSpecifics, out LangMatrixRow[] netSpecifics) {

    // get raw source from all CLDR file names
    var allCldrFiles = allLangIdsFromCldrFiles();

    // get all specific langs (e.g. cs-Latn-CZ)
    allSpecifics = allCldrFiles.
      Select(l => LocaleIdentifier.Parse(l).MostLikelySubtags()).
      Where(l => l.Script != "Cakm" && l.ToString().IndexOf("valencia") < 0).
      Distinct(LangMatrixComparer.Comparer).
      OrderBy(s => s.ToString()).
      ToArray();

    // add .NET
    netSpecifics = CldrUtils.fromNetCultureInfos(allSpecifics).ToArray();

    allSpecifics = allSpecifics.
      Concat(netSpecifics.Select(ns => LocaleIdentifier.Parse(ns.lang))).
      OrderBy(ns => ns, LangMatrixComparer.Comparer).
      ToArray();
  }

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
    {"-",Langs.invariantId },
    {"",Langs.invariantId },
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

enum oldLangs {
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

