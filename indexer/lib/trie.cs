using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using System.Collections;

namespace indexer {

  public static class TrieEncode {
    public static byte[] toBytes(IEnumerable<ListNode> list) {
      TrieNode root = new TrieNode(null);
      foreach (var node in list) insertNode(root, node);
      return root.toBytes().toBytes();
    }
    static void insertNode(TrieNode tnode, ListNode node) {
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

  public class ListNode { // : IComparer<ListNode> {
    public string key;
    public byte[] data;

    //int IComparer<ListNode>.Compare(ListNode x, ListNode y) {
    //  return string.Compare(x.key, y.key);
    //}
  }

  class TrieNode {
    internal TrieNode(byte[] data) {
      this.data = data;
    }
    internal Dictionary<char,TrieNode> childs;
    internal byte[] data;

    internal BytesBuilder toBytes() {

      var res = new BytesBuilder();

      var dataSize = getNumberSize(data == null ? 0 : data.Length);

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
        var childsCountSize = getNumberSize(childsCount);

        var childsData = childs.Select(ch => new { key = ch.Key, bytes = ch.Value.toBytes() }).ToArray();
        var childsDataSize = getNumberSize(childsData.Length);
        var keySize = getNumberSize(childsData.Max(kb => kb.key));

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

    int getNumberSize(int num) { // returns 0,1,2 or 3
      return num == 0 ? 0 : num <= 0xff ? 1 : (num <= 0xffff ? 2 : 3);
    }

    void writeNum(BytesBuilder res, int num, int size /*0,1,2,3*/) {
      if (num == 0) return;
      res.Add(
        size == 2 ? BitConverter.GetBytes((ushort)num) : (
        size == 1 ? BitConverter.GetBytes((byte)num) :
        BitConverter.GetBytes((uint)num)));
    }

  }

  class BytesBuilder {
    internal int len;
    List<byte[]> bytes = new List<byte[]>();
    internal void Add(byte[] data) {
      if (data == null) return;
      len += data.Length;
      bytes.Add(data);
    }
    internal void Add(BytesBuilder data) {
      if (data == null) return;
      len += data.len;
      bytes.AddRange(data.bytes);
    }
    internal byte[] toBytes() {
      var res = new byte[len];
      int pos = 0;
      foreach (var bs in bytes) {
        Buffer.BlockCopy(bs, 0, res, pos, bs.Length);
        pos += bs.Length;
      }
      if (pos != len) throw new Exception("pos != len");
      return res;
    }
  }

  //internal delegate int Compare<T>(T a, T b);

  //internal static class BinarySearch {

  //  internal static int Search<T>(T[] chars, int min, int len, T key, Compare<T> compare) {
  //    int max = min + len;
  //    while (min < max) {
  //      int mid = min + ((max - min) >> 1);
  //      var element = chars[mid];
  //      var comp = compare(element, key);
  //      if (comp == 0) return mid;
  //      if (comp < 0 /*element < key*/) min = mid + 1; else max = mid;
  //    }
  //    return -min - 1;
  //  }

  //  internal static int Search<T>(T[] chars, T key, Compare<T> comparer) {
  //    return Search(chars, 0, chars.Length, key, comparer);
  //  }

  //}
}
