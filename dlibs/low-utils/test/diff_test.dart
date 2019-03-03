import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/diff.dart' as diff;

main() {
  test.test('diff-test', () {
    var text1 = "aa bb cc dd aa bb cc dd";
    var text2 = " bb xy dd bb xy dd";

    var hist = diff.DiffHistory<TestInfo>();
    hist.push(text1, TestInfo(1));
    hist.push(text2, TestInfo(2));
    hist.push(text1, TestInfo(3));

    var strs = hist.getValues();
    test.expect(strs[2], test.equals(text1));
    test.expect(strs[1], test.equals(text2));

    var p = hist.pop();
    test.expect(hist.getValues()[1], test.equals(text2));

    p = hist.pop();
    test.expect(hist.getValues()[0], test.equals(text1));

    p = hist.pop();
    test.expect(hist.getValues().length, test.equals(0));
  });

}

class TestInfo {
  TestInfo(this.count);
  int count;
}
