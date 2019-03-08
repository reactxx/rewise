using Grpc.Core;
using System;

namespace DesignServer {
  class Program {
    public static void Main(string[] args) {
      Server server = new Server {
        Services = { Helloworld.Greeter.BindService(new Fake.GreeterImpl()) },
        Ports = { new ServerPort("localhost", 50052, ServerCredentials.Insecure) }
      };
      server.Start();

      Console.WriteLine("Press any key to stop the server...");
      Console.ReadKey();

      server.ShutdownAsync().Wait();
    }
  }
}
