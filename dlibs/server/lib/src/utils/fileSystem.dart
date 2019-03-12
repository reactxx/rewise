import 'package:rewise_low_utils/utils.dart' as utils;

class fileSystem {
  static final rjCsv = new utils.Dir(r'\rewise\data\01_RJCsv');
  static final rjMsg = new utils.Dir(r'\rewise\data\02_RJMsg');

  static final codeDartUtils = new utils.Dir(r'\rewise\dlibs\utils');
  static final codeDartServer = new utils.Dir(r'\rewise\dlibs\server');
}
