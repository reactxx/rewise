import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/rewise.dart' as rw;

main() {
  test.group("rewise", () {
    test.test('parseRaw', () {
      rw.Item it;
      it = rw.Item('');
      test.expect(it.errors?.length, test.equals(null));
      it = rw.Item('d');
      test.expect(it.errors?.length, test.equals(null));
      it = rw.Item('d[w]|[w]d');
      test.expect(it.errors?.length, test.equals(null));
      it = rw.Item('d^');
      test.expect(it.errors?.length, test.equals(null));
      it = rw.Item('d,');
      test.expect(it.errors?.length, test.equals(null));
      it = rw.Item('d[w]^d|d[w]');
      test.expect(it.errors?.length, test.equals(null));
      it = rw.Item('[w]d^d|[w]d,d|[w]d^d,d');
      test.expect(it.errors?.length, test.equals(null));
      it = rw.Item('[w]d^d|[w]d,d|[w]d^d,d(d)d');
      test.expect(it.errors?.length, test.equals(null));
    });

    test.test('regex', () {
      rw.Item it;
      // value checks
      // Errors checks
      it = rw.Item('(d   ');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eSyntax));
      it = rw.Item('d)');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eSyntax));
      it = rw.Item('d[');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eSyntax));
      it = rw.Item('d]');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eSyntax));
      it = rw.Item('d{');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eSyntax));
      it = rw.Item('}d');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eSyntax));

      it = rw.Item(' [w]  {s} (z)   ');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eEmpty));

      it = rw.Item('d^[w] d');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eWClsOther));
      it = rw.Item('d^[w] d [w]');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eWClsMore));
      it = rw.Item('d|d[w]');
      test.expect(it.errors[0].item2, test.equals(rw.Consts.eWClsMissing));
      it = rw.Item('d(|d[w]');
      test.expect(it.errors.length, test.equals(2));
    });

    test.test('join synonymous', () {
      rw.Item it;
      it = rw.Item('[w]d|[w]d(b),e{b},f');
      test.expect(it.child[1].text, test.equals('[w]d(b), e{b}, f'));
      test.expect(it.child[1].stemmingText, test.equals('   d   , e   , f'));
    });
  });
}
