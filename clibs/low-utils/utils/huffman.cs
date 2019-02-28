using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

//http://rosettacode.org/mw/index.php?title=Huffman_coding&action=edit&section=6
namespace Huffman_Encoding {

  public class PriorityQueue<T> where T : IComparable {
    protected List<T> LstHeap = new List<T>();

    public virtual int Count {
      get { return LstHeap.Count; }
    }

    public virtual void Add(T val) {
      LstHeap.Add(val);
      SetAt(LstHeap.Count - 1, val);
      UpHeap(LstHeap.Count - 1);
    }

    public virtual T Peek() {
      if (LstHeap.Count == 0) {
        throw new IndexOutOfRangeException("Peeking at an empty priority queue");
      }

      return LstHeap[0];
    }

    public virtual T Pop() {
      if (LstHeap.Count == 0) {
        throw new IndexOutOfRangeException("Popping an empty priority queue");
      }

      T valRet = LstHeap[0];

      SetAt(0, LstHeap[LstHeap.Count - 1]);
      LstHeap.RemoveAt(LstHeap.Count - 1);
      DownHeap(0);
      return valRet;
    }

    protected virtual void SetAt(int i, T val) {
      LstHeap[i] = val;
    }

    protected bool RightSonExists(int i) {
      return RightChildIndex(i) < LstHeap.Count;
    }

    protected bool LeftSonExists(int i) {
      return LeftChildIndex(i) < LstHeap.Count;
    }

    protected int ParentIndex(int i) {
      return (i - 1) / 2;
    }

    protected int LeftChildIndex(int i) {
      return 2 * i + 1;
    }

    protected int RightChildIndex(int i) {
      return 2 * (i + 1);
    }

    protected T ArrayVal(int i) {
      return LstHeap[i];
    }

    protected T Parent(int i) {
      return LstHeap[ParentIndex(i)];
    }

    protected T Left(int i) {
      return LstHeap[LeftChildIndex(i)];
    }

    protected T Right(int i) {
      return LstHeap[RightChildIndex(i)];
    }

    protected void Swap(int i, int j) {
      T valHold = ArrayVal(i);
      SetAt(i, LstHeap[j]);
      SetAt(j, valHold);
    }

    protected void UpHeap(int i) {
      while (i > 0 && ArrayVal(i).CompareTo(Parent(i)) > 0) {
        Swap(i, ParentIndex(i));
        i = ParentIndex(i);
      }
    }

    protected void DownHeap(int i) {
      while (i >= 0) {
        int iContinue = -1;

        if (RightSonExists(i) && Right(i).CompareTo(ArrayVal(i)) > 0) {
          iContinue = Left(i).CompareTo(Right(i)) < 0 ? RightChildIndex(i) : LeftChildIndex(i);
        } else if (LeftSonExists(i) && Left(i).CompareTo(ArrayVal(i)) > 0) {
          iContinue = LeftChildIndex(i);
        }

        if (iContinue >= 0 && iContinue < LstHeap.Count) {
          Swap(i, iContinue);
        }

        i = iContinue;
      }
    }
  }

  internal class HuffmanNode<T> : IComparable {
    internal HuffmanNode(double probability, T value) {
      Probability = probability;
      LeftSon = RightSon = Parent = null;
      Value = value;
      IsLeaf = true;
    }

    internal HuffmanNode(HuffmanNode<T> leftSon, HuffmanNode<T> rightSon) {
      LeftSon = leftSon;
      RightSon = rightSon;
      Probability = leftSon.Probability + rightSon.Probability;
      leftSon.IsZero = true;
      rightSon.IsZero = false;
      leftSon.Parent = rightSon.Parent = this;
      IsLeaf = false;
    }

    internal HuffmanNode<T> LeftSon;
    internal HuffmanNode<T> RightSon;
    internal HuffmanNode<T> Parent;
    internal T Value;
    internal bool IsLeaf;
    internal double Probability;
    internal bool IsZero;
    // encoded bits
    internal Bits.SmallArray encoded() {
      if (_encoded.count == 0) {
        var nodeCur = this;
        while (!nodeCur.IsRoot) {
          if (_encoded.count > 0)
            _encoded.value >>= 1; // not first bit => shift other bits value to the right
          if (!nodeCur.IsZero)
            _encoded.value = _encoded.value | 0x80000000; // put the non zero bit to start
          //if (_encoded.count > 0)
          //  _encoded.value <<= 1; // not first bit => shift other bits value to the right
          //if (!nodeCur.IsZero)
          //  _encoded.value = _encoded.value | 0x1; // put the non zero bit to start

          _encoded.count++;
          if (_encoded.count > 32)
            throw new Exception(); // more than 32 bits => error
          nodeCur = nodeCur.Parent;
        }
      }
      return _encoded;
    }
    internal Bits.SmallArray _encoded;

    internal bool IsRoot {
      get { return Parent == null; }
    }

    public int CompareTo(object obj) {
      return -Probability.CompareTo(((HuffmanNode<T>)obj).Probability);
    }
  }

  public class Huffman<T> where T : IComparable {
    private readonly Dictionary<T, HuffmanNode<T>> _leafDictionary = new Dictionary<T, HuffmanNode<T>>();
    private readonly HuffmanNode<T> _root;

    public Huffman(IEnumerable<T> values) {
      var counts = new Dictionary<T, int>();
      var priorityQueue = new PriorityQueue<HuffmanNode<T>>();
      int valueCount = 0;

      foreach (T value in values) {
        if (!counts.ContainsKey(value)) {
          counts[value] = 0;
        }
        counts[value]++;
        valueCount++;
      }

      foreach (T value in counts.Keys) {
        var node = new HuffmanNode<T>((double)counts[value] / valueCount, value);
        priorityQueue.Add(node);
        _leafDictionary[value] = node;
      }

      while (priorityQueue.Count > 1) {
        HuffmanNode<T> leftSon = priorityQueue.Pop();
        HuffmanNode<T> rightSon = priorityQueue.Pop();
        var parent = new HuffmanNode<T>(leftSon, rightSon);
        priorityQueue.Add(parent);
      }

      _root = priorityQueue.Pop();
      _root.IsZero = false;
    }

    public IEnumerable<bool> Encode(T value) {
      var node = _leafDictionary[value];
      for (var i = 0; i < node.encoded().count; i++)
        yield return Bits.getBit(node._encoded.value, i);
    }

    public byte[] Encode(T[] values) {
      var wr = new BitWriter();
      foreach (var v in values.Select(vv => _leafDictionary[vv].encoded()))
        wr.Write(v.value, v.count);
      wr.Align();
      return wr.data.ToArray();
      //return Bits.serializeArrays(values.Select(v => _leafDictionary[v].encoded()).ToArray());
    }

    public IEnumerable<T> Decode(IEnumerable<bool> bitString) {
      HuffmanNode<T> nodeCur = _root;
      foreach (var zero in bitString) {
        nodeCur = zero ? nodeCur.LeftSon : nodeCur.RightSon;
        if (nodeCur.IsLeaf) {
          yield return nodeCur.Value;
          nodeCur = _root;
        }
      }
      if (nodeCur != _root)
        throw new ArgumentException("Invalid bitstring in Decode");
    }
  }

  public static class Program {
    //const string Example = "见/見 this is an example for huffman encoding 败/敗 雙音節漢字雙音節漢字";
    const string Example = "bcaabcaabcaabcaa"; // codes a:0, b:01, c:11
    //static string Example =
    //  "ijklmnopq" +
    //  new string('a', 256) +
    //  new string('b', 128) +
    //  new string('c', 64) +
    //  new string('d', 32) +
    //  new string('e', 16) +
    //  new string('f', 8) +
    //  new string('g', 4) +
    //  new string('h', 2) +
    //  new string('a', 5) +
    //  //"ijklmnopq" + 
    //  "";
    static byte[] utf8 = Encoding.UTF8.GetBytes(Example);

    public static void Main() {
      var huffman = new Huffman<char>(Example);
      var encoding = huffman.Encode(Example.ToCharArray());
      encoding = null;
      //  var decoding = huffman.Decode(encoding).ToArray();
      //  var outString = new string(decoding.ToArray());
      //  Console.WriteLine(outString == Example ? "Encoding/decoding worked: " + 1.0 * encoding.Length / 8 / utf8.Length : "Encoding/Decoding failed");

      //  var chars = new HashSet<char>(Example);
      //  foreach (char c in chars) {
      //    encoding = huffman.Encode(c).ToArray();
      //    Console.Write("{0}:  ", c);
      //    foreach (var bit in encoding) {
      //      Console.Write("{0}", bit ? 0 : 1);
      //    }
      //    Console.WriteLine();
      //  }
      //  Console.ReadKey();
    }
  }
}
