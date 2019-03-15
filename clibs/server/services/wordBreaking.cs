using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using RewiseDom;
using StemmerBreaker;

public class WordBreakingService : Rw.WordBreaking.CSharpService.CSharpServiceBase {

  public override Task<Rw.WordBreaking.Response> Run(Rw.WordBreaking.Request req, ServerCallContext context) {
    return runLow(req, (arr, word) => toByteString(arr, false));
  }

  public override Task<Rw.WordBreaking.Response> RunEx(Rw.WordBreaking.Request req, ServerCallContext context) {
    return runLow(req, (arr, word) => {
      if (arr.Count == 0) return nullBytes;
      var first = arr[0];
      if (arr.Count == 1 && first.Pos == 0 && first.Len == word.Length) return nullBytes;
      return toByteString(arr, first.Pos == 0);
    });
  }

  static Google.Protobuf.ByteString nullBytes = Google.Protobuf.ByteString.CopyFrom(new byte[0], 0, 0);

  Task<Rw.WordBreaking.Response> runLow(Rw.WordBreaking.Request req, Func<List<TPosLen>, string, Google.Protobuf.ByteString> preprocess) {
    var breakService = Services.getService(req.Lang);
    var breaks = breakService.wordBreak(req.Facts);
    Debug.Assert(breaks.Length == req.Facts.Count);
    var res = new Rw.WordBreaking.Response();
    res.Facts.AddRange(breaks.Select((arr, idx) => {
      var brs = preprocess(arr, req.Facts[idx]);
      return brs == null ? null : new Rw.WordBreaking.Breaks {
        Breaks_ = brs
      };
    }));//. Google.Protobuf.ByteString.CopyFrom(arr.Cast<byte>().ToArray(), 0, arr.Count)));
    return Task.FromResult(res);
  }

  Google.Protobuf.ByteString toByteString(List<TPosLen> arr, bool skipFirst) {
    var lastPos = 0;
    byte[] bytes = arr.SelectMany(pl => {
      var pos = pl.Pos - lastPos;
      lastPos = pl.Pos + pl.Len;
      return Linq.Items((byte)pos, (byte)pl.Len);
    }).Skip(skipFirst ? 1 : 0).ToArray();
    return Google.Protobuf.ByteString.CopyFrom(bytes, 0, bytes.Length);
  }
  //public static BytesList run(WordBreakRequest req) {
  //  var breakService = Services.getService(req.Lang);
  //  var breaks = breakService.wordBreak(req.Words);
  //  Debug.Assert(breaks.Length == req.Words.Count);
  //  var res = new BytesList();
  //  res.List.AddRange(breaks.Select(arr => Google.Protobuf.ByteString.CopyFrom(arr.Cast<byte>().ToArray(), 0, arr.Count)));
  //  return res;
  //}
}
