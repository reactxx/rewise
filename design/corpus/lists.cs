﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Corpus {

  // wiki: 
  public static class Lists {

    public static void frekvent() =>
      Parallel.ForEach(Files.langs/*.Where(l => l != "en-GB" && l != "fr-FR")*/, new ParallelOptions { MaxDegreeOfParallelism = 4 }, l => {
        if (l.StartsWith("?")) File.WriteAllText(Dirs.frekvent + "ERROR." + l.Substring(1) + ".txt", "");
        else frekvent(l);
      });

    public static void frekvent(string lang) {
      // breaking
      var words = Files.getLangWords(lang).Select(w => w.ToLower());
      var res = new Dictionary<string, C>();
      foreach (var w in words)
        if (res.TryGetValue(w, out C c)) c.c++; else res[w] = new C { c = 1 };
      // stemming
      foreach (var w in res.Keys) {
      }
      var st = new CountIntervals.BoundStatus();
      File.WriteAllLines(Dirs.frekvent + lang + ".txt", res.OrderByDescending(kv => kv.Value.c).SelectMany(kv => CountIntervals.writeBound(kv.Key, kv.Value.c, st)));
      File.WriteAllLines(Dirs.frekvent + res.Count.ToString().PadLeft(7, '0') + "." + lang + ".txt", st.dump());
    }
    public class C { public int c; }
  }
}

