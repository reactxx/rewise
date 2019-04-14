using Grpc.Core;
using StemmerBreakerNew;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public class WordBreakingService : Rw.WordBreaking.CSharpService.CSharpServiceBase {

  public override Task<Rw.WordBreaking.Response2> Run2(Rw.WordBreaking.Request2 req, ServerCallContext context) {
    var sources = req.Facts.Select(f => f.Text.Normalize()).ToList();
    var breaks = Service.wordBreak(req.Lang, sources);
    Debug.Assert(breaks.Length == req.Facts.Count);
    var res = new Rw.WordBreaking.Response2();
    for (var i = 0; i < req.Facts.Count; i++) {
      //if (sources[i]!= req.Facts[i].Text)
      //  Console.Write(sources[i] + "!=" + req.Facts[i].Text);
      var f = new Rw.WordBreaking.FactResp { Id = req.Facts[i].Id, Text = sources[i] };
      f.PosLens.AddRange(breaks[i].Where(pl => noEmoji(sources[i].Substring(pl.Pos, pl.Len))).Select(pl => new Rw.WordBreaking.PosLen { Pos = pl.Pos, End = pl.Pos + pl.Len }));
      //if (f.Text.EndsWith("start")) {
      //  if (f.Text == null) return null;
      //}
      //return new Rw.WordBreaking.PosLen { Pos = pl.Pos, End = pl.Pos + pl.Len };
      //}));
      res.Facts.Add(f);
    }
    return Task.FromResult(res);
  }

  static bool noEmoji(string s) { return s.All(ch => !emojiChars.Contains(ch)); }
  static HashSet<char> emojiChars = new HashSet<char>() { '|', '^', ',', '(', ')', '[', ']', '{', '}', ':', ';' };

  public override Task<Rw.WordBreaking.Response> Run(Rw.WordBreaking.Request req, ServerCallContext context) {
    var breaks = Service.wordBreak(req.Lang, req.Facts);
    Debug.Assert(breaks.Length == req.Facts.Count);

    var withStemms = breaks.Select((poslens, idx) => {
      var brs = new Rw.WordBreaking.Breaks();
      if (poslens != null && poslens.Count > 0)
        brs.PosLens.AddRange(toByteStringRaw(poslens));
      return brs;
    });

    var res = new Rw.WordBreaking.Response();
    res.Facts.AddRange(withStemms);
    return Task.FromResult(res);
  }

  //public override Task<Rw.WordBreaking.Response> RunEx(Rw.WordBreaking.Request req, ServerCallContext context) {
  //  var breaks = Service.wordBreak(req.Lang, req.Facts);
  //  Debug.Assert(breaks.Length == req.Facts.Count);

  //  var withStemms = breaks.Select((poslens, idx) => {
  //    var isEmpty = poslens == null || poslens.Count == 0;
  //    var br = new Rw.WordBreaking.Breaks { Breaks_ = isEmpty ? nullBytes : toByteString(poslens) };
  //    if (isEmpty || !Creators.hasStemmer(req.Lang)) return br;
  //    //stemming
  //    var text = req.Facts[idx];
  //    var words = poslens.Select(pl => text.Substring(pl.Pos, pl.Len));
  //    Service.getSentenceStemms(req.Lang, words, (word, stms) => {
  //      //br.stemms[word] = stms
  //    });
  //    return br;
  //  }).ToArray();

  //  var res = new Rw.WordBreaking.Response();
  //  res.Facts.AddRange(withStemms);
  //  return Task.FromResult(res);
  //}

  static Google.Protobuf.ByteString nullBytes = Google.Protobuf.ByteString.CopyFrom(new byte[0], 0, 0);

  IEnumerable<Rw.WordBreaking.PosLen> toByteStringRaw(List<TPosLen> posLens) {
    return posLens.Select(pl => new Rw.WordBreaking.PosLen { Pos = pl.Pos, End = pl.Pos + pl.Len });
  }

  Google.Protobuf.ByteString toByteString(string lang, string txt, List<TPosLen> posLens) {
    var bytes = BreaksConverter.oldToNew(txt, posLens);
    return bytes == null ? nullBytes : Google.Protobuf.ByteString.CopyFrom(bytes, 0, bytes.Length);
  }
}
