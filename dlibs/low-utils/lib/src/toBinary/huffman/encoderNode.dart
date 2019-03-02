import 'dart:typed_data';
import 'package:rewise_low_utils/toBinary.dart' as binary;

class NodeEnc<T> {
  NodeEnc(this.bits, this.bitsCount, this.dump);
  Uint8List bits;
  int bitsCount;
  String dump;
}

class NodeDesign<T extends Comparable> implements Comparable<NodeDesign<T>> {
  NodeDesign.leaf(double probability, T value) {
    Probability = probability;
    LeftSon = RightSon = Parent = null;
    Value = value;
    IsLeaf = true;
  }

  NodeDesign(NodeDesign<T> leftSon, NodeDesign<T> rightSon) {
    LeftSon = leftSon;
    RightSon = rightSon;
    Probability = leftSon.Probability + rightSon.Probability;
    leftSon.IsZero = true;
    rightSon.IsZero = false;
    leftSon.Parent = rightSon.Parent = this;
    IsLeaf = false;
  }

  Iterable<bool> _getBools() sync* {
    assert(IsLeaf);
    var nodeCur = this;
    while (!nodeCur.IsRoot) {
      yield nodeCur.IsZero;
      nodeCur = nodeCur.Parent;
    }
  }

  NodeEnc<T> adjustBits() {
    final bools = _getBools().toList().reversed;
    final wr = binary.BitWriter.fromBools(bools);
    String dump;
    getDump () {
      dump = binary.BitReader.dump(bools);
      return true;
    }
    assert(getDump());
    return NodeEnc<T>(wr.toBytes(), wr.len, dump);
  }

  NodeDesign<T> LeftSon;
  NodeDesign<T> RightSon;
  NodeDesign<T> Parent;
  T Value;
  bool IsLeaf;
  double Probability;
  bool IsZero;
  Uint8List bits;
  int bitsCount;
  // // encoded bits
  // Bits.SmallArray encoded() {
  //   if (_encoded.count == 0) {
  //     var nodeCur = this;
  //     while (!nodeCur.IsRoot) {
  //       if (_encoded.count > 0)
  //         _encoded.value >>= 1; // not first bit => shift other bits value to the right
  //       if (!nodeCur.IsZero)
  //         _encoded.value = _encoded.value | 0x80000000; // put the non zero bit to start
  //       //if (_encoded.count > 0)
  //       //  _encoded.value <<= 1; // not first bit => shift other bits value to the right
  //       //if (!nodeCur.IsZero)
  //       //  _encoded.value = _encoded.value | 0x1; // put the non zero bit to start

  //       _encoded.count++;
  //       if (_encoded.count > 32)
  //         throw new Exception(); // more than 32 bits => error
  //       nodeCur = nodeCur.Parent;
  //     }
  //   }
  //   return _encoded;
  // }
  // Bits.SmallArray _encoded;

  bool get IsRoot => Parent == null;

  int compareTo(NodeDesign<T> obj) => -Probability.compareTo(obj.Probability);
}
