import 'package:test/test.dart' as test;
import 'package:tuple/tuple.dart';
import 'package:rewise_low_utils/trie.dart' as trie;
import 'package:rewise_low_utils/env.dart' as env;
import 'package:rewise_low_utils/linq.dart' as linq;
import 'dart:convert' as convert;

main() {
  test.setUp(() => env.DEV__ = false);
  test.tearDown(() => env.DEV__ = false);

  test.group("trie encoder", () {
    test.test('serialize node', () {
      // 1
      var text = trie.InputNode.fromList('a', [1, 2, 4, 8, 16, 32, 54, 128]).toJson();
      test.expect(
          trie.InputNode.fromJson(text).toJson(),
          test.equals(text));
      // 2
      text = trie.InputNode.fromList('bbb', []).toJson();
      test.expect(
          trie.InputNode.fromJson(text).toJson(),
          test.equals(text));
      // 3
      text = trie.InputNode.fromList('c').toJson();
      test.expect(
          trie.InputNode.fromJson(text).toJson(),
          test.equals(text));
    });
    test.test('toBytes, simple', () {
      final wr = trie.toBytes([trie.InputNode.fromList('a')]);
      final str = wr.hexDump();
      test.expect(str, test.equals('546100'));
    });
    test.test('findNode, [a]', () {
      final wr = trie.toBytes([trie.InputNode.fromList('a')]);
      final str = wr.hexDump();
      test.expect(str, test.equals('546100'));
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
      test.expect(str, test.equals('94036162630307010101010201020103010204'));
      final node = trie.findNode(wr.toBytes(), 'c');
      str = node.data?.hexDump();
      test.expect(str, test.equals('010204'));
    });

    test.test('findNode, deep', () {
      // len 16. -6 (data), -6 (chars) => chars + data + 4.
      final wr = trie.toBytes([
        trie.InputNode.fromList('ab', [1, 2]),
        trie.InputNode.fromList('a', [1]),
        trie.InputNode.fromList('abc', [1, 2, 4]),
      ]);
      var str = wr.hexDump();
      test.expect(str, test.equals('54615501016255020102630103010204'));
      final node = trie.findNode(wr.toBytes(), 'abc');
      str = node.data?.hexDump();
      test.expect(str, test.equals('010204'));
    });

    test.test('toBytes', () {
      // len 22. -6 (data), -13 (chars) => chars + data + 3.
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
          str, test.equals('54615462940263640955020102640102040801021020'));
      env.trace('*** FIND NODE');
      final node = trie.findNode(wr.toBytes(), 'abc');
      str = env.getTrace();
      str = node.data?.hexDump();
      test.expect(str, test.equals('0102'));
    });
    test.test('findNode, chinese', () {
      final wr = trie.toBytes([
        trie.InputNode.fromList('a'),
        trie.InputNode.fromList('汉', [1, 2]),
      ]);
      final node = trie.findNode(wr.toBytes(), '汉');
      var str = env.getTrace();
      str = node.data?.hexDump();
      test.expect(str, test.equals('0102'));
    });

    test.test('findNode, linear tree', () {
      // not optimalized: len=105, optimalized: 53
      final allChars = String.fromCharCodes(linq.range(97, 26));
      final wr = trie.toBytes([
        trie.InputNode.fromList(allChars),
      ]);
      final bytes = wr.toBytes();
      final node = trie.findNode(bytes, allChars);
      var str = env.getTrace();
      str = node.data?.hexDump();
      test.expect(str, test.equals(null));
    });

    test.test('findNode, large', () {
      final nodes = getLargeData(300, 57);
      //final nodes = getLargeData(97, 26);
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
      final found = List<String>();
      trie.findDescendantNodes(bytes, search, (node) {
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
