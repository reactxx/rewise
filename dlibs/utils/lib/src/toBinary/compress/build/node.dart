import 'package:rewise_low_utils/toBinary.dart' as binary;

class TreeNodeDesign<T extends Comparable> extends binary.TreeNode<T>
    implements Comparable<TreeNodeDesign<T>> {
  TreeNodeDesign.leaf(this._probability, T value) : super.leaf(value);

  TreeNodeDesign(TreeNodeDesign<T> leftSon, TreeNodeDesign<T> rightSon)
      : super(leftSon, rightSon) {
    _probability = leftSon._probability + rightSon._probability;
    leftSon.isZero = true;
    rightSon.isZero = false;
    leftSon._parent = rightSon._parent = this;
  }

  binary.EncodingMapItem toBits() {
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
    return binary.EncodingMapItem(wr.toBytes(), wr.len, dump);
  }

  TreeNodeDesign<T> _parent;
  double _probability;
  bool isZero;
  bool get _isRoot => _parent == null;

  int compareTo(TreeNodeDesign<T> obj) => -_probability.compareTo(obj._probability);
}
