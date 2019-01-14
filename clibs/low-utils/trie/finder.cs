using System;
using System.Collections.Generic;
using System.Linq;

//ref returns: https://blogs.msdn.microsoft.com/mazhou/2017/12/12/c-7-series-part-7-ref-returns/
//https://blogs.msdn.microsoft.com/mazhou/2018/03/25/c-7-series-part-10-spant-and-universal-memory-management/
//https://msdn.microsoft.com/en-us/magazine/mt814808.aspx
public class TrieFind {

  byte[] data;

  public TrieFind(byte[] data) {
    this.data = data;
  }

  public byte[] find(string word) {
    Span<byte> bytes = data;
    System.Memory<byte>
    BitConverter.
    return null;
  }

}

