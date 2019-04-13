import 'package:test/test.dart' as test;
//import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;
import 'package:rw_utils/sources.dart' as s;

class Breaked {
  Breaked.dev(this.src, [List<int> posLens]) : breaks = List<wbreak.PosLen>() {
    if (posLens == null)
      breaks.add(wbreak.PosLen()
        ..pos = 0
        ..end = src.length);
    else
      for (var i = 0; i < posLens.length; i += 2)
        breaks.add(wbreak.PosLen()
          ..pos = posLens[i]
          ..end = posLens[i] + posLens[i + 1]);
  }
  final String src;
  List<wbreak.PosLen> breaks;
}

main() {
  test.group("EXTRACT WORDS", () {
    s.Facts parse(String text, [List<int> posLens]) {
      var b = Breaked.dev(text, posLens);
      return s.parser(b.src, b.breaks);
      //return res.facts;
    }

    s.Facts parseEx(String text, [List<int> posLens]) {
      var b = Breaked.dev(text, posLens);
      var res = s.parser(b.src, b.breaks);
      return res;
    }

    test.test('Escape chars', () { 
      s.Facts res;
      var text = r'\\\(';
      res = parseEx(text, []);
      test.expect(res.toText(), test.equals(text));
      text = r'a\\\(b';
      res = parseEx(text, [0, 1, 5, 1]);
      test.expect(res.toText(), test.equals(text));
    });

    test.test('Cross word breaks', () {
      s.Facts res;
      res = parseEx('abcdef', [0, 3, 1, 5, 0, 4, 2, 4]);
      test.expect(res.toText(), test.equals('abcdef'));
    });

    test.test('Empty facts and words', () {
      s.Facts res;
      var txt = '-(x)-';
      res = parseEx(txt, [2, 1]);
      test.expect(res.facts[0].flagsText, test.equals("feNoWordInFact"));
      test.expect(res.toText(), test.equals(txt));
      txt = ',^-{}[](zcv())-^^,a,,,';
      res = parseEx(txt, []);
      test.expect(res.toText(), test.equals(txt));
      test.expect(res.facts[0].flagsText, test.equals("feNoWordInFact"));
    });

    test.test('Facts', () {
      s.Facts res;
      String txt;

      res = parseEx(txt = '-xxx-^-yyy-', [1, 3, 7, 3]);
      test.expect(res.toText(), test.equals(txt));

      res = parseEx(txt = 'a[b]|[c]d', [0, 1, 8, 1]);
      test.expect(res.toText(), test.equals(txt));

      res = parseEx(txt = '-( (n o) )?', [4, 1, 6, 1]);
      test.expect(res.toText(), test.equals(txt));

      res = parseEx(txt = '-xxx-{n}?', [1, 3]);
      test.expect(res.toText(), test.equals(txt));

      res = parseEx(txt = '-xxx-[n]?', [1, 3]);
      test.expect(res.toText(), test.equals(txt));
    });

    test.test('parser errors', () {
      s.Facts res;

      res = parse('a(^)', [0, 1]);
      test.expect(res.facts[0].flags, test.equals(s.Flags.feDelimInBracket));
      res = parse('a[|]', [0, 1]);
      test.expect(res.facts[0].flags, test.equals(s.Flags.feDelimInBracket));

      res = parse('a(x', [0, 1]);
      test.expect(res.facts[0].flags, test.equals(s.Flags.feMissingBr));
      res = parse('a{x', [0, 1]);
      test.expect(res.facts[0].flags, test.equals(s.Flags.feMissingCurlBr));
      res = parse('a[x', [0, 1]);
      test.expect(res.facts[0].flags, test.equals(s.Flags.feMissingSqBr));
      res = parse('a)', [0, 1]);
      test.expect(res.facts[0].flags, test.equals(s.Flags.feUnexpectedBr));
      res = parse('a}', [0, 1]);
      test.expect(res.facts[0].flags, test.equals(s.Flags.feUnexpectedCurlBr));
      res = parse('a]', [0, 1]);
      test.expect(res.facts[0].flags, test.equals(s.Flags.feUnexpectedSqBr));

      res = parse('abc', []);
      test.expect(res.facts[0].flags, test.equals(s.Flags.feNoWordInFact));

      // res = parse('a(b[)', [0, 1]);
      // test.expect(res[0].flags, test.equals(s.Flags.feMixingBrs));
      // res = parse('a(b{)', [0, 1]);
      // test.expect(res[0].flags, test.equals(s.Flags.feMixingBrs));
      // res = parse('a{[b}', [0, 1]);
      // test.expect(res[0].flags, test.equals(s.Flags.feMixingBrs));
      res = parse('a[(b]', [0, 1]);
      test.expect(res.facts[0].flags, test.equals(s.Flags.feMixingBrs)); 

      // res = parse('a[b]|d[e][f]', [0, 1, 5, 1]);
      // test.expect(res.facts[1].flags, test.equals(s.Flags.feSingleWClassAllowed));

      // res = parse('a[b]|d', [0, 1, 5, 1]);
      // test.expect(res.facts[1].flags, test.equals(s.Flags.feMissingWClass));

      // res = parse('a|[f]d', [0, 1, 5, 1]);
      // test.expect(res.facts[0].flags, test.equals(s.Flags.feMissingWClass));
      // res = parse('a[b]|[f]d,e,[m]g', [0, 1, 8, 1, 10, 1, 15, 1]);
      // test.expect(res.facts[3].flags, test.equals(s.Flags.feWClassNotInFirstFact));
      // res = parse('a[b]^[f]d', [0, 1, 8, 1]);
      // test.expect(res.facts[1].flags, test.equals(s.Flags.feWClassNotInFirstFact));
    });

    test.test('parser', () {
      s.Facts res;
      String txt;

      // res = parse('a[b]|[c]d', [0, 1, 8, 1]);
      // test.expect(res[0].wordClass, test.equals('b'));
      // test.expect(res[1].wordClass, test.equals('c'));

      res = parse(txt = '-( (n o) )?', [4, 1, 6, 1]);
      test.expect(res.toText(), test.equals(txt));

      res = parse(txt = '-xxx-[n]?', [1, 3]);
      //test.expect(res[0].wordClass, test.equals('n'));
      test.expect(res.toText(), test.equals(txt));

      res = parse(txt = '-xxx-{n}?', [1, 3]);
      test.expect(res.toText(), test.equals(txt));

      res = parse(txt = '-xxx-^-yyy-', [1, 3, 7, 3]);
      test.expect(
          res.toText(), test.equals(txt));

      res = parse('-xxx-^-yyy-', [1, 3, 7, 3]);
      test.expect(
          res.facts[0].words[0].before + res.facts[1].words[0].before, test.equals('--'));

      res = parse('xxx^yyy', [0, 3, 4, 3]);
      test.expect(res.facts.length, test.equals(2));

      res = parse('', []);
      test.expect(res.facts.length, test.equals(0));

      res = parse('xxx');
      test.expect(res.facts[0].words[0].text, test.equals('xxx'));
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

      lex('xxxyy ?', 3, [0, 3, 3, 2]);
      lex('xxxyy ?', 5, [0, 2, 1, 3, 2, 3, 0, 4]);
      lex('xxx yy ?', 4, [0, 3, 4, 2]);
      lex('xxx yy ?', 6, [0, 3, 0, 1, 2, 1, 4, 2]);
      lex('xxx (([ ]{})|^, yy', 13, []);
      lex('', 0, []);
    });
  });
}
