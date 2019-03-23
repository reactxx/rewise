import 'package:test/test.dart' as test;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;

main() {
  test.group("PARSE BOOK", () {
    test.test('parse text', () async {
      var resp = await client.WordBreaking_Run(wbreak.Request()
        ..lang = 'cs-CZ'
        ..facts.add('xxx'));
      test.expect(resp.facts.first.breaks, test.equals([0, 3]));
    });
  });
}
