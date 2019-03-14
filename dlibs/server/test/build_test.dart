// import 'dart:typed_data';
// import 'dart:convert' as convert;
// import 'package:protobuf/protobuf.dart' as proto;
@Timeout(const Duration(hours: 1))

import 'package:test/test.dart';
import 'package:rewise_low_utils/messages.dart' as messages;
import 'package:server_dart/utils.dart';
import 'package:server_dart/commands.dart';
import 'package:rewise_low_utils/utils.dart' as utils;

main() {
  group("TESTS", () {
    test('Hack JSON', () async {
      var msgData = messages.HelloReply()
        ..csharpId = 1
        ..dartId = 2;
      var jsonMsg = await hackToJson(msgData);
      var bytes = await hackFromJson(jsonMsg, msgData.info_.qualifiedMessageName);
      msgData = messages.HelloReply.fromBuffer(bytes);
      expect(msgData.csharpId, equals(1));
    }, skip: false);

    // NEEDS CSHARP AND DART SERVER:
    // dart lib/main.dart
    test('call hello world', () async {
      var resp = await makeRequest<messages.HelloReply>(
          (client) => client.sayHello(messages.HelloRequest()));
      expect(resp.dartId, equals(0));
    }, skip: true);

    // NEEDS CSHARP SERVER
    test('many simple requests', () async {
      final res = await Future.wait(utils.range(0, 1000).map((i) =>
          makeRequest<messages.HelloReply>((client) =>
              client.sayHello(messages.HelloRequest()..noRecursion = true))));
      final len = res.where((r) => r != null).length;
      expect(len == 0 || len == 1000, equals(true));
    }, skip: false);

    test('write test config', () async {
      var config = messages.Config();
      config.workSpaces['localhost'] = messages.WorkSpace()
        ..dartServer = (messages.Connection()
          ..host = 'localhost'
          ..port = 1234);
      var json = config.writeToJson();
      config = messages.Config.fromJson(
          '{ "workSpaces": { "localhost": { "csharpServer": { "host": "localhost", "port": 1234 } } } }',
          null);
      json = config.writeToJson();
      fileSystem.protobufs
          .writeAsString('rewise/testConfig.json', config.writeToJson());
    }, skip: false);
  }, skip: false);
}
