using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WikimediaProcessing;

public static class WikiRawParser {

  public static void ExtractSections() {
    var stat = WikiRawConsts.loadStat();
    Parallel.ForEach(WikiRawConsts.getRawFiles(WikiRawConsts.wiktionary), new ParallelOptions { MaxDegreeOfParallelism = 4 }, rf => {
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

      using (var wr = new JsonStreamReader(rf.fileNameDump() + ".sec.json")) {
        foreach (var sect in wr.Deserialize<Sections>()) {
          if (sectStat.Count > 5000) break;
          foreach (var s in sect.lines(0, "")) add(s);
        }
      }
      File.WriteAllLines(rf.fileNameDump() + ".sec-stat.txt", sectStat.Where(kv => kv.Value>=10).OrderBy(s => s.Key).Select(s => $"{s.Key} #{s.Value}"));
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
