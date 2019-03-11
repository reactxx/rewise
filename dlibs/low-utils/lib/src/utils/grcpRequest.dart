import 'package:grpc/grpc.dart' as grpc;

typedef AsyncGetter<TRepply> = Future<TRepply> Function(
    grpc.ClientChannel channel);

Future<TRepply> grpcRequest<TRepply>(AsyncGetter<TRepply> getter, int port, String host) async {
  final channel = grpc.ClientChannel(host,
      port: port,
      options: const grpc.ChannelOptions(
          credentials: const grpc.ChannelCredentials.insecure()));
  TRepply message;
  try {
    message = await getter(channel);
  } catch (e) {
    print('Caught error: $e');
  }
  await channel.shutdown();
  return message;
}
