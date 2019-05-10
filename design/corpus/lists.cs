using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Corpus {

  public static class Lists {

    public static void frekvent() =>
      Parallel.ForEach(Files.langs.Where(l => l != "en-GB"), l => {
        if (l.StartsWith("?")) File.WriteAllText(Dirs.frekvent + "c-" + l.Substring(1) + ".ERROR.txt", "");
        else frekvent(l);
      });

    public static void frekvent(string lang) {
      var words = Files.getLangWords(lang).Select(w => w.ToLower());
      var res = new Dictionary<string, C>();
      foreach (var w in words)
        if (res.TryGetValue(w, out C c)) c.c++; else res[w] = new C { c = 1 };
      File.WriteAllLines(Dirs.frekvent + lang + ".txt", res.OrderByDescending(kv => kv.Value.c).Select(kv => kv.Key + "=" + kv.Value.c));
      File.WriteAllText(Dirs.frekvent + "c-" + lang + "." + res.Count.ToString() + ".txt", "");
    }
    public class C { public int c; }
  }
}