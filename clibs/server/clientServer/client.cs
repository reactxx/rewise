using Google.Protobuf;
using System;

public static class Client {
  public static T makeRequest<T>(Func<Rw.HalloWorld.DartMain.DartMainClient, T> getter) where T: IMessage {
    return RewiseDom.Client.grpcRequest(channel => getter(new Rw.HalloWorld.DartMain.DartMainClient(channel)), 50053, "localhost");
  }
}
