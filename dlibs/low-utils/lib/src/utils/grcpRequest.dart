import 'package:grpc/grpc.dart' as grpc;

typedef AsyncGetter<TRepply> = Future<TRepply> Function (grpc.ClientChannel channel);

Future<TRepply> grcpRequest<TRepply>(grpc.ClientChannel channel, AsyncGetter<TRepply> getter) async {
  TRepply message;
  try {
    message = await getter(channel);
  } catch (e) {
    print('Caught error: $e');
  }
  await channel.shutdown();
  return message;
}
