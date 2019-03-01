import 'dart:typed_data';
import 'package:rewise_low_utils/trie.dart' as trie;
import 'package:rewise_low_utils/env.dart' as env;

Node findNode(Uint8List data, String key) {
  final rdr = trie.ByteReader(data);
  rdr.setPos(0);
  var node = _readNode(rdr, ''); // root
  var keyIdx = 0;
  for (final ch in key.codeUnits) {
    final childRdr = _moveToChildNode(node, ch);
    if (childRdr == null) return null;
    final nodeKey = key.substring(0, ++keyIdx);
    env.traceFunc(() => '$nodeKey=${childRdr.hexDump()}');
    node = _readNode(childRdr, nodeKey);
  }
  return node;
}

void visitDescendantNodes(
    Uint8List data, String key, bool onVisitNode(Node node)) {
  var node = findNode(data, key);
  if (node == null) return;
  node.findDeep = 0;
  _visitDescendantNodes(node, onVisitNode);
}

bool _visitDescendantNodes(Node node, bool onVisitNode(Node node)) {
  if (!onVisitNode(node)) return false;
  if (node.childsCount == 0) return true;
  for (var idx = 0; idx < node.childsCount; idx++) {
    final key = node.childIdx.readNum(node.keySize);
    final offset = idx == 0 ? 0 : node.childOffsets.readNum(node.offsetSize);
    final subRdr = node.rest.createSubReaderFromPos(offset);
    final subNode = _readNode(subRdr, node.key + String.fromCharCode(key));
    subNode.findDeep = node.findDeep + 1;
    if (!_visitDescendantNodes(subNode, onVisitNode)) return false;
  }
  return true;
}

// !!! end with rdr._pos = rdr._len, is it ok?
Node _readNode(trie.ByteReader rdr, String key) {
  // length flags: dataLenSize, keySize, offsetSize, childsCountSize
  final flags = rdr.readNum(1);
  // Node
  final node = Node();
  node.key = key;
  node.keySize = (flags >> 2) & 0x3;
  node.offsetSize = (flags >> 4) & 0x3;
  final childsCountSize = (flags >> 6) & 0x3;

  // data
  final dataLenSize = flags & 0x3;
  final dataLen = rdr.readNum(dataLenSize);
  node.data = dataLen == 0 ? null : rdr.createSubReader(dataLen);

  // child count
  // childsCountSize=0 => 0, childsCountSize=1 => 1, readNum(1) or readNum(2)
  node.childsCount /*max 64000*/ =
      childsCountSize <= 1 ? childsCountSize : rdr.readNum(childsCountSize - 1);
  if (node.childsCount > 0) {
    // node.childIdx is binarySearch table
    node.childIdx = rdr.createSubReader(node.childsCount * node.keySize);
    // node.childOffsets is offset to node.rest (for not first node). First noda has zero offset.
    node.childOffsets =
        rdr.createSubReader((node.childsCount - 1) * node.offsetSize);
  }
  // rdr._pos is end of data, is it OK?
  node.rest = rdr.createSubReader();

  return node;
}

trie.ByteReader _moveToChildNode(Node node, int childKey) {
  if (node.childIdx == null) throw ArgumentError();
  final res = node.childIdx.BinarySearch(node.keySize, childKey);
  if (res.item1 < 0) return null;
  int offset;
  if (res.item1 == 0)
    offset = 0;
  else {
    node.childOffsets.setPos((res.item1 - 1) * node.offsetSize);
    offset = node.childOffsets.readNum(node.offsetSize);
  }
  return node.rest.createSubReaderFromPos(offset);
}

class Node {
  trie.ByteReader data;
  trie.ByteReader childIdx;
  trie.ByteReader childOffsets;
  int childsCount;
  int keySize;
  int offsetSize;
  trie.ByteReader rest;
  String key;
  int findDeep;
}
