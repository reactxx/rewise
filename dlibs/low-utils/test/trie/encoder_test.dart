import 'package:test/test.dart' as test;
import 'package:tuple/tuple.dart';
import 'package:convert/convert.dart' as convert;
import 'package:rewise_low_utils/toBinary.dart' as binary;
import 'package:rewise_low_utils/env.dart' as env;
import 'package:rewise_low_utils/linq.dart' as linq;

main() {
  test.setUp(() => env.DEV__ = false);
  test.tearDown(() => env.DEV__ = false);

  test.group("trie encoder", () {
    test.test('toBytes, simple', () {
      final bytes =
          binary.TrieInputNodeToBytes([binary.TrieInputNode.fromList('a')]);
      final str = convert.hex.encode(bytes);
      test.expect(str, test.equals('546100'));
    });
    test.test('findNode, [a]', () {
      final bytes =
          binary.TrieInputNodeToBytes([binary.TrieInputNode.fromList('a')]);
      final str = convert.hex.encode(bytes);
      test.expect(str, test.equals('546100'));
      final node = binary.trieFindNode(bytes, 'a');
      test.expect(node?.data, test.equals(null));
    });

    test.test('findNode, BinarySearch test [a, b, c]', () {
      final bytes = binary.TrieInputNodeToBytes([
        binary.TrieInputNode.fromList('c', [1, 2, 4]),
        binary.TrieInputNode.fromList('a', [1]),
        binary.TrieInputNode.fromList('b', [1, 2]),
      ]);
      var str = convert.hex.encode(bytes);
      test.expect(str, test.equals('94036162630307010101010201020103010204'));
      final node = binary.trieFindNode(bytes, 'c');
      str = node.data?.hexDump();
      test.expect(str, test.equals('010204'));
    });

    test.test('findNode, deep', () {
      // len 16. -6 (data), -6 (chars) => chars + data + 4.
      final bytes = binary.TrieInputNodeToBytes([
        binary.TrieInputNode.fromList('ab', [1, 2]),
        binary.TrieInputNode.fromList('a', [1]),
        binary.TrieInputNode.fromList('abc', [1, 2, 4]),
      ]);
      var str = convert.hex.encode(bytes);
      test.expect(str, test.equals('54615501016255020102630103010204'));
      final node = binary.trieFindNode(bytes, 'abc');
      str = node.data?.hexDump();
      test.expect(str, test.equals('010204'));
    });

    test.test('toBytes', () {
      // len 22. -6 (data), -13 (chars) => chars + data + 3.
      env.clearTrace();
      env.trace('*** WRITE');
      List<binary.TrieInputNode> nodes = List.from([
        binary.TrieInputNode.fromList('a'),
        binary.TrieInputNode.fromList('abc', [1, 2]),
        binary.TrieInputNode.fromList('abd', [16, 32]),
        binary.TrieInputNode.fromList('ab'),
        binary.TrieInputNode.fromList('abcd', [4, 8]),
      ]);
      final bytes = binary.TrieInputNodeToBytes(nodes);
      String str;
      str = env.getTrace();
      str = convert.hex.encode(bytes);
      test.expect(
          str, test.equals('54615462940263640955020102640102040801021020'));
      env.trace('*** FIND NODE');
      final node = binary.trieFindNode(bytes, 'abc');
      str = env.getTrace();
      str = node.data?.hexDump();
      test.expect(str, test.equals('0102'));
    });
    test.test('findNode, chinese', () {
      final bytes = binary.TrieInputNodeToBytes([
        binary.TrieInputNode.fromList('a'),
        binary.TrieInputNode.fromList('汉', [1, 2]),
      ]);
      final node = binary.trieFindNode(bytes, '汉');
      var str = env.getTrace();
      str = node.data?.hexDump();
      test.expect(str, test.equals('0102'));
    });

    test.test('findNode, linear tree', () {
      // not optimalized: len=105, optimalized: 53
      final allChars = String.fromCharCodes(linq.range(97, 26));
      final bytes = binary.TrieInputNodeToBytes([
        binary.TrieInputNode.fromList(allChars),
      ]);
      final node = binary.trieFindNode(bytes, allChars);
      var str = env.getTrace();
      str = node.data?.hexDump();
      test.expect(str, test.equals(null));
    });

    test.test('findNode, large', () {
      final nodes = getLargeData(300, 57);
      //final nodes = getLargeData(97, 26);
      //final nodes = getLargeData(97, 2);
      final bytes = binary.TrieInputNodeToBytes(nodes.item1);
      final search =
          String.fromCharCodes([nodes.item2, nodes.item2, nodes.item2]);
      final node = binary.trieFindNode(bytes, search);
      String str;
      str = node?.data?.hexDump();
      test.expect(str, test.equals('010204'));
    });

    test.test('findDescendantNodes', () {
      final nodes = getLargeData(97, 26);
      final bytes = binary.TrieInputNodeToBytes(nodes.item1);
      final search = 'p';
      final found = List<String>();
      binary.trieVisitDescendantNodes(bytes, search, (node) {
        if (found.length > 55) {
          return false;
        }
        found.add(node.key);
        return true;
      });
      final str = found.join(',');
      test.expect(
          str,
          test.equals(
              'p,pa,paa,pab,pac,pad,pae,paf,pag,pah,pai,paj,pak,pal,pam,pan,pao,pap,paq,par,pas,pat,pau,pav,paw,pax,pay,paz,pb,pba,pbb,pbc,pbd,pbe,pbf,pbg,pbh,pbi,pbj,pbk,pbl,pbm,pbn,pbo,pbp,pbq,pbr,pbs,pbt,pbu,pbv,pbw,pbx,pby,pbz,pc'));
    });
  });
}

Tuple2<List<binary.TrieInputNode>, int> getLargeData(int from, int length) {
  //final codes = linq.range(from, to);
  final nodes = List<binary.TrieInputNode>();
  for (final c1 in linq.range(from, length)) {
    nodes.add(binary.TrieInputNode.fromList(String.fromCharCode(c1), [1]));
    for (final c2 in linq.range(from, length)) {
      nodes.add(binary.TrieInputNode.fromList(
          String.fromCharCode(c1) + String.fromCharCode(c2), [1, 2]));
      for (final c3 in linq.range(from, length)) {
        nodes.add(binary.TrieInputNode.fromList(
            String.fromCharCode(c1) +
                String.fromCharCode(c2) +
                String.fromCharCode(c3),
            [1, 2, 4]));
      }
    }
  }
  return Tuple2(nodes, from + length - 1);
}
