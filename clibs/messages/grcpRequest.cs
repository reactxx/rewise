using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace RewiseDom {
  public static class Client {
    public static TRepply grpcRequest<TRepply>(Func<Channel, TRepply> getter, int port, String host) {
      var channel = new Channel(host, port, ChannelCredentials.Insecure);
      try {
        var msg = getter(channel);
        return msg;
      } finally {
        channel.ShutdownAsync().Wait();
      }
    }
  }
}