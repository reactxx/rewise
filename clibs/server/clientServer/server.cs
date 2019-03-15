using Grpc.Core;
using System;

public static class Server {

  public static void Run(string host, int port, ServerCredentials cred = null) {
    Grpc.Core.Server server = new Grpc.Core.Server {
      Services = {
        Rw.HalloWorld.CSharpService.BindService(new HalloWorldService()),
        Rw.HackJson.CSharpService.BindService(new HackJsonService()),
        Rw.ToRaw.CSharpService.BindService(new ToRawService()),
      },
      Ports = { new ServerPort(host, port, cred ?? ServerCredentials.Insecure) }
    };
    server.Start();

    Console.WriteLine("Press any key to stop the server...");
    Console.ReadKey();

    server.ShutdownAsync().Wait();
  }
}

