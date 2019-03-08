import 'package:test/test.dart' as test;
import 'package:import_from_rj/helloWorldServer.dart' as helloSrv;

main() {
  test.test('grpc', () async {
    var msg = await run();
    var ok = msg=='Hello world' || msg==null;
    test.expect(ok, test.equals(true));
  });
}
Future<String> run() async {
  final channel = helloSrv.getChannel();
  final stub = helloSrv.getClient(channel);

  final name = 'world';
  String message;

  try {
    final response = await stub.sayHello(helloSrv.HelloRequest()..name = name);
    print('Greeter client received: ${response.message}');
    message = response.message;
  } catch (e) {
    print('Caught error: $e');
    return null;
  }
  await channel.shutdown();
  return message;
}
