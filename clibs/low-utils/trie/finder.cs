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
    var pos = 0;
    // length flags
    var flags = data[0]; pos++;
    var dataSize = flags & 0x3;
    var keySize = (flags >> 2) & 0x3;
    var childsDataSize = (flags >> 4) & 0x3;
    var childsCountSize = (flags >> 6) & 0x3;
    if (childsCountSize == 0) { // no childs
      
    }


    return null;
  }

}

