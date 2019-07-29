///
//  Generated code. Do not modify.
//  source: google/cloud/language/v1/language_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

import 'dart:async' as $async;

import 'package:grpc/service_api.dart' as $grpc;
import 'language_service.pb.dart';
export 'language_service.pb.dart';

class LanguageServiceClient extends $grpc.Client {
  static final _$analyzeSentiment =
      new $grpc.ClientMethod<AnalyzeSentimentRequest, AnalyzeSentimentResponse>(
          '/google.cloud.language.v1.LanguageService/AnalyzeSentiment',
          (AnalyzeSentimentRequest value) => value.writeToBuffer(),
          (List<int> value) => new AnalyzeSentimentResponse.fromBuffer(value));
  static final _$analyzeEntities =
      new $grpc.ClientMethod<AnalyzeEntitiesRequest, AnalyzeEntitiesResponse>(
          '/google.cloud.language.v1.LanguageService/AnalyzeEntities',
          (AnalyzeEntitiesRequest value) => value.writeToBuffer(),
          (List<int> value) => new AnalyzeEntitiesResponse.fromBuffer(value));
  static final _$analyzeEntitySentiment = new $grpc.ClientMethod<
          AnalyzeEntitySentimentRequest, AnalyzeEntitySentimentResponse>(
      '/google.cloud.language.v1.LanguageService/AnalyzeEntitySentiment',
      (AnalyzeEntitySentimentRequest value) => value.writeToBuffer(),
      (List<int> value) =>
          new AnalyzeEntitySentimentResponse.fromBuffer(value));
  static final _$analyzeSyntax =
      new $grpc.ClientMethod<AnalyzeSyntaxRequest, AnalyzeSyntaxResponse>(
          '/google.cloud.language.v1.LanguageService/AnalyzeSyntax',
          (AnalyzeSyntaxRequest value) => value.writeToBuffer(),
          (List<int> value) => new AnalyzeSyntaxResponse.fromBuffer(value));
  static final _$classifyText =
      new $grpc.ClientMethod<ClassifyTextRequest, ClassifyTextResponse>(
          '/google.cloud.language.v1.LanguageService/ClassifyText',
          (ClassifyTextRequest value) => value.writeToBuffer(),
          (List<int> value) => new ClassifyTextResponse.fromBuffer(value));
  static final _$annotateText =
      new $grpc.ClientMethod<AnnotateTextRequest, AnnotateTextResponse>(
          '/google.cloud.language.v1.LanguageService/AnnotateText',
          (AnnotateTextRequest value) => value.writeToBuffer(),
          (List<int> value) => new AnnotateTextResponse.fromBuffer(value));

  LanguageServiceClient($grpc.ClientChannel channel,
      {$grpc.CallOptions options})
      : super(channel, options: options);

  $grpc.ResponseFuture<AnalyzeSentimentResponse> analyzeSentiment(
      AnalyzeSentimentRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$analyzeSentiment, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<AnalyzeEntitiesResponse> analyzeEntities(
      AnalyzeEntitiesRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$analyzeEntities, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<AnalyzeEntitySentimentResponse> analyzeEntitySentiment(
      AnalyzeEntitySentimentRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$analyzeEntitySentiment, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<AnalyzeSyntaxResponse> analyzeSyntax(
      AnalyzeSyntaxRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$analyzeSyntax, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<ClassifyTextResponse> classifyText(
      ClassifyTextRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$classifyText, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }

  $grpc.ResponseFuture<AnnotateTextResponse> annotateText(
      AnnotateTextRequest request,
      {$grpc.CallOptions options}) {
    final call = $createCall(
        _$annotateText, new $async.Stream.fromIterable([request]),
        options: options);
    return new $grpc.ResponseFuture(call);
  }
}

abstract class LanguageServiceBase extends $grpc.Service {
  String get $name => 'google.cloud.language.v1.LanguageService';

  LanguageServiceBase() {
    $addMethod(new $grpc
            .ServiceMethod<AnalyzeSentimentRequest, AnalyzeSentimentResponse>(
        'AnalyzeSentiment',
        analyzeSentiment_Pre,
        false,
        false,
        (List<int> value) => new AnalyzeSentimentRequest.fromBuffer(value),
        (AnalyzeSentimentResponse value) => value.writeToBuffer()));
    $addMethod(new $grpc
            .ServiceMethod<AnalyzeEntitiesRequest, AnalyzeEntitiesResponse>(
        'AnalyzeEntities',
        analyzeEntities_Pre,
        false,
        false,
        (List<int> value) => new AnalyzeEntitiesRequest.fromBuffer(value),
        (AnalyzeEntitiesResponse value) => value.writeToBuffer()));
    $addMethod(new $grpc.ServiceMethod<AnalyzeEntitySentimentRequest,
            AnalyzeEntitySentimentResponse>(
        'AnalyzeEntitySentiment',
        analyzeEntitySentiment_Pre,
        false,
        false,
        (List<int> value) =>
            new AnalyzeEntitySentimentRequest.fromBuffer(value),
        (AnalyzeEntitySentimentResponse value) => value.writeToBuffer()));
    $addMethod(
        new $grpc.ServiceMethod<AnalyzeSyntaxRequest, AnalyzeSyntaxResponse>(
            'AnalyzeSyntax',
            analyzeSyntax_Pre,
            false,
            false,
            (List<int> value) => new AnalyzeSyntaxRequest.fromBuffer(value),
            (AnalyzeSyntaxResponse value) => value.writeToBuffer()));
    $addMethod(
        new $grpc.ServiceMethod<ClassifyTextRequest, ClassifyTextResponse>(
            'ClassifyText',
            classifyText_Pre,
            false,
            false,
            (List<int> value) => new ClassifyTextRequest.fromBuffer(value),
            (ClassifyTextResponse value) => value.writeToBuffer()));
    $addMethod(
        new $grpc.ServiceMethod<AnnotateTextRequest, AnnotateTextResponse>(
            'AnnotateText',
            annotateText_Pre,
            false,
            false,
            (List<int> value) => new AnnotateTextRequest.fromBuffer(value),
            (AnnotateTextResponse value) => value.writeToBuffer()));
  }

  $async.Future<AnalyzeSentimentResponse> analyzeSentiment_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return analyzeSentiment(call, await request);
  }

  $async.Future<AnalyzeEntitiesResponse> analyzeEntities_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return analyzeEntities(call, await request);
  }

  $async.Future<AnalyzeEntitySentimentResponse> analyzeEntitySentiment_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return analyzeEntitySentiment(call, await request);
  }

  $async.Future<AnalyzeSyntaxResponse> analyzeSyntax_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return analyzeSyntax(call, await request);
  }

  $async.Future<ClassifyTextResponse> classifyText_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return classifyText(call, await request);
  }

  $async.Future<AnnotateTextResponse> annotateText_Pre(
      $grpc.ServiceCall call, $async.Future request) async {
    return annotateText(call, await request);
  }

  $async.Future<AnalyzeSentimentResponse> analyzeSentiment(
      $grpc.ServiceCall call, AnalyzeSentimentRequest request);
  $async.Future<AnalyzeEntitiesResponse> analyzeEntities(
      $grpc.ServiceCall call, AnalyzeEntitiesRequest request);
  $async.Future<AnalyzeEntitySentimentResponse> analyzeEntitySentiment(
      $grpc.ServiceCall call, AnalyzeEntitySentimentRequest request);
  $async.Future<AnalyzeSyntaxResponse> analyzeSyntax(
      $grpc.ServiceCall call, AnalyzeSyntaxRequest request);
  $async.Future<ClassifyTextResponse> classifyText(
      $grpc.ServiceCall call, ClassifyTextRequest request);
  $async.Future<AnnotateTextResponse> annotateText(
      $grpc.ServiceCall call, AnnotateTextRequest request);
}
