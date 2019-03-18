import 'package:test/test.dart' as test;
import 'package:rw_utils/rewise.dart' as rw;

main() {
  test.group("PARSE FACT", () {
    test.test('parse text', () {
      var res;
      //res = rw.parseMachine('d{w}d[w]d(w)d');
      for (var i = 0; i < 100000; i++) res = rw.parseMachine('(([]{},),)d');
      test.expect(res == null, test.equals(false));
    });
  });
}
