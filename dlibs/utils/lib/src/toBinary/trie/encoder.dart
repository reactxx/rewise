import 'dart:typed_data';
import 'package:tuple/tuple.dart';

import 'package:rewise_low_utils/toBinary.dart' as binary;
import 'package:rewise_low_utils/utils.dart' show Linq;
//import 'package:rewise_low_utils/env.dart' as env;

class TrieInputNode {
  TrieInputNode(this.key, [this.data]);
  TrieInputNode.fromList(this.key, [Iterable<int> list])
      : data = list == null ? null : Uint8List.fromList(list);
  final String key;
  final Uint8List data;
}

Uint8List TrieInputNodeToBytes(Iterable<TrieInputNode> list) {
  TrieEncNode root = TrieEncNode(null, '');
  for (final node in list) _insertNode(root, node);
  return root.toBytes();
}

void _insertNode(TrieEncNode tnode, TrieInputNode node) {
  var keyIdx = 0;
  for (final ch in node.key.codeUnits) {
    if (tnode.childs == null) tnode.childs = Map<int, TrieEncNode>();
    keyIdx++;
    final child = tnode.childs
        .putIfAbsent(ch, () => TrieEncNode(null, node.key.substring(0, keyIdx)));
    tnode = child;
  }
  tnode.data = node.data;
}

class TrieEncNode {
  TrieEncNode(this.data, this.subKey) {}

  Map<int, TrieEncNode> childs; // int is char.code
  Uint8List data;
  String subKey;

  Uint8List toBytes() {
    final wr = binary.ByteWriter();

    final dataSize = binary.getIntSize(data?.length);

    if (childs == null || childs.isEmpty) {
      // no child

      // write flag, contains dataSize only
      binary.writeInt(wr,dataSize, 1);

      // write node data
      if (dataSize > 0) {
        binary.writeInt(wr,data.length, dataSize);
        wr.writeBytes(data);
      }

      //env.traceFunc(() => '$_subKey=${res.hexDump()}');
    } else {
      // childs exists

      // ** compute length flags
      final childsCount = childs.length;
      final childsCountSize = binary.getIntSize(childsCount);
      assert(childsCountSize <= 2); // max 64000

      // ******** RECURSION: convert all childs to <key, bytes> tuple
      final childsData = List.of(
          childs.entries.map((kv) => Tuple2(kv.key, kv.value.toBytes())),
          growable: false);
      childsData.sort((a, b) => a.item1 - b.item1); // sort childs
      // count childs data lens
      final childDataLen = Linq.sum(childsData.map((d) => d.item2.length));
      final childsDataSize = binary.getIntSize(childDataLen);
      final keySize = binary.getIntSize(
          Linq.max(childsData.map((kb) => kb.item1)));

      // childsCountSizeFlag==0 => 0 child, 1 => 1 child, 2 => 2..255 childs, 3 => 256..64000 childs
      final childsCountSizeFlag =
          childsCount <= 1 ? childsCount : childsCountSize + 1;
      // ** write length flags
      binary.writeInt(wr,
          (childsCountSizeFlag << 6) |
              (childsDataSize << 4) |
              (keySize << 2) |
              dataSize,
          1);

      // ** write node data
      if (dataSize > 0) {
        binary.writeInt(wr,data.length, dataSize);
        wr.writeBytes(data);
      }

      // ** write child num
      if (childsCountSizeFlag > 1)
        binary.writeInt(wr,childsCount, childsCountSize);

      // ** write keys
      for (var i = 0; i < childsCount; i++)
        binary.writeInt(wr,childsData[i].item1, keySize);

      // ** write offsets
      var childOffset = 0;
      for (var i = 0; i < childsCount; i++) {
        assert(i > 0 || childOffset == 0);
        if (i > 0) binary.writeInt(wr,childOffset, childsDataSize);
        childOffset += childsData[i].item2.length;
      }

      //env.traceFunc(() => '$_subKey=${res.hexDump()}');

      // write childs content
      for (var i = 0; i < childsCount; i++)
        wr.writeBytes(childsData[i].item2);
    }

    return wr.toBytes();
  }
}
