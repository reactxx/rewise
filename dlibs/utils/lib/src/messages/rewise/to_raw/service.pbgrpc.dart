///
//  Generated code. Do not modify.
//  source: rewise/to_raw/service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

import 'dart:async' as $async;

import 'package:grpc/service_api.dart' as $grpc;
import 'service.pb.dart';
export 'service.pb.dart';

class CSharpServiceClient extends $grpc.Client {
  static final _$run = new $grpc.ClientMethod<Request, Response>(
      '/rw.to_raw.CSharpService/Run',
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
}

abstract class CSharpServiceBase extends $grpc.Service {
  String get $name => 'rw.to_raw.CSharpService';

  CSharpServiceBase() {
    $addMethod(new $grpc.ServiceMethod<Request, Response>(
        'Run',
        run_Pre,
        false,
        false,
        (List<int> value) => new Request.fromBuffer(value),
        (Response value) => value.writeToBuffer()));
  }

  $async.Future<Response> run_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return run(call, await request);
  }

  $async.Future<Response> run($grpc.ServiceCall call, Request request);
}
