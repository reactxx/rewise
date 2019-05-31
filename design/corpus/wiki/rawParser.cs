using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WikimediaProcessing;

public static class WikiRawParser {

  public static string[] types;
  public static string[] langs;

  public static void run(string lang) {

    var tls = Directory.EnumerateFiles(Corpus.Dirs.wikiesRaw).
      Where(f => Path.GetExtension(f) == "").
      Select(f => {
        var parts = Path.GetFileNameWithoutExtension(f).Split(new[] { "wi" }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2) return null;
        return new { lang = parts[0], type = "wi" + parts[1] };
      }).
      Where(lt => lt != null).
      ToArray();
    var types = string.Join(",", tls.Select(tl => tl.type).Distinct().OrderBy(s => s).Select(s => $"\"{s}\""));
    var langs = string.Join(",", tls.Select(tl => tl.lang).Distinct().OrderBy(s => s).Select(s => $"\"{s}\""));

    void dumpSection(WikiSection sect) {
    }
    var wm = new Wikimedia("");

    IEnumerable<WikimediaPage> articles = wm.Articles.Where(article => !article.IsDisambiguation && !article.IsRedirect && !article.IsSpecialPage);
  }
}
