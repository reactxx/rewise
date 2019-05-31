using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WiktModel;
using static WiktConsts;
using static WiktIdManager;
using static WiktDB;

public static class WiktDumps {

  public static void run() {
    loadData();
    while (true) {
      Console.WriteLine("Press key to continue...");
      Console.ReadKey();
      treeCounts();
      counts();
    }
  }

  public static void counts() {
    // dump count
    var dumpDir = dir.Select((list, low) => new { list, low }).Where(li => li.list.Count() > 1).ToDictionary(li => li.low, li => li.list.Length);
    var lines = dumpDir.Select(kv => {
      decodeLowByte(kv.Key, out string lang, out string classUri);
      return new { lang, classUri, kv.Value };
    }).
    GroupBy(lcv => lcv.classUri).
    SelectMany(g => {
      return g.Select(lcv => $"{lcv.lang}={lcv.classUri}={lcv.Value}").
      Concat(Linq.Items($"**={g.Key}={g.Sum(lcv => lcv.Value)}"));
    }).
    OrderBy(s => s);
    File.WriteAllLines(LowUtilsDirs.logs + "dump-objects-count.txt", lines);
  }

  public static void treeCounts() {
    // dump page tree counts
    IEnumerable<string[]> pageDump(Page p) {
      yield return new[] { "p" };
      if (p.entries == null) { yield return new[] { "p", "a noentry" }; yield break; }
      var ens = p.entries.Length == 1 ? "b entry" : "c entries";
      yield return new[] { "p", ens };
      foreach (var en in p.entries) {
        if (en.otherForm == null) { yield return new[] { "p", ens, "a noform" }; yield break; }
        var fms = en.otherForm.Length == 1 ? "b form" : "c forms";
        yield return new[] { "p", ens, fms };
      }
    }
    var pageParts = new Dictionary<string, int>();
    foreach (var s in AllLangs.SelectMany(lang => getObjs<Page>(lang).
      SelectMany(p => pageDump(p)).
      Select(arr => string.Join("=", arr)).
      SelectMany(l => Linq.Items("**=" + l, lang + "=" + l)))) 
      pageParts[s] = pageParts.TryGetValue(s, out int c) ? c + 1 : 1;
    
    File.WriteAllLines(LowUtilsDirs.logs + "dump-page-parts.txt", pageParts.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key} {kv.Value}"));
  }
}
