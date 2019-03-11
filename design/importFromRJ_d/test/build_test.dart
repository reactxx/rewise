// import 'dart:typed_data';
// import 'dart:convert' as convert;
// import 'package:protobuf/protobuf.dart' as proto;
@Timeout(const Duration(hours: 1))

import 'package:test/test.dart';
import 'package:rewise_low_utils/messages.dart' as messages;
import 'package:import_from_rj/index.dart' show makeRequest;
import 'package:rewise_low_utils/utils.dart' as utils;

main() {
  group("BUILD BOOKS", () {
    // NEEDS CSHARP AND DART SERVER:
    // dart lib/main.dart
    test('call hello world', () async {
      var resp = await makeRequest<messages.HelloReply>(
          (client) => client.sayHello(messages.HelloRequest()));
      expect(resp.dartId, equals(0));
    }, skip: true);

    // NEEDS CSHARP SERVER
    test('many simple requests', () async {
      final res = await Future.wait(
        utils.range(0,1000).map((i) => makeRequest<messages.HelloReply>(
          (client) => client.sayHello(messages.HelloRequest()..noRecursion = true)))
      );
      final len = res.where((r) => r!=null).length;
      expect(len==0 || len==1000, equals(true));
    }, skip: false);


  }, skip: false);
}
