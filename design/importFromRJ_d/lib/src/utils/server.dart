import 'package:grpc/grpc.dart' as grpc;
import 'package:rewise_low_utils/messages.dart' as messages;
import 'dart:async' show Future;
import 'client.dart' show makeRequest;

class ServerEntryPoint extends messages.DartMainServiceBase {
  // Future<messages.Empty> matrixsToBookOuts(
  //     grpc.ServiceCall call, messages.FileNamesRequest request) {
  //   throw UnsupportedError('matrixsToBookOuts');
  // }

  // Future<messages.BytesList> callWordBreaks(
  //     grpc.ServiceCall call, messages.WordBreakRequest request) {
  //   throw UnsupportedError('callWordBreaks');
  // }

  Future<messages.HelloReply> sayHello(
          grpc.ServiceCall call, messages.HelloRequest request) =>
      _sayHello(call, request);

  static runServer() async {
    final server = grpc.Server([ServerEntryPoint()]);
    await server.serve(address: 'localhost', port: 50053);
  }

  Future<messages.HelloReply> _sayHello(
      grpc.ServiceCall call, messages.HelloRequest request) async {
    reqCount++;
    if (request.noRecursion || request.dartId > 50)
      return Future.value(messages.HelloReply()..dartCount = reqCount);

    final req = messages.HelloRequest()
      ..dartId = request.dartId + 1
      ..dartCount = reqCount
      ..csharpId = request.csharpId;

    final resp = await makeRequest<messages.HelloReply>(
        (client) => client.sayHello(req));
    var res = messages.HelloReply()
      ..dartId = resp.dartId
      ..dartCount = reqCount
      ..csharpId = resp.csharpId;
    return Future.value(res);
  }
}

int reqCount = 0;
