import 'package:grpc/grpc.dart' as grpc;
import 'src/helloworld/gen/helloworld.pbgrpc.dart';
export 'src/helloworld/gen/helloworld.pb.dart';

grpc.ClientChannel getChannel() => grpc.ClientChannel('localhost',
      port: 50052,
      options: const grpc.ChannelOptions(
          credentials: const grpc.ChannelCredentials.insecure()));

GreeterClient getClient(grpc.ClientChannel channel) => GreeterClient(channel);

typedef AsyncGetter<TRepply> = Future<TRepply> Function (grpc.ClientChannel channel);

Future<TRepply> run2<TRepply>(grpc.ClientChannel channel, AsyncGetter<TRepply> getter) async {
  TRepply message;

  try {
    final response = await getter(channel);
    message = response;
  } catch (e) {
    print('Caught error: $e');
    return null;
  }
  await channel.shutdown();
  return message;
}
