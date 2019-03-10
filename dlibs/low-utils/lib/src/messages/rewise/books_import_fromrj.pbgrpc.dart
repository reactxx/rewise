///
//  Generated code. Do not modify.
//  source: rewise/books_import_fromrj.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

import 'dart:async' as $async;

import 'package:grpc/service_api.dart' as $grpc;
import 'books_import_fromrj.pb.dart';
import '../google/protobuf/empty.pb.dart' as $0;
export 'books_import_fromrj.pb.dart';

class FromRJClient extends $grpc.Client {
  static final _$matrixsToBookOuts =
      new $grpc.ClientMethod<FileNamesRequest, $0.Empty>(
          '/rewiseDom.FromRJ/MatrixsToBookOuts',
          (FileNamesRequest value) => value.writeToBuffer(),
          (List<int> value) => new $0.Empty.fromBuffer(value));

  FromRJClient($grpc.ClientChannel channel, {$grpc.CallOptions options})
      : super(channel, options: options);

  $grpc.ResponseFuture<$0.Empty> matrixsToBookOuts(FileNamesRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$matrixsToBookOuts, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }
}

abstract class FromRJServiceBase extends $grpc.Service {
  String get $name => 'rewiseDom.FromRJ';

  FromRJServiceBase() {
    $addMethod(new $grpc.ServiceMethod<FileNamesRequest, $0.Empty>(
        'MatrixsToBookOuts',
        matrixsToBookOuts_Pre,
        false,
        false,
        (List<int> value) => new FileNamesRequest.fromBuffer(value),
        ($0.Empty value) => value.writeToBuffer()));
  }

  $async.Future<$0.Empty> matrixsToBookOuts_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return matrixsToBookOuts(call, await request);
  }

  $async.Future<$0.Empty> matrixsToBookOuts(
      $grpc.ServiceCall call, FileNamesRequest request);
}
