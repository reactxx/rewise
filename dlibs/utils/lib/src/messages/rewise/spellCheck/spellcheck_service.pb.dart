///
//  Generated code. Do not modify.
//  source: rewise/spellCheck/spellcheck_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

class Request extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Request', package: const $pb.PackageName('rw.spellcheck'))
    ..aOS(1, 'lang')
    ..aOS(2, 'html')
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

  String get html => $_getS(1, '');
  set html(String v) { $_setString(1, v); }
  bool hasHtml() => $_has(1);
  void clearHtml() => clearField(2);
}

class Response extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Response', package: const $pb.PackageName('rw.spellcheck'))
    ..p<int>(1, 'wrongIdxs', $pb.PbFieldType.P3)
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

  List<int> get wrongIdxs => $_getList(0);
}

