// import 'dart:typed_data';
// import 'dart:convert' as convert;
// import 'package:protobuf/protobuf.dart' as proto;
import 'package:test/test.dart' show test, group, equals, expect;
import 'package:rewise_low_utils/utils.dart' show grpcRequest;
import 'package:rewise_low_utils/messages.dart' as messages;
import 'package:rewise_low_utils/messages.dart' show MainClient;

main() {
  group("BUILD BOOKS", () {
    test('hello world', () async {
      var resp = await grpcRequest<messages.HelloReply>((channel) =>
          MainClient(channel)
              .sayHello(messages.HelloRequest()..name = 'world'));
      //messages.HelloRequest.fromJson("i");
      var ok = resp == null || resp.message == 'Hello world';
      expect(ok, equals(true));
    }, skip: false);
  }, skip: false);
}
