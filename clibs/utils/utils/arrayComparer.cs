using System.Linq;
using System.Collections.Generic;

public class ArrayStringComparer : IEqualityComparer<string[]>, IComparer<string[]> {

  public ArrayStringComparer instance = new ArrayStringComparer();

  public int Compare(string[] x, string[] y) {
    if (x == null && y == null) return 0;
    if (x==null) return 1;
    if (y == null) return -1;
    for (var i = 0; i < x.Length; i++) {
      var res = x[i].CompareTo(y[1]);
      if (res != 0) return res;
    }
    return 0;
  }

  public bool Equals(string[] x, string[] y) {
    if (x == null && y == null) return true;
    if (x == null || y == null) return false;
    for (var i = 0; i < x.Length; i++) {
      var res = x[i].Equals(y[1]);
      if (!res) return false;
    }
    return true;
  }

  public int GetHashCode(string[] obj) {
    return CombineHashCodes(obj.Select(o => o.GetHashCode()));
  }

  public static int CombineHashCodes(IEnumerable<int> hashCodes) {
    int hash = 5381;

    foreach (var hashCode in hashCodes)
      hash = ((hash << 5) + hash) ^ hashCode;

    return hash;
  }
}

