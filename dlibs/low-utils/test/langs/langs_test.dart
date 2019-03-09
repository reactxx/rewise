import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/langs.dart' as langs;

main() {
  test.test('old2new-data-test', () {
    var map = langs.oldToNewData();
    test.expect(map!=null, test.equals(true));
  });

  test.test('langs-data-test', () {
    var cldr = langs.Langs.meta;
    cldr = langs.Langs.meta;
    var map = langs.Langs.nameToMeta;
    var blocks = langs.UnicodeBlocks.blocks;
    cldr = null;
  });

}

