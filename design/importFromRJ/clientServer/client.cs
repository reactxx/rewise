using Google.Protobuf;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using static RewiseDom.DartMain;

public static class Client {
  public static T makeRequest<T>(Func<DartMainClient, T> getter) where T: IMessage {
    return RewiseDom.Client.grpcRequest(channel => getter(new DartMainClient(channel)), 50053, "localhost");
  }
}
