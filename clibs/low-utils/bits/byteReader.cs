//using System.Linq;
//using System;
//using System.Diagnostics;
//using System.Collections.Generic;

//public interface IByteStream {
//  byte ReadByte();
//  bool TryReadByte(out byte value);
//  void WriteByte(byte value);
//}

//public class MemoryByteStream : IByteStream {

//  public List<byte> writerData;

//  public static MemoryByteStream CreateWriter() {
//    return new MemoryByteStream { _writer = true, writerData = new List<byte>() };
//  }
//  public static MemoryByteStream CreateReader(IEnumerable<byte> data) {
//    return new MemoryByteStream { _readerData = data.GetEnumerator() };
//  }

//  public bool TryReadByte(out byte value) {
//    checkReader();
//    value = 0;
//    if (!_readerData.MoveNext()) return false;
//    value = _readerData.Current;
//    return true;
//  }

//  public byte ReadByte() {
//    checkReader();
//    if (!_readerData.MoveNext()) throw new Exception();
//    return _readerData.Current;
//  }

//  public void WriteByte(byte value) {
//    checkWritter();
//    writerData.Add(value);
//  }

//  //public uint ReadVLQ() {
//  //  checkReader();
//  //  var b1 = ReadByte();
//  //  if ((b1 & 0x80) != 0) return (uint)(b1 & 0x7f);
//  //  var b2 = ReadByte();
//  //  if ((b2 & 0x80) != 0) return (uint)((b1 << 7) + (b2 & 0x7f));
//  //  var b3 = ReadByte();
//  //  if ((b3 & 0x80) != 0) return (uint)((b1 << 14) + (b2 << 7) + (b3 & 0x7f));
//  //  var b4 = ReadByte();
//  //  if ((b4 & 0x80) != 0) return (uint)((b1 << 21) + (b2 << 14) + (b3 << 7) + (b4 & 0x7f));
//  //  throw new OverflowException();
//  //}

//  //public void WriteVLQ(uint num) {
//  //  checkWritter();
//  //  if (num <= max1) { WriteByte((byte)(num | 0x80)); return; }
//  //  if (num <= max2) { WriteByte((byte)(num >> 7)); WriteByte((byte)((num & 0x7f) | 0x80)); return; }
//  //  if (num <= max3) { WriteByte((byte)(num >> 14)); WriteByte((byte)((num >> 7) & 0x7f)); WriteByte((byte)((num & 0x7f) | 0x80)); return; }
//  //  if (num <= max4) { WriteByte((byte)(num >> 21)); WriteByte((byte)((num >> 14) & 0x7f)); WriteByte((byte)((num >> 7) & 0x7f)); WriteByte((byte)((num & 0x7f) | 0x80)); return; }
//  //  throw new OverflowException(num.ToString());
//  //}
//  //const int max1 = 0xFF >> 1;
//  //const int max2 = 0xFFFF >> 2;
//  //const int max3 = 0xFFFFFF >> 3;
//  //const int max4 = (int)(0xFFFFFFFF >> 4);

//  bool _writer;
//  void checkWritter() { if (!_writer) throw new Exception(); }
//  void checkReader() { if (_writer) throw new Exception(); }
//  IEnumerator<byte> _readerData;
//}

////https://rosettacode.org/wiki/Bitwise_IO#C.23
//public class BitReader {
//  ulong readData = 0;
//  int startPosition = 0;
//  int endPosition = 0;

//  IByteStream dataStream;

//  public BitReader(IEnumerable<byte> data) {
//    dataStream = MemoryByteStream.CreateReader(data);
//  }

//  void EnsureData(int bitCount) {
//    int readBits = bitCount - InBuffer;
//    while (readBits > 0) {
//      ulong b = dataStream.ReadByte();
//      readData |= b << (56 - endPosition);
//      endPosition += 8;
//      readBits -= 8;
//    }
//  }

//  int InBuffer {
//    get { return endPosition - startPosition; }
//  }

//  public bool ReadBit() {
//    return ReadBits(1).First();
//  }

//  public IEnumerable<bool> ReadAllBits() {
//    while (true) {
//      // return the rest of the readData
//      foreach (var b in ReadBits(InBuffer))
//        yield return b;
//      if (!dataStream.TryReadByte(out byte bb))
//        break;
//      for (var i = 0; i < 8; i++)
//        yield return (bb & ((0x1) << (8 - i - 1))) != 0;
//    }
//  }

//  public IEnumerable<bool> ReadBits(int bitCount) {
//    EnsureData(bitCount);

//    for (var i = 0; i < bitCount; i++)
//      yield return (readData & ((ulong)(0x1) << (63 - i - startPosition))) != 0;

//    startPosition += bitCount;
//    if (endPosition == startPosition) {
//      endPosition = startPosition = 0;
//      readData = 0;
//    } else if (startPosition >= 8) {
//      readData >>= startPosition;
//      endPosition -= startPosition;
//      startPosition = 0;
//    }
//  }

//  public void Align() {
//    endPosition = startPosition = 0;
//    readData = 0;
//  }
//}

//public class BitWriter {
//  ulong _data = 0;
//  int _dataLength = 0;

//  MemoryByteStream _dataStream = MemoryByteStream.CreateWriter();

//  public List<byte> data { get { return _dataStream.writerData; } }

//  public int BitsToAligment {
//    get { return (32 - _dataLength) % 8; }
//  }

//  public void WriteBit(bool value) {
//    WriteBits(value ? (uint)1 << 31 : 0, 1);
//  }

//  public void WriteBits(IEnumerable<bool> values) {
//    foreach (var b in values) WriteBit(b);
//  }

//  // bits are at the begining of the value, first bit is in (value & 0x80000000)
//  public void WriteBits(uint value, int length) {
//    ulong currentData = _data | ((ulong)value << (32 - _dataLength));
//    int currentLength = _dataLength + length;
//    while (currentLength >= 8) {
//      _dataStream.WriteByte((byte)(currentData >> 56));
//      currentData <<= 8;
//      currentLength -= 8;
//    }
//    _data = currentData;
//    _dataLength = currentLength;
//  }

//  public void Align() {
//    if (_dataLength == 0) return;
//    _dataStream.WriteByte((byte)(_data >> 56));
//    _data = 0;
//    _dataLength = 0;
//  }
//}

//public static class BitsProgram {
//  public static void Main() {
//    BitWriter writer = new BitWriter();
//    writer.WriteBit(true);
//    writer.WriteBits(0xffffffff, 31);

//    //writer.Write(0x0155, 32);

//    //writer.Write(0xffffffff, 32);
//    //writer.Write(0xffffffff, 32);
//    writer.Align();

//    BitReader reader = new BitReader(writer.data);
//    Console.WriteLine(reader.ReadBit());
//    //Console.WriteLine(reader.ReadBits(32).Select(b => b ? "1" : "0").JoinStrings());
//    Console.WriteLine(reader.ReadAllBits().Select(b => b ? "1" : "0").JoinStrings());
//    //Console.WriteLine(reader.Read(3));

//    //Console.WriteLine(reader.Read(32).ToString("x8"));

//    //Console.WriteLine(reader.Read(32).ToString("x8"));
//    //Console.WriteLine(reader.Read(32).ToString("x8"));
//    reader.Align();
//  }
//}

//public static class Bits {

//  public struct SmallArray {
//    public uint value;
//    public int count; // 1..32
//  }

//  //public static byte[] serializeArrays(SmallArray[] source, out int rest) {
//  //  var bits = source.Sum(s => s.count);
//  //  var bytes = bits >> 3; // divide by 8
//  //  rest = bits - (bytes << 3); // overflow bits
//  //  if (rest > 0) bytes++;
//  //  var res = new byte[bytes];
//  //  var byteIdx = 0; var notUsedBits = 8 /*1..8, number of unused bits in res[byteIdx]*/;
//  //  foreach (var s in source) {
//  //    // copy bits
//  //    var toCopyCount = s.count; var toCopyValue = s.value;
//  //    while (true) {
//  //      byte actByte = (byte)(toCopyValue >> (32 - notUsedBits)); // first notUsedBits bits from toCopyValue
//  //      res[byteIdx] = (byte)(res[byteIdx] | actByte); // copy to act byte
//  //      var copied = Math.Min(notUsedBits, toCopyCount);

//  //      var newNotUsed = notUsedBits - toCopyCount;
//  //      toCopyCount -= copied;

//  //      if (newNotUsed > 0) { // free space in act byte
//  //        notUsedBits = newNotUsed;
//  //        break;
//  //      }
//  //      byteIdx++;
//  //      notUsedBits = 8;
//  //      if (newNotUsed == 0)
//  //        break;
//  //      toCopyValue <<= notUsedBits;
//  //    }
//  //  }
//  //  if (notUsedBits == 8) byteIdx--;
//  //  Debug.Assert(byteIdx == res.Length - 1);
//  //  return res;
//  //}
//  //public static byte[] serializeArrays(SmallArray[] source) {
//  //  return serializeArrays(source, out int rest);
//  //}

//  //https://stackoverflow.com/questions/24250582/set-a-specific-bit-in-an-int
//  public static uint setBit(this uint i, int index) {
//    return i | (uint)(1 << index);
//  }

//  //https://stackoverflow.com/questions/4854207/get-a-specific-bit-from-byte
//  public static bool getBit(this uint i, int index) {
//    return (i & (1 << (index - 1))) != 0;
//  }
//}