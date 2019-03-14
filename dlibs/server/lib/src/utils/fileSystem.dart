import 'package:rewise_low_utils/utils.dart' as utils;

class fileSystem {
  static final rjCsv = utils.Dir(r'\rewise\data\01_RJCsv');
  static final rjMsg = utils.Dir(r'\rewise\data\02_RJMsg');
  static final rawBooks = utils.Dir(r'\rewise\data\03_RawBooks');
  static final protobufs = utils.Dir(r'\rewise\protobuf\compiler\include');

  static final protoDartMessages = utils.Dir(r'\rewise\dlibs\utils\lib\src\messages');
  static final codeDartUtils = utils.Dir(r'\rewise\dlibs\utils');
  static final codeDartServer = utils.Dir(r'\rewise\dlibs\server');
}
