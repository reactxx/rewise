import 'dart:collection';
import 'package:path/path.dart' as p;
import 'package:rw_utils/utils.dart' show fileSystem, Matrix;

class Lang {
  Lang(this.lang, this.data);
  final String lang;
  final List<String> data;
}

class LeftLang {
  LeftLang(this.lang, this.left, this.data);
  final String lang;
  final List<String> left;
  final List<String> data;
}

class LangDatas {
  LangDatas(this.type, this.path);
  final int type;
  final String path;
  // except etalk
  String lang;
  // for book
  String newName; // name in brackets
  // for book and KDict
  List<String> left;
  // for book
  List<int> lessons;
  // for book and dict
  final leftLangs = List<LeftLang>();
  // for etalk and KDict
  final langs = List<Lang>();
}

class SrcFiles {
  static const kdict = 0; // kdict
  static const dict = 1; // lingea and other dicts
  static const etalk = 2; // goethe, eurotalk
  static const book = 3; // templates and local dicts
  static const bookTrans = 4; // templates and local dicts
  static final filters = <String>[
    r'^dictionaries\\kdictionaries\\.*',
    r'^dictionaries\\.*',
    r'^local_dictionaries\\all\\.*',
    r'^templates\\.*',
    r'^local_dictionaries\\.*',
  ];
  static final files = filters
      .map((f) =>
          HashSet<String>.from(fileSystem.csv.list(regExp: f + r'\.csv$')))
      .toList();

  static Iterable<String> getBookFiles(String fn) {
    fn = p.basenameWithoutExtension(fn).toLowerCase();
    return getFiles(bookTrans).where((f) => f.toLowerCase().indexOf(fn) >= 0);
  }

  static Iterable<String> getFiles(int type) {
    switch (type) {
      case kdict:
      case etalk:
      case book:
        return files[type];
      case dict:
        return files[dict].difference(files[kdict]);
      case bookTrans:
        return files[bookTrans].difference(files[etalk]);
      default:
        throw Exception();
    }
  }

  static Iterable<LangDatas> getData() sync* {
    List<String> getColumn(Matrix matrix, int colIdx) =>
        matrix.rows.skip(1).map((r) => r.data[colIdx]).toList();

    for (var type in [0, 1, 2, 3]) {
      for (final fn in getFiles(type)) {
        final res = LangDatas(type, fn);
        final matrix = Matrix.fromFile(fileSystem.csv.absolute(fn));
        final firstRow = matrix.rows[0].data;
        switch (type) {
          case book:
            assert(firstRow.length == 2);
            assert(firstRow[0] == '_lesson');
            res.lang = toLang(firstRow[1]);
            res.left = getColumn(matrix, 1);
            res.newName = newNameRx.firstMatch(fn).group(1);
            res.lessons =
                getColumn(matrix, 0).map((s) => int.parse(s)).toList();
            var trFiles = getBookFiles(fn).toList();
            for (var tr in trFiles) {
              final trMatrix = Matrix.fromFile(fileSystem.csv.absolute(tr));
              final trFirstRow = trMatrix.rows[0].data;
              assert(trFirstRow.length == 2);
              assert(trFirstRow[0] == firstRow[1]);
              var trLang = toLang(trFirstRow[1]);
              res.leftLangs.add(LeftLang(
                  trLang, getColumn(trMatrix, 0), getColumn(trMatrix, 1)));
            }
            break;
          case kdict:
            res.lang = toLang(firstRow[0]);
            res.left = getColumn(matrix, 0);
            for (var i = 1; i < firstRow.length; i++)
              res.langs.add(Lang(toLang(firstRow[i]), getColumn(matrix, i)));
            break;
          case dict:
            assert(firstRow.length == 2);
            res.lang = toLang(firstRow[0]);
            var trLang = toLang(firstRow[1]);
            res.leftLangs.add(
                LeftLang(trLang, getColumn(matrix, 0), getColumn(matrix, 1)));
            break;
          case etalk:
            for (var i = 0; i < firstRow.length; i++)
              res.langs.add(Lang(toLang(firstRow[i]), getColumn(matrix, i)));
            break;
        }
        yield res;
      }
    }
  }

  static final newNameRx = RegExp(r'\((.*?)\)\.csv$');

  static String toLang(String lang) {
    var res = oldToNew[lang];
    assert(res != null && !res.startsWith('?'));
    return res;
  }
}

/*
on CSharp side:
        foreach (var fn in Directory.EnumerateFiles(@"d:\rewise\data\01_csv\", "*.csv", SearchOption.AllDirectories)) {
          var txt = File.ReadAllText(fn);
          File.WriteAllText(fn, txt, Encoding.UTF8);
        }
        var oldLangs = "be_by,uk_ua,ko_ko,el_GR,gl_es,ur_pk,af_za,co_fr,zh_hk,sq_al,mr_in,mi_nz,sv_se,zh_CN,nb_no,cs_cz,sr_cyrl,ml_in,de_de,sl_si,fi_fi,nl_nl,hu_hu,sr_latn_cs,eu_es,bn_in,ro_ro,lv_lv,ar_sa,es_es,ru_ru,bs_latn,en_us,mn_mn,ha_latn_ng,vi_VN,quz_pe,ga_ie,br_fr,nso_za,sw_ke,he_il,te_in,km_kh,yo_ng,pt_br,oc_fr,fr_fr,ja_jp,en_gb,vi_vn,uz_latn_uz,ky_kg,pa_in,lt_lt,ms_my,ko_kr,hi_in,zh_cn,et_ee,tr_tr,tn_za,bg_bg,pl_pl,_lesson,pt_pt,eo_001,hy_am,is_is,ps_af,ig_ng,fa_ir,bo_cn,mk_mk,zu_za,ca_es,ta_in,xh_za,da_dk,it_it,id_id,as_in,az_latn_az,ka_ge,hr_hr,sk_sk,mt_mt,el_gr,th_th".Split(',').OrderBy(s => s);
        var newLangs = oldLangs.Select(old => string.Format("'{0}':'{1}'", old, Langs.oldToNew(old))).JoinStrings(",");
        newLangs = null;
 */

List<String> oldLangs() =>
    HashSet<String>.from([0, 1, 2, 3].expand((type) => SrcFiles.getFiles(type)
        .map((fn) => fileSystem.csv.readAsLines(fn).first.split(';'))
        .expand((l) => l))).toList();

final oldToNew = <String, String>{
  '_lesson': '?-lesson',
  'af_za': 'af-ZA',
  'ar_sa': 'ar-SA',
  'as_in': 'as-IN',
  'az_latn_az': 'az-Latn',
  'be_by': 'be-BY',
  'bg_bg': 'bg-BG',
  'bn_in': 'bn-BD',
  'bo_cn': 'bo-CN',
  'br_fr': 'br-FR',
  'bs_latn': 'bs-Latn',
  'ca_es': 'ca-ES',
  'co_fr': 'co-FR',
  'cs_cz': 'cs-CZ',
  'da_dk': 'da-DK',
  'de_de': 'de-DE',
  'el_gr': 'el-GR',
  'el_GR': 'el-GR',
  'en_gb': 'en-GB',
  'en_us': 'en-US',
  'eo_001': 'eo-001',
  'es_es': 'es-ES',
  'et_ee': 'et-EE',
  'eu_es': 'eu-ES',
  'fa_ir': 'fa-IR',
  'fi_fi': 'fi-FI',
  'fr_fr': 'fr-FR',
  'ga_ie': 'ga-IE',
  'gl_es': 'gl-ES',
  'ha_latn_ng': 'ha-NG',
  'he_il': 'he-IL',
  'hi_in': 'hi-IN',
  'hr_hr': 'hr-HR',
  'hu_hu': 'hu-HU',
  'hy_am': 'hy-AM',
  'id_id': 'id-ID',
  'ig_ng': 'ig-NG',
  'is_is': 'is-IS',
  'it_it': 'it-IT',
  'ja_jp': 'ja-JP',
  'ka_ge': 'ka-GE',
  'km_kh': 'km-KH',
  'ko_ko': '?ko-ko',
  'ko_kr': 'ko-KR',
  'ky_kg': 'ky-KG',
  'lt_lt': 'lt-LT',
  'lv_lv': 'lv-LV',
  'mi_nz': 'mi-NZ',
  'mk_mk': 'mk-MK',
  'ml_in': 'ml-IN',
  'mn_mn': 'mn-MN',
  'mr_in': 'mr-IN',
  'ms_my': 'ms-MY',
  'mt_mt': 'mt-MT',
  'nb_no': 'nb-NO',
  'nl_nl': 'nl-NL',
  'nso_za': 'nso-ZA',
  'oc_fr': 'oc-FR',
  'pa_in': 'pa-Guru',
  'pl_pl': 'pl-PL',
  'ps_af': 'ps-AF',
  'pt_br': 'pt-BR',
  'pt_pt': 'pt-PT',
  'quz_pe': 'qu-PE',
  'ro_ro': 'ro-RO',
  'ru_ru': 'ru-RU',
  'sk_sk': 'sk-SK',
  'sl_si': 'sl-SI',
  'sq_al': 'sq-AL',
  'sr_cyrl': 'sr-Cyrl',
  'sr_latn_cs': 'sr-Latn',
  'sv_se': 'sv-SE',
  'sw_ke': 'sw-TZ',
  'ta_in': 'ta-IN',
  'te_in': 'te-IN',
  'th_th': 'th-TH',
  'tn_za': 'tn-ZA',
  'tr_tr': 'tr-TR',
  'uk_ua': 'uk-UA',
  'ur_pk': 'ur-PK',
  'uz_latn_uz': 'uz-Latn',
  'vi_vn': 'vi-VN',
  'vi_VN': 'vi-VN',
  'xh_za': 'xh-ZA',
  'yo_ng': 'yo-NG',
  'zh_cn': 'zh-Hans',
  'zh_CN': 'zh-Hans',
  'zh_hk': 'zh-Hant-HK',
  'zu_za': 'zu-ZA'
};
