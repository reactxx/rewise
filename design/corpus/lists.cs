using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Corpus {

  public static class Lists {
    public static void frekvent(string lang) {
      var words = Files.getLangWords(lang).Select(w => w.ToLower());
      var res = new Dictionary<string, C>();
      foreach (var w in words)
        if (res.TryGetValue(w, out C c)) c.c++; else res[w] = new C { c = 1 };
      File.WriteAllLines(Dirs.frekvent + lang + ".txt", res.OrderByDescending(kv => kv.Value.c).Select(kv => kv.Key + "=" + kv.Value.c));
    }
    public class C { public int c; }
  }
}