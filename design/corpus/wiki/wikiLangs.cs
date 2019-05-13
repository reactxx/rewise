using Sepia.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;

// https://en.wikipedia.org/wiki/Wikipedia:Language_recognition_chart
// https://archive.codeplex.com/?p=ntextcat
public static class WikiLangs {
  public static void Build() {
    var langs = Corpus.DownloadWikies.getUrls().Where(u => u.size > /*1000000*/0).Select(u => u.name.Split(new string[] { "wi" }, StringSplitOptions.RemoveEmptyEntries)[0]).Distinct().ToArray();
    var lmLangs = Langs.meta.Select(l => l.Lang).Distinct().ToArray();
    var notInWiki = lmLangs.Except(langs).ToArray();
    var validLangs = langs.Where(l => LocaleIdentifier.TryParse(l, out LocaleIdentifier li)).ToArray();
    var wikiLocs = validLangs.Select(l => LocaleIdentifier.Parse(l).MostLikelySubtags().ToString()).ToArray();
    var oks = wikiLocs.
      Select(loc => Langs.fullNameToMeta.TryGetValue(loc.ToString(), out Langs.CldrLang cl) ? cl : null).
      NotNulls().
      ToArray();
    var wrongs = wikiLocs.
      Select(loc => Langs.fullNameToMeta.TryGetValue(loc.ToString(), out Langs.CldrLang cl) ? null : loc).
      NotNulls().
      ToArray();
    //ALPHAs
    // from clibs\utils\unicode\unicodeBlocks.json
    //Armi (http://zuga.net/articles/unicode/script/imperial-aramaic/) and Goth (? https://en.wikipedia.org/wiki/Gothic_alphabet) missing
    var alphas = new HashSet<String> { "Latn", "Zyyy", "Grek", "Copt", "Cyrl", "Armn", "Hebr", "Arab", "Syrc", "Thaa", "Nkoo", "Samr", "Mand", "Deva", "Beng", "Guru", "Gujr", "Orya", "Taml", "Telu", "Knda", "Mlym", "Sinh", "Thai", "Laoo", "Tibt", "Mymr", "Geor", "Hang", "Ethi", "Cher", "Cans", "Ogam", "Runr", "Tglg", "Hano", "Buhd", "Tagb", "Khmr", "Mong", "Limb", "Tale", "Talu", "Bugi", "Lana", "Bali", "Sund", "Batk", "Lepc", "Olck", "Glag", "Tfng", "Hira", "Kana", "Bopo", "Hani", "Yiii", "Lisu", "Vaii", "Bamu", "Sylo", "Phag", "Saur", "Kali", "Rjng", "Java", "Cham", "Tavt", "Mtei" };
    var wrongAlphas = wrongs.Where(l => !alphas.Contains(LocaleIdentifier.Parse(l).Script)).ToArray();

    //var path = LangsDesignDirs.cldrRepo;


  }
}

