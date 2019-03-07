import 'package:test/test.dart' as test;
import 'package:grpc/grpc.dart' as grpc;

import 'gRPCgen/helloworld.pb.dart';
import 'gRPCgen/helloworld.pbgrpc.dart';

// ***** design time
// Download "protoc-3.4.0-win32.zip" from https://github.com/protocolbuffers/protobuf/releases/tag/v3.7.0
//   and unzip to c:\rewise\protobuf\ (and put to PATH)
// Install protoc_plugin globally:
//   https://pub.dartlang.org/packages/protoc_plugin
//   to DART globals cache etc.: c:\Users\pavel\AppData\Roaming\Pub\Cache\
// how to geneate message code: https://developers.google.com/protocol-buffers/docs/reference/dart-generated

// ****** runtime
// https://pub.dartlang.org/packages/protobuf
// https://pub.dartlang.org/packages/grpc

main() {
  test.test('grpc', () async {
    var msg = await run();
    var ok = msg=='Hello world' || msg==null;
    test.expect(ok, test.equals(true));
  });
}

Future<String> run() async {
  final channel = new grpc.ClientChannel('localhost',
      port: 50051,
      options: const grpc.ChannelOptions(
          credentials: const grpc.ChannelCredentials.insecure()));
  final stub = new GreeterClient(channel);

  final name = 'world';
  String message;

  try {
    final response = await stub.sayHello(new HelloRequest()..name = name);
    print('Greeter client received: ${response.message}');
    message = response.message;
  } catch (e) {
    print('Caught error: $e');
    return null;
  }
  await channel.shutdown();
  return message;
}
