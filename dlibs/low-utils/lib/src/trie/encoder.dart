import 'dart:typed_data';
import 'package:tuple/tuple.dart';
import 'writer.dart';
import 'dart:convert';

import 'package:rewise_low_utils/linq.dart' as linq;
import 'package:rewise_low_utils/env.dart' as env;
import 'package:json_annotation/json_annotation.dart';

part 'encoder.g.dart';

@JsonSerializable(nullable: true, explicitToJson: true, includeIfNull: false)
class InputNode {
  InputNode(this.key, this.data);
  InputNode.fromList(this.key, [List<int> list])
      : data = list == null ? null : Uint8List.fromList(list);
  factory InputNode.fromJson(String json) =>
      _$InputNodeFromJson(jsonDecode(json));

  final String key;

  @JsonKey(fromJson: base64Decode, toJson: base64Encode)
  final Uint8List data;

  String toJson() => jsonEncode(_$InputNodeToJson(this));
}

BytesWriter toBytes(Iterable<InputNode> list) {
  _TrieNode root = _TrieNode(null, '');
  for (final node in list) _insertNode(root, node);
  return root.toBytes();
}

void _insertNode(_TrieNode tnode, InputNode node) {
  var keyIdx = 0;
  for (final ch in node.key.codeUnits) {
    if (tnode.childs == null) tnode.childs = Map<int, _TrieNode>();
    keyIdx++;
    final child = tnode.childs
        .putIfAbsent(ch, () => _TrieNode(null, node.key.substring(0, keyIdx)));
    tnode = child;
  }
  tnode.data = node.data;
}

class _TrieNode {
  _TrieNode(this.data, this.key) {}

  Map<int, _TrieNode> childs; // int is char code (part of key)
  Uint8List data;
  String key;

  BytesWriter toBytes() {
    final res = BytesWriter();

    final dataSize = BytesWriter.getNumberSizeMask(data?.length);

    if (childs == null || childs.length == 0) {
      // no child

      // write flag, contains dataSize only
      res.writeNumber(dataSize, 1);

      // write node data
      if (dataSize > 0) {
        res.writeNumber(data.length, dataSize);
        res.writeBytes(data);
      }

      env.traceFunc(() => '$key=${res.hexDump()}');
    } else {
      // childs exists

      // ** compute length flags
      final childsCount = childs.length;
      final childsCountSize = BytesWriter.getNumberSizeMask(childsCount);
      assert(childsCountSize <= 2); // max 64000

      // ** RECURSION: convert all childs to <key, bytes> tuple
      final childsData = List.of(
          childs.entries.map((kv) => Tuple2(kv.key, kv.value.toBytes())),
          growable: false);
      childsData.sort((a, b) => a.item1 - b.item1); // sort childs
      // count childs data lens
      final childDataLen = linq.sum(childsData.map((d) => d.item2.len));
      final childsDataSize = BytesWriter.getNumberSizeMask(childDataLen);
      final keySize = BytesWriter.getNumberSizeMask(
          linq.max(childsData.map((kb) => kb.item1)));

      // childsCountSizeFlag==0 => 0 child, 1 => 1 child, 2 => 2..255 childs, 3 => 256..64000 childs
      final childsCountSizeFlag =
          childsCount <= 1 ? childsCount : childsCountSize + 1;
      // ** write length flags
      res.writeNumber(
          (childsCountSizeFlag << 6) |
              (childsDataSize << 4) |
              (keySize << 2) |
              dataSize,
          1);

      // ** write node data
      if (dataSize > 0) {
        res.writeNumber(data.length, dataSize);
        res.writeBytes(data);
      }

      // ** write child num
      if (childsCountSizeFlag > 1) res.writeNumber(childsCount, childsCountSize);

      // ** write keys
      for (var i = 0; i < childsCount; i++)
        res.writeNumber(childsData[i].item1, keySize);

      // ** write offsets
      var childOffset = 0;
      for (var i = 0; i < childsCount; i++) {
        assert(i > 0 || childOffset == 0);
        if (i > 0) res.writeNumber(childOffset, childsDataSize);
        childOffset += childsData[i].item2.len;
      }

      env.traceFunc(() => '$key=${res.hexDump()}');

      // write childs content
      for (var i = 0; i < childsCount; i++) res.writeWriter(childsData[i].item2);
    }

    return res;
  }
}
