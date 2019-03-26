import 'package:test/test.dart' as test;
import 'package:rw_utils/rewise.dart' as rw;

main() {
  test.group("PARSE FACT", () {
    test.test('parse text', () {
      rw.FactState it;
      //it = rw.parseMachine('ის [კაცი] ღარიბი იყო, ის [ქალი] კი – მდიდარი.');
      //test.expect(it.errors.length>0, test.equals(true));
      it = rw.parseMachine('abcd|');
      test.expect(it.errors.length>0, test.equals(true));
    });

    test.test('parseRaw', () {
      rw.FactState it;
      it = rw.parseMachine('');
      test.expect(it.devText, test.equals(''));
      it = rw.parseMachine('d');
      test.expect(it.devText, test.equals('d'));
      it = rw.parseMachine('d[w]|[w]d');
      test.expect(it.childs.length, test.equals(2));
      it = rw.parseMachine('d^');
      test.expect(it.childs.length, test.equals(1));
      it = rw.parseMachine('d,');
      test.expect(it.childs.length, test.equals(1));
      it = rw.parseMachine('d[w]^d{w}');
      test.expect(it.devText, test.equals('d, d{w}'));
      test.expect(it.devBreakText, test.equals('d, d   '));
      it = rw.parseMachine('[w]d^d|[w]d,d|[w]d^d,d');
      test.expect(it.devText, test.equals('d, d, d, d, d, d, d'));
      test.expect(it.devBreakText, test.equals('d, d, d, d, d, d, d'));
      it = rw.parseMachine('[w]{1}d^d(2),{3}d|[w]{4}d{5},d');
      test.expect(it.devText, test.equals('{1}d, d(2), {3}d, {4}d{5}, d'));
      test.expect(it.devBreakText, test.equals('   d, d   ,    d,    d   , d'));
    });

    test.test('tests', () {
      rw.FactState it;
      // value checks
      // Errors checks
      it = rw.parseMachine('()d   ');
      test.expect(it.errors[0].code, test.equals(rw.ErrorCodes.emptyBracket));
      it = rw.parseMachine('(d   ');
      test.expect(it.errors[0].code, test.equals(rw.ErrorCodes.missingCloseBracket));
      it = rw.parseMachine('d)');
      test.expect(it.errors[0].code, test.equals(rw.ErrorCodes.closeBracketWithoutOpenOne));
      it = rw.parseMachine('d[');
      test.expect(it.errors[0].code, test.equals(rw.ErrorCodes.emptyBracket));
      test.expect(it.errors[1].code, test.equals(rw.ErrorCodes.missingCloseBracket));
      it = rw.parseMachine('d]');
      test.expect(it.errors[0].code, test.equals(rw.ErrorCodes.closeBracketWithoutOpenOne));
      it = rw.parseMachine('d{');
      test.expect(it.errors[0].code, test.equals(rw.ErrorCodes.emptyBracket));
      test.expect(it.errors[1].code, test.equals(rw.ErrorCodes.missingCloseBracket));
      it = rw.parseMachine('}d');
      test.expect(it.errors[0].code, test.equals(rw.ErrorCodes.closeBracketWithoutOpenOne));

      it = rw.parseMachine(' [w]  {s} (z)   ');
      test.expect(
          it.errors[0].code, test.equals(rw.ErrorCodes.emptyFactBreakText));

      it = rw.parseMachine('d^[w] d');
      test.expect(it.errors[0].code, test.equals(rw.ErrorCodes.wordClassInOthers));
      it = rw.parseMachine('d^[w] d [w]');
      test.expect(it.errors[0].code, test.equals(rw.ErrorCodes.moreWordClasses));
      it = rw.parseMachine('d|d[w]');
      test.expect(it.errors[0].code, test.equals(rw.ErrorCodes.missingWordClass));
      it = rw.parseMachine('d(|d[w]');
      test.expect(it.errors.length, test.equals(3));
    });

    test.test('join synonymous', () {
      rw.FactState it;
      it = rw.parseMachine('[w]d|[w]d(b),e{b},f');
      test.expect(it.devText, test.equals('d, d(b), e{b}, f'));
      test.expect(it.devBreakText, test.equals('d, d   , e   , f'));
      it = rw.parseMachine('[w]d^{1}d,(2)d|[w]d(3),e{4},f^d(5),e{6},f');
      final brs = it.brackets.map((b) => b.value).join(';');
      test.expect(brs, test.equals('w;1;2;w;3;4;5;6'));
      test.expect(it.devText,
          test.equals('d, {1}d, (2)d, d(3), e{4}, f, d(5), e{6}, f'));
      test.expect(it.devBreakText,
          test.equals('d,    d,    d, d   , e   , f, d   , e   , f'));
    });
  });
}
