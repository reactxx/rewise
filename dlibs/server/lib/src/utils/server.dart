import 'package:grpc/grpc.dart' as grpc;
import 'package:rw_dom/hallo_world.dart' as hallow;
import 'package:rewise_low_utils/client.dart' as client;
import 'dart:async' show Future;

class ServerEntryPoint extends hallow.DartMainServiceBase {

  Future<hallow.HelloReply> sayHello(
          grpc.ServiceCall call, hallow.HelloRequest request) =>
      _sayHello(call, request);

  static runServer(String address, int port) async {
    final server = grpc.Server([ServerEntryPoint()]);
    await server.serve(address: address, port: port);
  }

  Future<hallow.HelloReply> _sayHello(
      grpc.ServiceCall call, hallow.HelloRequest request) async {
    reqCount++;
    if (request.noRecursion || request.dartId > 50)
      return Future.value(hallow.HelloReply()..dartCount = reqCount);

    final req = hallow.HelloRequest()
      ..dartId = request.dartId + 1
      ..dartCount = reqCount
      ..csharpId = request.csharpId;

    final resp = await client.HalloWorld_SayHello(req);

    var res = hallow.HelloReply()
      ..dartId = resp.dartId
      ..dartCount = reqCount
      ..csharpId = resp.csharpId;
    return Future.value(res);
  }
}

int reqCount = 0;
