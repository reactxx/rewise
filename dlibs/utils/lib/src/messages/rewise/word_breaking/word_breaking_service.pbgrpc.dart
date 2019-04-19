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
  static final _$run2 = new $grpc.ClientMethod<Request2, Response2>(
      '/rw.word_breaking.CSharpService/Run2',
      (Request2 value) => value.writeToBuffer(),
      (List<int> value) => new Response2.fromBuffer(value));

  CSharpServiceClient($grpc.ClientChannel channel, {$grpc.CallOptions options})
      : super(channel, options: options);

  $grpc.ResponseFuture<Response2> run2(Request2 request,
      {$grpc.CallOptions options}) {
    final call = $createCall(_$run2, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }
}

abstract class CSharpServiceBase extends $grpc.Service {
  String get $name => 'rw.word_breaking.CSharpService';

  CSharpServiceBase() {
    $addMethod(new $grpc.ServiceMethod<Request2, Response2>(
        'Run2',
        run2_Pre,
        false,
        false,
        (List<int> value) => new Request2.fromBuffer(value),
        (Response2 value) => value.writeToBuffer()));
  }

  $async.Future<Response2> run2_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return run2(call, await request);
  }

  $async.Future<Response2> run2($grpc.ServiceCall call, Request2 request);
}
