using System;
using System.Collections.Generic;
using System.Linq;

public static class TrieEncoder {

  public interface IListNode { // : IComparer<ListNode> {
    string key { get; }
    byte[] data { get; }
  }

  public static byte[] toBytes(IEnumerable<IListNode> list) {
    TrieNode root = new TrieNode(null);
    foreach (var node in list) insertNode(root, node);
    return root.toBytes().toBytes();
  }

  class TrieNode {
    internal TrieNode(byte[] data) {
      this.data = data;
    }
    internal Dictionary<char, TrieNode> childs;
    internal byte[] data;

    internal BytesBuilder toBytes() {

      var res = new BytesBuilder();

      var dataSize = getNumberSizeMask(data == null ? 0 : data.Length);

      if (childs == null || childs.Count == 0) { // no child

        // write length flags
        writeNum(res, dataSize, 1);

        // write node data
        if (dataSize > 0) {
          writeNum(res, data.Length, dataSize);
          res.Add(data);
        }

      } else { // childs exists

        // ** compute child data size
        var childsCount = childs.Count;
        var childsCountSize = getNumberSizeMask(childsCount);

        var childsData = childs.Select(ch => new { key = ch.Key, bytes = ch.Value.toBytes() }).ToArray();
        var childsDataSize = getNumberSizeMask(childsData.Length);
        var keySize = getNumberSizeMask(childsData.Max(kb => kb.key));

        // write length flags
        writeNum(res, childsDataSize << 4 + keySize << 2 + dataSize, 1);

        // write node data
        if (dataSize > 0) {
          writeNum(res, data.Length, dataSize);
          res.Add(data);
        }

        writeNum(res, childsCount, childsCountSize); // write child num

        for (var i = 0; i < childsCount; i++) // write keys
          writeNum(res, childsData[i].key, keySize);

        var childOffset = 0;
        for (var i = 0; i < childsCount; i++) { // write childs offsets
          writeNum(res, childOffset, childsDataSize);
          childOffset += childsData[0].bytes.len;
        }

        for (var i = 0; i < childsCount; i++) // write child data
          res.Add(childsData[i].bytes);
      }

      return res;

    }

    byte getNumberSizeMask(int num) { // returns 0,1,2 or 3
      return (byte)(num == 0 ? 0 : (num <= 0xff ? 1 : (num <= 0xffff ? 2 : 3)));
    }

    void writeNum(BytesBuilder res, int num, byte size /*0,1,2,3*/) {
      if (num == 0) return;
      res.Add(
        size == 2 ? BitConverter.GetBytes((ushort)num) : (
        size == 1 ? BitConverter.GetBytes((byte)num) :
        BitConverter.GetBytes((uint)num)));
    }

  }

  static void insertNode(TrieNode tnode, IListNode node) {
    foreach (var ch in node.key) {
      TrieNode child = null;
      if (tnode.childs == null)
        tnode.childs = new Dictionary<char, TrieNode>();
      else
        tnode.childs.TryGetValue(ch, out child);

      if (child == null)
        tnode.childs[ch] = child = new TrieNode(null);

      tnode = child;
    }
    tnode.data = node.data;
  }
}

