import 'dart:typed_data';

import 'package:rewise_low_utils/toBinary.dart' as binary;

abstract class Encoder<T extends Comparable> implements binary.KeyHandler<T> {
  Encoder();

  Encoder.fromInput(binary.BuildInput<T> input) {
    result = binary.build(input, this);
  }

  Encoder.fromData(Iterable<T> data) {
    final input = binary.BuildInput<T>();
    for (var d in data) {
      input.counts.update(d, (v) => v++, ifAbsent: () => 1);
      input.countAll++;
    }
    result = binary.build(input, this);
  }

  binary.BuildResult<T> result;

  void keyToBits(T key, binary.BitWriter wr);
  String keyToDump(T key) => key.toString();

  void encode(binary.BitWriter wr, List<T> data) {
    wr.writeDatas(data.map((d) => result.encodingMap[d]));
  }
}

abstract class Decoder<T extends Comparable> {
  Decoder(Uint8List decodingTree) {
    final rdr = binary.BitReader(decodingTree);
    tree = HuffNode<T>.decode(rdr, bitsToKey);
  }

  HuffNode<T> tree;

  T bitsToKey(binary.BitReader rdr);

  Iterable<T> decode(binary.BitReader rdr, int count) sync* {
    if (count==0) return;
    var node = tree;
    final iter = rdr.readBitStream().iterator;
    while (true) {
      if (!iter.moveNext()) return;
      node = iter.current ? node.rightSon : node.leftSon;
      if (node.isLeaf) {
        yield node.value;
        count--;
        if (count==0) return;
        node = tree;
      }
        //leftSon.isZero == b; NodeDesign constructor
    }
  }
}

typedef KeyToBits<T extends Comparable> = void Function(
    T key, binary.BitWriter wr);
typedef BitsToKey<T extends Comparable> = T Function(binary.BitReader rdr);

class HuffNode<T extends Comparable> {
  HuffNode<T> leftSon;
  HuffNode<T> rightSon;
  T value;
  bool get isLeaf => rightSon == null;

  HuffNode.leaf(this.value);
  HuffNode(this.leftSon, this.rightSon);
  HuffNode.decode(binary.BitReader rdr, BitsToKey bitsToKey) {
    //rdr.doAsert(174);
    if (rdr.readBit())
      value = bitsToKey(rdr);
    else {
      leftSon = HuffNode.decode(rdr, bitsToKey);
      rightSon = HuffNode.decode(rdr, bitsToKey);
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
