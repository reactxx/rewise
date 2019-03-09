using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class MatrixToDart {
  public class BookIn {
    public string name;
    public Meta meta;
    public string srcLang;
  }
  public class Meta {
  }
  public class BookOut: BookIn {
    public List<string> ERRORS_wrong_langs;
    public List<FactOut> façts;
    public int[] lessons;
  }
  public class FactOut {
    public string lang;
    public string[] words;
  }

  const string lessonRowName = "?_Lesson";

  public static BookOut import(string matrixFn, string metaFn, string srcLang /*null => all langs*/) {
    var matrix = new LangMatrix(matrixFn);
    //matrix.save(@"c:\temp\test.csv");
    var meta = metaFn == null ? null : Json.Deserialize<Meta>(metaFn);
    var less = matrix[lessonRowName];
    var bookOut = new BookOut {
      meta = meta,
      srcLang = srcLang,
      façts = new List<FactOut>(),
      lessons = less == null ? null : less.Select(l => int.Parse(l)).ToArray(),
      name = Path.GetFileNameWithoutExtension(matrixFn).ToLower()
    };

    matrix.langs.
      Select((lang, idx) => {
        var wrong = lang.StartsWith("?");
        if (wrong) (bookOut.ERRORS_wrong_langs ?? (bookOut.ERRORS_wrong_langs = new List<string>())).Add(lang);
        return new { lang, words = matrix.data[idx], wrong };
      }).
      Where(r => !r.wrong).
      ForEach(r => bookOut.façts.Add(new FactOut { lang = r.lang, words = r.words  }));
      //Select(r => {
      //  var breakService = Services.getService(r.lang);
      //  var breaks = breakService.wordBreak(r.words);
      //  Debug.Assert(breaks.Length == r.words.Length && (lessArray == null || lessArray.Length == r.words.Length));
      //  return new { r.lang, words = r.words.Select((w,wi) => new { word = w, br = breaks[wi] }).ToArray() };
      //}).ToArray();

    //Parallel.ForEach(beforeBreak, r =>  {
    //  var breakService = Services.getService(r.lang);
    //  var breaks = breakService.wordBreak(r.words);
    //  r.data.Add(breaks);
    //});

    return bookOut;

    //var afterBreak = matrix.langs.
    //  Select((lang, idx) => new { wrong = lang.StartsWith("?"), lang, words = matrix.data[idx] }).
    //  Where(r => !r.wrong).
    //  Select(r => {
    //    var breakService = Services.getService(r.lang);
    //    var breaks = breakService.wordBreak(r.words);
    //    Debug.Assert(breaks.Length == r.words.Length && (lessArray == null || lessArray.Length == r.words.Length));
    //    return new { r.lang, words = r.words.Select((w, wi) => new { word = w, br = breaks[wi] }).ToArray() };
    //  }).ToArray();

    //var wordCount = beforeBreak[0].words.Length;

    //bookOut.façts = Enumerable.Range(0, wordCount).Select(wi => new FactOut {
    //  lessonId = lessArray == null ? (int?)null : lessArray[wi],
    //  sides = beforeBreak.Select(lm => {
    //    var w = lm.words[wi];
    //    int[] breaks = w.br == null || w.br.Count == 0 || (w.br.Count == 1 && w.br[0].Pos == 0 && w.br[0].Len == w.word.Length)
    //    ? null
    //    : w.br.SelectMany(b => Linq.Items(b.Pos, b.Len)).ToArray();
    //    return new FactSideOut { lang = lm.lang, text = lm.words[wi].word, breaks = breaks };
    //  }).ToArray(),
    //}).ToArray();

    //return bookOut;
  }

  //  public static BookOut import(BookIn meta, string[] destLangs, FactIn[] facts) {
  //  return new MatrixToDart.BookOut {
  //    name = meta.name,
  //    meta = meta.meta,
  //    srcLang = meta.srcLang,
  //    //destLangs = destLangs,
  //    façts = FactInParser.parse(meta, facts),
  //  };
  //}
}

//public static class FactInParser {
//  public static MatrixToDart.FactOut[] parse(MatrixToDart.BookIn meta, MatrixToDart.FactIn[] facts) {
//    return null;
//  }
//}
