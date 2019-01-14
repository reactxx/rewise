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

    internal BytesWriter toBytes() {

      var res = new BytesWriter();

      var dataSize = BytesWriter.getNumberSizeMask(data == null ? 0 : data.Length);

      if (childs == null || childs.Count == 0) { // no child

        // write length flags
        res.Add(dataSize, 1);

        // write node data
        if (dataSize > 0) {
          res.Add(data.Length, dataSize);
          res.Add(data);
        }

      } else { // childs exists

        // ** compute child data size
        var childsCount = childs.Count;
        var childsCountSize = BytesWriter.getNumberSizeMask(childsCount);

        var childsData = childs.Select(kv => new { ch = (ushort)kv.Key, bytes = kv.Value.toBytes() }).ToArray();
        var childsDataSize = BytesWriter.getNumberSizeMask(childsData.Length);
        var keySize = BytesWriter.getNumberSizeMask(childsData.Max(kb => kb.ch));

        // write length flags
        res.Add((childsCountSize << 6) | (childsDataSize << 4) | (keySize << 2) | dataSize, 1);

        // write node data
        if (dataSize > 0) {
          res.Add(data.Length, dataSize);
          res.Add(data);
        }

        res.Add(childsCount, childsCountSize); // write child num

        for (var i = 0; i < childsCount; i++) // write keys
          res.Add(childsData[i].ch, keySize);

        var childOffset = 0;
        for (var i = 0; i < childsCount; i++) { // write childs offsets
          res.Add(childOffset, childsDataSize);
          childOffset += childsData[0].bytes.len;
        }

        for (var i = 0; i < childsCount; i++) // write child data
          res.Add(childsData[i].bytes);
      }

      return res;

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


