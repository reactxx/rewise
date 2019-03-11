///
//  Generated code. Do not modify.
//  source: rewise/main_dart.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

import 'dart:async' as $async;

import 'package:grpc/service_api.dart' as $grpc;
import 'utils/hello_world.pb.dart' as $0;
export 'main_dart.pb.dart';

class DartMainClient extends $grpc.Client {
  static final _$sayHello =
      new $grpc.ClientMethod<$0.HelloRequest, $0.HelloReply>(
          '/rewiseDom.DartMain/SayHello',
          ($0.HelloRequest value) => value.writeToBuffer(),
          (List<int> value) => new $0.HelloReply.fromBuffer(value));

  DartMainClient($grpc.ClientChannel channel, {$grpc.CallOptions options})
      : super(channel, options: options);

  $grpc.ResponseFuture<$0.HelloReply> sayHello($0.HelloRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$sayHello, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }
}

abstract class DartMainServiceBase extends $grpc.Service {
  String get $name => 'rewiseDom.DartMain';

  DartMainServiceBase() {
    $addMethod(new $grpc.ServiceMethod<$0.HelloRequest, $0.HelloReply>(
        'SayHello',
        sayHello_Pre,
        false,
        false,
        (List<int> value) => new $0.HelloRequest.fromBuffer(value),
        ($0.HelloReply value) => value.writeToBuffer()));
  }

  $async.Future<$0.HelloReply> sayHello_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return sayHello(call, await request);
  }

  $async.Future<$0.HelloReply> sayHello(
      $grpc.ServiceCall call, $0.HelloRequest request);
}
