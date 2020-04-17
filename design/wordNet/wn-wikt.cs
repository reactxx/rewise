using EntityFramework.BulkInsert;
using EntityFramework.BulkInsert.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using wordNet;

public static class WnWikt {
  public class Entry : IEqualityComparer<Entry> {
    public int Id { get; set; }
    public bool OriginNoWikt { get; set; }
    public string LangId { get; set; }
    public string PartOfSpeech { get; set; }
    public string Lemma { get; set; }
    public bool Equals(Entry x, Entry y) {
      return x.LangId == y.LangId && x.PartOfSpeech == y.PartOfSpeech && x.Lemma == y.Lemma;
    }
    public int GetHashCode(Entry obj) {
      return obj.LangId.GetHashCode() ^ obj.PartOfSpeech.GetHashCode() ^ obj.Lemma.GetHashCode();
    }
    public wordNetDB.Entry createDB() => new wordNetDB.Entry { Id = Id, PartOfSpeech = PartOfSpeech, Lemma = Lemma, LangId = LangId, OriginNoWikt = false };
  }
  public class Translation : IEqualityComparer<Translation> {
    public int EngSynsetId { get; set; }
    public int TransEntryId { get; set; }
    public string LangId { get; set; }
    public bool OriginNoWikt { get; set; }
    public bool Equals(Translation x, Translation y) => x.LangId == y.LangId && x.EngSynsetId == y.EngSynsetId && x.TransEntryId == y.TransEntryId;
    public int GetHashCode(Translation obj) => obj.LangId.GetHashCode() ^ obj.EngSynsetId.GetHashCode() ^ obj.TransEntryId.GetHashCode();
    public wordNetDB.Translation createDB() => new wordNetDB.Translation { LangId = LangId, TransEntryId = TransEntryId, EngSynsetId = EngSynsetId, OriginNoWikt = false };

  }
  public class Lang {
    public string Id { get; set; }
    public bool OriginNoWikt { get; set; }
    public wordNetDB.Lang createDB() => new wordNetDB.Lang { Id = Id, OriginNoWikt = false };
  }
  public class Tab {
    public string lang;
    public string synsetId;
    public string lemma;
  }
  public class NewSources {
    public List<Lang> langs { get; set; } = new List<Lang>();
    public List<Entry> entries { get; set; } = new List<Entry>();
    public List<Translation> translations { get; set; } = new List<Translation>();
    public List<string> ids { get; set; } = new List<string>();
  }

  public static void run() {
    Console.WriteLine("wn-wikt START");
    var news = createNewSource();
    Json.Serialize(Context.root + "wn-wikt.json", news);
    Console.WriteLine("wn-wikt bulk insert");
    using (var dbCtx = wordNetDB.Context.getContext()) {
      var opt = new BulkInsertOptions() {
        BulkCopyOptions = BulkCopyOptions.TableLock,
        BatchSize = 50000,
      };
      var entries = news.entries.Select(en => en.createDB()).OfType<wordNetDB.Entry>().ToArray();
      var trans = news.translations.Select(en => en.createDB()).OfType<wordNetDB.Translation>().ToArray();
      var langs = news.langs.Select(en => en.createDB()).OfType<wordNetDB.Lang>().ToArray();

      dbCtx.BulkInsert(langs);
      Console.WriteLine("Langs inserted");
      dbCtx.BulkInsert(entries);
      Console.WriteLine("Entries inserted");
      dbCtx.BulkInsert(trans);
      Console.WriteLine("Translation inserted");
    }
    Console.WriteLine("wn-wikt END");
  }
  public static NewSources createNewSource() {
    var news = new NewSources();
    var ctx = new Context(false);
    var maxId = ctx.ids.Where(kv => !kv.Key.StartsWith("wn-wikt-")).Select(kv => int.Parse(kv.Value.Split('=')[2])).Max();
    Tuple<int, string> addId(string lang) {
      maxId++;
      var val = $"wn-wikt-{maxId}={lang}=entry={maxId}";
      news.ids.Add(val);
      return new Tuple<int, string>(maxId, val);
    }

    using (var dbCtx = wordNetDB.Context.getContext()) {
      var synsets = dbCtx.Synsets.Where(s => s.LangId == "eng")
        .Select(s => new { s.Id, partOfSpeech = s.Senses.FirstOrDefault().Entry.PartOfSpeech, /*entryIds = s.Senses.Select(ss => ss.EntryId)*/ })
        .ToArray()
        .ToDictionary(s => ctx.getOrigId(s.Id));
      var entries = dbCtx.Entries.Where(e => e.OriginNoWikt)
        .Select(e => new Entry { Id = e.Id, LangId = e.LangId, PartOfSpeech = e.PartOfSpeech, Lemma = e.Lemma, OriginNoWikt = e.OriginNoWikt })
        .ToHashSet(new Entry());
      var translations = dbCtx.Translations.Where(e => e.OriginNoWikt)
        .Select(e => new Translation { LangId = e.LangId, TransEntryId = e.TransEntryId, EngSynsetId = e.EngSynsetId, OriginNoWikt = e.OriginNoWikt })
        .ToHashSet(new Translation());
      var langs = dbCtx.Langs.Where(e => e.OriginNoWikt)
        .Select(e => new Lang { Id = e.Id, OriginNoWikt = e.OriginNoWikt })
        .ToDictionary(e => e.Id);
      foreach (var tab in parseTabFiles()) {
        if (tab.lemma == "přístup") {
          if (tab == null) continue;
        }
        // ****** preparing and checking
        var id = $"eng-10-{tab.synsetId}";
        if (!synsets.TryGetValue(id, out var synset)) {
          // some lemmas from CLDR, e.g. "dop." or "dop." for czech;
          // Console.WriteLine(id);
          continue;
        }
        if (!tab.synsetId.EndsWith(synset.partOfSpeech)) {
          Console.WriteLine($"{tab.synsetId} does not ends with {synset.partOfSpeech}");
          continue;
        }
        if (!langs.ContainsKey(tab.lang)) {
          news.langs.Add(langs[tab.lang] = new Lang { Id = tab.lang, OriginNoWikt = false });
        }
        // ************* adding Entry, Translation and id
        var newEntry = new Entry { LangId = tab.lang, PartOfSpeech = synset.partOfSpeech, Lemma = tab.lemma, OriginNoWikt = false };
        var newTranslation = new Translation { LangId = tab.lang, EngSynsetId = synset.Id };
        if (!entries.TryGetValue(newEntry, out var origEntry)) {
          var newId = addId(tab.lang);
          newEntry.Id = newId.Item1;
          news.entries.Add(newEntry);
          Debug.Assert(entries.Add(newEntry));
          newTranslation.TransEntryId = newId.Item1;
          news.translations.Add(newTranslation);
          Debug.Assert(translations.Add(newTranslation));
        } else {
          newTranslation.TransEntryId = origEntry.Id;
          if (translations.Contains(newTranslation)) continue;
          news.translations.Add(newTranslation);
          Debug.Assert(translations.Add(newTranslation));
        }
      }
    }
    return news;
  }
  public static IEnumerable<Tab> parseTabFiles() {
    foreach (var fn in Directory.EnumerateFiles(Context.root + @"wn-wikt", "*.tab")) {
      Console.WriteLine(fn);
      var lang = fn.Split('-')[3].Split('.')[0];
      //if (lang != "ces") continue;
      if (lang.StartsWith("_")) continue;
      foreach (var line in File.ReadAllLines(fn)) {
        if (String.IsNullOrEmpty(line) || line[0] == '#') continue;
        var parts = line.Split('\t');
        yield return new Tab { lang = lang, synsetId = parts[0], lemma = parts[2] };
      }
    }
  }
}
