import 'data_unicode.dart';

class UnicodeBlocks {
  static UncBlocks get blocks => _blocks ?? (_blocks = UncBlocks.fromData());
  static UncBlocks _blocks;
}