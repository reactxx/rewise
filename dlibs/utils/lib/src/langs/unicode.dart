import 'data_unicodeData.dart' show getUnicodeData;
import 'messagesBoot.dart';

class UnicodeBlocks {
  static UncBlocks get blocks => _blocks ?? (_blocks = getUnicodeData());
  static UncBlocks _blocks;
}