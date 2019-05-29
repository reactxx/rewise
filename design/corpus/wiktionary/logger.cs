using System.Collections.Generic;
using System.IO;
using System.Linq;

public class WiktLogger {

  public void add(string lang, string cls, string prop, string enumVal) {
    lock (this) {
      addToList(D123, new[] { lang, cls, prop, enumVal });
      addToList(D12, new[] { lang, cls, prop });
    }
  }

  void addToList(Dictionary<string, int> list, string[] vals) {
    void add(string str) => list[str] = list.TryGetValue(str, out int c) ? c + 1 : 1;

    var s = ""; var t = "";
    add(s);
    for (var i = 0; i < vals.Length; i++) {
      if (vals[i] == null) break;
      if (i > 0) { s += "="; t += "="; }
      s += vals[i]; t += i == 0 ? "**" : vals[i];
      add(s); add(t);
    }
  }

  public void save(string fn) {
    if (D123.Count() == 0) return;
    save(fn + ".123.txt", D123);
    save(fn + ".12.txt", D12);
  }

  void save(string fn, Dictionary<string, int> list) =>
    File.WriteAllLines(fn, list.OrderBy(g => g.Key).Select(g => $"{g.Key} {g.Value}"));

  Dictionary<string, int> D123 = new Dictionary<string, int>();
  Dictionary<string, int> D12 = new Dictionary<string, int>();
}


