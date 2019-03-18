import 'data_unicodeData.dart' show getUnicodeData;
import 'dart:collection' show HashSet;
import 'package:collection/collection.dart' show binarySearch;

class Unicode {
  static void init() {
    if (_sorted == null) {
      final bl = getUnicodeData();
      var idx = 0;
      _sorted =
          bl.ranges.map((r) => Item(r.start, r.end, bl.iSO15924[idx])).toList();
      _sorted.sort(_sortCompare);
    }
  }

  static List<Item> _sorted;

  static Item item(int ch) {
    init();
    final idx = binarySearch(_sorted, ch, compare: _compare);
    return idx < 0 ? null : _sorted[idx];
  }

  static bool isLetter(int ch) => item(ch) != null;

  static bool isBlank(int ch) => _blanks.contains(ch);
  static final _blanks = HashSet.from(bls);
  //brackets:
  //http://xahlee.info/comp/unicode_matching_brackets.html

  static bool isLetters(String str) =>
      str.trim().codeUnits.every((ch) => isLetter(ch));

  static String filterLetters(String data) => data == null
      ? null
      : String.fromCharCodes(data.codeUnits.where((ch) => isLetter(ch)));

  static Map<String, Set<int>> scriptsFromTexts(Iterable<String> texts) {
    var res = Map<String, Set<int>>();
    for (var str in texts)
      if (str != null)
        for (var ch in str.codeUnits) {
          final it = item(ch);
          if (it == null) continue;
          res.update(it.script, (v) => v..add(ch),
              ifAbsent: () => Set.from([ch]));
        }
    return res;
  }

  static Map<String, Set<int>> scriptsFromText(String text) =>
      scriptsFromTexts([text]);

  static Iterable<Item> blockIdxs(String str) {
    var res = Set<Item>();
    for (final ch in str.codeUnits) {
      final it = item(ch);
      if (it != null) res.add(it);
    }
    return res;
  }

  static int _compare(x, y) {
    if (y > x.end) return -1;
    if (y < x.start) return 1;
    return 0;
  }

  static int _sortCompare(x, y) {
    if (y.start > x.end) return -1;
    if (y.end < x.start) return 1;
    return 0;
  }
}

class Item {
  Item(this.start, this.end, this.script);
  int start;
  int end;
  String script;
}

const bls = [
    0x9,
    0x10,
    0x11,
    0x12,
    0x20,
    0x85,
    0xA0,
    0x1680,
    0x2000,
    0x2001,
    0x2002,
    0x2003,
    0x2004,
    0x2005,
    0x2006,
    0x2007,
    0x2008,
    0x2009,
    0x200A,
    0x2028,
    0x2029,
    0x202F,
    0x205F,
    0x3000,
    0xFEFF
  ];