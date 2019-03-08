import 'package:test/test.dart' as test;
import 'package:import_from_rj/helloWorldServer.dart' as helloSrv;
import 'package:rewise_low_utils/utils.dart' as grcpRequest;

main() {
  test.test('grpc', () async {
    var resp = await grcpRequest.grcpRequest<helloSrv.HelloReply>(
        helloSrv.getChannel(),
        (channel) => helloSrv
            .getClient(channel)
            .sayHello(helloSrv.HelloRequest()..name = 'world'));

    var ok = resp == null || resp.message == 'Hello world';
    test.expect(ok, test.equals(true));
  });
}
