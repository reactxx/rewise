using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WikimediaProcessing;

public static class WikiRawParser {

  public static void ExtractSections() {
    var wikts = WikiRawConsts.loadStat().SelectMany(st => st.fileName("wiktionary")).Where(f => File.Exists(f));
    Parallel.ForEach(wikts, new ParallelOptions { MaxDegreeOfParallelism = 4 }, fn => {
      IEnumerable<WikimediaPage> pages = new Wikimedia(fn).Articles.Where(article => !article.IsDisambiguation && !article.IsRedirect && !article.IsSpecialPage);
      using (var wr = new JsonStreamWriter(fn + ".sec.json"))
        foreach (var sect in pages.Select(p => new Sections(p))) {
          wr.Serialize(sect);
        }
    });
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
