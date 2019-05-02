import 'package:test/test.dart' as test;
import 'package:rw_utils/utils.dart' as ut;

main() {
  test.test('matrix-test', () {
    var ints = ut.Interval.intervals(12,4);
    var res = ints.expand((i) => [i.start, i.end]).join(',');
    test.expect(res, test.equals('0,4,4,8,8,12'));

    ints = ut.Interval.intervalsMaxLen(12,4);
    res = ints.expand((i) => [i.start, i.end]).join(',');
    test.expect(res, test.equals('0,4,4,8,8,12'));

    ints = ut.Interval.intervals(5,4);
    res = ints.expand((i) => [i.start, i.end]).join(',');
    test.expect(res, test.equals('0,4,4,5'));
    ints = ut.Interval.intervalsMaxLen(5,4);
    res = ints.expand((i) => [i.start, i.end]).join(',');
    test.expect(res, test.equals('0,3,3,5'));
  });
}
