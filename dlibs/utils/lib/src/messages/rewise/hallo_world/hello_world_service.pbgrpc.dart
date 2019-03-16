///
//  Generated code. Do not modify.
//  source: rewise/hallo_world/hello_world_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

import 'dart:async' as $async;

import 'package:grpc/service_api.dart' as $grpc;
import 'hello_world_service.pb.dart';
export 'hello_world_service.pb.dart';

class CSharpServiceClient extends $grpc.Client {
  static final _$sayHello = new $grpc.ClientMethod<HelloRequest, HelloReply>(
      '/rw.hallo_world.CSharpService/SayHello',
      (HelloRequest value) => value.writeToBuffer(),
      (List<int> value) => new HelloReply.fromBuffer(value));

  CSharpServiceClient($grpc.ClientChannel channel, {$grpc.CallOptions options})
      : super(channel, options: options);

  $grpc.ResponseFuture<HelloReply> sayHello(HelloRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$sayHello, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }
}

abstract class CSharpServiceBase extends $grpc.Service {
  String get $name => 'rw.hallo_world.CSharpService';

  CSharpServiceBase() {
    $addMethod(new $grpc.ServiceMethod<HelloRequest, HelloReply>(
        'SayHello',
        sayHello_Pre,
        false,
        false,
        (List<int> value) => new HelloRequest.fromBuffer(value),
        (HelloReply value) => value.writeToBuffer()));
  }

  $async.Future<HelloReply> sayHello_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return sayHello(call, await request);
  }

  $async.Future<HelloReply> sayHello(
      $grpc.ServiceCall call, HelloRequest request);
}

class DartMainClient extends $grpc.Client {
  static final _$sayHello = new $grpc.ClientMethod<HelloRequest, HelloReply>(
      '/rw.hallo_world.DartMain/SayHello',
      (HelloRequest value) => value.writeToBuffer(),
      (List<int> value) => new HelloReply.fromBuffer(value));

  DartMainClient($grpc.ClientChannel channel, {$grpc.CallOptions options})
      : super(channel, options: options);

  $grpc.ResponseFuture<HelloReply> sayHello(HelloRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$sayHello, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }
}

abstract class DartMainServiceBase extends $grpc.Service {
  String get $name => 'rw.hallo_world.DartMain';

  DartMainServiceBase() {
    $addMethod(new $grpc.ServiceMethod<HelloRequest, HelloReply>(
        'SayHello',
        sayHello_Pre,
        false,
        false,
        (List<int> value) => new HelloRequest.fromBuffer(value),
        (HelloReply value) => value.writeToBuffer()));
  }

  $async.Future<HelloReply> sayHello_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return sayHello(call, await request);
  }

  $async.Future<HelloReply> sayHello(
      $grpc.ServiceCall call, HelloRequest request);
}
