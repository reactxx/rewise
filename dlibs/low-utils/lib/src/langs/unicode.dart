import 'data_unicodeData.dart' show getUnicodeData;
import 'package:rewise_low_utils/messages.dart' show UncBlocks;

class UnicodeBlocks {
  static UncBlocks get blocks => _blocks ?? (_blocks = getUnicodeData());
  static UncBlocks _blocks;
}