import 'package:test/test.dart' as test;
//import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;
import 'package:rw_utils/rewise.dart' as rew;

main() {
  test.group("EXTRACT WORDS", () {
    List<rew.LexFact> parse(String text, [List<int> posLens]) {
      var res = rew.parser(
          rew.lexanal(rew.Breaked.dev(text, posLens)).toList(), text);
      return res;
    }

    test.test('parser errors', () {
      List<rew.LexFact> res;

      res = parse('a(^)', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Flags.feDelimInBracket));
      res = parse('a[|]', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Flags.feDelimInBracket));

      res = parse('a(x', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Flags.feMissingBr));
      res = parse('a{x', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Flags.feMissingCurlBr));
      res = parse('a[x', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Flags.feMissingSqBr));
      res = parse('a)', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Flags.feUnexpectedBr));
      res = parse('a}', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Flags.feUnexpectedCurlBr));
      res = parse('a]', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Flags.feUnexpectedSqBr));

      res = parse('abc', []);
      test.expect(res[0].flags, test.equals(rew.Flags.feNoWordInFact));

      res = parse('a(b[)', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Flags.feMixingBrs));
      res = parse('a{[b}', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Flags.feMixingBrs));
      res = parse('a[(b]', [0, 1]);
      test.expect(res[0].flags, test.equals(rew.Flags.feMixingBrs));

      res = parse('a[b]|d[e][f]', [0, 1, 5, 1]);
      test.expect(res[1].flags, test.equals(rew.Flags.feSingleWClassAllowed));

      res = parse('a[b]|d', [0, 1, 5, 1]);
      test.expect(res[1].flags, test.equals(rew.Flags.feMissingWClass));

      res = parse('a|[f]d', [0, 1, 5, 1]);
      test.expect(res[0].flags, test.equals(rew.Flags.feMissingWClass));
      res = parse('a[b]|[f]d,e,[m]g', [0, 1, 8, 1, 10, 1, 15, 1]);
      test.expect(res[3].flags, test.equals(rew.Flags.feWClassNotInFirstFact));
      res = parse('a[b]^[f]d', [0, 1, 8, 1]);
      test.expect(res[1].flags, test.equals(rew.Flags.feWClassNotInFirstFact));
    });

    test.test('parser', () {
      List<rew.LexFact> res;

      res = parse('a[b]|[c]d', [0, 1, 8, 1]);
      test.expect(res[0].wordClass, test.equals('b'));
      test.expect(res[1].wordClass, test.equals('c'));

      res = parse('-( (n o) )?', [4, 1, 6, 1]);
      test.expect(res[0].words[0].dump, test.equals('-( (#n##1#'));
      test.expect(res[0].words[1].dump, test.equals(' #o#) )?#1#'));

      res = parse('-xxx-[n]?', [1, 3]);
      test.expect(res[0].wordClass, test.equals('n'));
      test.expect(res[0].words[0].dump, test.equals('-#xxx#-?#0#'));

      res = parse('-xxx-{n}?', [1, 3]);
      test.expect(res[0].words[1].dump, test.equals('-#{n}#?#4#'));

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
        var ts = rew.lexanal(rew.Breaked.dev(text, posLens)).toList();
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
