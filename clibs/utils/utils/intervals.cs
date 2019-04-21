using System.Collections.Generic;
using System.Linq;

public static class Intervals {
  
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

public static class Intervals2 {

  public struct interval {
    public interval(int start, int end) { this.start = start; this.end = end; }
    public int start; public int end;
    public int skip { get { return start; } }
    public int take { get { return end - start; } }
  }
  public static IEnumerable<interval> intervals(int count, int intLen) {
    int num = count / intLen;
    if (num * intLen < count) num++;
    for (int i = 0; i < num; i++) {
      int st = i * intLen; int en = st + intLen;
      if (en > count) en = count;
      yield return new interval(st, en);
    }
  }
}

