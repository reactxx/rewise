using System;

//https://github.com/rubendal/BitStream/blob/master/Int24.cs
public struct UInt24 {

  public byte[] buf;

  UInt24(int value) {
    buf = new byte[] { (byte)(value & 0xFF), (byte)((value >> 8) & 0xFF), (byte)((value >> 16) & 0xFF) };
  }

  public UInt24(byte[] data, int pos) {
    buf = new byte[3];
    Buffer.BlockCopy(data, pos, buf, 0, 3);
  }

  public static implicit operator UInt24(int value) {
    return new UInt24(value);
  }

  public static implicit operator int(UInt24 i) {
    return i.buf[0] | (i.buf[1] << 8) | (i.buf[2] << 16);
  }

}
