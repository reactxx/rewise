///
//  Generated code. Do not modify.
//  source: rewise/hack_json/hack_json_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

import 'dart:async' as $async;

import 'package:grpc/service_api.dart' as $grpc;
import 'hack_json_service.pb.dart';
import '../utils/common.pb.dart' as $0;
import '../../google/protobuf/empty.pb.dart' as $1;
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
  static final _$hackFromJsonFile =
      new $grpc.ClientMethod<$0.FromToFiles, $1.Empty>(
          '/rw.hack_json.CSharpService/HackFromJsonFile',
          ($0.FromToFiles value) => value.writeToBuffer(),
          (List<int> value) => new $1.Empty.fromBuffer(value));
  static final _$hackToJsonFile =
      new $grpc.ClientMethod<$0.FromToFiles, $1.Empty>(
          '/rw.hack_json.CSharpService/HackToJsonFile',
          ($0.FromToFiles value) => value.writeToBuffer(),
          (List<int> value) => new $1.Empty.fromBuffer(value));

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

  $grpc.ResponseFuture<$1.Empty> hackFromJsonFile($0.FromToFiles request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$hackFromJsonFile, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<$1.Empty> hackToJsonFile($0.FromToFiles request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$hackToJsonFile, new $async.Stream.fromIterable([request]),
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
    $addMethod(new $grpc.ServiceMethod<$0.FromToFiles, $1.Empty>(
        'HackFromJsonFile',
        hackFromJsonFile_Pre,
        false,
        false,
        (List<int> value) => new $0.FromToFiles.fromBuffer(value),
        ($1.Empty value) => value.writeToBuffer()));
    $addMethod(new $grpc.ServiceMethod<$0.FromToFiles, $1.Empty>(
        'HackToJsonFile',
        hackToJsonFile_Pre,
        false,
        false,
        (List<int> value) => new $0.FromToFiles.fromBuffer(value),
        ($1.Empty value) => value.writeToBuffer()));
  }

  $async.Future<HackJsonBytes> hackFromJson_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return hackFromJson(call, await request);
  }

  $async.Future<HackJsonString> hackToJson_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return hackToJson(call, await request);
  }

  $async.Future<$1.Empty> hackFromJsonFile_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return hackFromJsonFile(call, await request);
  }

  $async.Future<$1.Empty> hackToJsonFile_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return hackToJsonFile(call, await request);
  }

  $async.Future<HackJsonBytes> hackFromJson(
      $grpc.ServiceCall call, HackJsonString request);
  $async.Future<HackJsonString> hackToJson(
      $grpc.ServiceCall call, HackJsonBytes request);
  $async.Future<$1.Empty> hackFromJsonFile(
      $grpc.ServiceCall call, $0.FromToFiles request);
  $async.Future<$1.Empty> hackToJsonFile(
      $grpc.ServiceCall call, $0.FromToFiles request);
}
