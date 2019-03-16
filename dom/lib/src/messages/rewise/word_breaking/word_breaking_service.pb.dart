///
//  Generated code. Do not modify.
//  source: rewise/word_breaking/word_breaking_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

class Request extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Request', package: const $pb.PackageName('rw.word_breaking'))
    ..aOS(1, 'lang')
    ..pPS(2, 'facts')
    ..hasRequiredFields = false
  ;

  Request() : super();
  Request.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Request.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Request clone() => new Request()..mergeFromMessage(this);
  Request copyWith(void Function(Request) updates) => super.copyWith((message) => updates(message as Request));
  $pb.BuilderInfo get info_ => _i;
  static Request create() => new Request();
  Request createEmptyInstance() => create();
  static $pb.PbList<Request> createRepeated() => new $pb.PbList<Request>();
  static Request getDefault() => _defaultInstance ??= create()..freeze();
  static Request _defaultInstance;

  String get lang => $_getS(0, '');
  set lang(String v) { $_setString(0, v); }
  bool hasLang() => $_has(0);
  void clearLang() => clearField(1);

  List<String> get facts => $_getList(1);
}

class Response extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Response', package: const $pb.PackageName('rw.word_breaking'))
    ..pc<Breaks>(1, 'facts', $pb.PbFieldType.PM,Breaks.create)
    ..hasRequiredFields = false
  ;

  Response() : super();
  Response.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Response.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Response clone() => new Response()..mergeFromMessage(this);
  Response copyWith(void Function(Response) updates) => super.copyWith((message) => updates(message as Response));
  $pb.BuilderInfo get info_ => _i;
  static Response create() => new Response();
  Response createEmptyInstance() => create();
  static $pb.PbList<Response> createRepeated() => new $pb.PbList<Response>();
  static Response getDefault() => _defaultInstance ??= create()..freeze();
  static Response _defaultInstance;

  List<Breaks> get facts => $_getList(0);
}

class Breaks extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Breaks', package: const $pb.PackageName('rw.word_breaking'))
    ..a<List<int>>(1, 'breaks', $pb.PbFieldType.OY)
    ..hasRequiredFields = false
  ;

  Breaks() : super();
  Breaks.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Breaks.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Breaks clone() => new Breaks()..mergeFromMessage(this);
  Breaks copyWith(void Function(Breaks) updates) => super.copyWith((message) => updates(message as Breaks));
  $pb.BuilderInfo get info_ => _i;
  static Breaks create() => new Breaks();
  Breaks createEmptyInstance() => create();
  static $pb.PbList<Breaks> createRepeated() => new $pb.PbList<Breaks>();
  static Breaks getDefault() => _defaultInstance ??= create()..freeze();
  static Breaks _defaultInstance;

  List<int> get breaks => $_getN(0);
  set breaks(List<int> v) { $_setBytes(0, v); }
  bool hasBreaks() => $_has(0);
  void clearBreaks() => clearField(1);
}

