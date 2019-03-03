import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/env.dart' as env;
import 'package:rewise_low_utils/toBinary.dart' as binary;

main() {
  test.setUp(() => env.DEV__ = false);
  test.tearDown(() => env.DEV__ = false);

  test.group("huffman", () {
    test.test('tree', () {
      final tree = binary.stringBuildFromData('aaaaaaaabbbccc');

      var dump = tree.dump;
      test.expect(dump, test.equals('{"a":"0","b":"11","c":"10"}'));
    });
  });
}
