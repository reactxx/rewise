import 'package:rewise_low_utils/utils.dart' as utils;

class fileSystem {
  static final rjCsv = new utils.Dir(r'\rewise\data\01_RJCsv');
  static final rjMsg = new utils.Dir(r'\rewise\data\02_RJMsg');
  static final rawBooks = new utils.Dir(r'\rewise\data\03_RawBooks');
  static final protobufRewise = new utils.Dir(r'\rewise\protobuf\compiler\include\rewise');

  static final protoDartMessages = new utils.Dir(r'\rewise\dlibs\utils\lib\src\messages');
  static final codeDartUtils = new utils.Dir(r'\rewise\dlibs\utils');
  static final codeDartServer = new utils.Dir(r'\rewise\dlibs\server');
}
