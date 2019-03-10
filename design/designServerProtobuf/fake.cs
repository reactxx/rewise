﻿using Grpc.Core;
using System.Threading.Tasks;

public static class Fake {
  static Helloworld.Greeter.GreeterBase GreeterBase;
  static Helloworld.Greeter.GreeterClient GreeterClient;
  static Helloworld.HelloRequest HelloRequest;
  static Helloworld.HelloReply HelloReply;

  public class GreeterImpl : Helloworld.Greeter.GreeterBase {
    // Server side handler of the SayHello RPC
    public override Task<Helloworld.HelloReply> SayHello(Helloworld.HelloRequest request, ServerCallContext context) {
      return Task.FromResult(new Helloworld.HelloReply { Message = "Hello " + request.Name });
    }

    public void fake() {
      var msg = new Helloworld.HelloReply { Message = "Hello" };
      msg.WriteTo(new Google.Protobuf.CodedOutputStream(new byte[1000]));
      Helloworld.HelloReply.Parser.ParseFrom(new byte[1]);
    }
  }
}

