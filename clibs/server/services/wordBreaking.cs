﻿using Grpc.Core;
using StemmerBreakerNew;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public class WordBreakingService : Rw.WordBreaking.CSharpService.CSharpServiceBase {

  public override Task<Rw.WordBreaking.Response> Run(Rw.WordBreaking.Request req, ServerCallContext context) {
    var breaks = Service.wordBreak(req.Lang, req.Facts);
    Debug.Assert(breaks.Length == req.Facts.Count);

    var withStemms = breaks.Select((poslens, idx) =>
      new Rw.WordBreaking.Breaks { Breaks_ = poslens == null || poslens.Count == 0 ? nullBytes : toByteString(req.Facts[idx], poslens) });

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

  Google.Protobuf.ByteString toByteStringRaw(List<TPosLen> posLens) {
    byte[] bytes = posLens.SelectMany(pl => {
      if (pl.Pos > 255 || pl.Len > 255)
        throw new Exception();
      return Linq.Items((byte)pl.Pos, (byte)pl.Len);
    }).ToArray();

    return Google.Protobuf.ByteString.CopyFrom(bytes, 0, bytes.Length);
  }

  Google.Protobuf.ByteString toByteString(string txt, List<TPosLen> posLens) {
    var bytes = BreaksConverter.oldToNew(txt, posLens);
    return bytes==null ? nullBytes : Google.Protobuf.ByteString.CopyFrom(bytes, 0, bytes.Length);
  }
}
