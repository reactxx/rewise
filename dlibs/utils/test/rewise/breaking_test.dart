import 'package:test/test.dart' as test;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;

main() {
  test.group("PARSE BOOK", () {
    test.test('parse text', () async {
      var resp = await client.WordBreaking_RunEx(wbreak.Request()
        ..lang = 'cs-CZ'
        ..facts.add(''));
      test.expect(resp == null, test.equals(false));
    });
  });
}
