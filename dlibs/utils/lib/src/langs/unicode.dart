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

  static bool scriptOK(String langsScript, String unicodeScript) {
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

  static String latinOrScript(String langScript, String word) {
    if (word == null || word.isEmpty) return null;
    bool isLatn;
    String err = '';
    for (final ch in word.codeUnits) {
      final it = item(ch);
      if (it == null) continue;
      if (isLatn == true && it.script == 'Latn') continue;
      if (isLatn == false && scriptOK(langScript, it.script)) continue;
      if (isLatn == null) {
        isLatn = it.script == 'Latn';
        continue;
      }
      err += String.fromCharCode(ch);
    }
    return err.isEmpty ? null : err;
  }

  static Map<String, String> checkBlockNames(
      Iterable<String> texts, String script) {
    if (texts == null) return null;
    var res = Map<String, HashSet<int>>();
    for (final str in texts)
      if (str != null)
        for (final ch in str.codeUnits) {
          final it = item(ch);
          if (it == null || scriptOK(script, it.script)) continue;
          // if (script == "Jpan") {
          //   if (it.script == "Hani" ||
          //       it.script == "Hira" ||
          //       it.script == "Kana") continue;
          // } else if (script == "Kore") {
          //   if (it.script == "Hani" || it.script == "Hang") continue;
          // } else if (script == "Hant" || script == "Hans") {
          //   if (it.script == "Hani") continue;
          // } else if (it.script == script) continue;
          res.update(it.script, (h) => h..add(ch),
              ifAbsent: () => HashSet<int>.from([ch]));
        }
    if (res.isEmpty) return null;
    final ret = Map<String, String>();
    res.forEach((k, v) => ret[k] = String.fromCharCodes(v));
    return ret;
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
