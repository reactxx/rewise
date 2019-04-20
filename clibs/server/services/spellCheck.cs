using Grpc.Core;
using System.Threading.Tasks;

public class SpellCheckService : Rw.Spellcheck.CSharpService.CSharpServiceBase {

  public override Task<Rw.Spellcheck.Response> Spellcheck(Rw.Spellcheck.Request request, ServerCallContext context) {
    return Task.FromResult(WordSpellCheck.Spellcheck(request));
  }


}
