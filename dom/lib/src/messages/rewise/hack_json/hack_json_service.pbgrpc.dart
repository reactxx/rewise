///
//  Generated code. Do not modify.
//  source: rewise/hack_json/hack_json_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

import 'dart:async' as $async;

import 'package:grpc/service_api.dart' as $grpc;
import 'hack_json_service.pb.dart';
import '../../google/protobuf/empty.pb.dart' as $0;
export 'hack_json_service.pb.dart';

class CSharpServiceClient extends $grpc.Client {
  static final _$hackJson = new $grpc.ClientMethod<HackJsonPar, HackJsonPar>(
      '/rw.hack_json.CSharpService/HackJson',
      (HackJsonPar value) => value.writeToBuffer(),
      (List<int> value) => new HackJsonPar.fromBuffer(value));
  static final _$hackJsonFile =
      new $grpc.ClientMethod<HackJsonFilePar, $0.Empty>(
          '/rw.hack_json.CSharpService/HackJsonFile',
          (HackJsonFilePar value) => value.writeToBuffer(),
          (List<int> value) => new $0.Empty.fromBuffer(value));

  CSharpServiceClient($grpc.ClientChannel channel, {$grpc.CallOptions options})
      : super(channel, options: options);

  $grpc.ResponseFuture<HackJsonPar> hackJson(HackJsonPar request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$hackJson, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<$0.Empty> hackJsonFile(HackJsonFilePar request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$hackJsonFile, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }
}

abstract class CSharpServiceBase extends $grpc.Service {
  String get $name => 'rw.hack_json.CSharpService';

  CSharpServiceBase() {
    $addMethod(new $grpc.ServiceMethod<HackJsonPar, HackJsonPar>(
        'HackJson',
        hackJson_Pre,
        false,
        false,
        (List<int> value) => new HackJsonPar.fromBuffer(value),
        (HackJsonPar value) => value.writeToBuffer()));
    $addMethod(new $grpc.ServiceMethod<HackJsonFilePar, $0.Empty>(
        'HackJsonFile',
        hackJsonFile_Pre,
        false,
        false,
        (List<int> value) => new HackJsonFilePar.fromBuffer(value),
        ($0.Empty value) => value.writeToBuffer()));
  }

  $async.Future<HackJsonPar> hackJson_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return hackJson(call, await request);
  }

  $async.Future<$0.Empty> hackJsonFile_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return hackJsonFile(call, await request);
  }

  $async.Future<HackJsonPar> hackJson(
      $grpc.ServiceCall call, HackJsonPar request);
  $async.Future<$0.Empty> hackJsonFile(
      $grpc.ServiceCall call, HackJsonFilePar request);
}
