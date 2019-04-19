using Grpc.Core;
using System;

public static class Server {

  public static void Run(string host, int port, ServerCredentials cred = null) {
    using (var imp = new Impersonator.Impersonator("pavel", "LANGMaster", "zvahov88_")) {

      Grpc.Core.Server server = new Grpc.Core.Server {
        Services = {
        //Rw.HalloWorld.CSharpService.BindService(new HalloWorldService()),
        //Rw.HackJson.CSharpService.BindService(new HackJsonService()),
        //Rw.ToRaw.CSharpService.BindService(new ToRawService()),
        Rw.WordBreaking.CSharpService.BindService(new WordBreakingService()),
        Rw.Stemming.CSharpService.BindService(new StemmingService()),
        Rw.Spellcheck.CSharpService.BindService(new SpellCheckService()),
      },
        Ports = { new ServerPort(host, port, cred ?? ServerCredentials.Insecure) }
      };
      server.Start();

      Console.WriteLine("Press any key to stop the server...");
      Console.ReadKey();

      server.ShutdownAsync().Wait();
    }
  }
}

