import 'package:rw_low/code.dart' as utils;

class fileSystem {
  static final csv = utils.Dir(r'\rewise\data\01_csv');
  static final raw = utils.Dir(r'\rewise\data\02_raw');
  static final parsed = utils.Dir(r'\rewise\data\03_parsed');
  //static final parsedLang = utils.Dir(r'\rewise\data\03_parsedLang');
  static final log = utils.Dir(r'\rewise\data\log');
  static final stemmCache = utils.Dir(r'\rewise\data\stemmCache');

  static bool get ntb => utils.fileSystem.comp == 'ntb';
  static bool get desktop => utils.fileSystem.comp == 'desktop';

  //static final devFilter = r'dictionaries\\Indirect\\de_de\\de_de.csv';
  static String get devFilter => ntb ? r'^wordlists.*?goetheverlag\.' : (desktop ? r'^dictionaries\\.*' : throw Exception());


}
