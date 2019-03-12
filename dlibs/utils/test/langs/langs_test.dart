import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/langs.dart' as langs;

main() {
  test.test('langs-data-test', () {
    var blocks = langs.UnicodeBlocks.blocks;
    var cldr = langs.Langs.meta;
    cldr = langs.Langs.meta;
    var map = langs.Langs.nameToMeta;
    if (cldr == map && blocks == null) {
      return;  
    }
  });

  //final bytes = Uint8List.fromList(const [0x1, 0x2, 0xff, 0xfe]);
}
