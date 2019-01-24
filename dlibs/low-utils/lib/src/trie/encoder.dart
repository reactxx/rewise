import 'dart:typed_data';
import 'package:tuple/tuple.dart';
import './writer.dart';
import '../linq.dart';
import '../env.dart' as env;

class IListNode {
  IListNode(this.key, this.data);
  IListNode.fromList(this.key, [List<int> list])
      : data = list == null ? null : Uint8List.fromList(list);
  final String key;
  final Uint8List data;
}

BytesWriter toBytes(Iterable<IListNode> list) {
  TrieNode root = TrieNode(null, '');
  for (final node in list) insertNode(root, node);
  return root.toBytes();
}

void insertNode(TrieNode tnode, IListNode node) {
  var keyIdx = 0;
  for (final ch in node.key.codeUnits) {
    TrieNode child = null;
    if (tnode.childs == null) tnode.childs = Map<int, TrieNode>();
    keyIdx++;
    child = tnode.childs
        .putIfAbsent(ch, () => TrieNode(null, node.key.substring(0, keyIdx)));
    tnode = child;
  }
  tnode.data = node.data;
}

class TrieNode {
  TrieNode(this.data, this.key) {}

  Map<int, TrieNode> childs;
  Uint8List data;
  String key;

  BytesWriter toBytes() {
    final res = BytesWriter();

    final dataSize = getNumberSizeMask(data?.length);

    if (childs == null || childs.length == 0) {
      // no child

      // write length flags
      res.addNumber(dataSize, 1);

      // write node data
      if (dataSize > 0) {
        res.addNumber(data.length, dataSize);
        res.addBytes(data);
      }

      env.traceFunc(() => '$key=${res.hexDump()}');
    } else {
      // childs exists

      // ** compute child data size
      final childsCount = childs.length;
      final childsCountSize = getNumberSizeMask(childsCount);

      final childsData = List.of(
          childs.entries.map((kv) => Tuple2(kv.key, kv.value.toBytes())), growable:false);
      childsData.sort((a, b) => a.item1 - b.item1);
      final childDataLen = sum(childsData.map((d) => d.item2.len));
      final childsDataSize = getNumberSizeMask(childDataLen);
      final keySize = getNumberSizeMask(max(childsData.map((kb) => kb.item1)));

      // write length flags
      res.addNumber(
          (childsCountSize << 6) |
              (childsDataSize << 4) |
              (keySize << 2) |
              dataSize,
          1);

      // write node data
      if (dataSize > 0) {
        res.addNumber(data.length, dataSize);
        res.addBytes(data);
      }

      res.addNumber(childsCount, childsCountSize); // write child num

      for (var i = 0; i < childsCount; i++) // write keys
        res.addNumber(childsData[i].item1, keySize);

      // write childs offsets
      var childOffset = 0;
      for (var i = 0; i < childsCount; i++) {
        res.addNumber(childOffset, childsDataSize);
        childOffset += childsData[i].item2.len;
      }

      env.traceFunc(() => '$key=${res.hexDump()}');

      for (var i = 0; i < childsCount; i++) // write child data
        res.addWriter(childsData[i].item2);
    }

    return res;
  }
}
