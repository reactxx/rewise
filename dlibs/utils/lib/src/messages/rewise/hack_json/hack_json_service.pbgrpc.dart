///
//  Generated code. Do not modify.
//  source: rewise/hack_json/hack_json_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

import 'dart:async' as $async;

import 'package:grpc/service_api.dart' as $grpc;
import 'hack_json_service.pb.dart';
export 'hack_json_service.pb.dart';

class CSharpServiceClient extends $grpc.Client {
  static final _$hackFromJson =
      new $grpc.ClientMethod<HackJsonString, HackJsonBytes>(
          '/rw.hack_json.CSharpService/HackFromJson',
          (HackJsonString value) => value.writeToBuffer(),
          (List<int> value) => new HackJsonBytes.fromBuffer(value));
  static final _$hackToJson =
      new $grpc.ClientMethod<HackJsonBytes, HackJsonString>(
          '/rw.hack_json.CSharpService/HackToJson',
          (HackJsonBytes value) => value.writeToBuffer(),
          (List<int> value) => new HackJsonString.fromBuffer(value));

  CSharpServiceClient($grpc.ClientChannel channel, {$grpc.CallOptions options})
      : super(channel, options: options);

  $grpc.ResponseFuture<HackJsonBytes> hackFromJson(HackJsonString request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$hackFromJson, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<HackJsonString> hackToJson(HackJsonBytes request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$hackToJson, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }
}

abstract class CSharpServiceBase extends $grpc.Service {
  String get $name => 'rw.hack_json.CSharpService';

  CSharpServiceBase() {
    $addMethod(new $grpc.ServiceMethod<HackJsonString, HackJsonBytes>(
        'HackFromJson',
        hackFromJson_Pre,
        false,
        false,
        (List<int> value) => new HackJsonString.fromBuffer(value),
        (HackJsonBytes value) => value.writeToBuffer()));
    $addMethod(new $grpc.ServiceMethod<HackJsonBytes, HackJsonString>(
        'HackToJson',
        hackToJson_Pre,
        false,
        false,
        (List<int> value) => new HackJsonBytes.fromBuffer(value),
        (HackJsonString value) => value.writeToBuffer()));
  }

  $async.Future<HackJsonBytes> hackFromJson_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return hackFromJson(call, await request);
  }

  $async.Future<HackJsonString> hackToJson_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return hackToJson(call, await request);
  }

  $async.Future<HackJsonBytes> hackFromJson(
      $grpc.ServiceCall call, HackJsonString request);
  $async.Future<HackJsonString> hackToJson(
      $grpc.ServiceCall call, HackJsonBytes request);
}
