﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WiktModel;
using static WiktConsts;
using static WiktDB;
using static WiktIdManager;

public static class WiktDumps {

  public static void run() {
    loadData();
    while (true) {
      Console.WriteLine("Press key to continue...");
      Console.ReadKey();
      if (new DateTime() == null) continue;
      //dumpPageParts();
      //dumpObjectCount();
      //checkTranslationsAndNyms(true, false);
      //checkTranslationsAndNyms(false, false);
      //checkTranslationsAndNyms(true, true);
      //checkTranslationsAndNyms(false, true);
      //checkSense();
      checkTransGlossSense();
    }
  }

  public static void checkSenseInPage() {
    foreach (var page in getObjs<Page>().Where(p => p.entries != null)) {
      var senses = page.entries.Where(en => en.senseIds != null).SelectMany(en => en.senseIds.Select(sid => getObj<Sense>(sid)));
      var senseEntries = senses;
    }
  }

  public static void checkTransGlossSense() {
    var counts = new Dictionary<string, int>();
    void addKey(string key) => counts[key] = counts.TryGetValue(key, out int c) ? c + 1 : 1;

    foreach (var page in getObjs<Page>().Where(p => p.entries != null)) {
      decodeLowByte(page.id, out byte langMask, out byte cls);
      var lang = AllLangs[langMask];
      void addKeys(string key) { /*addKey($"{lang}{key}"); addKey($"**{key}");*/ addKey(key); }

      foreach (var en in page.entries) {
        var senseIds = en.senseIds == null ? new string[0] : en.senseIds.Select(sid => getObj<Sense>(sid).senseNumber).Distinct().OrderBy(s => s).ToArray();
        var tNums = en.translations == null ? new string[0] : en.translations.Where(t => t.glossId != null).
          Select(t => getObj<Gloss>(t.glossId)).
          SelectMany(g => Linq.Items(g.gloss.rank.ToString(), g.gloss.senseNumber)).
          Where(s => !string.IsNullOrEmpty(s) && !senseIds.Contains(s)).
          Distinct().
          OrderBy(s => s).
          ToArray();
        var errors = tNums.Length;
        if (errors == 1 && (en.senseIds == null || en.senseIds.Count == 1)) errors = 0;
        if (errors > 0) {
          addKeys($"{lang} {WiktIdManager.wikionaryPageUrl(page.id)} # {string.Join(" | ", senseIds)}  => {string.Join(" | ", tNums)}");
          //addKeys($"={errors.ToString().PadLeft(2)}");
          //http://kaiko.getalp.org/dbnary/eng/
          //addKeys($"=page");
          continue;
        }
      }
    }

    var lines = counts.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key} #{kv.Value}");
    File.WriteAllLines(LowUtilsDirs.logs + "dump-check-trans-glos-sense.txt", lines);
  }

  public static void checkSense() {
    var counts = new Dictionary<string, int>();

    void addKey(string key) => counts[key] = counts.TryGetValue(key, out int c) ? c + 1 : 1;

    foreach (var page in getObjs<Page>().Where(p => p.entries != null)) {
      decodeLowByte(page.id, out byte langMask, out byte cls);
      var lang = AllLangs[langMask];
      void addKeys(string key) { addKey($"{lang}{key}"); addKey($"**{key}"); }

      foreach (var en in page.entries) {
        addKeys("=entry");
        if (en.senseIds == null) { addKeys("=entry=no-sense"); continue; }
        foreach (var sId in en.senseIds) {
          var sense = getObj<Sense>(sId);
          addKeys("=entry=sense");
          if (sense.senseNumber != null) addKeys("=entry=sense=senseNumber");
        }
      }
    }

    var lines = counts.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key} {kv.Value}");
    File.WriteAllLines(LowUtilsDirs.logs + "dump-check-sense.txt", lines);
  }
  public static void checkTranslationsAndNyms(bool sumOf, bool isNyms = false) {

    var counts = new Dictionary<string, int>();

    void add(string name, int id, List<TranslationData> trans, List<NymRel> nyms) {
      decodeLowByte(id, out byte langMask, out byte cls);
      var lang = AllLangs[langMask];

      void addKey(string key, int cnt) => counts[key] = (counts.TryGetValue(key, out int c) ? c : 0) + (sumOf ? cnt : 1);
      void addKeys(int cnt, string subKey = "") {
        addKey($"{lang}={name}{subKey}", cnt);
        addKey($"**={name}{subKey}", cnt);
      }

      if (isNyms) {
        addKeys(nyms == null ? 0 : nyms.Count);
      } else {
        addKeys(trans == null ? 0 : trans.Count);
        if (trans == null) return;
        if (sumOf) foreach (var tr in trans) {
            if (tr.glossId != null) {
              var gloss = getObj<Gloss>(tr.glossId);
              addKeys(1, "=gloss");
              if (gloss.gloss.rank != null)
                addKeys(1, "=gloss=rank");
              if (gloss.gloss.senseNumber != null)
                addKeys(1, "=gloss=senseNumber");
            }
          }
      }
    }

    foreach (var page in getObjs<Page>()) {
      add("page", page.id, page.translations, page.nyms);
      if (page.entries == null) add("entry", page.id, null, null);
      else foreach (var en in page.entries) add("entry", en.id, en.translations, en.nyms);
    }
    foreach (var sens in getObjs<Sense>()) add("sense", sens.id, sens.translations, sens.nyms);

    var lines = counts.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key} {kv.Value}");

    var fn = sumOf ? (isNyms ? "nym-num" : "trans-num") : (isNyms ? "contains-nym-num" : "contains-trans-num");

    File.WriteAllLines(LowUtilsDirs.logs + "dump-" + fn + ".txt", lines);
  }

  public static void dumpObjectCount() {
    // dump count
    var lowByteToCount = database.Select((list, low) => new { list, low }).Where(li => li.list.Count() > 1).ToDictionary(li => li.low, li => li.list.Length);
    var lines = lowByteToCount.Select(kv => {
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

  public static void dumpPageParts() {
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
    foreach (var s in AllLangs.SelectMany(lang => getObjsStr<Page>(lang).
      SelectMany(p => pageDump(p)).
      Select(arr => string.Join("=", arr)).
      SelectMany(l => Linq.Items("**=" + l, lang + "=" + l))))
      pageParts[s] = pageParts.TryGetValue(s, out int c) ? c + 1 : 1;

    File.WriteAllLines(LowUtilsDirs.logs + "dump-page-parts.txt", pageParts.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key} {kv.Value}"));
  }
}