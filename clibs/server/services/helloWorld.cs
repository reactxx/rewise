using Grpc.Core;
using System;
using System.Threading.Tasks;
using System.Linq;

public class HalloWorldService : Rw.HalloWorld.CSharpService.CSharpServiceBase {

  public override Task<Rw.HalloWorld.HelloReply> SayHello(Rw.HalloWorld.HelloRequest request, ServerCallContext context) {
    return _SayHello(request, context);
  }

  Task<Rw.HalloWorld.HelloReply> _SayHello(Rw.HalloWorld.HelloRequest request, ServerCallContext context) {

    if (request.NoRecursion || request.CsharpId > 50)
      return Task.FromResult(new Rw.HalloWorld.HelloReply { DartId = request.DartId, CsharpId = request.CsharpId, DartCount = request.DartCount });

    var maxDartCount = 0;
    Parallel.ForEach(Enumerable.Range(0, 100), i => {
      var pr = Client.makeRequest((client) => client.SayHello(new Rw.HalloWorld.HelloRequest { NoRecursion = true, DartCount = maxDartCount }));
      lock (this) maxDartCount = Math.Max(maxDartCount, pr.DartCount);
    });

    var req = new Rw.HalloWorld.HelloRequest { DartCount = maxDartCount, CsharpId = request.CsharpId + 1, DartId = request.DartId };
    var resp = Client.makeRequest<Rw.HalloWorld.HelloReply>((client) => client.SayHello(req));

    var res = new Rw.HalloWorld.HelloReply { DartCount = maxDartCount, CsharpId = request.CsharpId, DartId = request.DartId };
    return Task.FromResult(res);
  }

}