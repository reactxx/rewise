import 'package:rw_low/code.dart' as utils;
import 'package:path/path.dart' as p;

class fileSystem {

  static final comp = utils.fileSystem.comp;
  static bool get ntb => comp == 'ntb';
  static bool get desktop => comp == 'desktop';

  static final current = utils.Dir(p.current);
  static final data = utils.Dir(r'\rewise\data');
  static final csv = utils.Dir(r'\rewise\data\01_csv');
  static final transTasks = utils.Dir(r'\rewise\data\trans_tasks');
  //static final transTasks = utils.Dir(r'\rewise\dlibs\utils\build\');
  //static final csv = utils.Dir(ntb ? r'\rewise\data\01_csv' : r'\rewise\data\01_csv_back');
  //static final raw = utils.Dir(r'\rewise\data\02_raw');
  static final source = utils.Dir(r'\rewise\data\02_source');
  static final edits = utils.Dir(r'\rewise\data\03_edits');
  //static final rawCsv = utils.Dir(r'\rewise\data\02_rawCsv');
  static final parsed = utils.Dir(r'\rewise\data\03_parsed');

  //static final log = utils.Dir(r'\rewise\data\log');
  //static final statParsed = utils.Dir(r'\rewise\data\log\parsed');
  //static final statStemmed = utils.Dir(r'\rewise\data\log\stemmed');

  static final stemmCache = utils.Dir(r'\rewise\data\stemmCache');
  static final spellCheckCache = utils.Dir(r'\rewise\data\spellCheckCache');
  static final spellCheckDump = utils.Dir(r'\rewise\data\#spellCheckDump');


  //static final desktopPaths = r'^dictionaries\\Lingea\\cs_cz\\.*';
  //static final desktopPaths = r'^(local_dictionaries|templates|dictionaries)\\.*';
  //static final desktopPaths = r'^(local_dictionaries)\\.*';
  
  //static final ntbPaths = r'^wordlists.*?goetheverlag\.';
  //static final ntbPaths = r'^(dir3)\\.*';
  //static String get devFilter => ntb ? ntbPaths : (desktop ? desktopPaths : throw Exception());
  //static String get devFilter => '';
}
