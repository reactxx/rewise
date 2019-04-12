import 'package:test/test.dart' as test;
//import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;
import 'package:rw_utils/sources.dart' as s;

class Breaked {
  Breaked.dev(this.src, [List<int> posLens]) : breaks = List<wbreak.PosLen>() {
    if (posLens == null)
      breaks.add(wbreak.PosLen()
        ..pos = 0
        ..len = src.length);
    else
      for (var i = 0; i < posLens.length; i += 2)
        breaks.add(wbreak.PosLen()
          ..pos = posLens[i]
          ..len = posLens[i + 1]);
  }
  final String src;
  List<wbreak.PosLen> breaks;
}

main() {
  test.group("EXTRACT WORDS", () {
    List<s.LexFact> parse(String text, [List<int> posLens]) {
      var b = Breaked.dev(text, posLens);
      var res = s.parser(b.src, b.breaks);
      return res.facts;
    }

    s.LexFacts parseEx(String text, [List<int> posLens]) {
      var b = Breaked.dev(text, posLens);
      var res = s.parser(b.src, b.breaks);
      return res;
    }

    test.test('Cross word breaks', () {
      s.LexFacts res;
      res = parseEx('abcdef', [0, 3, 1, 5, 0, 4, 2, 4]);
      test.expect(res.toText(), test.equals('abcdef'));
    });

    test.test('Empty facts and words', () {
      s.LexFacts res;
      res = parseEx('-(x)-', [2, 1]);
      test.expect(res.facts[0].flagsText, test.equals("feNoWordInFact"));
      res = parseEx(',^-{}[](zcv())-^^,a,,,', []);
      test.expect(res.toText(), test.equals("-{}(zcv())-^a,"));
      test.expect(res.facts[0].flagsText, test.equals("feNoWordInFact"));
    });

    test.test('LexFacts', () {
      s.LexFacts res;

      res = parseEx('-xxx-^-yyy-', [1, 3, 7, 3]);
      test.expect(res.toText(), test.equals('-xxx-^-yyy-'));

      res = parseEx('a[b]|[c]d', [0, 1, 8, 1]);
      test.expect(res.toText(), test.equals('[b]a|[c]d'));

      res = parseEx('-( (n o) )?', [4, 1, 6, 1]);
      test.expect(res.toText(), test.equals('-( (n o) )?'));

      res = parseEx('-xxx-{n}?', [1, 3]);
      test.expect(res.toText(), test.equals('-xxx-{n}?'));

      res = parseEx('-xxx-[n]?', [1, 3]);
      test.expect(res.toText(), test.equals('[n]-xxx-?'));
    });

    test.test('parser errors', () {
      List<s.LexFact> res;

      res = parse('a(^)', [0, 1]);
      test.expect(res[0].flags, test.equals(s.Flags.feDelimInBracket));
      res = parse('a[|]', [0, 1]);
      test.expect(res[0].flags, test.equals(s.Flags.feDelimInBracket));

      res = parse('a(x', [0, 1]);
      test.expect(res[0].flags, test.equals(s.Flags.feMissingBr));
      res = parse('a{x', [0, 1]);
      test.expect(res[0].flags, test.equals(s.Flags.feMissingCurlBr));
      res = parse('a[x', [0, 1]);
      test.expect(res[0].flags, test.equals(s.Flags.feMissingSqBr));
      res = parse('a)', [0, 1]);
      test.expect(res[0].flags, test.equals(s.Flags.feUnexpectedBr));
      res = parse('a}', [0, 1]);
      test.expect(res[0].flags, test.equals(s.Flags.feUnexpectedCurlBr));
      res = parse('a]', [0, 1]);
      test.expect(res[0].flags, test.equals(s.Flags.feUnexpectedSqBr));

      res = parse('abc', []);
      test.expect(res[0].flags, test.equals(s.Flags.feNoWordInFact));

      // res = parse('a(b[)', [0, 1]);
      // test.expect(res[0].flags, test.equals(s.Flags.feMixingBrs));
      // res = parse('a(b{)', [0, 1]);
      // test.expect(res[0].flags, test.equals(s.Flags.feMixingBrs));
      // res = parse('a{[b}', [0, 1]);
      // test.expect(res[0].flags, test.equals(s.Flags.feMixingBrs));
      res = parse('a[(b]', [0, 1]);
      test.expect(res[0].flags, test.equals(s.Flags.feMixingBrs));

      res = parse('a[b]|d[e][f]', [0, 1, 5, 1]);
      test.expect(res[1].flags, test.equals(s.Flags.feSingleWClassAllowed));

      res = parse('a[b]|d', [0, 1, 5, 1]);
      test.expect(res[1].flags, test.equals(s.Flags.feMissingWClass));

      res = parse('a|[f]d', [0, 1, 5, 1]);
      test.expect(res[0].flags, test.equals(s.Flags.feMissingWClass));
      res = parse('a[b]|[f]d,e,[m]g', [0, 1, 8, 1, 10, 1, 15, 1]);
      test.expect(res[3].flags, test.equals(s.Flags.feWClassNotInFirstFact));
      res = parse('a[b]^[f]d', [0, 1, 8, 1]);
      test.expect(res[1].flags, test.equals(s.Flags.feWClassNotInFirstFact));
    });

    test.test('parser', () {
      List<s.LexFact> res;

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
          res[0].words[0].after + res[1].words[0].after, test.equals('-^-'));

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
        var b = Breaked.dev(text, posLens);
        var ts = s.lexanal(b.src, b.breaks).toList();
        var newText = s.tokensToString(ts, text);
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
      s.sortBreaks(breaks);
      var lens = breaks.map((b) => b.len).toList();
      test.expect(lens, test.equals([2, 6, 5, 1]));
    });
  });
}
