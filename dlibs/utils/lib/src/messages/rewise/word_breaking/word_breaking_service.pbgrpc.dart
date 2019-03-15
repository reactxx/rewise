///
//  Generated code. Do not modify.
//  source: rewise/word_breaking/word_breaking_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

import 'dart:async' as $async;

import 'package:grpc/service_api.dart' as $grpc;
import 'word_breaking_service.pb.dart';
export 'word_breaking_service.pb.dart';

class CSharpServiceClient extends $grpc.Client {
  static final _$run = new $grpc.ClientMethod<Request, Response>(
      '/rw.word_breaking.CSharpService/Run',
      (Request value) => value.writeToBuffer(),
      (List<int> value) => new Response.fromBuffer(value));
  static final _$runEx = new $grpc.ClientMethod<Request, Response>(
      '/rw.word_breaking.CSharpService/RunEx',
      (Request value) => value.writeToBuffer(),
      (List<int> value) => new Response.fromBuffer(value));

  CSharpServiceClient($grpc.ClientChannel channel, {$grpc.CallOptions options})
      : super(channel, options: options);

  $grpc.ResponseFuture<Response> run(Request request,
      {$grpc.CallOptions options}) {
    final call = $createCall(_$run, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<Response> runEx(Request request,
      {$grpc.CallOptions options}) {
    final call = $createCall(_$runEx, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }
}

abstract class CSharpServiceBase extends $grpc.Service {
  String get $name => 'rw.word_breaking.CSharpService';

  CSharpServiceBase() {
    $addMethod(new $grpc.ServiceMethod<Request, Response>(
        'Run',
        run_Pre,
        false,
        false,
        (List<int> value) => new Request.fromBuffer(value),
        (Response value) => value.writeToBuffer()));
    $addMethod(new $grpc.ServiceMethod<Request, Response>(
        'RunEx',
        runEx_Pre,
        false,
        false,
        (List<int> value) => new Request.fromBuffer(value),
        (Response value) => value.writeToBuffer()));
  }

  $async.Future<Response> run_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return run(call, await request);
  }

  $async.Future<Response> runEx_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return runEx(call, await request);
  }

  $async.Future<Response> run($grpc.ServiceCall call, Request request);
  $async.Future<Response> runEx($grpc.ServiceCall call, Request request);
}