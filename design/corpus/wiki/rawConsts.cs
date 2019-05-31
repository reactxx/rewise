using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WikimediaProcessing;

public class WikiRawConsts {

  public class Files {
    public string lang;
    public string[] types;
    public long[] sizes;

    public IEnumerable<string> fileName(string type = null) => types.Where(t => type == null || type == t).Select(t => $"{Corpus.Dirs.wikiesRaw}{lang}{t}");
  }

  public const string wiki = "wiki";
  public const string wikibooks = "wikibooks";
  public const string wikimedia = "wikimedia";
  public const string wikinews = "wikinews";
  public const string wikiquote = "wikiquote";
  public const string wikisource = "wikisource";
  public const string wikiversity = "wikiversity";
  public const string wikivoyage = "wikivoyage";
  public const string wiktionary = "wiktionary";

  public static string[] allTypes = new[] { wiki, wikibooks, wikimedia, wikinews, wikiquote, wikisource, wikiversity, wikivoyage, wiktionary };

  public static Files[] loadStat() => stats ?? (stats = Json.Deserialize<Files[]>(Directory.GetCurrentDirectory() + @"\wiki\rawParser.json"));
  static Files[] stats;

  public static void createStat() {
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
    var items = tls.GroupBy(tl => tl.item.lang).Select(g => new Files {
      lang = g.First().item.lang,
      types = g.Select(it => it.type).ToArray(),
      sizes = g.Select(it => it.size).ToArray(),
    }).ToArray();
    Json.Serialize(@"d:\rewise\design\corpus\wiki\rawParser.json", items);
  }
}
