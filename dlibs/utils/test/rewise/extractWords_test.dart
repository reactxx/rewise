import 'package:test/test.dart' as test;
//import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;
import 'package:rw_utils/rewise.dart' as rew;

main() {
  test.group("EXTRACT WORDS", () {
    test.test('parser', () {
      parse(String text, [List<int> posLens]) {
        rew.parser(rew.lexAnal(rew.Breaked.dev(text, posLens)), text);
      }
    });

    test.test('lex anal', () {
      lex(String text, int tokenCounts, [List<int> posLens]) {
        var ts = rew.lexAnal(rew.Breaked.dev(text, posLens)).toList();
        var newText = rew.tokensToString(ts, text);
        test.expect(newText, test.equals(text));
        if (tokenCounts != null)
          test.expect(tokenCounts, test.equals(ts.length));
      }

      lex('xxx yy ?', 4, [0, 3, 0, 1, 2, 1, 4, 2]);
      lex('xxx yy ?', 4, [0, 3, 4, 2]);
      lex('xxx (([ ]{})|^, yy', 13, []);
      lex('', 0, []);
    });

    test.test('sort breaks', () {
      var breaks = [
        wbreak.PosLen()
          ..pos = 4
          ..len = 9 - 4,
        wbreak.PosLen()
          ..pos = 9
          ..len = 10 - 9,
        wbreak.PosLen()
          ..pos = 4
          ..len = 10 - 4,
        wbreak.PosLen()
          ..pos = 1
          ..len = 3 - 1,
      ];
      rew.sortBreaks(breaks);
      var lens = breaks.map((b) => b.len).toList();
      test.expect(lens, test.equals([2, 6, 5, 1]));
    });
  });
}
