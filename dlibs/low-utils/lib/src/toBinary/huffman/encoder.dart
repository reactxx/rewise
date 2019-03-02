import 'encoderNode.dart';

class Encoder<T extends Comparable> {
  Encoder(this.encodingMap);
  Map<T, NodeEnc> encodingMap;

  // Iterable<bool> Encode(T value) {
  //   var node = _leafDictionary[value];
  //   for (var i = 0; i < node.encoded().count; i++)
  //     yield Bits.getBit(node._encoded.value, i);
  // }

  // byte[] Encode(T[] values) {
  //   var wr = new BitWriter();
  //   foreach (var v in values.Select(vv => _leafDictionary[vv].encoded()))
  //     wr.WriteBits(v.value, v.count);
  //   wr.Align();
  //   return wr.data.ToArray();
  //   //return Bits.serializeArrays(values.Select(v => _leafDictionary[v].encoded()).ToArray());
  // }

  // Iterable<T> Decode(Iterable<bool> bitString) {
  //   HuffmanNode<T> nodeCur = _root;
  //   for (var zero in bitString) {
  //     nodeCur = zero ? nodeCur.LeftSon : nodeCur.RightSon;
  //     if (nodeCur.IsLeaf) {
  //       yield nodeCur.Value;
  //       nodeCur = _root;
  //     }
  //   }
  //   if (nodeCur != _root)
  //     throw new Exception("Invalid bitstring in Decode");
  // }
}
