using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WikimediaProcessing;

public static class WikiRawParser {

  public static void ExtractSections() {
    var stat = WikiRawConsts.loadStat();
    Parallel.ForEach(WikiRawConsts.getRawFiles(WikiRawConsts.wiktionary).Where(rf => rf.pages > 5000), new ParallelOptions { MaxDegreeOfParallelism = 6 }, rf => {
      IEnumerable<WikimediaPage> pages = new Wikimedia(rf.fileName()).Articles.Where(article => !article.IsDisambiguation && !article.IsRedirect && !article.IsSpecialPage);
      var cnt = 0;
      using (var wr = new JsonStreamWriter(rf.fileNameDump() + ".sec.json"))
        foreach (var sect in pages.Select(p => new Sections(p))) {
          if (cnt % 100000 == 0) Console.WriteLine($"{rf.lang} {cnt}");
          cnt++;
          wr.Serialize(sect);
        }
      lock (stat) {
        stat.First(s => s.type == WikiRawConsts.wiktionary && s.lang == rf.lang).pages = cnt;
      }
    });
    WikiRawConsts.saveStat();
  }

  public static void SectionStats() {
    Parallel.ForEach(WikiRawConsts.getRawFiles(WikiRawConsts.wiktionary), new ParallelOptions { MaxDegreeOfParallelism = 4 }, rf => {
      var sectStat = new Dictionary<string, int>();
      void add(string l) => sectStat[l] = sectStat.TryGetValue(l, out int c) ? c + 1 : 1;

      using (var rdr = new JsonStreamReader(rf.fileNameDump() + ".sec.json")) {
        foreach (var sect in rdr.Deserialize<Sections>()) {
          if (sectStat.Count > 5000) break;
          foreach (var s in sect.lines(0, "")) add(s);
        }
      }
      File.WriteAllLines(rf.fileNameDump() + ".sec-stat.txt", sectStat.Where(kv => kv.Value >= 10).OrderBy(s => s.Key).Select(s => $"{s.Key} #{s.Value}"));
    });
  }


  public static void CSWordSenses() {
    var fn = WikiRawConsts.loadStat().First(f => f.lang == "cs" && f.type == WikiRawConsts.wiktionary).fileNameDump();
    var names = WikiRawConsts.csWordSenses.ToHashSet();
    var lines = new List<string>();
    using (var rdr = new JsonStreamReader(fn + ".sec.json")) {
      foreach (var sect in rdr.Deserialize<Sections>()) {
        if (sect.subsections == null) continue;
        var cs = sect.subsections.FirstOrDefault(s => s.title == "čeština");
        if (cs == null || cs.subsections == null) continue;
        var senses = cs.subsections.Select(scs => scs.title).Where(s => names.Contains(s)).Distinct().ToArray();
        if (senses.Length == 0) continue;
        lines.Add($"{sect.title}={string.Join(",", senses)}");
      }
    }
    File.WriteAllLines(fn + ".cs-senses.txt", lines.OrderBy(s => s));
  }

  public static void ParseToJson() {
    //var xxx = WikiRawConsts.loadStat().Where(rf => rf.type== WikiRawConsts.wiktionary && rf.pages > 10000).ToArray();
    Parallel.ForEach(WikiRawConsts.getRawFiles(WikiRawConsts.wiktionary).Where(rf => rf.pages > 5000), new ParallelOptions { MaxDegreeOfParallelism = 6 }, rf => {
      //var rf = WikiRawConsts.loadStat().First(f => f.lang == "cs" && f.type == WikiRawConsts.wiktionary);
      IEnumerable<WikimediaPage> pages = new Wikimedia(rf.fileName()).Articles.Where(article => !article.IsDisambiguation && !article.IsRedirect && !article.IsSpecialPage);
      var cnt = 0;
      using (var wr = new JsonStreamWriter(rf.fileNameDump() + ".parsed.json"))
        foreach (var page in pages.Where(p => p.Sections.FirstOrDefault(s => rf.lang != "cs" || s.SectionName.Trim().ToLower() == "čeština") != null)) {
          if (cnt % 10000 == 0) Console.WriteLine($"{rf.lang} {cnt}");
          cnt++;
          page.Text = null;
          wr.Serialize(page);
        }
    });
  }


  public class Sections {
    public Sections() { }
    public Sections(WikimediaPage page) {
      title = page.Title.Trim().ToLower();
      var sub = page.Sections.GetEnumerator();
      while (sub.MoveNext()) {
        if (subsections == null) subsections = new List<Sections>();
        subsections.Add(new Sections(sub.Current));
      }
    }
    public Sections(WikiSection sec) {
      title = sec.SectionName.Trim().ToLower(); ;
      var sub = sec.SubSections.GetEnumerator();
      while (sub.MoveNext()) {
        if (subsections == null) subsections = new List<Sections>();
        subsections.Add(new Sections(sub.Current));
      }
    }
    public string title;
    public List<Sections> subsections;
    public IEnumerable<string> lines(int deep, string line) {
      if (deep == 1 && title != "čeština")
        yield break;
      //if (deep > 2) yield break;
      var tit = title.TrimEnd('(', ')', ' ', '1', '2', '3', '4', '5', '6');
      var subLine = deep == 0 ? line : $"{line}={tit}";
      if (deep > 0) {
        yield return subLine;
      }
      if (subsections == null) yield break;
      foreach (var sect in subsections) {
        foreach (var s in sect.lines(deep + 1, subLine)) yield return s;
      }
    }
  }
}
