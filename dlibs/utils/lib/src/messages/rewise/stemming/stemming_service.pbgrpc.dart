///
//  Generated code. Do not modify.
//  source: rewise/stemming/stemming_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

import 'dart:async' as $async;

import 'package:grpc/service_api.dart' as $grpc;
import 'stemming_service.pb.dart';
export 'stemming_service.pb.dart';

class CSharpServiceClient extends $grpc.Client {
  static final _$stemm = new $grpc.ClientMethod<Request, Response>(
      '/rw.stemming.CSharpService/Stemm',
      (Request value) => value.writeToBuffer(),
      (List<int> value) => new Response.fromBuffer(value));

  CSharpServiceClient($grpc.ClientChannel channel, {$grpc.CallOptions options})
      : super(channel, options: options);

  $grpc.ResponseFuture<Response> stemm(Request request,
      {$grpc.CallOptions options}) {
    final call = $createCall(_$stemm, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }
}

abstract class CSharpServiceBase extends $grpc.Service {
  String get $name => 'rw.stemming.CSharpService';

  CSharpServiceBase() {
    $addMethod(new $grpc.ServiceMethod<Request, Response>(
        'Stemm',
        stemm_Pre,
        false,
        false,
        (List<int> value) => new Request.fromBuffer(value),
        (Response value) => value.writeToBuffer()));
  }

  $async.Future<Response> stemm_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return stemm(call, await request);
  }

  $async.Future<Response> stemm($grpc.ServiceCall call, Request request);
}
