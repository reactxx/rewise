import 'package:test/test.dart' as test;
import 'package:rw_utils/rewise.dart' as rw;

main() {
  test.group("PARSE FACT", () {
    test.test('parse text', () {
      var res = rw.parseMachine('d{w}d[w]d(w)d');
      test.expect(res==null, test.equals(false));
    });

  });
}
