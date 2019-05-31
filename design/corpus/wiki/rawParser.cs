using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WikimediaProcessing;

public static class WikiRawParser {

  public class WikiItem {
    public string lang;
    public string[] types;
    public long[] sizes;
  }

  public static string[] types;
  public static string[] langs;

  public static void run(string lang) {

    var allWikiLangs = Wiki.WikiStat.load().Where(l => l.lang != null).ToDictionary(l => l.lang);

    var tls = Directory.EnumerateFiles(Corpus.Dirs.wikiesRaw).
      Where(f => Path.GetExtension(f) == "").
      Select(f => {
        var parts = Path.GetFileNameWithoutExtension(f).Split(new[] { "wi" }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2) return null;
        return new { lang = parts[0], type = "wi" + parts[1], size = new FileInfo(f).Length };
      }).
      Where(lt => lt != null).
      Select(lt => new { item = allWikiLangs.TryGetValue(lt.lang, out Wiki.WikiStat it) ? it : null, lt.type, lt.size }).
      Where(lt => lt.item != null).
      ToArray();
    var types = string.Join(",", tls.Select(tl => tl.type).Distinct().OrderBy(s => s).Select(s => $"\"{s}\""));
    var items = tls.GroupBy(tl => tl.item.lang).Select(g => new WikiItem {
      lang = g.First().item.lang,
      types = g.Select(it => it.type).ToArray(),
      sizes = g.Select(it => it.size).ToArray(),
    }).ToArray();
    Json.Serialize(@"d:\rewise\design\corpus\wiki\rawParser.json", items);

    void dumpSection(WikiSection sect) {
    }
    var wm = new Wikimedia("");

    IEnumerable<WikimediaPage> articles = wm.Articles.Where(article => !article.IsDisambiguation && !article.IsRedirect && !article.IsSpecialPage);
  }
}
