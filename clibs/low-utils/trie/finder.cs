using System;

//ref returns: https://blogs.msdn.microsoft.com/mazhou/2017/12/12/c-7-series-part-7-ref-returns/
//https://blogs.msdn.microsoft.com/mazhou/2018/03/25/c-7-series-part-10-spant-and-universal-memory-management/
//https://msdn.microsoft.com/en-us/magazine/mt814808.aspx

public class TrieReader : reader.BytesReader {

  public TrieReader(byte[] data) : base(data) { }

  Node readNode() {
    // length flags
    var flags = (byte)readNum(1);

    // Node
    var node = new Node();
    node.keySize = (byte)((flags >> 2) & 0x3);
    node.offsetSize = (byte)((flags >> 4) & 0x3);
    var childsCountSize = (byte)((flags >> 6) & 0x3);

    // data
    var dataSize = (byte)(flags & 0x3);
    node.data = innerReader(dataSize);

    // child count
    node.childsCount = childsCountSize > 0 ? readNum(childsCountSize) : 0;
    if (node.childsCount > 0) {
      node.childIdx = innerReader(node.childsCount * node.keySize);
      node.childOffsets = innerReader(node.childsCount * node.offsetSize);
    }
    node.rest = innerReader();

    return node;
  }

  reader.BytesReader moveToNode(Node node, char ch) {
    if (node.childIdx == null) throw new ArgumentException();
    var key = ch;
    var res = node.childIdx.BinarySearch(node.keySize, key);
    if (res.Item1 < 0) return null;
    node.childOffsets.setPos(res.Item1 * node.offsetSize);
    var offset = node.childOffsets.readNum(node.offsetSize);
    return new reader.BytesReader(this, offset);
  }

  class Node {
    public reader.BytesReader data;
    public reader.BytesReader childIdx;
    public reader.BytesReader childOffsets;
    public int childsCount;
    public byte keySize;
    public byte offsetSize;
    public reader.BytesReader rest;
  }

}

