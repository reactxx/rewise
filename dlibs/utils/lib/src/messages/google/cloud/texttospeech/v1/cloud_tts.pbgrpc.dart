///
//  Generated code. Do not modify.
//  source: google/cloud/texttospeech/v1/cloud_tts.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

import 'dart:async' as $async;

import 'package:grpc/service_api.dart' as $grpc;
import 'cloud_tts.pb.dart';
export 'cloud_tts.pb.dart';

class TextToSpeechClient extends $grpc.Client {
  static final _$listVoices =
      new $grpc.ClientMethod<ListVoicesRequest, ListVoicesResponse>(
          '/google.cloud.texttospeech.v1.TextToSpeech/ListVoices',
          (ListVoicesRequest value) => value.writeToBuffer(),
          (List<int> value) => new ListVoicesResponse.fromBuffer(value));
  static final _$synthesizeSpeech =
      new $grpc.ClientMethod<SynthesizeSpeechRequest, SynthesizeSpeechResponse>(
          '/google.cloud.texttospeech.v1.TextToSpeech/SynthesizeSpeech',
          (SynthesizeSpeechRequest value) => value.writeToBuffer(),
          (List<int> value) => new SynthesizeSpeechResponse.fromBuffer(value));

  TextToSpeechClient($grpc.ClientChannel channel, {$grpc.CallOptions options})
      : super(channel, options: options);

  $grpc.ResponseFuture<ListVoicesResponse> listVoices(ListVoicesRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$listVoices, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<SynthesizeSpeechResponse> synthesizeSpeech(
      SynthesizeSpeechRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$synthesizeSpeech, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }
}

abstract class TextToSpeechServiceBase extends $grpc.Service {
  String get $name => 'google.cloud.texttospeech.v1.TextToSpeech';

  TextToSpeechServiceBase() {
    $addMethod(new $grpc.ServiceMethod<ListVoicesRequest, ListVoicesResponse>(
        'ListVoices',
        listVoices_Pre,
        false,
        false,
        (List<int> value) => new ListVoicesRequest.fromBuffer(value),
        (ListVoicesResponse value) => value.writeToBuffer()));
    $addMethod(new $grpc
            .ServiceMethod<SynthesizeSpeechRequest, SynthesizeSpeechResponse>(
        'SynthesizeSpeech',
        synthesizeSpeech_Pre,
        false,
        false,
        (List<int> value) => new SynthesizeSpeechRequest.fromBuffer(value),
        (SynthesizeSpeechResponse value) => value.writeToBuffer()));
  }

  $async.Future<ListVoicesResponse> listVoices_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return listVoices(call, await request);
  }

  $async.Future<SynthesizeSpeechResponse> synthesizeSpeech_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return synthesizeSpeech(call, await request);
  }

  $async.Future<ListVoicesResponse> listVoices(
      $grpc.ServiceCall call, ListVoicesRequest request);
  $async.Future<SynthesizeSpeechResponse> synthesizeSpeech(
      $grpc.ServiceCall call, SynthesizeSpeechRequest request);
}
