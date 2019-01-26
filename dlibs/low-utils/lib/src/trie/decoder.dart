import 'dart:typed_data';
import 'package:rewise_low_utils/trie.dart' as trie;
import 'package:rewise_low_utils/env.dart' as env;

Node findNode(Uint8List data, String key) {
  final rdr = trie.BytesReader(data);
  rdr.setPos(0);
  var node = _readNode(rdr, '');
  var keyIdx = 0;
  for (final ch in key.codeUnits) {
    final subRdr = _moveToNode(node, ch);
    if (subRdr == null) return null;
    final nodeKey = key.substring(0, ++keyIdx);
    env.traceFunc(() => '$nodeKey=${subRdr.hexDump()}');
    node = _readNode(subRdr, nodeKey);
  }
  return node;
}

void findDescendantNodes(Uint8List data, String key, bool onNode(Node node)) {
  var node = findNode(data, key);
  if (node == null) return;
  node.findDeep = 0;
  _getDescendantNodes(node, onNode);
}

bool _getDescendantNodes(Node node, bool onNode(Node node)) {
  if (!onNode(node)) return false;
  if (node.childsCount == 0) return true;
  for (var idx = 0; idx < node.childsCount; idx++) {
    final key = node.childIdx.readNum(node.keySize);
    final offset = idx == 0 ? 0 : node.childOffsets.readNum(node.offsetSize);
    final subRdr = node.rest.readReaderFromPos(offset);
    final subNode = _readNode(subRdr, node.key + String.fromCharCode(key));
    subNode.findDeep = node.findDeep + 1;
    if (!_getDescendantNodes(subNode, onNode)) return false;
  }
  return true;
}

Node _readNode(trie.BytesReader rdr, String key) {
  // length flags
  final flags = rdr.readNum(1);
  // Node
  final node = Node();
  node.key = key;
  node.keySize = (flags >> 2) & 0x3;
  node.offsetSize = (flags >> 4) & 0x3;
  final childsCountSizeFlag = (flags >> 6) & 0x3;
  final childsCountSize = childsCountSizeFlag > 1 ? childsCountSizeFlag - 1 : 0;

  // data
  final dataLenSize = flags & 0x3;
  final dataLen = rdr.readNum(dataLenSize);
  node.data = dataLen == 0 ? null : rdr.readReader(dataLen);

  // child count
  node.childsCount = childsCountSizeFlag == 0
      ? 0
      : (childsCountSizeFlag == 1 ? 1 : rdr.readNum(childsCountSize));
  if (node.childsCount > 0) {
    node.childIdx = rdr.readReader(node.childsCount * node.keySize);
    node.childOffsets =
        rdr.readReader((node.childsCount - 1) * node.offsetSize);
  }
  node.rest = rdr.readReader();

  return node;
}

trie.BytesReader _moveToNode(Node node, int ch) {
  if (node.childIdx == null) throw ArgumentError();
  final res = node.childIdx.BinarySearch(node.keySize, ch);
  if (res.item1 < 0) return null;
  int offset;
  if (res.item1 == 0)
    offset = 0;
  else {
    node.childOffsets.setPos((res.item1 - 1) * node.offsetSize);
    offset = node.childOffsets.readNum(node.offsetSize);
  }
  return node.rest.readReaderFromPos(offset);
}

class Node {
  trie.BytesReader data;
  trie.BytesReader childIdx;
  trie.BytesReader childOffsets;
  int childsCount;
  int keySize;
  int offsetSize;
  trie.BytesReader rest;
  String key;
  int findDeep;
}
