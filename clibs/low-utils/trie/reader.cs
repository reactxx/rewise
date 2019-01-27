using System;
using System.Runtime;

public static class reader {

  public class BytesReader {

    int start;
    int len;
    byte[] data;
    int pos;

    public BytesReader(byte[] _data) {
      data = _data;
      len = data.Length;
    }

    public BytesReader(BytesReader parent, int _len = 0) {
      data = parent.data;
      pos = start = parent.pos;
      len = _len == 0 ? data.Length : start + _len;
      if (len > data.Length) throw new ArgumentException();
    }

    public BytesReader setPos(int newPos) {
      pos = start + newPos;
      if (pos < start || pos >= data.Length) throw new ArgumentException();
      return this;
    }

    public int readNum(byte size) {
      switch (size) {
        case 0: return 0;
        case 1: return data[pos++];
        case 2: return data[pos++] | data[pos++] << 8;
        case 3: return data[pos++] | data[pos++] << 8 | data[pos++] << 16;
        default: throw new NotImplementedException();
      }
    }

    public BytesReader innerReader(int dataSize = 0) {
      if (dataSize == 0) dataSize = len - pos;
      pos += dataSize;
      return new BytesReader(this, dataSize);
    }

    public Tuple<int, int> BinarySearch(byte size, int key) {
      int min = 0;
      int max = (len - pos) / size;
      while (min < max) {
        int mid = min + ((max - min) >> 1);
        var element = setPos(min * size).readNum(size);
        var comp = element - key;
        if (element == key) return Tuple.Create(mid, element);
        if (element < key) min = mid + 1; else max = mid;
      }
      return Tuple.Create(-min - 1, -1);
    }

  }


}