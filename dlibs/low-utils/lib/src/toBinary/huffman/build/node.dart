import 'package:rewise_low_utils/toBinary.dart' as binary;

class NodeDesign<T extends Comparable> extends binary.HuffNode<T>
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
      bools.add(!nodeCur.isZero);
      nodeCur = nodeCur._parent;
    }
    final revBools = bools.reversed;
    final wr = binary.BitWriter.fromBools(revBools);
    String dump;
    assert(() {
      dump = binary.dumpIterableBoolBits(revBools);
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
