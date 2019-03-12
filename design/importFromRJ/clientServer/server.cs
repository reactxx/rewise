using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using RewiseDom;
using System.Linq;

public class ServerEntryPoint : RewiseDom.ServerEntryPoint {

  public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
    return _SayHello(request, context);
  }

  public override Task<Empty> MatrixsToBookOuts(FileNamesRequest request, ServerCallContext context) {
    throw new RpcException(new Status(StatusCode.Unimplemented, "MatrixsToBookOuts"));
  }

  public override Task<BytesList> CallWordBreaks(WordBreakRequest request, ServerCallContext context) {
    throw new RpcException(new Status(StatusCode.Unimplemented, "CallWordBreaks"));
  }

  Task<HelloReply> _SayHello(HelloRequest request, ServerCallContext context) {

    if (request.NoRecursion || request.CsharpId > 50)
      return Task.FromResult(new HelloReply { DartId = request.DartId, CsharpId = request.CsharpId, DartCount = request.DartCount });

    var maxDartCount = 0;
    Parallel.ForEach(Enumerable.Range(0, 100), i => {
      var pr = Client.makeRequest<HelloReply>((client) => client.SayHello(new HelloRequest { NoRecursion = true, DartCount = maxDartCount }));
      lock (this) maxDartCount = Math.Max(maxDartCount, pr.DartCount);
    });

    var req = new HelloRequest { DartCount = maxDartCount, CsharpId = request.CsharpId + 1, DartId = request.DartId };
    var resp = Client.makeRequest<HelloReply>((client) => client.SayHello(req));

    var res = new HelloReply { DartCount = maxDartCount, CsharpId = request.CsharpId, DartId = request.DartId };
    return Task.FromResult(res);
  }


  public static void RunServer(string host, int port, ServerCredentials cred = null) {
    Server server = new Server {
      Services = { CSharpMain.BindService(new ServerEntryPoint()) },
      Ports = { new ServerPort(host, port, cred ?? ServerCredentials.Insecure) }
    };
    server.Start();

    Console.WriteLine("Press any key to stop the server...");
    Console.ReadKey();

    server.ShutdownAsync().Wait();
  }
}

