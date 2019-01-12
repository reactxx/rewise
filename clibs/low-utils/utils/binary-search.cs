public delegate int Compare<T>(T a, T b);

public static class BinarySearch {

  public static int Search<T>(T[] items, int min, int len, T key, Compare<T> compare) {
    int max = min + len;
    while (min < max) {
      int mid = min + ((max - min) >> 1);
      var element = items[mid];
      var comp = compare(element, key);
      if (comp == 0) return mid;
      if (comp < 0 /*element < key*/) min = mid + 1; else max = mid;
    }
    return -min - 1;
  }

  public static int Search<T>(T[] items, T key, Compare<T> comparer) {
    return Search(items, 0, items.Length, key, comparer);
  }

}
