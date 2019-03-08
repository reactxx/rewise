import 'package:grpc/grpc.dart' as grpc;
import 'src/helloworld/gen/helloworld.pbgrpc.dart';
export 'src/helloworld/gen/helloworld.pb.dart';

getChannel() => grpc.ClientChannel('localhost',
      port: 50052,
      options: const grpc.ChannelOptions(
          credentials: const grpc.ChannelCredentials.insecure()));

getClient(grpc.ClientChannel channel) => GreeterClient(channel);