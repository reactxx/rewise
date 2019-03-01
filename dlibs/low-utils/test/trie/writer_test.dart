import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/env.dart' as env;
import 'package:rewise_low_utils/trie.dart' as trie;

main() {
  test.setUp(() => env.DEV__ = false);
  test.tearDown(() => env.DEV__ = false);

  test.group("trie writer", () {
    test.test('addNumber', () {

      final wr = trie.BytesWriter();

      var writeNum = (int n) {
        wr.writeNumber(n, trie.BytesWriter.getNumberSizeMask(n));
      };

      String str;
      writeNum(0);
      str = wr.hexDump();
      test.expect(str, test.equals(''));

      writeNum(1);
      str = wr.hexDump();
      test.expect(str, test.equals('01'));

      writeNum(0x100);
      str = wr.hexDump();
      test.expect(str, test.equals('010001'));

      writeNum(0x10000);
      str = wr.hexDump();
      test.expect(str, test.equals('010001000001'));

      writeNum(0xffffff);
      str = wr.hexDump();
      test.expect(str, test.equals('010001000001ffffff'));
    });

    test.test('addList', () {
      final wr = trie.BytesWriter();
      wr.writeList([1, 2, 4, 8, 16]);
      var str = wr.hexDump();
      test.expect(str, test.equals('0102040810'));
    });

    test.test('addWriter', () {
      final wr = trie.BytesWriter();
      trie.BytesWriter subwr = trie.BytesWriter();
      subwr.writeList([1, 2]);
      wr.writeWriter(subwr);
      wr.writeList([4, 8, 16]);
      var str = wr.hexDump();
      test.expect(str, test.equals('0102040810'));
    });
  });
}
