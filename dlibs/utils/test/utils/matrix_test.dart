import 'package:test/test.dart' as test;
import 'package:rw_utils/utils.dart' as ut;

main() {
  test.test('matrix-test', () {
    const data = 'cs-CZ;ahoj;jak;se;mas';
    ut.Row rw;
    setRw() => rw = ut.Row(null, data)..delim = ';';

    // READ
    setRw();
    test.expect(rw.str, test.equals(data));

    test.expect(rw.raw, test.equals('ahoj;jak;se;mas'));
    test.expect(rw.lang, test.equals('cs-CZ'));

    test.expect(rw.lang, test.equals('cs-CZ'));
    test.expect(rw.raw, test.equals('ahoj;jak;se;mas'));

    test.expect(rw.lang, test.equals('cs-CZ'));
    test.expect(rw.langData, test.equals(['ahoj','jak','se','mas']));

    test.expect(rw.langData, test.equals(['ahoj','jak','se','mas']));
    test.expect(rw.lang, test.equals('cs-CZ'));

    test.expect(rw.str, test.equals(data));

    // WRITE
    setRw();
    test.expect((rw..setLangData(0, 'Ahoj')).raw, test.equals('Ahoj;jak;se;mas'));
    test.expect((rw..lang = 'es-ES'..setData(4, 'mas?')).str, test.equals('es-ES;Ahoj;jak;se;mas?'));
    test.expect(rw.getData(0), test.equals('es-ES'));
    test.expect(rw.getLangData(0), test.equals('Ahoj'));
    test.expect(rw.dataLength, test.equals(5));
    test.expect(rw.langDataLength, test.equals(4));

  });
}
