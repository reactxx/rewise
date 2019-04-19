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
    foreach (var fns in request.Files) {
      try {
        var matrix = new LangMatrix(fns.Src);
        if (!Directory.Exists(Path.GetDirectoryName(fns.Dest)))
          Directory.CreateDirectory(Path.GetDirectoryName(fns.Dest));
        using (var wr = new StreamWriter(fns.Dest, false, Encoding.UTF8)) matrix.saveRaw(wr, "\t");
      } catch {
        sb.AppendLine(fns.Src);
      }
    }
    return Task.FromResult(new Rw.ToRaw.Response { Error = sb.ToString() });
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

}

