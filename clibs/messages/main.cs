using Grpc.Core;
using System.Threading.Tasks;

namespace RewiseDom {
  public static class EntryPoint {
    public class Impl : Main.MainBase {
      // Server side handler of the SayHello RPC
      public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
      }

      //public void fake() {
      //  var msg = new RewiseDom.HelloReply { Message = "Hello" };
      //  msg.WriteTo(new Google.Protobuf.CodedOutputStream(new byte[1000]));
      //  RewiseDom.HelloReply.Parser.ParseFrom(new byte[1]);
      //}
    }
  }
}