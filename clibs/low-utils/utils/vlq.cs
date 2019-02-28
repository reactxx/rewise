//https://stackoverflow.com/questions/51350777/encoding-a-long-to-a-vlq-byte-array-and-writing-it-to-system-io-binarywriter
//https://rosettacode.org/wiki/Variable-length_quantity
using System;
using System.IO;

public static class VLQ {
  const int max1 = 0xFF >> 1; //0b0111_1111;
  const int max2 = 0xFFFF >> 2; //0b0011_1111_1111_1111;
  const int max3 = 0xFFFFFF >> 3; //0b0001_1111_1111_1111_1111_1111;
  const int max4 = (int)(0xFFFFFFFF >> 4); //0b0000_1111_1111_1111_1111_1111_1111_1111;
  public static int Read(this BinaryReader r) {
    var b1 = r.ReadByte();
    if ((b1 & 0x80) != 0) return b1 & 0x7f;
    var b2 = r.ReadByte();
    if ((b2 & 0x80) != 0) return (b1 << 7) + (b2 & 0x7f);
    var b3 = r.ReadByte();
    if ((b3 & 0x80) != 0) return (b1 << 14) + (b2 << 7) + (b3 & 0x7f);
    var b4 = r.ReadByte();
    if ((b4 & 0x80) != 0) return (b1 << 21) + (b2 << 14) + (b3 << 7) + (b4 & 0x7f);
    throw new OverflowException();
  }
  public static void Write(this BinaryWriter w, int num) {
    if (num <= max1) { w.Write((byte)(num | 0x80)); return; }
    if (num <= max2) { w.Write((byte)(num >> 7)); w.Write((byte)((num & 0x7f) | 0x80)); return; }
    if (num <= max3) { w.Write((byte)(num >> 14)); w.Write((byte)((num >> 7) & 0x7f)); w.Write((byte)((num & 0x7f) | 0x80)); return; }
    if (num <= max4) { w.Write((byte)(num >> 21)); w.Write((byte)((num >> 14) & 0x7f)); w.Write((byte)((num >> 7) & 0x7f)); w.Write((byte)((num & 0x7f) | 0x80)); return; }
    throw new OverflowException(num.ToString());
  }
  public static void Test() {
    var mem = new MemoryStream();
    var wr = new BinaryWriter(mem);
    Write(wr, 0);
    Write(wr, max1);
    Write(wr, max1 + 1);
    Write(wr, max2);
    Write(wr, max2 + 1);
    Write(wr, max3);
    Write(wr, max3 + 1);
    Write(wr, max4);
    // 20 bytes writen
    mem.Seek(0, SeekOrigin.Begin);
    int res;
    var rdr = new BinaryReader(mem);
    if ((res = Read(rdr)) != 0) throw new Exception();
    if ((res = Read(rdr)) != max1) throw new Exception();
    if ((res = Read(rdr)) != max1 + 1) throw new Exception();
    if ((res = Read(rdr)) != max2) throw new Exception();
    if ((res = Read(rdr)) != max2 + 1) throw new Exception();
    if ((res = Read(rdr)) != max3) throw new Exception();
    if ((res = Read(rdr)) != max3 + 1) throw new Exception();
    if ((res = Read(rdr)) != max4) throw new Exception();
  }
}