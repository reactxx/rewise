import 'package:test/test.dart' as test;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/dom/word_breaking.dart' as wb;

main() {
  test.group("EXTRACT WORDS", () {
    test.test('breaking', () async {
      final req = wb.Request2()
        ..lang = 'en-GB'
        ..path = 'xxx';
      var texts = ['Ahoj, jak se mas?'];
      req.facts.addAll(texts.map((e) => wb.FactReq()
        ..text = e
        ..id = 1));
      final resp = await client.WordBreaking_Run2(req);

      var btexts = resp.facts
          .expand(
              (f) => f.posLens.map((pl) => f.text.substring(pl.pos, pl.end)))
          .join('|');
      test.expect(btexts, test.equals('Ahoj|jak|se|mas'));
    });
  });
}
