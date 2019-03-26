import 'package:rw_low/code.dart' as utils;

class fileSystem {
  static final csv = utils.Dir(r'\rewise\data\01_csv');
  static final raw = utils.Dir(r'\rewise\data\02_raw');
  static final parsed = utils.Dir(r'\rewise\data\03_parsed');
  //static final parsedLang = utils.Dir(r'\rewise\data\03_parsedLang');
  static final log = utils.Dir(r'\rewise\data\log');
  static final stemmCache = utils.Dir(r'\rewise\data\stemmCache');

  static final devFilter = r'^wordlists.*?goetheverlag\.';
}
