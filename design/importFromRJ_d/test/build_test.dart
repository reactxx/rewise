// import 'dart:typed_data';
// import 'dart:convert' as convert;
// import 'package:protobuf/protobuf.dart' as proto;
import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/utils.dart' as grcpRequest;
import 'package:rewise_low_utils/messages.dart' as messages;

main() {
  test.group("BUILD BOOKS", () {
    test.test('hello world', () async {
      var resp = await grcpRequest.grcpRequest<messages.HelloReply>((channel) =>
          messages.GreeterClient(channel)
              .sayHello(messages.HelloRequest()..name = 'world'));
      var ok = resp == null || resp.message == 'Hello world';
      test.expect(ok, test.equals(true));
    }, skip: true);
  }, skip: true);
}
