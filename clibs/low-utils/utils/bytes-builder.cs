using System;
using System.Collections.Generic;

public class BytesBuilder {
  List<byte[]> bytes = new List<byte[]>();

  public int len;

  public void Add(byte[] data) {
    if (data == null) return;
    len += data.Length;
    bytes.Add(data);
  }

  public void Add(BytesBuilder data) {
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
}
