///
//  Generated code. Do not modify.
//  source: rewise/main_csharp.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

import 'dart:async' as $async;

import 'package:grpc/service_api.dart' as $grpc;
import 'books_import/books_import_fromrj.pb.dart' as $0;
import '../google/protobuf/empty.pb.dart' as $1;
import 'books_import/books_import_wordbreak.pb.dart' as $2;
import 'utils/bytes_list.pb.dart' as $3;
import 'utils/hello_world.pb.dart' as $4;
import 'utils/hack_json.pb.dart' as $5;
export 'main_csharp.pb.dart';

class CSharpMainClient extends $grpc.Client {
  static final _$matrixsToBookOuts =
      new $grpc.ClientMethod<$0.FileNamesRequest, $1.Empty>(
          '/rewiseDom.CSharpMain/MatrixsToBookOuts',
          ($0.FileNamesRequest value) => value.writeToBuffer(),
          (List<int> value) => new $1.Empty.fromBuffer(value));
  static final _$callWordBreaks =
      new $grpc.ClientMethod<$2.WordBreakRequest, $3.BytesList>(
          '/rewiseDom.CSharpMain/CallWordBreaks',
          ($2.WordBreakRequest value) => value.writeToBuffer(),
          (List<int> value) => new $3.BytesList.fromBuffer(value));
  static final _$sayHello =
      new $grpc.ClientMethod<$4.HelloRequest, $4.HelloReply>(
          '/rewiseDom.CSharpMain/SayHello',
          ($4.HelloRequest value) => value.writeToBuffer(),
          (List<int> value) => new $4.HelloReply.fromBuffer(value));
  static final _$hackFromJson =
      new $grpc.ClientMethod<$5.HackJsonString, $5.HackJsonBytes>(
          '/rewiseDom.CSharpMain/HackFromJson',
          ($5.HackJsonString value) => value.writeToBuffer(),
          (List<int> value) => new $5.HackJsonBytes.fromBuffer(value));
  static final _$hackToJson =
      new $grpc.ClientMethod<$5.HackJsonBytes, $5.HackJsonString>(
          '/rewiseDom.CSharpMain/HackToJson',
          ($5.HackJsonBytes value) => value.writeToBuffer(),
          (List<int> value) => new $5.HackJsonString.fromBuffer(value));

  CSharpMainClient($grpc.ClientChannel channel, {$grpc.CallOptions options})
      : super(channel, options: options);

  $grpc.ResponseFuture<$1.Empty> matrixsToBookOuts($0.FileNamesRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$matrixsToBookOuts, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<$3.BytesList> callWordBreaks($2.WordBreakRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$callWordBreaks, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<$4.HelloReply> sayHello($4.HelloRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$sayHello, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<$5.HackJsonBytes> hackFromJson($5.HackJsonString request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$hackFromJson, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<$5.HackJsonString> hackToJson($5.HackJsonBytes request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$hackToJson, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }
}

abstract class CSharpMainServiceBase extends $grpc.Service {
  String get $name => 'rewiseDom.CSharpMain';

  CSharpMainServiceBase() {
    $addMethod(new $grpc.ServiceMethod<$0.FileNamesRequest, $1.Empty>(
        'MatrixsToBookOuts',
        matrixsToBookOuts_Pre,
        false,
        false,
        (List<int> value) => new $0.FileNamesRequest.fromBuffer(value),
        ($1.Empty value) => value.writeToBuffer()));
    $addMethod(new $grpc.ServiceMethod<$2.WordBreakRequest, $3.BytesList>(
        'CallWordBreaks',
        callWordBreaks_Pre,
        false,
        false,
        (List<int> value) => new $2.WordBreakRequest.fromBuffer(value),
        ($3.BytesList value) => value.writeToBuffer()));
    $addMethod(new $grpc.ServiceMethod<$4.HelloRequest, $4.HelloReply>(
        'SayHello',
        sayHello_Pre,
        false,
        false,
        (List<int> value) => new $4.HelloRequest.fromBuffer(value),
        ($4.HelloReply value) => value.writeToBuffer()));
    $addMethod(new $grpc.ServiceMethod<$5.HackJsonString, $5.HackJsonBytes>(
        'HackFromJson',
        hackFromJson_Pre,
        false,
        false,
        (List<int> value) => new $5.HackJsonString.fromBuffer(value),
        ($5.HackJsonBytes value) => value.writeToBuffer()));
    $addMethod(new $grpc.ServiceMethod<$5.HackJsonBytes, $5.HackJsonString>(
        'HackToJson',
        hackToJson_Pre,
        false,
        false,
        (List<int> value) => new $5.HackJsonBytes.fromBuffer(value),
        ($5.HackJsonString value) => value.writeToBuffer()));
  }

  $async.Future<$1.Empty> matrixsToBookOuts_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return matrixsToBookOuts(call, await request);
  }

  $async.Future<$3.BytesList> callWordBreaks_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return callWordBreaks(call, await request);
  }

  $async.Future<$4.HelloReply> sayHello_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return sayHello(call, await request);
  }

  $async.Future<$5.HackJsonBytes> hackFromJson_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return hackFromJson(call, await request);
  }

  $async.Future<$5.HackJsonString> hackToJson_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return hackToJson(call, await request);
  }

  $async.Future<$1.Empty> matrixsToBookOuts(
      $grpc.ServiceCall call, $0.FileNamesRequest request);
  $async.Future<$3.BytesList> callWordBreaks(
      $grpc.ServiceCall call, $2.WordBreakRequest request);
  $async.Future<$4.HelloReply> sayHello(
      $grpc.ServiceCall call, $4.HelloRequest request);
  $async.Future<$5.HackJsonBytes> hackFromJson(
      $grpc.ServiceCall call, $5.HackJsonString request);
  $async.Future<$5.HackJsonString> hackToJson(
      $grpc.ServiceCall call, $5.HackJsonBytes request);
}
