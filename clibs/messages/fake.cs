using Grpc.Core;
using System.Threading.Tasks;

public static class Fake {
  public class GreeterImpl : RewiseDom.Main.MainBase {
    // Server side handler of the SayHello RPC
    public override Task<RewiseDom.HelloReply> SayHello(RewiseDom.HelloRequest request, ServerCallContext context) {
      return Task.FromResult(new RewiseDom.HelloReply { Message = "Hello " + request.Name });
    }

    public void fake() {
      var msg = new RewiseDom.HelloReply { Message = "Hello" };
      msg.WriteTo(new Google.Protobuf.CodedOutputStream(new byte[1000]));
      RewiseDom.HelloReply.Parser.ParseFrom(new byte[1]);
    }
  }
}

