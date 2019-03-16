import 'dart:typed_data';

import 'package:rw_utils/toBinary.dart' as binary;
import 'package:rw_utils/utils.dart' show Linq;

abstract class Encoder<T extends Comparable> implements binary.KeyHandler<T> {
  Encoder();

  Encoder.fromInput(binary.BuildInput<T> input) {
    buildResult = binary.build(input, this);
  }

  Encoder.fromData(Iterable<Iterable<T>> data) {
    final input = binary.BuildInput<T>();
    for (var ds in data) {
      input.counts.update(eof, (v) => v++, ifAbsent: () => 1);
      input.countAll++;
      for (var d in ds) {
        assert(validKey(d));
        input.counts.update(d, (v) => v++, ifAbsent: () => 1);
        input.countAll++;
      }
    }
    buildResult = binary.build(input, this);
  }

  binary.BuildResult<T> buildResult;

  void keyToBits(T key, binary.BitWriter wr);
  String keyToDump(T key) => key.toString();
  bool validKey(T key) => true;
  T eof;

  void encode(binary.BitWriter wr, Iterable<T> data) {
    wr.writeDatas(data.map((d) => buildResult.encodingMap[d]));
  }

  Uint8List encodeData(Iterable<T> data) {
    final wr = binary.BitWriter();
    encode(wr, Linq.concat(data, [eof]));
    return wr.toBytes();
  }

}

abstract class Decoder<T extends Comparable> {
  Decoder(Uint8List decodingTree) {
    final rdr = binary.BitReader(decodingTree);
    tree = TreeNode<T>.decode(rdr, bitsToKey);
  }

  TreeNode<T> tree;

  T bitsToKey(binary.BitReader rdr);
  T eof;

  Iterable<T> decode(binary.BitReader rdr) sync* {
    var node = tree;
    final iter = rdr.readBitStream().iterator;
    while (true) {
      if (!iter.moveNext()) return;
      node = iter.current ? node.rightSon : node.leftSon;
      if (node.isLeaf) {
        if (node.value==eof) return;
        yield node.value;
        node = tree;
      }
    }
  }

  Iterable<T> decodeData(Uint8List encoded) {
    final rdr = binary.BitReader(encoded);
    return decode(rdr);
  }

}

typedef KeyToBits<T extends Comparable> = void Function(
    T key, binary.BitWriter wr);
typedef BitsToKey<T extends Comparable> = T Function(binary.BitReader rdr);

//https://stackoverflow.com/questions/759707/efficient-way-of-storing-huffman-tree
class TreeNode<T extends Comparable> {
  TreeNode<T> leftSon;
  TreeNode<T> rightSon;
  T value;
  bool get isLeaf => rightSon == null;

  TreeNode.leaf(this.value);
  TreeNode(this.leftSon, this.rightSon);
  TreeNode.decode(binary.BitReader rdr, BitsToKey bitsToKey) {
    //rdr.doAsert(174);
    if (rdr.readBit())
      value = bitsToKey(rdr);
    else {
      leftSon = TreeNode.decode(rdr, bitsToKey);
      rightSon = TreeNode.decode(rdr, bitsToKey);
    }
  }

  void encode(binary.BitWriter wr, KeyToBits keyToBits) {
    //wr.doAsert(174);
    wr.writeBool(isLeaf);
    if (isLeaf) {
      keyToBits(value, wr);
    } else {
      if (leftSon != null) leftSon.encode(wr, keyToBits);
      if (rightSon != null) rightSon.encode(wr, keyToBits);
    }
  }
}
