using System;
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
      all();
      Console.WriteLine("Press key to continue...");
      Console.ReadKey();
      //if (new DateTime() == null) continue;
      //dumpPageParts();
      //dumpObjectCount();
      //checkTranslationsAndNyms(true, false);
      //checkTranslationsAndNyms(false, false);
      //checkTranslationsAndNyms(true, true);
      //checkTranslationsAndNyms(false, true);
      //checkSense();
      //countTrans();
      //countTransByLang(true);
      countTransByLang(false);
      //countTrans2(true);
      //break;
    }
  }

  public static void all() {
    loadData();
    foreach (var lang in AllLangs) {
      File.WriteAllLines(@"d:\rewise\data\wiktionary\dbnary\dumps\" + lang + ".txt", 
        getObjsStr<Entry>(lang).Select(f => f.toString()));
    }
  }
  public static void countTransByLang(bool fromLang) {
    var counts = new Dictionary<string, int>();
    void addKey(string key, int cnt) => counts[key] = (counts.TryGetValue(key, out int c) ? c : 0) + cnt;
    foreach (var page in getObjs<Page>()) {
      decodeLowByte(page.id, out byte langMask, out byte cls);
      var lang = AllLangs[langMask];
      void addKeys(string subKey, List<TranslationData> translations) {
        if (translations == null) return;
        foreach (var t in translations) {
          //addKey($"{t.targetLanguage}={lang}={subKey}", 1);
          //addKey($"{t.targetLanguage}=**={subKey}", 1);
          //addKey($"{t.targetLanguage}={lang}=**", 1);
          //addKey($"{t.targetLanguage}=**=**", 1);
          if (fromLang)
            addKey($"{lang} => {t.targetLanguage}", 1);
          else
            addKey($"{t.targetLanguage} <= {lang}", 1);
        }
      }

      addKeys("page", page.translations);
      if (page.entries != null) foreach (var e in page.entries) {
          addKeys("entry", e.translations);
          if (e.senses != null) foreach (var s in e.senses)
              addKeys("sense", s.translations);
        }
    }

    var lines = counts.Where(kv => kv.Value >= 1000).OrderBy(kv => kv.Key).Select(kv => $"{kv.Key} {kv.Value}");
    File.WriteAllLines(LowUtilsDirs.logs + "dump-trans-" + (fromLang ? "from" : "to") + "-lang.txt", lines);
  }

  public static void countTrans() {
    var counts = new Dictionary<string, int>();
    void addKey(string key, int cnt) => counts[key] = (counts.TryGetValue(key, out int c) ? c : 0) + cnt;

    foreach (var page in getObjs<Page>()) {
      decodeLowByte(page.id, out byte langMask, out byte cls);
      var lang = AllLangs[langMask];
      void addKeys(string subKey = "") {
        addKey($"{lang}={subKey}", 1);
        addKey($"**={subKey}", 1);
      }

      var pt = page.translations == null ? "pt0-" : "pt+-";
      if (page.entries == null) {
        addKeys("p-e0");
        addKeys(pt + "e0");
      } else {
        var e1 = page.entries.Length == 1;
        var e_ = e1 ? "e1" : "e*";
        addKeys("p-" + e_);
        if (e1) {
          var e_0 = page.entries[0];
          addKeys(e_0.senses == null ? "p-e1-s0" : (e_0.senses.Length == 1 ? "p-e1-s1" : "p-e1-s*"));
        }

        foreach (var e in page.entries) {
          addKeys(e_);
          var et = e_ + (e.translations == null ? "t0-" : "t+-");
          if (e.senses == null) {
            addKeys(e_ + "-s0");
            addKeys(pt + et + "s0");
          } else {
            var s1 = e.senses.Length == 1;
            var s_ = s1 ? "s1" : "s*";
            addKeys(e_ + "-" + s_);
            foreach (var s in e.senses) {
              addKeys(s_);
              var st = s_ + (s.translations == null ? "t0" : "t+");
              addKeys(st);
              addKeys(pt + et + st);
            }
          }
        }
      }


    }

    var lines = counts.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key} {kv.Value}");
    File.WriteAllLines(LowUtilsDirs.logs + "dump-trans-count.txt", lines);
  }

  public static void checkTransGlossSense() {
    var counts = new Dictionary<string, int>();
    //void addKeys(string key, int count = 1) => counts[key] = counts.TryGetValue(key, out int c) ? c + count : count;

    foreach (var page in getObjs<Page>().Where(p => p.entries != null)) {
      decodeLowByte(page.id, out byte langMask, out byte cls);
      var lang = AllLangs[langMask];

      //IEnumerable<string> expandLow(string str) {
      //  var parts = str.Split(new[] { '-', '\u2013', '\u2014' }, StringSplitOptions.RemoveEmptyEntries).
      //    Select(s => s.Trim('a', 'b', 'c', 'd', 'e', 'f', 'g')).ToArray();

      //  switch (parts.Length) {
      //    case 1: yield return parts[0]; break;
      //    case 2:
      //      if (int.TryParse(parts[0], out int l) && int.TryParse(parts[1], out int r)) {
      //        for (var i = l; i <= r; i++) yield return i.ToString();
      //      } else yield return str;
      //      break;
      //    default:
      //      yield return str;
      //      break;
      //  }
      //}

      //IEnumerable<string> expand(string str) =>
      //  str == null ? Enumerable.Empty<string>() : str.ToLower().Split(new[] { "sens général", "0", "[", "]", " ", ",", " a", " e", "ou", "&", "et", ")", "/" }, StringSplitOptions.RemoveEmptyEntries).
      //  SelectMany(s => expandLow(s)).Select(s => s.Trim('a', 'b', 'c', 'd', 'e', 'f', 'g', '-', '\u2013', '\u2014'));

      //foreach (var en in page.entries) {
      //  if (en.translationGlosses == null) continue;

      //  var inSense = en.senses == null ? new string[0] : en.senses.Select(s => s.senseNumber).
      //    SelectMany(s => expand(s)).Distinct().OrderBy(s => s).ToArray();

      //  var inTrans = en.translationGlosses.SelectMany(g => Linq.Items(g.gloss.rank.ToString(), g.gloss.senseNumber)).
      //    SelectMany(s => expand(s)).Distinct().OrderBy(s => s).ToArray();
      //  if (inTrans.Length == 0) continue;

      //  var notFound = inTrans.Except(inSense).ToArray();

      //  if (notFound.Length == 0) {
      //    addKeys($"{lang} 1 OK");
      //    addKeys($"{lang} 2 ok", inTrans.Length);
      //  } else {
      //    addKeys($"{lang} 1 WRONG");
      //    addKeys($"{lang} 2 wrong", notFound.Length);
      //    addKeys($"{lang}## '{string.Join("*", notFound)}' #=# '{string.Join("*", inTrans)}' #-# '{string.Join("*", inSense)}'");
      //  }
      //  //WiktIdManager.wikionaryPageUrl(page.id)

      //  //var errors = missing.Length;
      //  //if (errors == 1 && (en.senses == null || en.senses.Length == 1)) errors = 0;
      //  //if (errors > 0) {
      //  //  addKeys($"{lang} {WiktIdManager.wikionaryPageUrl(page.id)} # {string.Join(" | ", senseIds)}  => {string.Join(" | ", missing)}");
      //  //  //addKeys($"={errors.ToString().PadLeft(2)}");
      //  //  //http://kaiko.getalp.org/dbnary/eng/
      //  //  //addKeys($"=page");
      //  //  continue;
      //  //}
      //}
    }

    var lines = counts.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key}                  {kv.Value}");
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
        if (en.senses == null) { addKeys("=entry=no-sense"); continue; }
        foreach (var sense in en.senses) {
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

    //void add(string name, int id, List<TranslationData> trans, List<NymRel> nyms) {
    //  decodeLowByte(id, out byte langMask, out byte cls);
    //  var lang = AllLangs[langMask];

    //  void addKey(string key, int cnt) => counts[key] = (counts.TryGetValue(key, out int c) ? c : 0) + (sumOf ? cnt : 1);
    //  void addKeys(int cnt, string subKey = "") {
    //    addKey($"{lang}={name}{subKey}", cnt);
    //    addKey($"**={name}{subKey}", cnt);
    //  }

    //  if (isNyms) {
    //    addKeys(nyms == null ? 0 : nyms.Count);
    //  } else {
    //    addKeys(trans == null ? 0 : trans.Count);
    //    if (trans == null) return;
    //    //if (sumOf) foreach (var tr in trans) {
    //    //    if (tr.glossId != null) {
    //    //      var gloss = getObj<Gloss>(tr.glossId);
    //    //      addKeys(1, "=gloss");
    //    //      if (gloss.gloss.rank != null)
    //    //        addKeys(1, "=gloss=rank");
    //    //      if (gloss.gloss.senseNumber != null)
    //    //        addKeys(1, "=gloss=senseNumber");
    //    //    }
    //    //  }
    //  }
    //}

    //foreach (var page in getObjs<Page>()) {
    //  add("page", page.id, page.translations, page.nyms);
    //  if (page.entries == null) add("entry", page.id, null, null);
    //  else foreach (var en in page.entries) {
    //      add("entry", en.id, en.translations, en.nyms);
    //      if (en.translationGlosses != null) foreach (var gl in en.translationGlosses) {
    //          add("entry:gloss", en.id, gl.gloss.translations, null);
    //        }
    //    }
    //}
    //foreach (var sens in getObjs<Sense>()) add("sense", sens.id, sens.translations, sens.sense.nyms);

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
