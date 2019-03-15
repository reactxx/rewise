//https://code.google.com/archive/p/csdiff/source/default/source

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

public class DiffHistory<TInfo> {

  public struct ValueObj {
    public string value;
    public TInfo info;
  }

  public string Value = "";

  public string push(string str, TInfo info) {
    items.Add(new Item { diffs = Diff.CreateDiff(Value, str), info = info });
    Value = str;
    return Value;
  }

  public ValueObj pop() {
    Debug.Assert(items.Count > 0, "Empty history");
    if (items.Count == 1) {
      items.Clear();
      Value = "";
    } else {
      string res = null;
      for (var i = 0; i < items.Count - 1; i++)
        res = Diff.Merge(res == null ? "" : res, items[i].diffs);
      items.RemoveAt(items.Count - 1);
      Value = res;
    }
    return new ValueObj { info = items.Count > 0 ? items[items.Count - 1].info : default(TInfo), value = Value };
  }

  public IEnumerable<ValueObj> getItems() {
    string res = null;
    foreach (var item in items) {
      res = Diff.Merge(res == null ? "" : res, item.diffs);
      yield return new ValueObj { info = item.info, value = res };
    }
  }

  public List<string> getValues() {
    var lst = new List<string>();
    string res = null;
    foreach (var item in items) {
      res = Diff.Merge(res == null ? "" : res, item.diffs);
      lst.Add(res);
    }
    return lst;
  }

  internal class Item {
    public List<Diff.DiffEntry> diffs;
    public TInfo info;
  }

  List<Item> items = new List<Item>();

}

public static class Diff {

  public enum DiffEntryType {
    Remove,
    Add,
    Equal
  };

  public class DiffEntry {
    public DiffEntryType EntryType;
    public string Value;
    public int Count;
  }


  public static string Merge(string orig, IEnumerable<DiffEntry> diff) {
    var sb = new StringBuilder();
    var srcCount = 0;
    foreach (var d in diff) {
      switch (d.EntryType) {
        case DiffEntryType.Equal:
          sb.Append(orig.Substring(srcCount, d.Count));
          srcCount += d.Count;
          break;
        case DiffEntryType.Remove:
          srcCount += d.Count;
          break;
        case DiffEntryType.Add:
          sb.Append(d.Value);
          break;
      }
    };

    sb.Append(orig.Substring(srcCount));

    return sb.ToString();
  }

  public class TestInfo {
    public int count;
  }

  public static void Test() {
    var text1 = "aa bb cc dd aa bb cc dd"; var text2 = " bb xy dd bb xy dd";

    var hist = new DiffHistory<TestInfo>();
    hist.push(text1, new TestInfo { count = 1 });
    hist.push(text2, new TestInfo { count = 2 });
    hist.push(text1, new TestInfo { count = 3 });

    var strs = hist.getValues();
    Debug.Assert(strs[2] == text1 && strs[1] == text2, "Wrong");

    var p = hist.pop();
    Debug.Assert(hist.getValues()[1] == text2, "Wrong");

    p = hist.pop();
    Debug.Assert(hist.getValues()[0] == text1, "Wrong");

    p = hist.pop();
    Debug.Assert(hist.getValues().Count == 0, "Wrong");
  }

  /// <summary>
  /// Creates a list of diff entries that represent the differences between arr1 and arr2.
  /// </summary>
  /// <param name="arr1">Array of units.</param>
  /// <param name="arr2">Array of units.</param>
  /// <returns>List of DiffEntry classes.</returns>
  public static List<DiffEntry> CreateDiff(string arr1, string arr2) {
    if (arr1 == null && arr2 == null) return new List<DiffEntry>();
    if (arr1 == null) arr1 = ""; else if (arr2 == null) arr2 = "";

    int start = 0;
    int end = 0;

    // Strip off the beginning and end, if it's equal
    while (start < Math.Min(arr1.Length, arr2.Length)) {
      if (arr1[start].CompareTo(arr2[start]) != 0)
        break;
      start++;
    }

    if (start == arr1.Length && start == arr2.Length)
      return new List<DiffEntry>();

    for (int i = 0; i < Math.Min(arr1.Length, arr2.Length) - start; i++) {
      if (arr1[arr1.Length - i - 1].CompareTo(arr2[arr2.Length - i - 1]) != 0)
        break;
      end++;
    }

    int lines1_cnt = arr1.Length - start - end;
    int lines2_cnt = arr2.Length - start - end;

    int[,] lcs = new int[lines1_cnt, lines2_cnt];

    // Calculate longest common sequence
    for (int i = 0; i < lines1_cnt; i++) {
      for (int j = 0; j < lines2_cnt; j++) {
        int iVal = i + start;
        int jVal = j + start;

        if (arr1[iVal].CompareTo(arr2[jVal]) != 0) {
          if (i == 0 && j == 0)
            lcs[i, j] = 0;
          else if (i == 0 && j != 0)
            lcs[i, j] = Math.Max(0, lcs[i, j - 1]);
          else if (i != 0 && j == 0)
            lcs[i, j] = Math.Max(lcs[i - 1, j], 0);
          else // if (i != 0 && j != 0)
            lcs[i, j] = Math.Max(lcs[i - 1, j], lcs[i, j - 1]);
        } else {
          if (i == 0 || j == 0)
            lcs[i, j] = 1;
          else
            lcs[i, j] = 1 + lcs[i - 1, j - 1];
        }
      }
    }

    // Build the list of differences
    Stack<int[]> stck = new Stack<int[]>();
    List<DiffEntry> diffList = new List<DiffEntry>();

    stck.Push(new int[2] { lines1_cnt - 1, lines2_cnt - 1 });
    do {
      int[] data = stck.Pop();

      int i = data[0];
      int j = data[1];

      if (i >= 0 && j >= 0 && arr1[i + start].CompareTo(arr2[j + start]) == 0) {
        stck.Push(new int[2] { i - 1, j - 1 });
        addEntry(diffList, DiffEntryType.Equal);
      } else {
        if (j >= 0 && (i <= 0 || j == 0 || lcs[i, j - 1] >= lcs[i - 1, j])) {
          stck.Push(new int[2] { i, j - 1 });
          addEntry(diffList, DiffEntryType.Add, arr2[j + start]);
        } else if (i >= 0 && (j <= 0 || i == 0 || lcs[i, j - 1] < lcs[i - 1, j])) {
          stck.Push(new int[2] { i - 1, j });
          addEntry(diffList, DiffEntryType.Remove);
        }
      }
    } while (stck.Count > 0);

    diffList.Reverse();

    return diffList;
  }

  static void addEntry(List<DiffEntry> diffList, DiffEntryType type, char? value = null) {
    var lastDiff = diffList.Count > 0 ? diffList[diffList.Count - 1] : null;

    if (lastDiff != null && lastDiff.EntryType == type)
      if (type == DiffEntryType.Add) {
        if (lastDiff.Value.Length < 256) {
          lastDiff.Value = value.ToString() + lastDiff.Value;
          return;
        }
      } else {
        if (lastDiff.Count < 256) {
          lastDiff.Count++;
          return;
        }
      }

    diffList.Add(new DiffEntry { EntryType = type, Count = 1, Value = value == null ? null : value.ToString() });
  }

}