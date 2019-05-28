using System.Collections.Generic;
using System.IO;
using System.Linq;

public class WiktLogger {

  public void add(string lang, string cls, string prop, string enumVal) {
    lock (this) {
      addToList(L0123, new[] { lang, cls, prop, enumVal });
      addToList(L012, new[] { lang, cls, prop });
      addToList(L123, new[] { cls, prop, enumVal });
      addToList(L12, new[] { cls, prop });
    }
  }

  void addToList(List<string> list, string[] vals) {
    var s = "";
    list.Add(s);
    for (var i = 0; i < vals.Length; i++) {
      if (vals[i] == null) break;
      if (i > 0) s += "=";
      s += vals[i];
      list.Add(s);
    }
  }

  public void save(string fn) {
    if (L0123.Count == 0) return;
    save(fn + ".0123.txt", L0123);
    save(fn + ".012.txt", L012);
    save(fn + ".123.txt", L123);
    save(fn + ".12.txt", L12);
  }

  void save(string fn, List<string> list) => 
    File.WriteAllLines(fn, list.GroupBy(s => s).OrderBy(g => g.Key).Select(g => $"{g.Key} {g.Count()}"));

  List<string> L0123 = new List<string>();
  List<string> L012 = new List<string>();
  List<string> L123 = new List<string>();
  List<string> L12 = new List<string>();

}


