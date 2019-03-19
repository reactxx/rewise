import 'package:test/test.dart' as test;
import 'package:rw_utils/rewise.dart' as rw;

main() {
  test.group("PARSE FACT", () {
    test.test('parse text', () {
      var res = rw.parse('d{w}d[w]d');
      rw.ParsedFact it;
      it = rw.ParsedFact('ის [კაცი] და ის [ქალი]');
      test.expect(it == res, test.equals(false));
    });

    test.test('parseRaw', () {
      rw.ParsedFact it;
      it = rw.ParsedFact('');
      test.expect(it.text, test.equals(''));
      it = rw.ParsedFact('d');
      test.expect(it.text, test.equals('d'));
      it = rw.ParsedFact('d[w]|[w]d');
      test.expect(it.childs.length, test.equals(2));
      it = rw.ParsedFact('d^');
      test.expect(it.childs.length, test.equals(2));
      it = rw.ParsedFact('d,');
      test.expect(it.childs, test.equals(null));
      it = rw.ParsedFact('d[w]^d{w}');
      test.expect(it.devText, test.equals('d, d{w}'));
      test.expect(it.devBreakText, test.equals('d, d   '));
      it = rw.ParsedFact('[w]d^d|[w]d,d|[w]d^d,d');
      test.expect(it.devText, test.equals('d, d, d, d, d, d, d'));
      test.expect(it.devBreakText, test.equals('d, d, d, d, d, d, d'));
      it = rw.ParsedFact('[w]{1}d^d(2),{3}d|[w]{4}d{5},d');
      test.expect(it.devText, test.equals('{1}d, d(2), {3}d, {4}d{5}, d'));
      test.expect(it.devBreakText, test.equals('   d, d   ,    d,    d   , d'));
    });

    test.test('tests', () {
      rw.ParsedFact it;
      // value checks
      // Errors checks
      it = rw.ParsedFact('()d   ');
      test.expect(it.errors[0].code, test.equals(rw.Consts.eEmptyBracket));
      it = rw.ParsedFact('(d   ');
      test.expect(it.errors[0].code, test.equals(rw.Consts.eSyntax));
      it = rw.ParsedFact('d)');
      test.expect(it.errors[0].code, test.equals(rw.Consts.eSyntax));
      it = rw.ParsedFact('d[');
      test.expect(it.errors[0].code, test.equals(rw.Consts.eSyntax));
      it = rw.ParsedFact('d]');
      test.expect(it.errors[0].code, test.equals(rw.Consts.eSyntax));
      it = rw.ParsedFact('d{');
      test.expect(it.errors[0].code, test.equals(rw.Consts.eSyntax));
      it = rw.ParsedFact('}d');
      test.expect(it.errors[0].code, test.equals(rw.Consts.eSyntax));

      it = rw.ParsedFact(' [w]  {s} (z)   ');
      test.expect(
          it.errors[0].code, test.equals(rw.Consts.eEmptyWithoutBrackets));

      it = rw.ParsedFact('d^[w] d');
      test.expect(it.errors[0].code, test.equals(rw.Consts.eWClsOther));
      it = rw.ParsedFact('d^[w] d [w]');
      test.expect(it.errors[0].code, test.equals(rw.Consts.eWClsMore));
      it = rw.ParsedFact('d|d[w]');
      test.expect(it.errors[0].code, test.equals(rw.Consts.eWClsMissing));
      it = rw.ParsedFact('d(|d[w]');
      test.expect(it.errors.length, test.equals(2));
    });

    test.test('join synonymous', () {
      rw.ParsedFact it;
      it = rw.ParsedFact('[w]d|[w]d(b),e{b},f');
      test.expect(it.devText, test.equals('d, d(b), e{b}, f'));
      test.expect(it.devBreakText, test.equals('d, d   , e   , f'));
      it = rw.ParsedFact('[w]d^{1}d,(2)d|[w]d(3),e{4},f^d(5),e{6},f');
      final brs = it.brackets.map((b) => b.value).join(';');
      test.expect(brs, test.equals('w;1;2;w;3;4;5;6'));
      test.expect(it.devText,
          test.equals('d, {1}d, (2)d, d(3), e{4}, f, d(5), e{6}, f'));
      test.expect(it.devBreakText,
          test.equals('d,    d,    d, d   , e   , f, d   , e   , f'));
    });
  });
}
