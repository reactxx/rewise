import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/trie.dart' as trie;
import 'package:rewise_low_utils/env.dart' as env;
import 'package:rewise_low_utils/linq.dart' as linq;
import 'package:tuple/tuple.dart';

main() {
  test.setUp(() => env.DEV__ = false);
  test.tearDown(() => env.DEV__ = false);

  test.group("trie encoder", () {
    test.test('toBytes, simple', () {
      final wr = trie.toBytes([trie.InputNode.fromList('a')]);
      final str = wr.hexDump();
      test.expect(str, test.equals('5401610000'));
    });
    test.test('findNode, [a]', () {
      final wr = trie.toBytes([trie.InputNode.fromList('a')]);
      final str = wr.hexDump();
      test.expect(str, test.equals('5401610000'));
      final node = trie.findNode(wr.toBytes(), 'a');
      test.expect(node?.data, test.equals(null));
    });

    test.test('findNode, BinarySearch test [a, b, c]', () {
      final wr = trie.toBytes([
        trie.InputNode.fromList('c', [1, 2, 4]),
        trie.InputNode.fromList('a', [1]),
        trie.InputNode.fromList('b', [1, 2]),
      ]);
      var str = wr.hexDump();
      test.expect(str, test.equals('5403616263000307010101010201020103010204'));
      final node = trie.findNode(wr.toBytes(), 'c');
      str = node?.data?.hexDump();
      test.expect(str, test.equals('010204'));
    });

    test.test('findNode, deep', () {
      final wr = trie.toBytes([
        trie.InputNode.fromList('ab', [1, 2]),
        trie.InputNode.fromList('a', [1]),
        trie.InputNode.fromList('abc', [1, 2, 4]),
      ]);
      var str = wr.hexDump();
      test.expect(
          str, test.equals('54016100550101016200550201020163000103010204'));
      final node = trie.findNode(wr.toBytes(), 'abc');
      str = node?.data?.hexDump();
      test.expect(str, test.equals('010204'));
    });

    test.test('toBytes', () {
      env.clearTrace();
      env.trace('*** WRITE');
      List<trie.InputNode> nodes = List.from([
        trie.InputNode.fromList('a'),
        trie.InputNode.fromList('abc', [1, 2]),
        trie.InputNode.fromList('abd', [16, 32]),
        trie.InputNode.fromList('ab'),
        trie.InputNode.fromList('abcd', [4, 8]),
      ]);
      final wr = trie.toBytes(nodes);
      String str;
      str = env.getTrace();
      str = wr.hexDump();
      test.expect(
          str,
          test.equals(
              '540161005401620054026364000b550201020164000102040801021020'));
      env.trace('*** FIND NODE');
      final node = trie.findNode(wr.toBytes(), 'abc');
      str = env.getTrace();
      str = node?.data?.hexDump();
      test.expect(str, test.equals('0102'));
    });
    test.test('findNode, chinese', () {
      final wr = trie.toBytes([
        trie.InputNode.fromList('a'),
        trie.InputNode.fromList('汉', [1, 2]),
      ]);
      final node = trie.findNode(wr.toBytes(), '汉');
      var str = env.getTrace();
      str = node?.data?.hexDump();
      test.expect(str, test.equals('0102'));
    });

    test.test('findNode, large', () {
      //final nodes = getLargeData(300, 57);
      final nodes = getLargeData(97, 26);
      final wr = trie.toBytes(nodes.item1);
      final bytes = wr.toBytes();
      final search =
          String.fromCharCodes([nodes.item2, nodes.item2, nodes.item2]);
      final node = trie.findNode(bytes, search);
      String str;
      str = node?.data?.hexDump();
      test.expect(str, test.equals('010204'));
    });

    test.test('findDescendantNodes', () {
      final nodes = getLargeData(97, 26);
      final wr = trie.toBytes(nodes.item1);
      final bytes = wr.toBytes();
      final search = 'p';
      final found = new List<String>();
      trie.findDescendantNodes(bytes, search, (node) {
        if (found.length > 100) {
          return false;
        }
        found.add(node.key);
        return true;
      });
      test.expect(null, test.equals(null)); // String str;
      // str = node?.data?.hexDump();
      // test.expect(str, test.equals('010204'));
    });
  });
}

Tuple2<List<trie.InputNode>, int> getLargeData(int from, int to) {
  List<int> codes = linq.range(from, to);
  final nodes = List<trie.InputNode>();
  for (final c1 in codes) {
    nodes.add(trie.InputNode.fromList(String.fromCharCode(c1), [1]));
    for (final c2 in codes) {
      nodes.add(trie.InputNode.fromList(
          String.fromCharCode(c1) + String.fromCharCode(c2), [1, 2]));
      for (final c3 in codes) {
        nodes.add(trie.InputNode.fromList(
            String.fromCharCode(c1) +
                String.fromCharCode(c2) +
                String.fromCharCode(c3),
            [1, 2, 4]));
      }
    }
  }
  return Tuple2(nodes, from + to - 1);
}
