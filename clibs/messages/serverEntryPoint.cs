using Grpc.Core;
using System.Threading.Tasks;

namespace RewiseDom {
    public class ServerEntryPoint : CSharpMain.CSharpMainBase {
      public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        return Task.FromResult(new HelloReply { DartId = request.DartId, CsharpId = request.CsharpId + 1 });
      }
    }
}