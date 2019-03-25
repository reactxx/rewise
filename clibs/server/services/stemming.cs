using Grpc.Core;
using StemmerBreakerNew;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public class StemmingService : Rw.Stemming.CSharpService.CSharpServiceBase {
  public override Task<Rw.Stemming.Response> Stemm(Rw.Stemming.Request req, ServerCallContext context) {
    var res = new Rw.Stemming.Response() { Lang = req.Lang };

    Service.getSentenceStemmsAndSubStemm(req.Lang, req.Words.Select(w => w.ToLower()), stemmList => {
      stemmList.ForEach(sl => {
        sl.stemms.Sort();
        sl.asString = sl.stemms.JoinStrings(",");
      });
      var first = stemmList.First();
      var grp = stemmList.GroupBy(sl => sl.asString).First(g => g.Key == first.asString);
      var ownWords = grp.Select(w => w.word).ToArray();
      var otherWords = first.stemms.Except(ownWords);
      var word = new Rw.Stemming.Word();
      word.Stemms.AddRange(ownWords);
      word.Stemms.AddRange(otherWords);
      word.OwnLen = ownWords.Length;
      res.Words.Add(word);
    });

    return Task.FromResult(res);
  }

}
