import 'package:rewise_low_utils/utils.dart' as utils;

class fileSystem {
  static final csv = utils.Dir(r'\rewise\data\01_csv');
  static final raw = utils.Dir(r'\rewise\data\02_raw');
  static final bookSources = utils.Dir(r'\rewise\data\03_bookSources');

  static final protobufs = utils.Dir(r'\rewise\protobuf\compiler\include');

  static final protoDartMessages = utils.Dir(r'\rewise\dlibs\utils\lib\src\messages');
  static final codeDartUtils = utils.Dir(r'\rewise\dlibs\utils');
  static final codeDartServer = utils.Dir(r'\rewise\dlibs\server');
}
