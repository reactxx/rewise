//https://code.google.com/archive/p/csdiff/source/default/source
import 'dart:math';

class ValueObj<TInfo> {
  ValueObj(this.value, this.info);
  String value;
  TInfo info;
}

class Item<TInfo> {
  Item(this.diffs, this.info);
  List<DiffEntry> diffs;
  TInfo info;
}

class DiffHistory<TInfo> {
  var Value = "";

  String push(String str, TInfo info) {
    items.add(Item<TInfo>(Diff.CreateDiff(Value, str), info));
    Value = str;
    return Value;
  }

  ValueObj pop() {
    assert(items.length > 0, "Empty history");
    if (items.length == 1) {
      items.clear();
      Value = "";
    } else {
      String res = null;
      for (var i = 0; i < items.length - 1; i++)
        res = Diff.Merge(res == null ? "" : res, items[i].diffs);
      items.removeAt(items.length - 1);
      Value = res;
    }
    return ValueObj(
        Value, items.length > 0 ? items[items.length - 1].info : null);
  }

  Iterable<ValueObj> getItems() sync* {
    String res = null;
    for (var item in items) {
      res = Diff.Merge(res == null ? "" : res, item.diffs);
      yield ValueObj(res, item.info);
    }
  }

  List<String> getValues() {
    var lst = List<String>();
    String res = null;
    for (var item in items) {
      res = Diff.Merge(res == null ? "" : res, item.diffs);
      lst.add(res);
    }
    return lst;
  }

  List<Item> items = List<Item>();
}

enum DiffEntryType { Start, Remove, Add, Equal }

class DiffEntry {
  DiffEntry(this.EntryType, this.Count, this.Value);
  DiffEntryType EntryType;
  String Value;
  int Count;
}

class Diff {
  static String Merge(String orig, Iterable<DiffEntry> diff) {
    List<String> sb = [];
    var srcCount = 0;
    for (var d in diff) {
      switch (d.EntryType) {
        case DiffEntryType.Equal:
          sb.add(orig.substring(srcCount, srcCount + d.Count));
          srcCount += d.Count;
          break;
        case DiffEntryType.Remove:
          srcCount += d.Count;
          break;
        case DiffEntryType.Add:
          sb.add(d.Value);
          break;
        case DiffEntryType.Start:
          break;
      }
    }
    ;

    sb.add(orig.substring(srcCount));

    return sb.join();
  }

  /// <summary>
  /// Creates a list of diff entries that represent the differences between arr1 and arr2.
  /// </summary>
  /// <param name="arr1">Array of units.</param>
  /// <param name="arr2">Array of units.</param>
  /// <returns>List of DiffEntry classes.</returns>
  static List<DiffEntry> CreateDiff(String arr1, String arr2) {
    if (arr1 == null && arr2 == null) return List<DiffEntry>();
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

    if (start == arr1.length && start == arr2.length) return List<DiffEntry>();

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
    List<DiffEntry> diffList = List<DiffEntry>();

    stck.add([lines1_cnt - 1, lines2_cnt - 1]);
    do {
      var data = stck.removeLast();

      int i = data[0];
      int j = data[1];

      if (i >= 0 && j >= 0 && arr1[i + start] == arr2[j + start]) {
        stck.add([i - 1, j - 1]);
        addEntry(diffList, DiffEntryType.Equal);
      } else {
        if (j >= 0 && (i <= 0 || j == 0 || lcs[i][j - 1] >= lcs[i - 1][j])) {
          stck.add([i, j - 1]);
          addEntry(diffList, DiffEntryType.Add, arr2[j + start]);
        } else if (i >= 0 &&
            (j <= 0 || i == 0 || lcs[i][j - 1] < lcs[i - 1][j])) {
          stck.add([i - 1, j]);
          addEntry(diffList, DiffEntryType.Remove);
        }
      }
    } while (stck.length > 0);

    return diffList.reversed.toList();
  }

  static void addEntry(List<DiffEntry> diffList, DiffEntryType type,
      [String value]) {
    var lastDiff = diffList.length > 0 ? diffList[diffList.length - 1] : null;
    if (lastDiff != null && lastDiff.EntryType == type) {
      if (type == DiffEntryType.Add) {
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
    diffList.add(DiffEntry(type, 1, value == null ? null : value));
  }
}
