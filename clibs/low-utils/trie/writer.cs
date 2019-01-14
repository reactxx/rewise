using System;
using System.Collections.Generic;

public class BytesWriter {

  List<byte[]> bytes = new List<byte[]>();

  public int len;

  public void Add(int num, byte size /*0,1,2,3*/) {
    if (num == 0) return;
    Add(
      size == 1 ? new byte[] { (byte)num } : (
      size == 2 ? new byte[] { (byte)(num & 0xFF), (byte)((num >> 8) & 0xFF) } :
      ((UInt24)num).buf));
  }
  public void Add(byte[] data) {
    if (data == null) return;
    len += data.Length;
    bytes.Add(data);
  }

  public void Add(BytesWriter data) {
    if (data == null) return;
    len += data.len;
    bytes.AddRange(data.bytes);
  }

  public byte[] toBytes() {
    var res = new byte[len];
    int pos = 0;
    foreach (var bs in bytes) {
      Buffer.BlockCopy(bs, 0, res, pos, bs.Length);
      pos += bs.Length;
    }
    if (pos != len) throw new Exception("pos != len");
    return res;
  }

  public static byte getNumberSizeMask(int num) { // returns 0,1,2 or 3
    if (num > 0xffffff || num < 0) throw new ArgumentOutOfRangeException();
    return (byte)(num == 0 ? 0 : (num <= 0xff ? 1 : (num <= 0xffff ? 2 : 3)));
  }

}
