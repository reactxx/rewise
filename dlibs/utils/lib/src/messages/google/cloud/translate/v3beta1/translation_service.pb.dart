///
//  Generated code. Do not modify.
//  source: google/cloud/translate/v3beta1/translation_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:fixnum/fixnum.dart';
import 'package:protobuf/protobuf.dart' as $pb;

import '../../../protobuf/timestamp.pb.dart' as $2;

import 'translation_service.pbenum.dart';

export 'translation_service.pbenum.dart';

class TranslateTextGlossaryConfig extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('TranslateTextGlossaryConfig', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'glossary')
    ..aOB(2, 'ignoreCase')
    ..hasRequiredFields = false
  ;

  TranslateTextGlossaryConfig() : super();
  TranslateTextGlossaryConfig.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  TranslateTextGlossaryConfig.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  TranslateTextGlossaryConfig clone() => new TranslateTextGlossaryConfig()..mergeFromMessage(this);
  TranslateTextGlossaryConfig copyWith(void Function(TranslateTextGlossaryConfig) updates) => super.copyWith((message) => updates(message as TranslateTextGlossaryConfig));
  $pb.BuilderInfo get info_ => _i;
  static TranslateTextGlossaryConfig create() => new TranslateTextGlossaryConfig();
  TranslateTextGlossaryConfig createEmptyInstance() => create();
  static $pb.PbList<TranslateTextGlossaryConfig> createRepeated() => new $pb.PbList<TranslateTextGlossaryConfig>();
  static TranslateTextGlossaryConfig getDefault() => _defaultInstance ??= create()..freeze();
  static TranslateTextGlossaryConfig _defaultInstance;

  String get glossary => $_getS(0, '');
  set glossary(String v) { $_setString(0, v); }
  bool hasGlossary() => $_has(0);
  void clearGlossary() => clearField(1);

  bool get ignoreCase => $_get(1, false);
  set ignoreCase(bool v) { $_setBool(1, v); }
  bool hasIgnoreCase() => $_has(1);
  void clearIgnoreCase() => clearField(2);
}

class TranslateTextRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('TranslateTextRequest', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..pPS(1, 'contents')
    ..aOS(3, 'mimeType')
    ..aOS(4, 'sourceLanguageCode')
    ..aOS(5, 'targetLanguageCode')
    ..aOS(6, 'model')
    ..a<TranslateTextGlossaryConfig>(7, 'glossaryConfig', $pb.PbFieldType.OM, TranslateTextGlossaryConfig.getDefault, TranslateTextGlossaryConfig.create)
    ..aOS(8, 'parent')
    ..hasRequiredFields = false
  ;

  TranslateTextRequest() : super();
  TranslateTextRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  TranslateTextRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  TranslateTextRequest clone() => new TranslateTextRequest()..mergeFromMessage(this);
  TranslateTextRequest copyWith(void Function(TranslateTextRequest) updates) => super.copyWith((message) => updates(message as TranslateTextRequest));
  $pb.BuilderInfo get info_ => _i;
  static TranslateTextRequest create() => new TranslateTextRequest();
  TranslateTextRequest createEmptyInstance() => create();
  static $pb.PbList<TranslateTextRequest> createRepeated() => new $pb.PbList<TranslateTextRequest>();
  static TranslateTextRequest getDefault() => _defaultInstance ??= create()..freeze();
  static TranslateTextRequest _defaultInstance;

  List<String> get contents => $_getList(0);

  String get mimeType => $_getS(1, '');
  set mimeType(String v) { $_setString(1, v); }
  bool hasMimeType() => $_has(1);
  void clearMimeType() => clearField(3);

  String get sourceLanguageCode => $_getS(2, '');
  set sourceLanguageCode(String v) { $_setString(2, v); }
  bool hasSourceLanguageCode() => $_has(2);
  void clearSourceLanguageCode() => clearField(4);

  String get targetLanguageCode => $_getS(3, '');
  set targetLanguageCode(String v) { $_setString(3, v); }
  bool hasTargetLanguageCode() => $_has(3);
  void clearTargetLanguageCode() => clearField(5);

  String get model => $_getS(4, '');
  set model(String v) { $_setString(4, v); }
  bool hasModel() => $_has(4);
  void clearModel() => clearField(6);

  TranslateTextGlossaryConfig get glossaryConfig => $_getN(5);
  set glossaryConfig(TranslateTextGlossaryConfig v) { setField(7, v); }
  bool hasGlossaryConfig() => $_has(5);
  void clearGlossaryConfig() => clearField(7);

  String get parent => $_getS(6, '');
  set parent(String v) { $_setString(6, v); }
  bool hasParent() => $_has(6);
  void clearParent() => clearField(8);
}

class TranslateTextResponse extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('TranslateTextResponse', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..pc<Translation>(1, 'translations', $pb.PbFieldType.PM,Translation.create)
    ..pc<Translation>(3, 'glossaryTranslations', $pb.PbFieldType.PM,Translation.create)
    ..hasRequiredFields = false
  ;

  TranslateTextResponse() : super();
  TranslateTextResponse.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  TranslateTextResponse.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  TranslateTextResponse clone() => new TranslateTextResponse()..mergeFromMessage(this);
  TranslateTextResponse copyWith(void Function(TranslateTextResponse) updates) => super.copyWith((message) => updates(message as TranslateTextResponse));
  $pb.BuilderInfo get info_ => _i;
  static TranslateTextResponse create() => new TranslateTextResponse();
  TranslateTextResponse createEmptyInstance() => create();
  static $pb.PbList<TranslateTextResponse> createRepeated() => new $pb.PbList<TranslateTextResponse>();
  static TranslateTextResponse getDefault() => _defaultInstance ??= create()..freeze();
  static TranslateTextResponse _defaultInstance;

  List<Translation> get translations => $_getList(0);

  List<Translation> get glossaryTranslations => $_getList(1);
}

class Translation extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Translation', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'translatedText')
    ..aOS(2, 'model')
    ..a<TranslateTextGlossaryConfig>(3, 'glossaryConfig', $pb.PbFieldType.OM, TranslateTextGlossaryConfig.getDefault, TranslateTextGlossaryConfig.create)
    ..aOS(4, 'detectedLanguageCode')
    ..hasRequiredFields = false
  ;

  Translation() : super();
  Translation.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Translation.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Translation clone() => new Translation()..mergeFromMessage(this);
  Translation copyWith(void Function(Translation) updates) => super.copyWith((message) => updates(message as Translation));
  $pb.BuilderInfo get info_ => _i;
  static Translation create() => new Translation();
  Translation createEmptyInstance() => create();
  static $pb.PbList<Translation> createRepeated() => new $pb.PbList<Translation>();
  static Translation getDefault() => _defaultInstance ??= create()..freeze();
  static Translation _defaultInstance;

  String get translatedText => $_getS(0, '');
  set translatedText(String v) { $_setString(0, v); }
  bool hasTranslatedText() => $_has(0);
  void clearTranslatedText() => clearField(1);

  String get model => $_getS(1, '');
  set model(String v) { $_setString(1, v); }
  bool hasModel() => $_has(1);
  void clearModel() => clearField(2);

  TranslateTextGlossaryConfig get glossaryConfig => $_getN(2);
  set glossaryConfig(TranslateTextGlossaryConfig v) { setField(3, v); }
  bool hasGlossaryConfig() => $_has(2);
  void clearGlossaryConfig() => clearField(3);

  String get detectedLanguageCode => $_getS(3, '');
  set detectedLanguageCode(String v) { $_setString(3, v); }
  bool hasDetectedLanguageCode() => $_has(3);
  void clearDetectedLanguageCode() => clearField(4);
}

enum DetectLanguageRequest_Source {
  content, 
  notSet
}

class DetectLanguageRequest extends $pb.GeneratedMessage {
  static const Map<int, DetectLanguageRequest_Source> _DetectLanguageRequest_SourceByTag = {
    1 : DetectLanguageRequest_Source.content,
    0 : DetectLanguageRequest_Source.notSet
  };
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('DetectLanguageRequest', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'content')
    ..aOS(3, 'mimeType')
    ..aOS(4, 'model')
    ..aOS(5, 'parent')
    ..oo(0, [1])
    ..hasRequiredFields = false
  ;

  DetectLanguageRequest() : super();
  DetectLanguageRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  DetectLanguageRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  DetectLanguageRequest clone() => new DetectLanguageRequest()..mergeFromMessage(this);
  DetectLanguageRequest copyWith(void Function(DetectLanguageRequest) updates) => super.copyWith((message) => updates(message as DetectLanguageRequest));
  $pb.BuilderInfo get info_ => _i;
  static DetectLanguageRequest create() => new DetectLanguageRequest();
  DetectLanguageRequest createEmptyInstance() => create();
  static $pb.PbList<DetectLanguageRequest> createRepeated() => new $pb.PbList<DetectLanguageRequest>();
  static DetectLanguageRequest getDefault() => _defaultInstance ??= create()..freeze();
  static DetectLanguageRequest _defaultInstance;

  DetectLanguageRequest_Source whichSource() => _DetectLanguageRequest_SourceByTag[$_whichOneof(0)];
  void clearSource() => clearField($_whichOneof(0));

  String get content => $_getS(0, '');
  set content(String v) { $_setString(0, v); }
  bool hasContent() => $_has(0);
  void clearContent() => clearField(1);

  String get mimeType => $_getS(1, '');
  set mimeType(String v) { $_setString(1, v); }
  bool hasMimeType() => $_has(1);
  void clearMimeType() => clearField(3);

  String get model => $_getS(2, '');
  set model(String v) { $_setString(2, v); }
  bool hasModel() => $_has(2);
  void clearModel() => clearField(4);

  String get parent => $_getS(3, '');
  set parent(String v) { $_setString(3, v); }
  bool hasParent() => $_has(3);
  void clearParent() => clearField(5);
}

class DetectedLanguage extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('DetectedLanguage', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'languageCode')
    ..a<double>(2, 'confidence', $pb.PbFieldType.OF)
    ..hasRequiredFields = false
  ;

  DetectedLanguage() : super();
  DetectedLanguage.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  DetectedLanguage.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  DetectedLanguage clone() => new DetectedLanguage()..mergeFromMessage(this);
  DetectedLanguage copyWith(void Function(DetectedLanguage) updates) => super.copyWith((message) => updates(message as DetectedLanguage));
  $pb.BuilderInfo get info_ => _i;
  static DetectedLanguage create() => new DetectedLanguage();
  DetectedLanguage createEmptyInstance() => create();
  static $pb.PbList<DetectedLanguage> createRepeated() => new $pb.PbList<DetectedLanguage>();
  static DetectedLanguage getDefault() => _defaultInstance ??= create()..freeze();
  static DetectedLanguage _defaultInstance;

  String get languageCode => $_getS(0, '');
  set languageCode(String v) { $_setString(0, v); }
  bool hasLanguageCode() => $_has(0);
  void clearLanguageCode() => clearField(1);

  double get confidence => $_getN(1);
  set confidence(double v) { $_setFloat(1, v); }
  bool hasConfidence() => $_has(1);
  void clearConfidence() => clearField(2);
}

class DetectLanguageResponse extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('DetectLanguageResponse', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..pc<DetectedLanguage>(1, 'languages', $pb.PbFieldType.PM,DetectedLanguage.create)
    ..hasRequiredFields = false
  ;

  DetectLanguageResponse() : super();
  DetectLanguageResponse.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  DetectLanguageResponse.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  DetectLanguageResponse clone() => new DetectLanguageResponse()..mergeFromMessage(this);
  DetectLanguageResponse copyWith(void Function(DetectLanguageResponse) updates) => super.copyWith((message) => updates(message as DetectLanguageResponse));
  $pb.BuilderInfo get info_ => _i;
  static DetectLanguageResponse create() => new DetectLanguageResponse();
  DetectLanguageResponse createEmptyInstance() => create();
  static $pb.PbList<DetectLanguageResponse> createRepeated() => new $pb.PbList<DetectLanguageResponse>();
  static DetectLanguageResponse getDefault() => _defaultInstance ??= create()..freeze();
  static DetectLanguageResponse _defaultInstance;

  List<DetectedLanguage> get languages => $_getList(0);
}

class GetSupportedLanguagesRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('GetSupportedLanguagesRequest', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'displayLanguageCode')
    ..aOS(2, 'model')
    ..aOS(3, 'parent')
    ..hasRequiredFields = false
  ;

  GetSupportedLanguagesRequest() : super();
  GetSupportedLanguagesRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  GetSupportedLanguagesRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  GetSupportedLanguagesRequest clone() => new GetSupportedLanguagesRequest()..mergeFromMessage(this);
  GetSupportedLanguagesRequest copyWith(void Function(GetSupportedLanguagesRequest) updates) => super.copyWith((message) => updates(message as GetSupportedLanguagesRequest));
  $pb.BuilderInfo get info_ => _i;
  static GetSupportedLanguagesRequest create() => new GetSupportedLanguagesRequest();
  GetSupportedLanguagesRequest createEmptyInstance() => create();
  static $pb.PbList<GetSupportedLanguagesRequest> createRepeated() => new $pb.PbList<GetSupportedLanguagesRequest>();
  static GetSupportedLanguagesRequest getDefault() => _defaultInstance ??= create()..freeze();
  static GetSupportedLanguagesRequest _defaultInstance;

  String get displayLanguageCode => $_getS(0, '');
  set displayLanguageCode(String v) { $_setString(0, v); }
  bool hasDisplayLanguageCode() => $_has(0);
  void clearDisplayLanguageCode() => clearField(1);

  String get model => $_getS(1, '');
  set model(String v) { $_setString(1, v); }
  bool hasModel() => $_has(1);
  void clearModel() => clearField(2);

  String get parent => $_getS(2, '');
  set parent(String v) { $_setString(2, v); }
  bool hasParent() => $_has(2);
  void clearParent() => clearField(3);
}

class SupportedLanguages extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('SupportedLanguages', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..pc<SupportedLanguage>(1, 'languages', $pb.PbFieldType.PM,SupportedLanguage.create)
    ..hasRequiredFields = false
  ;

  SupportedLanguages() : super();
  SupportedLanguages.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  SupportedLanguages.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  SupportedLanguages clone() => new SupportedLanguages()..mergeFromMessage(this);
  SupportedLanguages copyWith(void Function(SupportedLanguages) updates) => super.copyWith((message) => updates(message as SupportedLanguages));
  $pb.BuilderInfo get info_ => _i;
  static SupportedLanguages create() => new SupportedLanguages();
  SupportedLanguages createEmptyInstance() => create();
  static $pb.PbList<SupportedLanguages> createRepeated() => new $pb.PbList<SupportedLanguages>();
  static SupportedLanguages getDefault() => _defaultInstance ??= create()..freeze();
  static SupportedLanguages _defaultInstance;

  List<SupportedLanguage> get languages => $_getList(0);
}

class SupportedLanguage extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('SupportedLanguage', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'languageCode')
    ..aOS(2, 'displayName')
    ..aOB(3, 'supportSource')
    ..aOB(4, 'supportTarget')
    ..hasRequiredFields = false
  ;

  SupportedLanguage() : super();
  SupportedLanguage.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  SupportedLanguage.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  SupportedLanguage clone() => new SupportedLanguage()..mergeFromMessage(this);
  SupportedLanguage copyWith(void Function(SupportedLanguage) updates) => super.copyWith((message) => updates(message as SupportedLanguage));
  $pb.BuilderInfo get info_ => _i;
  static SupportedLanguage create() => new SupportedLanguage();
  SupportedLanguage createEmptyInstance() => create();
  static $pb.PbList<SupportedLanguage> createRepeated() => new $pb.PbList<SupportedLanguage>();
  static SupportedLanguage getDefault() => _defaultInstance ??= create()..freeze();
  static SupportedLanguage _defaultInstance;

  String get languageCode => $_getS(0, '');
  set languageCode(String v) { $_setString(0, v); }
  bool hasLanguageCode() => $_has(0);
  void clearLanguageCode() => clearField(1);

  String get displayName => $_getS(1, '');
  set displayName(String v) { $_setString(1, v); }
  bool hasDisplayName() => $_has(1);
  void clearDisplayName() => clearField(2);

  bool get supportSource => $_get(2, false);
  set supportSource(bool v) { $_setBool(2, v); }
  bool hasSupportSource() => $_has(2);
  void clearSupportSource() => clearField(3);

  bool get supportTarget => $_get(3, false);
  set supportTarget(bool v) { $_setBool(3, v); }
  bool hasSupportTarget() => $_has(3);
  void clearSupportTarget() => clearField(4);
}

class GcsSource extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('GcsSource', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'inputUri')
    ..hasRequiredFields = false
  ;

  GcsSource() : super();
  GcsSource.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  GcsSource.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  GcsSource clone() => new GcsSource()..mergeFromMessage(this);
  GcsSource copyWith(void Function(GcsSource) updates) => super.copyWith((message) => updates(message as GcsSource));
  $pb.BuilderInfo get info_ => _i;
  static GcsSource create() => new GcsSource();
  GcsSource createEmptyInstance() => create();
  static $pb.PbList<GcsSource> createRepeated() => new $pb.PbList<GcsSource>();
  static GcsSource getDefault() => _defaultInstance ??= create()..freeze();
  static GcsSource _defaultInstance;

  String get inputUri => $_getS(0, '');
  set inputUri(String v) { $_setString(0, v); }
  bool hasInputUri() => $_has(0);
  void clearInputUri() => clearField(1);
}

enum InputConfig_Source {
  gcsSource, 
  notSet
}

class InputConfig extends $pb.GeneratedMessage {
  static const Map<int, InputConfig_Source> _InputConfig_SourceByTag = {
    2 : InputConfig_Source.gcsSource,
    0 : InputConfig_Source.notSet
  };
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('InputConfig', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'mimeType')
    ..a<GcsSource>(2, 'gcsSource', $pb.PbFieldType.OM, GcsSource.getDefault, GcsSource.create)
    ..oo(0, [2])
    ..hasRequiredFields = false
  ;

  InputConfig() : super();
  InputConfig.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  InputConfig.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  InputConfig clone() => new InputConfig()..mergeFromMessage(this);
  InputConfig copyWith(void Function(InputConfig) updates) => super.copyWith((message) => updates(message as InputConfig));
  $pb.BuilderInfo get info_ => _i;
  static InputConfig create() => new InputConfig();
  InputConfig createEmptyInstance() => create();
  static $pb.PbList<InputConfig> createRepeated() => new $pb.PbList<InputConfig>();
  static InputConfig getDefault() => _defaultInstance ??= create()..freeze();
  static InputConfig _defaultInstance;

  InputConfig_Source whichSource() => _InputConfig_SourceByTag[$_whichOneof(0)];
  void clearSource() => clearField($_whichOneof(0));

  String get mimeType => $_getS(0, '');
  set mimeType(String v) { $_setString(0, v); }
  bool hasMimeType() => $_has(0);
  void clearMimeType() => clearField(1);

  GcsSource get gcsSource => $_getN(1);
  set gcsSource(GcsSource v) { setField(2, v); }
  bool hasGcsSource() => $_has(1);
  void clearGcsSource() => clearField(2);
}

class GcsDestination extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('GcsDestination', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'outputUriPrefix')
    ..hasRequiredFields = false
  ;

  GcsDestination() : super();
  GcsDestination.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  GcsDestination.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  GcsDestination clone() => new GcsDestination()..mergeFromMessage(this);
  GcsDestination copyWith(void Function(GcsDestination) updates) => super.copyWith((message) => updates(message as GcsDestination));
  $pb.BuilderInfo get info_ => _i;
  static GcsDestination create() => new GcsDestination();
  GcsDestination createEmptyInstance() => create();
  static $pb.PbList<GcsDestination> createRepeated() => new $pb.PbList<GcsDestination>();
  static GcsDestination getDefault() => _defaultInstance ??= create()..freeze();
  static GcsDestination _defaultInstance;

  String get outputUriPrefix => $_getS(0, '');
  set outputUriPrefix(String v) { $_setString(0, v); }
  bool hasOutputUriPrefix() => $_has(0);
  void clearOutputUriPrefix() => clearField(1);
}

enum OutputConfig_Destination {
  gcsDestination, 
  notSet
}

class OutputConfig extends $pb.GeneratedMessage {
  static const Map<int, OutputConfig_Destination> _OutputConfig_DestinationByTag = {
    1 : OutputConfig_Destination.gcsDestination,
    0 : OutputConfig_Destination.notSet
  };
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('OutputConfig', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..a<GcsDestination>(1, 'gcsDestination', $pb.PbFieldType.OM, GcsDestination.getDefault, GcsDestination.create)
    ..oo(0, [1])
    ..hasRequiredFields = false
  ;

  OutputConfig() : super();
  OutputConfig.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  OutputConfig.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  OutputConfig clone() => new OutputConfig()..mergeFromMessage(this);
  OutputConfig copyWith(void Function(OutputConfig) updates) => super.copyWith((message) => updates(message as OutputConfig));
  $pb.BuilderInfo get info_ => _i;
  static OutputConfig create() => new OutputConfig();
  OutputConfig createEmptyInstance() => create();
  static $pb.PbList<OutputConfig> createRepeated() => new $pb.PbList<OutputConfig>();
  static OutputConfig getDefault() => _defaultInstance ??= create()..freeze();
  static OutputConfig _defaultInstance;

  OutputConfig_Destination whichDestination() => _OutputConfig_DestinationByTag[$_whichOneof(0)];
  void clearDestination() => clearField($_whichOneof(0));

  GcsDestination get gcsDestination => $_getN(0);
  set gcsDestination(GcsDestination v) { setField(1, v); }
  bool hasGcsDestination() => $_has(0);
  void clearGcsDestination() => clearField(1);
}

class BatchTranslateTextRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('BatchTranslateTextRequest', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'parent')
    ..aOS(2, 'sourceLanguageCode')
    ..pPS(3, 'targetLanguageCodes')
    ..m<String, String>(4, 'models', 'BatchTranslateTextRequest.ModelsEntry',$pb.PbFieldType.OS, $pb.PbFieldType.OS, null, null, null , const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..pc<InputConfig>(5, 'inputConfigs', $pb.PbFieldType.PM,InputConfig.create)
    ..a<OutputConfig>(6, 'outputConfig', $pb.PbFieldType.OM, OutputConfig.getDefault, OutputConfig.create)
    ..m<String, TranslateTextGlossaryConfig>(7, 'glossaries', 'BatchTranslateTextRequest.GlossariesEntry',$pb.PbFieldType.OS, $pb.PbFieldType.OM, TranslateTextGlossaryConfig.create, null, null , const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..hasRequiredFields = false
  ;

  BatchTranslateTextRequest() : super();
  BatchTranslateTextRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  BatchTranslateTextRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  BatchTranslateTextRequest clone() => new BatchTranslateTextRequest()..mergeFromMessage(this);
  BatchTranslateTextRequest copyWith(void Function(BatchTranslateTextRequest) updates) => super.copyWith((message) => updates(message as BatchTranslateTextRequest));
  $pb.BuilderInfo get info_ => _i;
  static BatchTranslateTextRequest create() => new BatchTranslateTextRequest();
  BatchTranslateTextRequest createEmptyInstance() => create();
  static $pb.PbList<BatchTranslateTextRequest> createRepeated() => new $pb.PbList<BatchTranslateTextRequest>();
  static BatchTranslateTextRequest getDefault() => _defaultInstance ??= create()..freeze();
  static BatchTranslateTextRequest _defaultInstance;

  String get parent => $_getS(0, '');
  set parent(String v) { $_setString(0, v); }
  bool hasParent() => $_has(0);
  void clearParent() => clearField(1);

  String get sourceLanguageCode => $_getS(1, '');
  set sourceLanguageCode(String v) { $_setString(1, v); }
  bool hasSourceLanguageCode() => $_has(1);
  void clearSourceLanguageCode() => clearField(2);

  List<String> get targetLanguageCodes => $_getList(2);

  Map<String, String> get models => $_getMap(3);

  List<InputConfig> get inputConfigs => $_getList(4);

  OutputConfig get outputConfig => $_getN(5);
  set outputConfig(OutputConfig v) { setField(6, v); }
  bool hasOutputConfig() => $_has(5);
  void clearOutputConfig() => clearField(6);

  Map<String, TranslateTextGlossaryConfig> get glossaries => $_getMap(6);
}

class BatchTranslateMetadata extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('BatchTranslateMetadata', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..e<BatchTranslateMetadata_State>(1, 'state', $pb.PbFieldType.OE, BatchTranslateMetadata_State.STATE_UNSPECIFIED, BatchTranslateMetadata_State.valueOf, BatchTranslateMetadata_State.values)
    ..aInt64(2, 'translatedCharacters')
    ..aInt64(3, 'failedCharacters')
    ..aInt64(4, 'totalCharacters')
    ..a<$2.Timestamp>(5, 'submitTime', $pb.PbFieldType.OM, $2.Timestamp.getDefault, $2.Timestamp.create)
    ..hasRequiredFields = false
  ;

  BatchTranslateMetadata() : super();
  BatchTranslateMetadata.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  BatchTranslateMetadata.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  BatchTranslateMetadata clone() => new BatchTranslateMetadata()..mergeFromMessage(this);
  BatchTranslateMetadata copyWith(void Function(BatchTranslateMetadata) updates) => super.copyWith((message) => updates(message as BatchTranslateMetadata));
  $pb.BuilderInfo get info_ => _i;
  static BatchTranslateMetadata create() => new BatchTranslateMetadata();
  BatchTranslateMetadata createEmptyInstance() => create();
  static $pb.PbList<BatchTranslateMetadata> createRepeated() => new $pb.PbList<BatchTranslateMetadata>();
  static BatchTranslateMetadata getDefault() => _defaultInstance ??= create()..freeze();
  static BatchTranslateMetadata _defaultInstance;

  BatchTranslateMetadata_State get state => $_getN(0);
  set state(BatchTranslateMetadata_State v) { setField(1, v); }
  bool hasState() => $_has(0);
  void clearState() => clearField(1);

  Int64 get translatedCharacters => $_getI64(1);
  set translatedCharacters(Int64 v) { $_setInt64(1, v); }
  bool hasTranslatedCharacters() => $_has(1);
  void clearTranslatedCharacters() => clearField(2);

  Int64 get failedCharacters => $_getI64(2);
  set failedCharacters(Int64 v) { $_setInt64(2, v); }
  bool hasFailedCharacters() => $_has(2);
  void clearFailedCharacters() => clearField(3);

  Int64 get totalCharacters => $_getI64(3);
  set totalCharacters(Int64 v) { $_setInt64(3, v); }
  bool hasTotalCharacters() => $_has(3);
  void clearTotalCharacters() => clearField(4);

  $2.Timestamp get submitTime => $_getN(4);
  set submitTime($2.Timestamp v) { setField(5, v); }
  bool hasSubmitTime() => $_has(4);
  void clearSubmitTime() => clearField(5);
}

class BatchTranslateResponse extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('BatchTranslateResponse', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aInt64(1, 'totalCharacters')
    ..aInt64(2, 'translatedCharacters')
    ..aInt64(3, 'failedCharacters')
    ..a<$2.Timestamp>(4, 'submitTime', $pb.PbFieldType.OM, $2.Timestamp.getDefault, $2.Timestamp.create)
    ..a<$2.Timestamp>(5, 'endTime', $pb.PbFieldType.OM, $2.Timestamp.getDefault, $2.Timestamp.create)
    ..hasRequiredFields = false
  ;

  BatchTranslateResponse() : super();
  BatchTranslateResponse.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  BatchTranslateResponse.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  BatchTranslateResponse clone() => new BatchTranslateResponse()..mergeFromMessage(this);
  BatchTranslateResponse copyWith(void Function(BatchTranslateResponse) updates) => super.copyWith((message) => updates(message as BatchTranslateResponse));
  $pb.BuilderInfo get info_ => _i;
  static BatchTranslateResponse create() => new BatchTranslateResponse();
  BatchTranslateResponse createEmptyInstance() => create();
  static $pb.PbList<BatchTranslateResponse> createRepeated() => new $pb.PbList<BatchTranslateResponse>();
  static BatchTranslateResponse getDefault() => _defaultInstance ??= create()..freeze();
  static BatchTranslateResponse _defaultInstance;

  Int64 get totalCharacters => $_getI64(0);
  set totalCharacters(Int64 v) { $_setInt64(0, v); }
  bool hasTotalCharacters() => $_has(0);
  void clearTotalCharacters() => clearField(1);

  Int64 get translatedCharacters => $_getI64(1);
  set translatedCharacters(Int64 v) { $_setInt64(1, v); }
  bool hasTranslatedCharacters() => $_has(1);
  void clearTranslatedCharacters() => clearField(2);

  Int64 get failedCharacters => $_getI64(2);
  set failedCharacters(Int64 v) { $_setInt64(2, v); }
  bool hasFailedCharacters() => $_has(2);
  void clearFailedCharacters() => clearField(3);

  $2.Timestamp get submitTime => $_getN(3);
  set submitTime($2.Timestamp v) { setField(4, v); }
  bool hasSubmitTime() => $_has(3);
  void clearSubmitTime() => clearField(4);

  $2.Timestamp get endTime => $_getN(4);
  set endTime($2.Timestamp v) { setField(5, v); }
  bool hasEndTime() => $_has(4);
  void clearEndTime() => clearField(5);
}

enum GlossaryInputConfig_Source {
  gcsSource, 
  notSet
}

class GlossaryInputConfig extends $pb.GeneratedMessage {
  static const Map<int, GlossaryInputConfig_Source> _GlossaryInputConfig_SourceByTag = {
    1 : GlossaryInputConfig_Source.gcsSource,
    0 : GlossaryInputConfig_Source.notSet
  };
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('GlossaryInputConfig', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..a<GcsSource>(1, 'gcsSource', $pb.PbFieldType.OM, GcsSource.getDefault, GcsSource.create)
    ..oo(0, [1])
    ..hasRequiredFields = false
  ;

  GlossaryInputConfig() : super();
  GlossaryInputConfig.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  GlossaryInputConfig.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  GlossaryInputConfig clone() => new GlossaryInputConfig()..mergeFromMessage(this);
  GlossaryInputConfig copyWith(void Function(GlossaryInputConfig) updates) => super.copyWith((message) => updates(message as GlossaryInputConfig));
  $pb.BuilderInfo get info_ => _i;
  static GlossaryInputConfig create() => new GlossaryInputConfig();
  GlossaryInputConfig createEmptyInstance() => create();
  static $pb.PbList<GlossaryInputConfig> createRepeated() => new $pb.PbList<GlossaryInputConfig>();
  static GlossaryInputConfig getDefault() => _defaultInstance ??= create()..freeze();
  static GlossaryInputConfig _defaultInstance;

  GlossaryInputConfig_Source whichSource() => _GlossaryInputConfig_SourceByTag[$_whichOneof(0)];
  void clearSource() => clearField($_whichOneof(0));

  GcsSource get gcsSource => $_getN(0);
  set gcsSource(GcsSource v) { setField(1, v); }
  bool hasGcsSource() => $_has(0);
  void clearGcsSource() => clearField(1);
}

class Glossary_LanguageCodePair extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Glossary.LanguageCodePair', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'sourceLanguageCode')
    ..aOS(2, 'targetLanguageCode')
    ..hasRequiredFields = false
  ;

  Glossary_LanguageCodePair() : super();
  Glossary_LanguageCodePair.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Glossary_LanguageCodePair.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Glossary_LanguageCodePair clone() => new Glossary_LanguageCodePair()..mergeFromMessage(this);
  Glossary_LanguageCodePair copyWith(void Function(Glossary_LanguageCodePair) updates) => super.copyWith((message) => updates(message as Glossary_LanguageCodePair));
  $pb.BuilderInfo get info_ => _i;
  static Glossary_LanguageCodePair create() => new Glossary_LanguageCodePair();
  Glossary_LanguageCodePair createEmptyInstance() => create();
  static $pb.PbList<Glossary_LanguageCodePair> createRepeated() => new $pb.PbList<Glossary_LanguageCodePair>();
  static Glossary_LanguageCodePair getDefault() => _defaultInstance ??= create()..freeze();
  static Glossary_LanguageCodePair _defaultInstance;

  String get sourceLanguageCode => $_getS(0, '');
  set sourceLanguageCode(String v) { $_setString(0, v); }
  bool hasSourceLanguageCode() => $_has(0);
  void clearSourceLanguageCode() => clearField(1);

  String get targetLanguageCode => $_getS(1, '');
  set targetLanguageCode(String v) { $_setString(1, v); }
  bool hasTargetLanguageCode() => $_has(1);
  void clearTargetLanguageCode() => clearField(2);
}

class Glossary_LanguageCodesSet extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Glossary.LanguageCodesSet', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..pPS(1, 'languageCodes')
    ..hasRequiredFields = false
  ;

  Glossary_LanguageCodesSet() : super();
  Glossary_LanguageCodesSet.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Glossary_LanguageCodesSet.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Glossary_LanguageCodesSet clone() => new Glossary_LanguageCodesSet()..mergeFromMessage(this);
  Glossary_LanguageCodesSet copyWith(void Function(Glossary_LanguageCodesSet) updates) => super.copyWith((message) => updates(message as Glossary_LanguageCodesSet));
  $pb.BuilderInfo get info_ => _i;
  static Glossary_LanguageCodesSet create() => new Glossary_LanguageCodesSet();
  Glossary_LanguageCodesSet createEmptyInstance() => create();
  static $pb.PbList<Glossary_LanguageCodesSet> createRepeated() => new $pb.PbList<Glossary_LanguageCodesSet>();
  static Glossary_LanguageCodesSet getDefault() => _defaultInstance ??= create()..freeze();
  static Glossary_LanguageCodesSet _defaultInstance;

  List<String> get languageCodes => $_getList(0);
}

enum Glossary_Languages {
  languagePair, 
  languageCodesSet, 
  notSet
}

class Glossary extends $pb.GeneratedMessage {
  static const Map<int, Glossary_Languages> _Glossary_LanguagesByTag = {
    3 : Glossary_Languages.languagePair,
    4 : Glossary_Languages.languageCodesSet,
    0 : Glossary_Languages.notSet
  };
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Glossary', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'name')
    ..a<Glossary_LanguageCodePair>(3, 'languagePair', $pb.PbFieldType.OM, Glossary_LanguageCodePair.getDefault, Glossary_LanguageCodePair.create)
    ..a<Glossary_LanguageCodesSet>(4, 'languageCodesSet', $pb.PbFieldType.OM, Glossary_LanguageCodesSet.getDefault, Glossary_LanguageCodesSet.create)
    ..a<GlossaryInputConfig>(5, 'inputConfig', $pb.PbFieldType.OM, GlossaryInputConfig.getDefault, GlossaryInputConfig.create)
    ..a<int>(6, 'entryCount', $pb.PbFieldType.O3)
    ..a<$2.Timestamp>(7, 'submitTime', $pb.PbFieldType.OM, $2.Timestamp.getDefault, $2.Timestamp.create)
    ..a<$2.Timestamp>(8, 'endTime', $pb.PbFieldType.OM, $2.Timestamp.getDefault, $2.Timestamp.create)
    ..oo(0, [3, 4])
    ..hasRequiredFields = false
  ;

  Glossary() : super();
  Glossary.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Glossary.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Glossary clone() => new Glossary()..mergeFromMessage(this);
  Glossary copyWith(void Function(Glossary) updates) => super.copyWith((message) => updates(message as Glossary));
  $pb.BuilderInfo get info_ => _i;
  static Glossary create() => new Glossary();
  Glossary createEmptyInstance() => create();
  static $pb.PbList<Glossary> createRepeated() => new $pb.PbList<Glossary>();
  static Glossary getDefault() => _defaultInstance ??= create()..freeze();
  static Glossary _defaultInstance;

  Glossary_Languages whichLanguages() => _Glossary_LanguagesByTag[$_whichOneof(0)];
  void clearLanguages() => clearField($_whichOneof(0));

  String get name => $_getS(0, '');
  set name(String v) { $_setString(0, v); }
  bool hasName() => $_has(0);
  void clearName() => clearField(1);

  Glossary_LanguageCodePair get languagePair => $_getN(1);
  set languagePair(Glossary_LanguageCodePair v) { setField(3, v); }
  bool hasLanguagePair() => $_has(1);
  void clearLanguagePair() => clearField(3);

  Glossary_LanguageCodesSet get languageCodesSet => $_getN(2);
  set languageCodesSet(Glossary_LanguageCodesSet v) { setField(4, v); }
  bool hasLanguageCodesSet() => $_has(2);
  void clearLanguageCodesSet() => clearField(4);

  GlossaryInputConfig get inputConfig => $_getN(3);
  set inputConfig(GlossaryInputConfig v) { setField(5, v); }
  bool hasInputConfig() => $_has(3);
  void clearInputConfig() => clearField(5);

  int get entryCount => $_get(4, 0);
  set entryCount(int v) { $_setSignedInt32(4, v); }
  bool hasEntryCount() => $_has(4);
  void clearEntryCount() => clearField(6);

  $2.Timestamp get submitTime => $_getN(5);
  set submitTime($2.Timestamp v) { setField(7, v); }
  bool hasSubmitTime() => $_has(5);
  void clearSubmitTime() => clearField(7);

  $2.Timestamp get endTime => $_getN(6);
  set endTime($2.Timestamp v) { setField(8, v); }
  bool hasEndTime() => $_has(6);
  void clearEndTime() => clearField(8);
}

class CreateGlossaryRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('CreateGlossaryRequest', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'parent')
    ..a<Glossary>(2, 'glossary', $pb.PbFieldType.OM, Glossary.getDefault, Glossary.create)
    ..hasRequiredFields = false
  ;

  CreateGlossaryRequest() : super();
  CreateGlossaryRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  CreateGlossaryRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  CreateGlossaryRequest clone() => new CreateGlossaryRequest()..mergeFromMessage(this);
  CreateGlossaryRequest copyWith(void Function(CreateGlossaryRequest) updates) => super.copyWith((message) => updates(message as CreateGlossaryRequest));
  $pb.BuilderInfo get info_ => _i;
  static CreateGlossaryRequest create() => new CreateGlossaryRequest();
  CreateGlossaryRequest createEmptyInstance() => create();
  static $pb.PbList<CreateGlossaryRequest> createRepeated() => new $pb.PbList<CreateGlossaryRequest>();
  static CreateGlossaryRequest getDefault() => _defaultInstance ??= create()..freeze();
  static CreateGlossaryRequest _defaultInstance;

  String get parent => $_getS(0, '');
  set parent(String v) { $_setString(0, v); }
  bool hasParent() => $_has(0);
  void clearParent() => clearField(1);

  Glossary get glossary => $_getN(1);
  set glossary(Glossary v) { setField(2, v); }
  bool hasGlossary() => $_has(1);
  void clearGlossary() => clearField(2);
}

class GetGlossaryRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('GetGlossaryRequest', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'name')
    ..hasRequiredFields = false
  ;

  GetGlossaryRequest() : super();
  GetGlossaryRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  GetGlossaryRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  GetGlossaryRequest clone() => new GetGlossaryRequest()..mergeFromMessage(this);
  GetGlossaryRequest copyWith(void Function(GetGlossaryRequest) updates) => super.copyWith((message) => updates(message as GetGlossaryRequest));
  $pb.BuilderInfo get info_ => _i;
  static GetGlossaryRequest create() => new GetGlossaryRequest();
  GetGlossaryRequest createEmptyInstance() => create();
  static $pb.PbList<GetGlossaryRequest> createRepeated() => new $pb.PbList<GetGlossaryRequest>();
  static GetGlossaryRequest getDefault() => _defaultInstance ??= create()..freeze();
  static GetGlossaryRequest _defaultInstance;

  String get name => $_getS(0, '');
  set name(String v) { $_setString(0, v); }
  bool hasName() => $_has(0);
  void clearName() => clearField(1);
}

class DeleteGlossaryRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('DeleteGlossaryRequest', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'name')
    ..hasRequiredFields = false
  ;

  DeleteGlossaryRequest() : super();
  DeleteGlossaryRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  DeleteGlossaryRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  DeleteGlossaryRequest clone() => new DeleteGlossaryRequest()..mergeFromMessage(this);
  DeleteGlossaryRequest copyWith(void Function(DeleteGlossaryRequest) updates) => super.copyWith((message) => updates(message as DeleteGlossaryRequest));
  $pb.BuilderInfo get info_ => _i;
  static DeleteGlossaryRequest create() => new DeleteGlossaryRequest();
  DeleteGlossaryRequest createEmptyInstance() => create();
  static $pb.PbList<DeleteGlossaryRequest> createRepeated() => new $pb.PbList<DeleteGlossaryRequest>();
  static DeleteGlossaryRequest getDefault() => _defaultInstance ??= create()..freeze();
  static DeleteGlossaryRequest _defaultInstance;

  String get name => $_getS(0, '');
  set name(String v) { $_setString(0, v); }
  bool hasName() => $_has(0);
  void clearName() => clearField(1);
}

class ListGlossariesRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('ListGlossariesRequest', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'parent')
    ..a<int>(2, 'pageSize', $pb.PbFieldType.O3)
    ..aOS(3, 'pageToken')
    ..aOS(4, 'filter')
    ..hasRequiredFields = false
  ;

  ListGlossariesRequest() : super();
  ListGlossariesRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  ListGlossariesRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  ListGlossariesRequest clone() => new ListGlossariesRequest()..mergeFromMessage(this);
  ListGlossariesRequest copyWith(void Function(ListGlossariesRequest) updates) => super.copyWith((message) => updates(message as ListGlossariesRequest));
  $pb.BuilderInfo get info_ => _i;
  static ListGlossariesRequest create() => new ListGlossariesRequest();
  ListGlossariesRequest createEmptyInstance() => create();
  static $pb.PbList<ListGlossariesRequest> createRepeated() => new $pb.PbList<ListGlossariesRequest>();
  static ListGlossariesRequest getDefault() => _defaultInstance ??= create()..freeze();
  static ListGlossariesRequest _defaultInstance;

  String get parent => $_getS(0, '');
  set parent(String v) { $_setString(0, v); }
  bool hasParent() => $_has(0);
  void clearParent() => clearField(1);

  int get pageSize => $_get(1, 0);
  set pageSize(int v) { $_setSignedInt32(1, v); }
  bool hasPageSize() => $_has(1);
  void clearPageSize() => clearField(2);

  String get pageToken => $_getS(2, '');
  set pageToken(String v) { $_setString(2, v); }
  bool hasPageToken() => $_has(2);
  void clearPageToken() => clearField(3);

  String get filter => $_getS(3, '');
  set filter(String v) { $_setString(3, v); }
  bool hasFilter() => $_has(3);
  void clearFilter() => clearField(4);
}

class ListGlossariesResponse extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('ListGlossariesResponse', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..pc<Glossary>(1, 'glossaries', $pb.PbFieldType.PM,Glossary.create)
    ..aOS(2, 'nextPageToken')
    ..hasRequiredFields = false
  ;

  ListGlossariesResponse() : super();
  ListGlossariesResponse.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  ListGlossariesResponse.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  ListGlossariesResponse clone() => new ListGlossariesResponse()..mergeFromMessage(this);
  ListGlossariesResponse copyWith(void Function(ListGlossariesResponse) updates) => super.copyWith((message) => updates(message as ListGlossariesResponse));
  $pb.BuilderInfo get info_ => _i;
  static ListGlossariesResponse create() => new ListGlossariesResponse();
  ListGlossariesResponse createEmptyInstance() => create();
  static $pb.PbList<ListGlossariesResponse> createRepeated() => new $pb.PbList<ListGlossariesResponse>();
  static ListGlossariesResponse getDefault() => _defaultInstance ??= create()..freeze();
  static ListGlossariesResponse _defaultInstance;

  List<Glossary> get glossaries => $_getList(0);

  String get nextPageToken => $_getS(1, '');
  set nextPageToken(String v) { $_setString(1, v); }
  bool hasNextPageToken() => $_has(1);
  void clearNextPageToken() => clearField(2);
}

class CreateGlossaryMetadata extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('CreateGlossaryMetadata', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'name')
    ..e<CreateGlossaryMetadata_State>(2, 'state', $pb.PbFieldType.OE, CreateGlossaryMetadata_State.STATE_UNSPECIFIED, CreateGlossaryMetadata_State.valueOf, CreateGlossaryMetadata_State.values)
    ..a<$2.Timestamp>(3, 'submitTime', $pb.PbFieldType.OM, $2.Timestamp.getDefault, $2.Timestamp.create)
    ..hasRequiredFields = false
  ;

  CreateGlossaryMetadata() : super();
  CreateGlossaryMetadata.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  CreateGlossaryMetadata.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  CreateGlossaryMetadata clone() => new CreateGlossaryMetadata()..mergeFromMessage(this);
  CreateGlossaryMetadata copyWith(void Function(CreateGlossaryMetadata) updates) => super.copyWith((message) => updates(message as CreateGlossaryMetadata));
  $pb.BuilderInfo get info_ => _i;
  static CreateGlossaryMetadata create() => new CreateGlossaryMetadata();
  CreateGlossaryMetadata createEmptyInstance() => create();
  static $pb.PbList<CreateGlossaryMetadata> createRepeated() => new $pb.PbList<CreateGlossaryMetadata>();
  static CreateGlossaryMetadata getDefault() => _defaultInstance ??= create()..freeze();
  static CreateGlossaryMetadata _defaultInstance;

  String get name => $_getS(0, '');
  set name(String v) { $_setString(0, v); }
  bool hasName() => $_has(0);
  void clearName() => clearField(1);

  CreateGlossaryMetadata_State get state => $_getN(1);
  set state(CreateGlossaryMetadata_State v) { setField(2, v); }
  bool hasState() => $_has(1);
  void clearState() => clearField(2);

  $2.Timestamp get submitTime => $_getN(2);
  set submitTime($2.Timestamp v) { setField(3, v); }
  bool hasSubmitTime() => $_has(2);
  void clearSubmitTime() => clearField(3);
}

class DeleteGlossaryMetadata extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('DeleteGlossaryMetadata', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'name')
    ..e<DeleteGlossaryMetadata_State>(2, 'state', $pb.PbFieldType.OE, DeleteGlossaryMetadata_State.STATE_UNSPECIFIED, DeleteGlossaryMetadata_State.valueOf, DeleteGlossaryMetadata_State.values)
    ..a<$2.Timestamp>(3, 'submitTime', $pb.PbFieldType.OM, $2.Timestamp.getDefault, $2.Timestamp.create)
    ..hasRequiredFields = false
  ;

  DeleteGlossaryMetadata() : super();
  DeleteGlossaryMetadata.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  DeleteGlossaryMetadata.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  DeleteGlossaryMetadata clone() => new DeleteGlossaryMetadata()..mergeFromMessage(this);
  DeleteGlossaryMetadata copyWith(void Function(DeleteGlossaryMetadata) updates) => super.copyWith((message) => updates(message as DeleteGlossaryMetadata));
  $pb.BuilderInfo get info_ => _i;
  static DeleteGlossaryMetadata create() => new DeleteGlossaryMetadata();
  DeleteGlossaryMetadata createEmptyInstance() => create();
  static $pb.PbList<DeleteGlossaryMetadata> createRepeated() => new $pb.PbList<DeleteGlossaryMetadata>();
  static DeleteGlossaryMetadata getDefault() => _defaultInstance ??= create()..freeze();
  static DeleteGlossaryMetadata _defaultInstance;

  String get name => $_getS(0, '');
  set name(String v) { $_setString(0, v); }
  bool hasName() => $_has(0);
  void clearName() => clearField(1);

  DeleteGlossaryMetadata_State get state => $_getN(1);
  set state(DeleteGlossaryMetadata_State v) { setField(2, v); }
  bool hasState() => $_has(1);
  void clearState() => clearField(2);

  $2.Timestamp get submitTime => $_getN(2);
  set submitTime($2.Timestamp v) { setField(3, v); }
  bool hasSubmitTime() => $_has(2);
  void clearSubmitTime() => clearField(3);
}

class DeleteGlossaryResponse extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('DeleteGlossaryResponse', package: const $pb.PackageName('google.cloud.translation.v3beta1'))
    ..aOS(1, 'name')
    ..a<$2.Timestamp>(2, 'submitTime', $pb.PbFieldType.OM, $2.Timestamp.getDefault, $2.Timestamp.create)
    ..a<$2.Timestamp>(3, 'endTime', $pb.PbFieldType.OM, $2.Timestamp.getDefault, $2.Timestamp.create)
    ..hasRequiredFields = false
  ;

  DeleteGlossaryResponse() : super();
  DeleteGlossaryResponse.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  DeleteGlossaryResponse.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  DeleteGlossaryResponse clone() => new DeleteGlossaryResponse()..mergeFromMessage(this);
  DeleteGlossaryResponse copyWith(void Function(DeleteGlossaryResponse) updates) => super.copyWith((message) => updates(message as DeleteGlossaryResponse));
  $pb.BuilderInfo get info_ => _i;
  static DeleteGlossaryResponse create() => new DeleteGlossaryResponse();
  DeleteGlossaryResponse createEmptyInstance() => create();
  static $pb.PbList<DeleteGlossaryResponse> createRepeated() => new $pb.PbList<DeleteGlossaryResponse>();
  static DeleteGlossaryResponse getDefault() => _defaultInstance ??= create()..freeze();
  static DeleteGlossaryResponse _defaultInstance;

  String get name => $_getS(0, '');
  set name(String v) { $_setString(0, v); }
  bool hasName() => $_has(0);
  void clearName() => clearField(1);

  $2.Timestamp get submitTime => $_getN(1);
  set submitTime($2.Timestamp v) { setField(2, v); }
  bool hasSubmitTime() => $_has(1);
  void clearSubmitTime() => clearField(2);

  $2.Timestamp get endTime => $_getN(2);
  set endTime($2.Timestamp v) { setField(3, v); }
  bool hasEndTime() => $_has(2);
  void clearEndTime() => clearField(3);
}

