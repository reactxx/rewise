using Grpc.Core;
using System.Threading.Tasks;

public class SpellCheckService : Rw.Spellcheck.CSharpService.CSharpServiceBase {

  public override Task<Rw.Spellcheck.Response> Spellcheck(Rw.Spellcheck.Request request, ServerCallContext context) {
    var resp = new Rw.Spellcheck.Response();
    resp.WrongIdxs.AddRange(WordSpellCheck.Spellcheck(request.Lang, request.Words));
    return Task.FromResult(resp);
  }
  
}
