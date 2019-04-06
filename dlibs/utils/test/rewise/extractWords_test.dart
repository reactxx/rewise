// DOLGIT

import 'package:test/test.dart' as test;
//import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;
import 'package:rw_utils/rewise.dart' as rew;

main() {
  test.group("EXTRACT WORDS", () {
    List<rew.LexFact> parse(String text, [List<int> posLens]) {
      var res = rew.parser(
          rew.lexAnal(rew.Breaked.dev(text, posLens)).toList(), text);
      return res;
    }

    test.test('parser errors', () {
      List<rew.LexFact> res;

      res = parse('a(^)', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Fact.e1));
      res = parse('a[|]', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Fact.e1));

      res = parse('a(x', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Fact.e3));
      res = parse('a{x', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Fact.e4));
      res = parse('a[x', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Fact.e5));
      res = parse('a)', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Fact.e6));
      res = parse('a}', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Fact.e7));
      res = parse('a]', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Fact.e8));

      res = parse('abc', []);
      test.expect(res[0].flags, test.equals(rew.Fact.e9));


      res = parse('a(b[)', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Fact.ea));
      res = parse('a{[b}', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Fact.ea));
      res = parse('a[(b]', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Fact.ea));

      res = parse('a[b]|d[e][f]', [0, 1, 5, 1]);
      test.expect(res[1].flags, test.equals(rew.Fact.eb));

      res = parse('a[b]|d', [0, 1, 5, 1]);
      test.expect(res[1].flags, test.equals(rew.Fact.ec));

      res = parse('a[b]^[f]d', [0, 1, 8, 1]);
      test.expect(res[1].flags, test.equals(rew.Fact.ed));
    });

    test.test('parser', () {
      List<rew.LexFact> res;

      res = parse('a[b]|[c]d', [0, 1, 8, 1]);
      test.expect(res[0].wordClass, test.equals('b'));
      test.expect(res[1].wordClass, test.equals('c'));

      res = parse('-( (n o) )?', [4, 1, 6, 1]);
      test.expect(res[0].words[0].dump, test.equals('-( (#n##|(#'));
      test.expect(res[0].words[1].dump, test.equals(' #o#) )?#|(#'));

      res = parse('-xxx-[n]?', [1, 3]);
      test.expect(res[0].wordClass, test.equals('n'));
      test.expect(res[0].words[0].dump, test.equals('-#xxx#-?##'));

      res = parse('-xxx-{n}?', [1, 3]);
      test.expect(res[0].words[1].dump, test.equals('-#{n}#?#|{#'));

      res = parse('-xxx-^-yyy-', [1, 3, 7, 3]);
      test.expect(
          res[0].words[0].after + res[1].words[0].after, test.equals('--'));

      res = parse('-xxx-^-yyy-', [1, 3, 7, 3]);
      test.expect(
          res[0].words[0].before + res[1].words[0].before, test.equals('--'));

      res = parse('xxx^yyy', [0, 3, 4, 3]);
      test.expect(res.length, test.equals(2));

      res = parse('', []);
      test.expect(res.length, test.equals(0));

      res = parse('xxx');
      test.expect(res[0].words[0].text, test.equals('xxx'));
    });

    test.test('lex anal', () {
      lex(String text, int tokenCounts, [List<int> posLens]) {
        var ts = rew.lexAnal(rew.Breaked.dev(text, posLens)).toList();
        var newText = rew.tokensToString(ts, text);
        test.expect(newText, test.equals(text));
        if (tokenCounts != null)
          test.expect(tokenCounts, test.equals(ts.length));
      }

      lex('xxx yy ?', 6, [0, 3, 0, 1, 2, 1, 4, 2]);
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
