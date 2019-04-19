using Grpc.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SpellCheckService : Rw.Spellcheck.CSharpService.CSharpServiceBase {
  public override Task<Rw.Spellcheck.Response> Spellcheck(Rw.Spellcheck.Request request, ServerCallContext context) {
    Rw.Spellcheck.Response res = null;
    return Task.FromResult(res);
  }


  }
