using Grpc.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ToRawService : Rw.ToRaw.CSharpService.CSharpServiceBase {

  public override Task<Rw.ToRaw.Response> Run(Rw.ToRaw.Request request, ServerCallContext context) {
    var sb = new StringBuilder();
    foreach (var fns in request.Files) {
      run(fns.Src, fns.Dest, out string err);
      if (err != null) sb.AppendLine(string.Format("Wrong {0} langs in {1}", err, fns.Src));
    }
    var resp = new Rw.ToRaw.Response { Error = sb.Length == 0 ? "" : sb.ToString() };
    return Task.FromResult(resp);
  }
  public override Task<Rw.ToRaw.Response> ToMatrix(Rw.ToRaw.Request request, ServerCallContext context) {
    var sb = new StringBuilder();
    string err = "";
    try {
      foreach (var fns in request.Files) {
        var matrix = new LangMatrix(fns.Src);
        var parts = fns.Dest.Split(new char[] {'\\','.' });

        if (!Directory.Exists(Path.GetDirectoryName(fns.Dest)))
          Directory.CreateDirectory(Path.GetDirectoryName(fns.Dest));
        using (var wr = new StreamWriter(fns.Dest, false, Encoding.UTF8)) matrix.saveRaw(wr);
      }
    } catch (Exception exp) {
      err = exp.ToString();
    }
    return Task.FromResult(new Rw.ToRaw.Response { Error = err });
  }

  static HashSet<string> specColls = new HashSet<string>() { "?-lesson", "?-idg", "?-ide", "?-id" };

  const string lessonRowName = "?_Lesson";

  static void run(string matrixFn, string binFn, out string error) {
    try {
      var matrix = new LangMatrix(matrixFn);
      string err = null;
      //matrix.save(@"c:\temp\test.csv");
      var less = matrix[lessonRowName];
      var bookOut = new Rw.ToParsed.RawBooks {
        Name = Path.GetFileNameWithoutExtension(matrixFn).ToLower()
      };
      if (less != null)
        bookOut.Lessons.AddRange(less.Select(l => int.Parse(l)));

      matrix.langs.
        Select((lang, idx) => {
          var wrong = lang.StartsWith("?");
          if (wrong && !specColls.Contains(lang)) err += (err != null ? ", " : "") + lang;
          return new { lang, words = matrix.data[idx], wrong };
        }).
        Where(r => !r.wrong).
        ForEach(r => {
          var f = new Rw.ToParsed.RawBook { Lang = r.lang };
          f.Facts.Add(r.words.Where(w => w != null).Select(w => w.Replace("@@s", ";").Normalize()));
          bookOut.Books.Add(f);
        });
      if (!Directory.Exists(Path.GetDirectoryName(binFn)))
        Directory.CreateDirectory(Path.GetDirectoryName(binFn));
      File.WriteAllBytes(binFn, Protobuf.ToBytes(bookOut));
      error = err;
    } catch {
      error = "Wrong file: " + matrixFn;
    }
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
