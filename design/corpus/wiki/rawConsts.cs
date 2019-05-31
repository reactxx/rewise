using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class WikiRawConsts {

  public class RawFile {
    public string lang;
    public string type;
    public long size;
    public int pages;

    public string fileName() => $"{dirRaw}{lang}{type}";
    public string fileNameDump() => $"{dirRawDump}{lang}{type}";
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


  public static RawFile[] loadStat() => stats ?? (stats = Json.Deserialize<RawFile[]>(statFn));
  public static void saveStat() => Json.Serialize(statFnDesign, stats);
  static RawFile[] stats;
  static string statFn = Directory.GetCurrentDirectory() + @"\wiki\rawConsts.json";
  static string statFnDesign = Corpus.Dirs.driver + @":\rewise\design\corpus\wiki\rawConsts.json";

  public static string dirRaw = Corpus.Dirs.driver + @":\rewise\data\wikies\raw\";
  public static string dirRawDump = Corpus.Dirs.driver + @":\rewise\data\wikies\raw-dump\";

  public static string[] csWordSenses = new[] { "podstatné jméno", "přídavné jméno", "příslovce", "sloveso", "zájmeno", };

  public static IEnumerable<string> getRawFileNames(string type) => getRawFiles(type).Select(s => s.fileName()).Where(fn => File.Exists(fn));
  public static IEnumerable<RawFile> getRawFiles(string type) => loadStat().Where(s => s.type == type && File.Exists(s.fileName()));

  public static void createStat() {
    var allWikiLangs = Wiki.WikiStat.load().Where(l => l.lang != null).ToDictionary(l => l.lang);

    var files = Directory.EnumerateFiles(dirRaw).
      Where(f => Path.GetExtension(f) == "").
      Select(f => {
        var parts = Path.GetFileNameWithoutExtension(f).Split(new[] { "wi" }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2) return null;
        return new { lang = parts[0], type = "wi" + parts[1], size = new FileInfo(f).Length };
      }).
      Where(lt => lt != null).
      Select(lt => new { item = allWikiLangs.TryGetValue(lt.lang, out Wiki.WikiStat it) ? it : null, lt.type, lt.size }).
      Where(lt => lt.item != null).
      Select(lt => new RawFile { lang = lt.item.lang, size = lt.size, type = lt.type }).
      ToArray();
    Json.Serialize(statFnDesign, files);
  }
}
