using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public static class Pom {
  public static void run() {
    //var langs = Langs.meta.Where(m => m.StemmerClass!=null).Select(m => m.Id).ToArray();
    var langs = new string[] { "cs-CZ" };
    foreach (var lang in langs) {
      //Parallel.ForEach(langs, lang => {
      var fn = string.Format(@"c:\temp\wrongWords\wrong.{0}.txt", lang);
      if (!File.Exists(fn)) return;
      var meta = Langs.nameToMeta[lang];
      var chars = new HashSet<char>();
      var words = File.ReadAllText(fn).Split(',');
      foreach (var word in words) {
        var stemms = StemmerBreakerNew.Service.getWordStemms(lang, word);
        if (stemms.Count <= 1) continue;
        foreach (var ch in word) chars.Add(ch);
      }
      var allChars = new string(chars.OrderBy(ch => ch).ToArray());
      string addInChars = "";
      if (meta.Alphabet != null) {
        var alphaChars = (meta.Alphabet + meta.AlphabetUpper).ToHashSet();
        addInChars = new string(alphaChars.Except(chars).ToArray());
      }
      if (!string.IsNullOrEmpty(addInChars))
        File.WriteAllText(Path.ChangeExtension(fn, ".log"), addInChars);
    }
    //});
  }


  //var lang = "en-GB";
  //foreach (var txt in new string[] { "flowers'", "flowers’" }) {
  //  var res = StemmerBreakerNew.Service.wordBreak(lang, new List<String>() { txt });
  //  var words = res[0].Select(p => txt.Substring(p.Pos, p.Len)).ToArray();
  //  foreach (var w in words) {
  //    var res2 = StemmerBreakerNew.Service.getWordStemms(lang, w).JoinStrings(">");
  //    res2 = null;
  //  }
  //}
  //lang = null;

}
