import 'package:test/test.dart' as test;
import 'package:rw_utils/langs.dart' show Langs, Unicode;

main() {
  test.group('langs', () {
    test.test('langs', () {
      var map = Langs.nameToMeta['cs-CZ'];
      test.expect(map.scriptId, test.equals('Latn'));
    });

    test.test('unicode', () {
      test.expect(Unicode.isLetter('a'.codeUnitAt(0)), test.equals(true));
      test.expect(Unicode.isLetter(32), test.equals(false));

      test.expect(Unicode.isLetter(3647), test.equals(false));
      test.expect(Unicode.isLetter(3648), test.equals(true));
      test.expect(Unicode.isLetter(3653), test.equals(true));
      test.expect(Unicode.isLetter(3654), test.equals(false));
      test.expect(Unicode.item(3653).script, test.equals('Thai'));

      var res = Unicode.scriptsFromText('فہقلمنچڈویڑےآؤئابکتثجحخدگذرزسشصضطظعٹغںپھ');
      test.expect(res.keys.first, test.equals('Arab')); 

    });
  });
}
