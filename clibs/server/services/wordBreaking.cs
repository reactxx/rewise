using Grpc.Core;
using StemmerBreakerNew;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public class WordBreakingService : Rw.WordBreaking.CSharpService.CSharpServiceBase {

  public override Task<Rw.WordBreaking.Response> Run(Rw.WordBreaking.Request req, ServerCallContext context) {
    return runLow(req, (posLens, word) => toByteString(posLens, false));
  }

  public override Task<Rw.WordBreaking.Response> RunEx(Rw.WordBreaking.Request req, ServerCallContext context) {
    return runLow(req, (posLens, word) => {
      if (posLens.Count == 0)
        return nullBytes;
      var first = posLens[0];
      if (posLens.Count == 1 && first.Pos == 0 && first.Len == word.Length)
        return nullBytes;
      return toByteString(posLens, first.Pos == 0);
    });
  }

  static Google.Protobuf.ByteString nullBytes = Google.Protobuf.ByteString.CopyFrom(new byte[0], 0, 0);

  Task<Rw.WordBreaking.Response> runLow(Rw.WordBreaking.Request req, Func<List<TPosLen>, string, Google.Protobuf.ByteString> preprocess) {
    var breaks = Service.wordBreak(req.Lang, req.Facts);
    Debug.Assert(breaks.Length == req.Facts.Count);
    var res = new Rw.WordBreaking.Response();
    res.Facts.AddRange(breaks.Select((poslens, idx) => {
      var brs = preprocess(poslens, req.Facts[idx]);
      return brs == null ? null : new Rw.WordBreaking.Breaks {
        Breaks_ = brs
      };
    }));
    return Task.FromResult(res);
  }

  Google.Protobuf.ByteString toByteString(List<TPosLen> posLens, bool skipFirst) {
    var lastPos = 0;
    byte[] bytes = posLens.SelectMany(pl => {
      var pos = pl.Pos - lastPos;
      lastPos = pl.Pos + pl.Len;
      return Linq.Items((byte)pos, (byte)pl.Len);
    }).Skip(skipFirst ? 1 : 0).ToArray();
    return Google.Protobuf.ByteString.CopyFrom(bytes, 0, bytes.Length);
  }
}
