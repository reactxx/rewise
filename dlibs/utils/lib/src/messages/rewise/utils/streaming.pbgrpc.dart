///
//  Generated code. Do not modify.
//  source: rewise/utils/streaming.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

import 'dart:async' as $async;

import 'package:grpc/service_api.dart' as $grpc;
import 'streaming.pb.dart';
export 'streaming.pb.dart';

class CSharpServiceClient extends $grpc.Client {
  static final _$streaming = new $grpc.ClientMethod<Request, Response>(
      '/rw.stemming.CSharpService/Streaming',
      (Request value) => value.writeToBuffer(),
      (List<int> value) => new Response.fromBuffer(value));

  CSharpServiceClient($grpc.ClientChannel channel, {$grpc.CallOptions options})
      : super(channel, options: options);

  $grpc.ResponseStream<Response> streaming($async.Stream<Request> request,
      {$grpc.CallOptions options}) {
    final call = $createCall(_$streaming, request, options: options);
    return new $grpc.ResponseStream(call);
  }
}

abstract class CSharpServiceBase extends $grpc.Service {
  String get $name => 'rw.stemming.CSharpService';

  CSharpServiceBase() {
    $addMethod(new $grpc.ServiceMethod<Request, Response>(
        'Streaming',
        streaming,
        true,
        true,
        (List<int> value) => new Request.fromBuffer(value),
        (Response value) => value.writeToBuffer()));
  }

  $async.Stream<Response> streaming(
      $grpc.ServiceCall call, $async.Stream<Request> request);
}
