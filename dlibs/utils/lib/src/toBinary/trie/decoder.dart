import 'dart:typed_data';
import 'package:rw_utils/toBinary.dart' as binary;

TrieNode trieFindNode(Uint8List data, String key) {
  final rdr = binary.MemoryReader(data);
  rdr.setPos(0);
  var node = _readNode(rdr, ''); // root
  var keyIdx = 0;
  for (final ch in key.codeUnits) {
    final childRdr = _moveToChildNode(node, ch);
    if (childRdr == null) return null;
    final nodeKey = key.substring(0, ++keyIdx);
    node = _readNode(childRdr, nodeKey);
  }
  return node;
}

void trieVisitDescendantNodes(
    Uint8List data, String key, bool onVisitNode(TrieNode node)) {
  var node = trieFindNode(data, key);
  if (node == null) return;
  node.findDeep = 0;
  _visitDescendantNodes(node, onVisitNode);
}

bool _visitDescendantNodes(TrieNode node, bool onVisitNode(TrieNode node)) {
  if (!onVisitNode(node)) return false;
  if (node.childsCount == 0) return true;
  for (var idx = 0; idx < node.childsCount; idx++) {
    final key = binary.readInt(node.childIdx, node.keySize);
    final offset = idx == 0 ? 0 : binary.readInt(node.childOffsets, node.offsetSize);
    final subRdr = node.rest.createSubReaderFromPos(offset);
    final subNode = _readNode(subRdr, node.key + String.fromCharCode(key));
    subNode.findDeep = node.findDeep + 1;
    if (!_visitDescendantNodes(subNode, onVisitNode)) return false;
  }
  return true;
}

// !!! end with rdr._pos = rdr._len, is it ok?
TrieNode _readNode(binary.MemoryReader rdr, String key) {
  // length flags: dataLenSize, keySize, offsetSize, childsCountSize
  final flags = binary.readInt(rdr, 1);
  // Node
  final node = TrieNode();
  node.key = key;
  node.keySize = (flags >> 2) & 0x3;
  node.offsetSize = (flags >> 4) & 0x3;
  final childsCountSize = (flags >> 6) & 0x3;

  // data
  final dataLenSize = flags & 0x3;
  final dataLen = binary.readInt(rdr, dataLenSize);
  node.data = dataLen == 0 ? null : rdr.createSubReader(dataLen);

  // child count
  // childsCountSize=0 => 0, childsCountSize=1 => 1, readNum(1) or readNum(2)
  node.childsCount /*max 64000*/ =
      childsCountSize <= 1 ? childsCountSize : binary.readInt(rdr, childsCountSize - 1);
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

binary.MemoryReader _moveToChildNode(TrieNode node, int childKey) {
  if (node.childIdx == null) throw ArgumentError();
  final res = node.childIdx.BinarySearch(node.keySize, childKey);
  if (res.item1 < 0) return null;
  int offset;
  if (res.item1 == 0)
    offset = 0;
  else {
    node.childOffsets.setPos((res.item1 - 1) * node.offsetSize);
    offset = binary.readInt(node.childOffsets, node.offsetSize);
  }
  return node.rest.createSubReaderFromPos(offset);
}

class TrieNode {
  String key;
  int childsCount;
  int keySize;
  int offsetSize;
  int findDeep;
  binary.MemoryReader data;
  binary.MemoryReader childIdx;
  binary.MemoryReader childOffsets;
  binary.MemoryReader rest;
}
