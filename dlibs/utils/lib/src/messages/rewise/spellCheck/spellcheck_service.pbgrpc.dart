///
//  Generated code. Do not modify.
//  source: rewise/spellCheck/spellcheck_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

import 'dart:async' as $async;

import 'package:grpc/service_api.dart' as $grpc;
import 'spellcheck_service.pb.dart';
export 'spellcheck_service.pb.dart';

class CSharpServiceClient extends $grpc.Client {
  static final _$spellcheck = new $grpc.ClientMethod<Request, Response>(
      '/rw.spellcheck.CSharpService/Spellcheck',
      (Request value) => value.writeToBuffer(),
      (List<int> value) => new Response.fromBuffer(value));

  CSharpServiceClient($grpc.ClientChannel channel, {$grpc.CallOptions options})
      : super(channel, options: options);

  $grpc.ResponseFuture<Response> spellcheck(Request request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$spellcheck, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }
}

abstract class CSharpServiceBase extends $grpc.Service {
  String get $name => 'rw.spellcheck.CSharpService';

  CSharpServiceBase() {
    $addMethod(new $grpc.ServiceMethod<Request, Response>(
        'Spellcheck',
        spellcheck_Pre,
        false,
        false,
        (List<int> value) => new Request.fromBuffer(value),
        (Response value) => value.writeToBuffer()));
  }

  $async.Future<Response> spellcheck_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return spellcheck(call, await request);
  }

  $async.Future<Response> spellcheck($grpc.ServiceCall call, Request request);
}
