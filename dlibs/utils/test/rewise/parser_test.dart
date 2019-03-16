import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/rewise.dart' as rw;

main() {
  test.group("rewise", () {
    test.test('parseRaw', () {
      rw.ParsedFact it;
      it = rw.ParsedFact('');
      test.expect(it.errors?.length, test.equals(null));
      it = rw.ParsedFact('d');
      test.expect(it.errors?.length, test.equals(null));
      it = rw.ParsedFact('d[w]|[w]d');
      test.expect(it.errors?.length, test.equals(null));
      it = rw.ParsedFact('d^');
      test.expect(it.errors?.length, test.equals(null));
      it = rw.ParsedFact('d,');
      test.expect(it.errors?.length, test.equals(null));
      it = rw.ParsedFact('d[w]^d|d[w]');
      test.expect(it.errors?.length, test.equals(null));
      it = rw.ParsedFact('[w]d^d|[w]d,d|[w]d^d,d');
      test.expect(it.errors?.length, test.equals(null));
      it = rw.ParsedFact('[w]d^d|[w]d,d|[w]d^d,d(d)d');
      test.expect(it.errors?.length, test.equals(null));
    });

    test.test('regex', () {
      rw.ParsedFact it;
      // value checks
      // Errors checks
      it = rw.ParsedFact('(d   ');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eSyntax));
      it = rw.ParsedFact('d)');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eSyntax));
      it = rw.ParsedFact('d[');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eSyntax));
      it = rw.ParsedFact('d]');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eSyntax));
      it = rw.ParsedFact('d{');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eSyntax));
      it = rw.ParsedFact('}d');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eSyntax));

      it = rw.ParsedFact(' [w]  {s} (z)   ');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eEmptyWithoutBrackets));

      it = rw.ParsedFact('d^[w] d');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eWClsOther));
      it = rw.ParsedFact('d^[w] d [w]');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eWClsMore));
      it = rw.ParsedFact('d|d[w]');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eWClsMissing));
      it = rw.ParsedFact('d(|d[w]');
      test.expect(it.errors.length, test.equals(2));
    });

    test.test('join synonymous', () {
      rw.ParsedFact it;
      it = rw.ParsedFact('[w]d|[w]d(b),e{b},f');
      test.expect(it.child[1].text, test.equals('d(b), e{b}, f'));
      test.expect(it.child[1].toBreak, test.equals('d   , e   , f'));
      it = rw.ParsedFact('[w]d^{1}d,(2)d|[w]d(3),e{4},f^d(5),e{6},f');
      final brs = it.brackets.map((b)=>b.value).join(';');
      final cht = it.child.map((b)=>b.text).join(';');
      final chb = it.child.map((b)=>b.toBreak).join(';');
      test.expect(brs, test.equals('w;1;2;w;3;4;5;6'));
      test.expect(cht, test.equals('d;{1}d, (2)d;d(3), e{4}, f;d(5), e{6}, f'));
      test.expect(chb, test.equals('d;   d,    d;d   , e   , f;d   , e   , f'));
    });
  });
}
