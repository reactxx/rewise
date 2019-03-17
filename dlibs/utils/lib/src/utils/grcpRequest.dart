import 'package:grpc/grpc.dart' as grpc;

typedef AsyncGetter<TRepply> = Future<TRepply> Function(
    grpc.ClientChannel channel);

class Host {
  Host(this.host, this.port);
  String host;
  int port;
}

typedef GetHost = Host Function(String name);

GetHost getHost = (String name) => Host('localhost', 50052);

Future<TRepply> MakeRequest<TRepply>(
        AsyncGetter<TRepply> callRPCMethod, Host hostPort) =>
    _grpcRequest(callRPCMethod, hostPort.port, hostPort.host);

//********** private
Future<TRepply> _grpcRequest<TRepply>(
    AsyncGetter<TRepply> callRPCMethod, int port, String host) async {
  final channel = grpc.ClientChannel(host,
      port: port,
      options: const grpc.ChannelOptions(
          credentials: const grpc.ChannelCredentials.insecure()));
  TRepply message;
  try {
    message = await callRPCMethod(channel);
  } catch (e) {
    print('Caught error: $e');
  }
  await channel.shutdown();
  return message;
}


