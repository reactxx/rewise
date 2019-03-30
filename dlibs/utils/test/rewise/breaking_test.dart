import 'package:test/test.dart' as test;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;
import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/rewise.dart' as rew;

main() {
  test.group("PARSE BOOK", () {
    test.test('parse text', () async {
      final toParse = 'фитнес (зала)';
      final lang = 'es-ES';

      var resp = await client.WordBreaking_Run(wbreak.Request()
        ..lang = lang
        ..facts.add(toParse));

      final err = StringBuffer();
      final sf = toPars.ParsedSubFact()..text = toParse;
      rew.mergeBreakingLow('es-ES', sf, resp.facts.first.posLens, err);

      test.expect(sf.breaks, test.equals([0, 0]));
    });
  });
}
