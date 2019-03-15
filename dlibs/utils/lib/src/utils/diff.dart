//https://code.google.com/archive/p/csdiff/source/default/source
//https://medium.com/skyrise/the-myers-diff-algorithm-and-kotlin-observable-properties-69dfb18541b
import 'dart:math';

class ValueObj<TInfo> {
  ValueObj(this.value, this.info);
  String value;
  TInfo info;
}

class DiffHistory<TInfo> {
  var _value = "";

  String push(String str, TInfo info) {
    _items.add(_Item<TInfo>(Diff.CreateDiff(_value, str), info));
    _value = str;
    return _value;
  }

  ValueObj pop() {
    assert(_items.length > 0, "Empty history");
    if (_items.length == 1) {
      _items.clear();
      _value = "";
    } else {
      String res = null;
      for (var i = 0; i < _items.length - 1; i++)
        res = Diff.Merge(res == null ? "" : res, _items[i].diffs);
      _items.removeAt(_items.length - 1);
      _value = res;
    }
    return ValueObj(
        _value, _items.length > 0 ? _items[_items.length - 1].info : null);
  }

  Iterable<ValueObj> getItems() sync* {
    String res = null;
    for (var item in _items) {
      res = Diff.Merge(res == null ? "" : res, item.diffs);
      yield ValueObj(res, item.info);
    }
  }

  List<String> getValues() {
    var lst = List<String>();
    String res = null;
    for (var item in _items) {
      res = Diff.Merge(res == null ? "" : res, item.diffs);
      lst.add(res);
    }
    return lst;
  }

  List<_Item> _items = List<_Item>();
}

class _Item<TInfo> {
  _Item(this.diffs, this.info);
  List<_DiffEntry> diffs;
  TInfo info;
}

enum _DiffEntryType { Remove, Add, Equal }

class _DiffEntry {
  _DiffEntry(this.EntryType, this.Count, this.Value);
  _DiffEntryType EntryType;
  String Value;
  int Count;
}

class Diff {
  static String Merge(String orig, Iterable<_DiffEntry> diff) {
    var sb = new StringBuffer();
    var srcCount = 0;
    for (var d in diff) {
      if (d.EntryType==null) continue;
      switch (d.EntryType) {
        case _DiffEntryType.Equal:
          sb.write(orig.substring(srcCount, srcCount + d.Count));
          srcCount += d.Count;
          break;
        case _DiffEntryType.Remove:
          srcCount += d.Count;
          break;
        case _DiffEntryType.Add:
          sb.write(d.Value);
          break;
      }
    }
    sb.write(orig.substring(srcCount));

    return sb.toString();
  }

  /// <summary>
  /// Creates a list of diff entries that represent the differences between arr1 and arr2.
  /// </summary>
  /// <param name="arr1">Array of units.</param>
  /// <param name="arr2">Array of units.</param>
  /// <returns>List of DiffEntry classes.</returns>
  static List<_DiffEntry> CreateDiff(String arr1, String arr2) {
    if (arr1 == null && arr2 == null) return List<_DiffEntry>();
    if (arr1 == null)
      arr1 = "";
    else if (arr2 == null) arr2 = "";

    int start = 0;
    int end = 0;

    // Strip off the beginning and end, if it's equal
    while (start < min(arr1.length, arr2.length)) {
      if (arr1[start] != arr2[start]) break;
      start++;
    }

    if (start == arr1.length && start == arr2.length) return List<_DiffEntry>();

    for (int i = 0; i < min(arr1.length, arr2.length) - start; i++) {
      if (arr1[arr1.length - i - 1] != arr2[arr2.length - i - 1]) break;
      end++;
    }

    int lines1_cnt = arr1.length - start - end;
    int lines2_cnt = arr2.length - start - end;

    List<List<int>> lcs = List.generate(
        lines1_cnt, (_) => List(lines2_cnt)); // [lines1_cnt, lines2_cnt];

    // Calculate longest common sequence
    for (int i = 0; i < lines1_cnt; i++) {
      for (int j = 0; j < lines2_cnt; j++) {
        int iVal = i + start;
        int jVal = j + start;

        if (arr1[iVal] != arr2[jVal]) {
          if (i == 0 && j == 0)
            lcs[i][j] = 0;
          else if (i == 0 && j != 0)
            lcs[i][j] = max(0, lcs[i][j - 1]);
          else if (i != 0 && j == 0)
            lcs[i][j] = max(lcs[i - 1][j], 0);
          else // if (i != 0 && j != 0)
            lcs[i][j] = max(lcs[i - 1][j], lcs[i][j - 1]);
        } else {
          if (i == 0 || j == 0)
            lcs[i][j] = 1;
          else
            lcs[i][j] = 1 + lcs[i - 1][j - 1];
        }
      }
    }

    // Build the list of differences
    List<List<int>> stck = List();
    List<_DiffEntry> diffList = List<_DiffEntry>();

    stck.add([lines1_cnt - 1, lines2_cnt - 1]);
    do {
      var data = stck.removeLast();

      int i = data[0];
      int j = data[1];

      if (i >= 0 && j >= 0 && arr1[i + start] == arr2[j + start]) {
        stck.add([i - 1, j - 1]);
        addEntry(diffList, _DiffEntryType.Equal);
      } else {
        if (j >= 0 && (i <= 0 || j == 0 || lcs[i][j - 1] >= lcs[i - 1][j])) {
          stck.add([i, j - 1]);
          addEntry(diffList, _DiffEntryType.Add, arr2[j + start]);
        } else if (i >= 0 &&
            (j <= 0 || i == 0 || lcs[i][j - 1] < lcs[i - 1][j])) {
          stck.add([i - 1, j]);
          addEntry(diffList, _DiffEntryType.Remove);
        }
      }
    } while (stck.length > 0);

    return diffList.reversed.toList();
  }

  static void addEntry(List<_DiffEntry> diffList, _DiffEntryType type,
      [String value]) {
    var lastDiff = diffList.length > 0 ? diffList[diffList.length - 1] : null;
    if (lastDiff != null && lastDiff.EntryType == type) {
      if (type == _DiffEntryType.Add) {
        if (lastDiff.Value.length < 255) {
          lastDiff.Value = value + lastDiff.Value;
          return;
        }
      } else {
        if (lastDiff.Count < 255) {
          lastDiff.Count++;
          return;
        }
      }
    }
    // different type OR Value.length | Count eq 255:
    diffList.add(_DiffEntry(type, 1, value == null ? null : value));
  }
}
