///
//  Generated code. Do not modify.
//  source: rewise/main_csharp.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

import 'dart:async' as $async;

import 'package:grpc/service_api.dart' as $grpc;
import 'utils/hello_world.pb.dart' as $0;
import 'utils/hack_json.pb.dart' as $1;
import 'books_import/books_import_fromrj.pb.dart' as $2;
import '../google/protobuf/empty.pb.dart' as $3;
import 'books_import/books_import_word_break.pb.dart' as $4;
import 'utils/bytes_list.pb.dart' as $5;
export 'main_csharp.pb.dart';

class CSharpMainClient extends $grpc.Client {
  static final _$sayHello =
      new $grpc.ClientMethod<$0.HelloRequest, $0.HelloReply>(
          '/rewiseDom.CSharpMain/SayHello',
          ($0.HelloRequest value) => value.writeToBuffer(),
          (List<int> value) => new $0.HelloReply.fromBuffer(value));
  static final _$hackFromJson =
      new $grpc.ClientMethod<$1.HackJsonString, $1.HackJsonBytes>(
          '/rewiseDom.CSharpMain/HackFromJson',
          ($1.HackJsonString value) => value.writeToBuffer(),
          (List<int> value) => new $1.HackJsonBytes.fromBuffer(value));
  static final _$hackToJson =
      new $grpc.ClientMethod<$1.HackJsonBytes, $1.HackJsonString>(
          '/rewiseDom.CSharpMain/HackToJson',
          ($1.HackJsonBytes value) => value.writeToBuffer(),
          (List<int> value) => new $1.HackJsonString.fromBuffer(value));
  static final _$matrixsToBookOuts =
      new $grpc.ClientMethod<$2.ImportRJRequest, $3.Empty>(
          '/rewiseDom.CSharpMain/MatrixsToBookOuts',
          ($2.ImportRJRequest value) => value.writeToBuffer(),
          (List<int> value) => new $3.Empty.fromBuffer(value));
  static final _$wordBreak =
      new $grpc.ClientMethod<$4.WordBreakRequest, $5.BytesList>(
          '/rewiseDom.CSharpMain/WordBreak',
          ($4.WordBreakRequest value) => value.writeToBuffer(),
          (List<int> value) => new $5.BytesList.fromBuffer(value));

  CSharpMainClient($grpc.ClientChannel channel, {$grpc.CallOptions options})
      : super(channel, options: options);

  $grpc.ResponseFuture<$0.HelloReply> sayHello($0.HelloRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$sayHello, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<$1.HackJsonBytes> hackFromJson($1.HackJsonString request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$hackFromJson, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<$1.HackJsonString> hackToJson($1.HackJsonBytes request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$hackToJson, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<$3.Empty> matrixsToBookOuts($2.ImportRJRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$matrixsToBookOuts, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<$5.BytesList> wordBreak($4.WordBreakRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$wordBreak, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }
}

abstract class CSharpMainServiceBase extends $grpc.Service {
  String get $name => 'rewiseDom.CSharpMain';

  CSharpMainServiceBase() {
    $addMethod(new $grpc.ServiceMethod<$0.HelloRequest, $0.HelloReply>(
        'SayHello',
        sayHello_Pre,
        false,
        false,
        (List<int> value) => new $0.HelloRequest.fromBuffer(value),
        ($0.HelloReply value) => value.writeToBuffer()));
    $addMethod(new $grpc.ServiceMethod<$1.HackJsonString, $1.HackJsonBytes>(
        'HackFromJson',
        hackFromJson_Pre,
        false,
        false,
        (List<int> value) => new $1.HackJsonString.fromBuffer(value),
        ($1.HackJsonBytes value) => value.writeToBuffer()));
    $addMethod(new $grpc.ServiceMethod<$1.HackJsonBytes, $1.HackJsonString>(
        'HackToJson',
        hackToJson_Pre,
        false,
        false,
        (List<int> value) => new $1.HackJsonBytes.fromBuffer(value),
        ($1.HackJsonString value) => value.writeToBuffer()));
    $addMethod(new $grpc.ServiceMethod<$2.ImportRJRequest, $3.Empty>(
        'MatrixsToBookOuts',
        matrixsToBookOuts_Pre,
        false,
        false,
        (List<int> value) => new $2.ImportRJRequest.fromBuffer(value),
        ($3.Empty value) => value.writeToBuffer()));
    $addMethod(new $grpc.ServiceMethod<$4.WordBreakRequest, $5.BytesList>(
        'WordBreak',
        wordBreak_Pre,
        false,
        false,
        (List<int> value) => new $4.WordBreakRequest.fromBuffer(value),
        ($5.BytesList value) => value.writeToBuffer()));
  }

  $async.Future<$0.HelloReply> sayHello_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return sayHello(call, await request);
  }

  $async.Future<$1.HackJsonBytes> hackFromJson_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return hackFromJson(call, await request);
  }

  $async.Future<$1.HackJsonString> hackToJson_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return hackToJson(call, await request);
  }

  $async.Future<$3.Empty> matrixsToBookOuts_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return matrixsToBookOuts(call, await request);
  }

  $async.Future<$5.BytesList> wordBreak_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return wordBreak(call, await request);
  }

  $async.Future<$0.HelloReply> sayHello(
      $grpc.ServiceCall call, $0.HelloRequest request);
  $async.Future<$1.HackJsonBytes> hackFromJson(
      $grpc.ServiceCall call, $1.HackJsonString request);
  $async.Future<$1.HackJsonString> hackToJson(
      $grpc.ServiceCall call, $1.HackJsonBytes request);
  $async.Future<$3.Empty> matrixsToBookOuts(
      $grpc.ServiceCall call, $2.ImportRJRequest request);
  $async.Future<$5.BytesList> wordBreak(
      $grpc.ServiceCall call, $4.WordBreakRequest request);
}
