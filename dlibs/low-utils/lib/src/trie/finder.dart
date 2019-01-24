import 'dart:typed_data';
import './reader.dart';
import '../env.dart' as env;

class TrieReader extends BytesReader {
  TrieReader(Uint8List data) : super(data) {}
}

BytesReader findData(Uint8List data, String key) {
  final rdr = BytesReader(data);
  final node = findNode(rdr, key);
  return node?.data;
}

Node findNode(BytesReader rdr, String key) {
  rdr.setPos(0);
  var node = readNode(rdr);
  var keyIdx = 0;
  for (final ch in key.codeUnits) {
    final subRdr = moveToNode(node, ch);
    if (subRdr == null) return null;
    keyIdx++;
    env.trace('${key.substring(0, keyIdx)}=${subRdr.hexDump()}');
    node = readNode(subRdr);
  }
  return node;
}

Node readNode(BytesReader rdr) {
  // length flags
  final flags = rdr.readNum(1);
  // Node
  final node = Node();
  node.keySize = (flags >> 2) & 0x3;
  node.offsetSize = (flags >> 4) & 0x3;
  final childsCountSize = (flags >> 6) & 0x3;

  // data
  final dataLenSize = flags & 0x3;
  final dataLen = rdr.readNum(dataLenSize);
  node.data = dataLen==0 ? null : rdr.innerReader(len: dataLen);

  // child count
  node.childsCount = childsCountSize > 0 ? rdr.readNum(childsCountSize) : 0;
  if (node.childsCount > 0) {
    node.childIdx = rdr.innerReader(len: node.childsCount * node.keySize);
    node.childOffsets = rdr.innerReader(len: node.childsCount * node.offsetSize);
  }
  node.rest = rdr.innerReader();

  return node;
}

BytesReader moveToNode(Node node, int ch) {
  if (node.childIdx == null) throw ArgumentError();
  final key = ch;
  final res = node.childIdx.BinarySearch(node.keySize, key);
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
}
