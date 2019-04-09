import 'dart:collection';
import 'package:tuple/tuple.dart';
import 'package:path/path.dart' as p;
import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
import 'package:rw_utils/dom/dom.dart' as d;
import 'package:rw_utils/threading.dart';
import 'dom.dart';

Future importCSVFiles() async {
  FromCSV.checkUniqueBookName();
  return;
  final all = FromCSV._getAllFiles();
  if (fileSystem.desktop) {
    final tasks = all.map((rel) => StringMsg.encode(rel));
    await Parallel(tasks, 4, _entryPoint, taskLen: all.length).run();
  } else {
    for (final tn in all) await _importCSVFile(StringMsg(tn));
  }
}

Future<List> _importCSVFile(StringMsg msg) {
  final ld = FromCSV._readCSVFile(msg.strValue);
  for (var mf in FromCSV._toMsgFiles(ld)) {
    mf.save();
    mf.toCSV();
  }
  return Future.value(null);
}

void _entryPoint(List workerInitMsg) =>
    parallelEntryPoint<StringMsg>(workerInitMsg, _importCSVFile);

class FromCSV {
  static const kdict = 0; // kdict
  static const dict = 1; // lingea and +other dicts
  static const etalk = 2; // goethe, eurotalk
  static const book = 3; // templates and local dicts
  static const bookTrans = 4; // templates and local dicts

  static HashSet<String> _bookTransFiles;

  static List<String> _getAllFiles() => _getAllFilesLow().map((t) => '${t.item1.toString()}|${t.item2}').toList();

  static void checkUniqueBookName() {
    final names = HashSet<String>();
    for(var t in _getAllFilesLow()) {
      final newName = toNewName(t.item1, t.item2);
      assert(!names.contains(newName));
      names.add(newName)
    }
  }

  static String toNewName(int type, String fn) {
    final fnParts = p.split(fn);
    switch(type) {
      case book: return _newNameRx.firstMatch(fn).group(1).toLowerCase();
      case etalk: return p.basenameWithoutExtension(fn).toLowerCase();
      case kdict: return fnParts[fnParts.length - 2].toLowerCase();
      case dict: return fnParts[fnParts.length - 3].toLowerCase();
      default: throw Exception();
    }
  }

  static Iterable<Tuple2<int,String>> _getAllFilesLow() sync* {
    final _filters = <String>[
      r'^dictionaries\\kdictionaries\\.*',
      r'^dictionaries\\.*',
      r'^local_dictionaries\\all\\.*',
      r'^templates\\.*',
      r'^local_dictionaries\\.*',
    ];
    _hashSetFiles(int idx) => HashSet<String>.from(
        fileSystem.csv.list(regExp: _filters[idx] + r'\.csv$'));

    final kdictFiles = _hashSetFiles(kdict);
    final etalkFiles = _hashSetFiles(etalk);
    _bookTransFiles = _hashSetFiles(bookTrans).difference(etalkFiles);

    Iterable<String> _fileNamesFromType(int type) {
      switch (type) {
        case kdict:
          return kdictFiles;
        case etalk:
          return etalkFiles;
        case book:
          return _hashSetFiles(book);
        case dict:
          return _hashSetFiles(dict).difference(kdictFiles);
        case bookTrans:
          return _bookTransFiles;
        default:
          throw Exception();
      }
    }

    yield* [0, 1, 2, 3]
        .expand(
            (idx) => _fileNamesFromType(idx).map((f) => Tuple2(idx, f)))
        .toList();
  }

  static LangDatas _readCSVFile(String typeName) {
    List<String> getColumn(Matrix matrix, int colIdx) =>
        matrix.rows.skip(1).map((r) => r.data[colIdx]).toList();
    Iterable<String> getTranslatedBookFiles(String fn) {
      fn = p.basenameWithoutExtension(fn).toLowerCase();
      return _bookTransFiles.where((f) => f.toLowerCase().indexOf(fn) >= 0);
    }

    final parts = typeName.split('|');
    final type = int.parse(parts[0]);
    final fn = parts[1];

    final res = LangDatas(type, fn);
    final matrix = Matrix.fromFile(fileSystem.csv.absolute(fn));
    final firstRow = matrix.rows[0].data;
    res.newName = toNewName(type, fn);
    final fnParts = p.split(fn);
    switch (type) {
      case book:
        assert(firstRow.length == 2);
        assert(firstRow[0] == '_lesson');
        res.lang = _toNewLang(firstRow[1]);
        res.left = getColumn(matrix, 1);
        res.lessons = getColumn(matrix, 0).map((s) => int.parse(s)).toList();
        var trFiles = getTranslatedBookFiles(fn).toList();
        for (var tr in trFiles) {
          final trMatrix = Matrix.fromFile(fileSystem.csv.absolute(tr));
          final trFirstRow = trMatrix.rows[0].data;
          assert(trFirstRow.length == 2);
          assert(trFirstRow[0] == firstRow[1]);
          var trLang = _toNewLang(trFirstRow[1]);
          res.leftLangs.add(
              LeftLang(trLang, getColumn(trMatrix, 0), getColumn(trMatrix, 1)));
        }
        break;
      case kdict:
        res.lang = _toNewLang(firstRow[0]);
        res.left = getColumn(matrix, 0);
        for (var i = 1; i < firstRow.length; i++)
          res.langs.add(Lang(_toNewLang(firstRow[i]), getColumn(matrix, i)));
        break;
      case dict:
        assert(firstRow.length == 2);
        res.lang = _toNewLang(firstRow[0]);
        var trLang = _toNewLang(firstRow[1]);
        res.leftLangs
            .add(LeftLang(trLang, getColumn(matrix, 0), getColumn(matrix, 1)));
        break;
      case etalk:
        for (var i = 0; i < firstRow.length; i++)
          res.langs.add(Lang(_toNewLang(firstRow[i]), getColumn(matrix, i)));
        break;
    }
    return res;
  }

  static Iterable<File> _toMsgFiles(LangDatas ld) sync* {
    File create(int fileType, List<String> data,
            [String rightLang = '']) =>
        File()
          ..leftLang = ld.lang ?? ''
          ..lang = rightLang
          ..bookName = ld.newName
          ..bookType = ld.type
          ..fileType = fileType
          ..factss.addAll(data.map((s) => d.FactsMsg()..asString = s));

    switch (ld.type) {
      case FromCSV.kdict:
        yield create(FileMsg_FileType.LEFT, ld.left);
        for (var l in ld.langs)
          yield create(FileMsg_FileType.LANG, l.data, l.lang);
        break;
      case FromCSV.etalk:
        for (var l in ld.langs)
          yield create(FileMsg_FileType.LANG, l.data, l.lang);
        break;
      case FromCSV.dict:
        for (var l in ld.leftLangs) {
          yield create(FileMsg_FileType.LANGLEFT, l.left, l.lang);
          yield create(FileMsg_FileType.LANG, l.data, l.lang);
        }
        break;
      case FromCSV.book:
        yield create(FileMsg_FileType.LEFT, ld.left);
        for (var l in ld.leftLangs) {
          yield create(FileMsg_FileType.LANGLEFT, l.left, l.lang);
          yield create(FileMsg_FileType.LANG, l.data, l.lang);
        }
        break;
    }
  }

  static final _newNameRx = RegExp(r'\((.*?)\)\.csv$');

  static String _toNewLang(String lang) {
    var res = oldToNew[lang];
    assert(res != null && !res.startsWith('?'));
    return res;
  }
}

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

  String get subPath => '$lang\\$newName\\';
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

// List<String> oldLangs() => HashSet<String>.from([0, 1, 2, 3].expand((type) =>
//     FromCSV._fileNamesFromType(type)
//         .map((fn) => fileSystem.csv.readAsLines(fn).first.split(';'))
//         .expand((l) => l))).toList();

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
