using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class Linq {

  public static string JoinStrings(this IEnumerable<string> strings, string delim, int skip, int take = int.MaxValue, StringBuilder sb = null) {
    if (sb == null)
      return strings.Skip(skip).Take(take).Aggregate((r, i) => r + delim + i);
    else {
      sb.Clear();
      foreach (var str in strings.Skip(skip).Take(take)) {
        if (sb.Length > 0) sb.Append(delim);
        sb.Append(str);
      }
      return sb.ToString();
    }
  }

  public static string JoinStrings(this IEnumerable<string> strings, string delim, StringBuilder sb = null) {
    return JoinStrings(strings, delim, 0, int.MaxValue, sb);
  }

  public static IEnumerable<T> Items<T>(params T[] items) {
    return items;
  }

  public static IEnumerable<T> NullsWhenEmpty<T>(this IEnumerable<T> items, int num) where T : class {
    return items == null || !items.Any() ? GetNulls<T>(num) : items;
  }

  public static IEnumerable<T> GetNulls<T>(int num) where T : class {
    return Enumerable.Range(0, num).Select(i => null as T);
  }

  public static IEnumerable<T> NotNulls<T>(this IEnumerable<T> items, Func<T,bool> isEmpty = null) where T : class {
    return items.Where(it => it != null && (isEmpty==null || !isEmpty(it)));
  }

  public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> act) {
    var count = 0;
    foreach (var it in items) act(it, count++);
  }

  public static void ForEach<T>(this IEnumerable<T> items, Action<T> act) {
    foreach (var it in items) act(it);
  }

  public static void ToArray<T>(this IEnumerable<T> items, T[] res, int startIdx) {
    foreach (var item in items) res[startIdx++] = item;
  }

  public static IEnumerable<string> ReadAllLines(this StreamReader rdr) {
    while (!rdr.EndOfStream) yield return rdr.ReadLine();
  }


}