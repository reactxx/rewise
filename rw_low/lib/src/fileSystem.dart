import 'dart:io';
import 'dir.dart' show Dir;

class fileSystem {
  static final lowRoot = Dir(r'\rewise\rw_low');
  static final utilsRoot = Dir(r'\rewise\dlibs\utils');

  static String get comp => Platform.environment['REWISE'];
}
