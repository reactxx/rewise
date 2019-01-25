import 'package:tuple/tuple.dart';
import 'package:rewise_low_utils/trie.dart' as trie;
//import 'package:rewise_low_utils/env.dart' as env;
import 'package:rewise_low_utils/linq.dart' as linq;

import 'dart:html';

void main() {
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

  querySelector('#root').text = found.join(', ');
  //querySelector('#root').text = 'xx';
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
