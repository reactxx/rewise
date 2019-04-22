import 'data_unicodeData.dart' show getUnicodeData;
import 'dart:collection' show HashSet;
import 'package:collection/collection.dart' show binarySearch;

class Unicode {
  static void init() {
    if (_sorted == null) {
      final bl = getUnicodeData();
      _sorted = bl.ranges
          .map((r) => Item(r.start, r.end, bl.iSO15924[r.idx]))
          .toList();
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
  static final _blanks = HashSet.from(_blankCodes);
  //brackets:
  //http://xahlee.info/comp/unicode_matching_brackets.html

  static bool isLetters(String str) =>
      str.trim().codeUnits.every((ch) => isLetter(ch));

  // static String filterLetters(String data) => data == null
  //     ? null
  //     : String.fromCharCodes(data.codeUnits.where((ch) => isLetter(ch)));

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

  static bool scriptsEq(String langsScript, String unicodeScript) {
    if (unicodeScript == langsScript) return true;
    if (langsScript == "Jpan") {
      if (unicodeScript == "Hani" ||
          unicodeScript == "Hira" ||
          unicodeScript == "Kana") return true;
    } else if (langsScript == "Kore") {
      if (unicodeScript == "Hani" || unicodeScript == "Hang") return true;
    } else if (langsScript == "Hant" || langsScript == "Hans") {
      if (unicodeScript == "Hani") return true;
    }
    return false;
  }


  static Map<String, String> checkBlockNames(
      Iterable<String> texts, String script) {
    if (texts == null) return null;
    var res = Map<String, HashSet<int>>();
    for (final str in texts)
      if (str != null)
        for (final ch in str.codeUnits) {
          final it = item(ch);
          if (it == null || scriptsEq(script, it.script)) continue;
          res.update(it.script, (h) => h..add(ch),
              ifAbsent: () => HashSet<int>.from([ch]));
        }
    if (res.isEmpty) return null;
    final ret = Map<String, String>();
    res.forEach((k, v) => ret[k] = String.fromCharCodes(v));
    return ret;
  }

  static bool isDigit(int c) =>
    (c >= 48 && c <= 57) ||
    (c >= 1633 && c <= 1641) ||
    (c >= 1777 && c <= 1785) ||
    (c >= 1985 && c <= 1993) ||
    (c >= 2407 && c <= 2415) ||
    (c >= 2535 && c <= 2543) ||
    (c >= 2663 && c <= 2671) ||
    (c >= 2791 && c <= 2799) ||
    (c >= 2919 && c <= 2927) ||
    (c >= 3047 && c <= 3055) ||
    (c >= 3175 && c <= 3183) ||
    (c >= 3303 && c <= 3311) ||
    (c >= 3431 && c <= 3439) ||
    (c >= 3559 && c <= 3567) ||
    (c >= 3665 && c <= 3673) ||
    (c >= 3793 && c <= 3801) ||
    (c >= 3873 && c <= 3881) ||
    (c >= 4161 && c <= 4169) ||
    (c >= 4241 && c <= 4249) ||
    (c >= 6113 && c <= 6121) ||
    (c >= 6161 && c <= 6169) ||
    (c >= 6471 && c <= 6479) ||
    (c >= 6609 && c <= 6617) ||
    (c >= 6785 && c <= 6793) ||
    (c >= 6801 && c <= 6809) ||
    (c >= 6993 && c <= 7001) ||
    (c >= 7089 && c <= 7097) ||
    (c >= 7233 && c <= 7241) ||
    (c >= 7249 && c <= 7257) ||
    (c >= 42529 && c <= 42537) ||
    (c >= 43217 && c <= 43225) ||
    (c >= 43265 && c <= 43273) ||
    (c >= 43473 && c <= 43481) ||
    (c >= 43505 && c <= 43513) ||
    (c >= 43601 && c <= 43609) ||
    (c >= 44017 && c <= 44025) ||
    (c >= 65297 && c <= 65305);

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

const _blankCodes = [
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
