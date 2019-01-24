import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/index.dart';

main() {

  test.setUp(() => DEV__ = false);
  test.tearDown(() => DEV__ = false);

  test.group("trie encoder", () {
    test.test('toBytes, simple', () {
      final wr = toBytes([IListNode.fromList('a')]);
      final str = wr.hexDump();
      test.expect(str, test.equals('5401610000'));
    });
    test.test('findNode, [a]', () {
      final wr = toBytes([IListNode.fromList('a')]);
      final str = wr.hexDump();
      test.expect(str, test.equals('5401610000'));
      final node = findNode(wr.toBytes(), 'a');
      test.expect(node?.data, test.equals(null));
    });

    test.test('findNode, BinarySearch test [a, b, c]', () {
      final wr = toBytes([
        IListNode.fromList('c', [1, 2, 4]),
        IListNode.fromList('a', [1]),
        IListNode.fromList('b', [1, 2]),
      ]);
      var str = wr.hexDump();
      test.expect(str, test.equals('5403616263000307010101010201020103010204'));
      final node = findNode(wr.toBytes(), 'c');
      str = node?.data?.hexDump();
      test.expect(str, test.equals('010204'));
    });

    test.test('findNode, deep', () {
      final wr = toBytes([
        IListNode.fromList('ab', [1, 2]),
        IListNode.fromList('a', [1]),
        IListNode.fromList('abc', [1, 2, 4]),
      ]);
      var str = wr.hexDump();
      test.expect(
          str, test.equals('54016100550101016200550201020163000103010204'));
      final node = findNode(wr.toBytes(), 'abc');
      str = node?.data?.hexDump();
      test.expect(str, test.equals('010204'));
    });

    test.test('toBytes', () {
      clearTrace();
      trace('*** WRITE');
      List<IListNode> nodes = List.from([
        IListNode.fromList('a'),
        IListNode.fromList('abc', [1, 2]),
        IListNode.fromList('abd', [16, 32]),
        IListNode.fromList('ab'),
        IListNode.fromList('abcd', [4, 8]),
      ]);
      final wr = toBytes(nodes);
      String str;
      str = getTrace();
      str = wr.hexDump();
      test.expect(
          str,
          test.equals(
              '540161005401620054026364000b550201020164000102040801021020'));
      trace('*** FIND NODE');
      final node = findNode(wr.toBytes(), 'abc');
      str = getTrace();
      str = node?.data?.hexDump();
      test.expect(str, test.equals('0102'));
    });
    test.test('findNode, chinese', () {
      final wr = toBytes([
        IListNode.fromList('a'),
        IListNode.fromList('汉', [1, 2]),
      ]);
      final node = findNode(wr.toBytes(), '汉');
      var str = getTrace();
      str = node?.data?.hexDump();
      test.expect(str, test.equals('0102'));
    });

    test.test('findNode, large', () {
      List<int> codes = range(300, 57);
      final nodes = List<IListNode>();
      for (final c1 in codes) {
        nodes.add(IListNode.fromList(String.fromCharCode(c1), [c1]));
        for (final c2 in codes) {
          nodes.add(IListNode.fromList(String.fromCharCode(c1) + String.fromCharCode(c2), [c1, c2]));
          for (final c3 in codes) {
            nodes.add(IListNode.fromList(String.fromCharCode(c1) + String.fromCharCode(c2) + String.fromCharCode(c3), [c1, c2, c3]));
          }
        }
      }
      final wr = toBytes(nodes);
      final bytes = wr.toBytes();
      final node = findNode(bytes, String.fromCharCodes([356,356,356]));
      String str;
      str = node?.data?.hexDump();
      test.expect(str, test.equals('646464'));
    });
  });
}
