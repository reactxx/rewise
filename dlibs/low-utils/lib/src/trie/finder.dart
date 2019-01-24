import 'dart:typed_data';
import './reader.dart';
import '../env.dart' as env;

Node findNode(Uint8List data, String key) {
  final rdr = BytesReader(data);
  rdr.setPos(0);
  var node = _readNode(rdr, '');
  var keyIdx = 0;
  for (final ch in key.codeUnits) {
    final subRdr = _moveToNode(node, ch);
    if (subRdr == null) return null;
    keyIdx++;
    final nodeKey = key.substring(0, ++keyIdx);
    env.traceFunc(() => '$nodeKey=${subRdr.hexDump()}');
    node = _readNode(subRdr, nodeKey);
  }
  return node;
}

class findPar {
  int deep;
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
    final key = node.childIdx.setPos(idx * node.keySize).readNum(node.keySize);
    final offset = node.childOffsets
        .setPos(idx * node.offsetSize)
        .readNum(node.offsetSize);
    final subRdr = node.rest.innerReader(pos: offset);
    final subNode = _readNode(subRdr, node.key + String.fromCharCode(key));
    subNode.findDeep = node.findDeep + 1;
    if (!_getDescendantNodes(subNode, onNode)) return false;
  }
  return true;
}

Node _readNode(BytesReader rdr, String key) {
  // length flags
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
  node.data = dataLen == 0 ? null : rdr.innerReader(len: dataLen);

  // child count
  node.childsCount = childsCountSize > 0 ? rdr.readNum(childsCountSize) : 0;
  if (node.childsCount > 0) {
    node.childIdx = rdr.innerReader(len: node.childsCount * node.keySize);
    node.childOffsets =
        rdr.innerReader(len: node.childsCount * node.offsetSize);
  }
  node.rest = rdr.innerReader();

  return node;
}

BytesReader _moveToNode(Node node, int ch) {
  if (node.childIdx == null) throw ArgumentError();
  final res = node.childIdx.BinarySearch(node.keySize, ch);
  if (res.item1 < 0) return null;
  node.childOffsets.setPos(res.item1 * node.offsetSize);
  final offset = node.childOffsets.readNum(node.offsetSize);
  return node.rest.innerReader(pos: offset);
}

class Node {
  BytesReader data;
  BytesReader childIdx;
  BytesReader childOffsets;
  int childsCount;
  int keySize;
  int offsetSize;
  BytesReader rest;
  String key;
  int findDeep;
}
