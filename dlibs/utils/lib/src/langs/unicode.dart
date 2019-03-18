import 'data_unicodeData.dart' show getUnicodeData;
import 'package:rw_utils/dom/utils.dart';
import 'package:collection/algorithms.dart' show binarySearch;

class UnicodeBlocks {
  static void init() {
    if (_sorted == null) {
      final bl = getUnicodeData();
      var idx = 0;
      _sorted =
          bl.ranges.map((r) => _Item(r.start, r.end, bl.iSO15924[idx], idx++));
      _sorted.sort(Compare);
    }
  }

  static List<_Item> _sorted;

  int index(int ch) => binarySearch(_sorted, ch, compare: Compare);

  bool isLetter(int ch) => index(ch) >= 0;

  //    static Map<String, Set<int>> getBlockNames(Iterable<String> texts) {
  //   var res = Map<String, Set<int>>();
  //   for (var str in texts)
  //     if (str != null) for (var ch in str.co) {
  //         forSearch.Start = forSearch.End = Convert.ToUInt16(ch);
  //         if (!sorted.TryGetValue(forSearch, out UncRange found)) continue;
  //         var name = ISO15924[found.Idx];
  //         if (!res.TryGetValue(name, out HashSet<char> hs))
  //           res[name] = hs = new HashSet<char>();
  //         hs.Add(ch);
  //       }
  //   return res;
  // }

}

class _Item {
  _Item(this.start, this.end, this.iso, this.idx);
  int idx;
  int start;
  int end;
  String iso;
}

int Compare(x, y) {
  if (y > x.end) return -1;
  if (y < x.start) return 1;
  return 0;
}
