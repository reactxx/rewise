import 'package:rewise_low_utils/toBinary.dart' as binary;

typedef KeyToBits<T extends Comparable> = void Function(
    T key, binary.BitWriter wr);
typedef BitsToKey<T extends Comparable> = T Function(binary.BitReader rdr);

class Node<T extends Comparable> {
  Node<T> leftSon;
  Node<T> rightSon;
  T value;
  bool get isLeaf => rightSon == null;

  Node.leaf(this.value);
  Node(this.leftSon, this.rightSon);
  Node.decode(binary.BitReader rdr, BitsToKey<T> bitsToKey) {
    if (rdr.readBit() == 0)
      value = bitsToKey(rdr);
    else {
      leftSon = Node.decode(rdr, bitsToKey);
      rightSon = Node.decode(rdr, bitsToKey);
    }
  }

  void binaryEncode(KeyToBits keyToBits, binary.BitWriter wr) {
    wr.writeBool(isLeaf);
    if (isLeaf) {
      keyToBits(value, wr);
    } else {
      if (leftSon != null) leftSon.binaryEncode(keyToBits, wr);
      if (rightSon != null) rightSon.binaryEncode(keyToBits, wr);
    }
  }
}

class NodeDesign<T extends Comparable> extends Node<T>
    implements Comparable<NodeDesign<T>> {
  NodeDesign.leaf(this._probability, T value) : super.leaf(value);

  NodeDesign(NodeDesign<T> leftSon, NodeDesign<T> rightSon)
      : super(leftSon, rightSon) {
    _probability = leftSon._probability + rightSon._probability;
    leftSon.isZero = true;
    rightSon.isZero = false;
    leftSon._parent = rightSon._parent = this;
  }

  binary.NodeEnc toBits() {
    assert(isLeaf);
    final bools = new List<bool>();
    var nodeCur = this;
    while (!nodeCur._isRoot) {
      bools.add(nodeCur.isZero);
      nodeCur = nodeCur._parent;
    }
    final revBools = bools.reversed;
    final wr = binary.BitWriter.fromBools(revBools);
    String dump;
    assert(() {
      dump = binary.BitReader.dump(revBools);
      return true;
    }()); // called only in Debug mode
    return binary.NodeEnc(wr.toBytes(), wr.len, dump);
  }

  //https://stackoverflow.com/questions/759707/efficient-way-of-storing-huffman-tree

  NodeDesign<T> _parent;
  double _probability;
  bool isZero;
  bool get _isRoot => _parent == null;

  int compareTo(NodeDesign<T> obj) => -_probability.compareTo(obj._probability);
}
