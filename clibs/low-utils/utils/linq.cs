using System.Collections.Generic;
using System.Linq;

public static class Linq {

  public static string JoinStrings(this IEnumerable<string> strings, string delim) {
    return strings.Aggregate((r, i) => r + delim + i);
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

  public static IEnumerable<T> NotNulls<T>(this IEnumerable<T> items) where T : class {
    return items.Where(it => it != null);
  }

}