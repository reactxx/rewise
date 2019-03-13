using System.IO;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using RewiseDom;

public static class matrixsToBookOuts {
  const string lessonRowName = "?_Lesson";

  public static Empty run(FileNamesRequest request) {
      foreach (var fns in request.FileNames)
        run(fns.Matrix, fns.Bin);
    return new Empty();
  }

  static void run(string matrixFn, string binFn) {
    var matrix = new LangMatrix(matrixFn);
    //matrix.save(@"c:\temp\test.csv");
    var less = matrix[lessonRowName];
    var bookOut = new BookOut {
      Name = Path.GetFileNameWithoutExtension(matrixFn).ToLower()
    };
    if (less != null)
      bookOut.Lessons.AddRange(less.Select(l => int.Parse(l)));

    matrix.langs.
      Select((lang, idx) => {
        var wrong = lang.StartsWith("?");
        if (wrong) bookOut.ErrorWrongLangs.Add(lang);
        return new { lang, words = matrix.data[idx], wrong };
      }).
      Where(r => !r.wrong).
      ForEach(r => {
        var f = new FactOut { Lang = r.lang };
        f.Words.Add(r.words.Where(w => w != null));
        bookOut.Facts.Add(f);
      });
    if (!Directory.Exists(Path.GetDirectoryName(binFn)))
      Directory.CreateDirectory(Path.GetDirectoryName(binFn));
    File.WriteAllBytes(binFn, Protobuf.ToBytes(bookOut));
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
