using System.Diagnostics;
using System.IO;
using System.Linq;
using RewiseDom;
using StemmerBreaker;

public static class WordBreakingTask {

  public static BytesList run(WordBreakRequest req) {
    var breakService = Services.getService(req.Lang);
    var breaks = breakService.wordBreak(req.Words);
    Debug.Assert(breaks.Length == req.Words.Count);
    var res = new BytesList();
    res.List.AddRange(breaks.Select(arr => Google.Protobuf.ByteString.CopyFrom(arr.Cast<byte>().ToArray(), 0, arr.Count)));
    return res;
  }
}
