using System.Collections.Generic;

public static class Hash64
{
  public static ulong ComputeHashCode(string url)
  {
    const ulong p = 1099511628211;

    ulong hash = 14695981039346656037;

    for (int i = 0; i < url.Length; ++i)
      hash = (hash ^ url[i]) * p;

    // Wang64 bit mixer
    hash = (~hash) + (hash << 21);
    hash = hash ^ (hash >> 24);
    hash = (hash + (hash << 3)) + (hash << 8);
    hash = hash ^ (hash >> 14);
    hash = (hash + (hash << 2)) + (hash << 4);
    hash = hash ^ (hash >> 28);
    hash = hash + (hash << 31);

    if (hash == (ulong)0)
      ++hash;

    return hash;
  }
}

public static class LowUtils {
  
  public struct interval {
    public interval(int start, int end) { this.start = start; this.end = end; }
    public int start; public int end;
    public int skip { get { return start; } }
    public int take { get { return end - start + 1; } }
  }
  public static IEnumerable<interval> intervals(int count, int intLen) {
    int num = count / intLen;
    if (num * intLen < count) num++;
    for (int i = 0; i < num; i++) {
      int st = i * intLen; int en = st + intLen - 1;
      if (en > count - 1) en = count - 1;
      yield return new interval(st, en);
    }
  }
}