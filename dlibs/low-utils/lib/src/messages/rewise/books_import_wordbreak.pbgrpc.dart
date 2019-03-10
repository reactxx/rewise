///
//  Generated code. Do not modify.
//  source: rewise/books_import_wordbreak.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

import 'dart:async' as $async;

import 'package:grpc/service_api.dart' as $grpc;
import 'books_import_wordbreak.pb.dart';
import 'bytes_list.pb.dart' as $0;
export 'books_import_wordbreak.pb.dart';

class WordBreakClient extends $grpc.Client {
  static final _$callWordBreaks =
      new $grpc.ClientMethod<WordBreakRequest, $0.BytesList>(
          '/rewiseDom.WordBreak/CallWordBreaks',
          (WordBreakRequest value) => value.writeToBuffer(),
          (List<int> value) => new $0.BytesList.fromBuffer(value));

  WordBreakClient($grpc.ClientChannel channel, {$grpc.CallOptions options})
      : super(channel, options: options);

  $grpc.ResponseFuture<$0.BytesList> callWordBreaks(WordBreakRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$callWordBreaks, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }
}

abstract class WordBreakServiceBase extends $grpc.Service {
  String get $name => 'rewiseDom.WordBreak';

  WordBreakServiceBase() {
    $addMethod(new $grpc.ServiceMethod<WordBreakRequest, $0.BytesList>(
        'CallWordBreaks',
        callWordBreaks_Pre,
        false,
        false,
        (List<int> value) => new WordBreakRequest.fromBuffer(value),
        ($0.BytesList value) => value.writeToBuffer()));
  }

  $async.Future<$0.BytesList> callWordBreaks_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return callWordBreaks(call, await request);
  }

  $async.Future<$0.BytesList> callWordBreaks(
      $grpc.ServiceCall call, WordBreakRequest request);
}
