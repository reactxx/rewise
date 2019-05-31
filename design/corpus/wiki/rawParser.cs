using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WikimediaProcessing;

public static class WikiRawParser {

  public static void ExtractSections() {
    var stat = WikiRawConsts.loadStat();
    Parallel.ForEach(WikiRawConsts.getRawFiles(WikiRawConsts.wiktionary), new ParallelOptions { MaxDegreeOfParallelism = 6 }, rf => {
      var fn = rf.fileName();
      if (!File.Exists(fn)) return;
      IEnumerable<WikimediaPage> pages = new Wikimedia(fn).Articles.Where(article => !article.IsDisambiguation && !article.IsRedirect && !article.IsSpecialPage);
      var cnt = 0; 
      using (var wr = new JsonStreamWriter(fn + ".sec.json"))
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
  public class Sections {
    public Sections(WikimediaPage page) {
      title = page.Title;
      var sub = page.Sections.GetEnumerator();
      while (sub.MoveNext()) {
        if (subsections == null) subsections = new List<Sections>();
        subsections.Add(new Sections(sub.Current));
      }
    }
    public Sections(WikiSection sec) {
      title = sec.SectionName;
      var sub = sec.SubSections.GetEnumerator();
      while (sub.MoveNext()) {
        if (subsections == null) subsections = new List<Sections>();
        subsections.Add(new Sections(sub.Current));
      }
    }
    public string title;
    public List<Sections> subsections;
  }
}
