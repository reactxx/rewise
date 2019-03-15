///
//  Generated code. Do not modify.
//  source: rewise/to_raw/to_raw_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

class Request extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Request', package: const $pb.PackageName('rw.to_raw'))
    ..pc<Files>(1, 'files', $pb.PbFieldType.PM,Files.create)
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

  List<Files> get files => $_getList(0);
}

class Files extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Files', package: const $pb.PackageName('rw.to_raw'))
    ..aOS(1, 'srcRj')
    ..aOS(2, 'destRaw')
    ..hasRequiredFields = false
  ;

  Files() : super();
  Files.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Files.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Files clone() => new Files()..mergeFromMessage(this);
  Files copyWith(void Function(Files) updates) => super.copyWith((message) => updates(message as Files));
  $pb.BuilderInfo get info_ => _i;
  static Files create() => new Files();
  Files createEmptyInstance() => create();
  static $pb.PbList<Files> createRepeated() => new $pb.PbList<Files>();
  static Files getDefault() => _defaultInstance ??= create()..freeze();
  static Files _defaultInstance;

  String get srcRj => $_getS(0, '');
  set srcRj(String v) { $_setString(0, v); }
  bool hasSrcRj() => $_has(0);
  void clearSrcRj() => clearField(1);

  String get destRaw => $_getS(1, '');
  set destRaw(String v) { $_setString(1, v); }
  bool hasDestRaw() => $_has(1);
  void clearDestRaw() => clearField(2);
}

class Response extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Response', package: const $pb.PackageName('rw.to_raw'))
    ..aOS(1, 'error')
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

  String get error => $_getS(0, '');
  set error(String v) { $_setString(0, v); }
  bool hasError() => $_has(0);
  void clearError() => clearField(1);
}

