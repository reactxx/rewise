@Timeout(const Duration(hours: 1))

import 'package:test/test.dart';
import 'package:rewise_low_utils/rw/client.dart' as client;
import 'package:rewise_low_utils/rw/hallo_world.dart' as hallow;
import 'package:rewise_low_utils/rw/utils.dart' as mutils;
import 'package:rewise_low_utils/designTime.dart';
import 'package:rewise_low_utils/utils.dart' as utils;
import 'package:server_dart/utils.dart' as utilss;

main() {
  group("TESTS", () {
    test('Hack JSON', () async {
      var msgData = hallow.HelloReply()
        ..csharpId = 1
        ..dartId = 2;
      var jsonMsg = await utilss.hackToJson(msgData);
      var bytes = await utilss.hackFromJson(
          jsonMsg, msgData.info_.qualifiedMessageName);
      msgData = hallow.HelloReply.fromBuffer(bytes);
      expect(msgData.csharpId, equals(1));
    }, skip: false);

    // NEEDS CSHARP AND DART SERVER:
    // dart lib/main.dart
    test('call hello world', () async {
      var resp = await client.HalloWorld_SayHello(hallow.HelloRequest());
      expect(resp.dartId, equals(0));
    }, skip: true);

    // NEEDS CSHARP SERVER
    test('many simple requests', () async {
      final res = await Future.wait(utils.range(0, 1000).map((i) =>
          client.HalloWorld_SayHello(
              hallow.HelloRequest()..noRecursion = true)));
      final len = res.where((r) => r != null).length;
      expect(len == 0 || len == 1000, equals(true));
    }, skip: false);

    test('write test config', () async {
      var config = mutils.Config();
      config.workSpaces['localhost'] = mutils.WorkSpace()
        ..dartServer = (mutils.Connection()
          ..host = 'localhost'
          ..port = 1234);
      var json = config.writeToJson();
      config = mutils.Config.fromJson(
          '{ "workSpaces": { "localhost": { "csharpServer": { "host": "localhost", "port": 1234 } } } }',
          null);
      json = config.writeToJson();
      if (json != null) // unused warning
        fileSystem.protobufs
            .writeAsString('rewise/testConfig.json', config.writeToJson());
    }, skip: false);
  }, skip: false);
}
