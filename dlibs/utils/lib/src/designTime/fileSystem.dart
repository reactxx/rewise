import 'package:rewise_low_utils/utils.dart' as utils;

class fileSystem {
  static final csv = utils.Dir(r'\rewise\data\01_csv');
  static final raw = utils.Dir(r'\rewise\data\02_raw');
  static final parsed = utils.Dir(r'\rewise\data\03_parsed');
  static final log = utils.Dir(r'\rewise\data\log');

  static final rwdomInclude = utils.Dir(r'\rewise\dom\include');
  static final rwdomMessages = utils.Dir(r'\rewise\dom\lib\src\messages');
  static final rwdom = utils.Dir(r'\rewise\dom\');
  
  static final codeDartUtils = utils.Dir(r'\rewise\dlibs\utils');
  static final codeDartServer = utils.Dir(r'\rewise\dlibs\server');
}
