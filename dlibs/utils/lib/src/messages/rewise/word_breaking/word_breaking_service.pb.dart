///
//  Generated code. Do not modify.
//  source: rewise/word_breaking/word_breaking_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

class Request2 extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Request2', package: const $pb.PackageName('rw.word_breaking'))
    ..aOS(1, 'lang')
    ..pc<FactReq>(2, 'facts', $pb.PbFieldType.PM,FactReq.create)
    ..aOS(3, 'path')
    ..hasRequiredFields = false
  ;

  Request2() : super();
  Request2.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Request2.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Request2 clone() => new Request2()..mergeFromMessage(this);
  Request2 copyWith(void Function(Request2) updates) => super.copyWith((message) => updates(message as Request2));
  $pb.BuilderInfo get info_ => _i;
  static Request2 create() => new Request2();
  Request2 createEmptyInstance() => create();
  static $pb.PbList<Request2> createRepeated() => new $pb.PbList<Request2>();
  static Request2 getDefault() => _defaultInstance ??= create()..freeze();
  static Request2 _defaultInstance;

  String get lang => $_getS(0, '');
  set lang(String v) { $_setString(0, v); }
  bool hasLang() => $_has(0);
  void clearLang() => clearField(1);

  List<FactReq> get facts => $_getList(1);

  String get path => $_getS(2, '');
  set path(String v) { $_setString(2, v); }
  bool hasPath() => $_has(2);
  void clearPath() => clearField(3);
}

class FactReq extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('FactReq', package: const $pb.PackageName('rw.word_breaking'))
    ..a<int>(1, 'id', $pb.PbFieldType.O3)
    ..aOS(2, 'text')
    ..hasRequiredFields = false
  ;

  FactReq() : super();
  FactReq.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  FactReq.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  FactReq clone() => new FactReq()..mergeFromMessage(this);
  FactReq copyWith(void Function(FactReq) updates) => super.copyWith((message) => updates(message as FactReq));
  $pb.BuilderInfo get info_ => _i;
  static FactReq create() => new FactReq();
  FactReq createEmptyInstance() => create();
  static $pb.PbList<FactReq> createRepeated() => new $pb.PbList<FactReq>();
  static FactReq getDefault() => _defaultInstance ??= create()..freeze();
  static FactReq _defaultInstance;

  int get id => $_get(0, 0);
  set id(int v) { $_setSignedInt32(0, v); }
  bool hasId() => $_has(0);
  void clearId() => clearField(1);

  String get text => $_getS(1, '');
  set text(String v) { $_setString(1, v); }
  bool hasText() => $_has(1);
  void clearText() => clearField(2);
}

class Response2 extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Response2', package: const $pb.PackageName('rw.word_breaking'))
    ..pc<FactResp>(1, 'facts', $pb.PbFieldType.PM,FactResp.create)
    ..hasRequiredFields = false
  ;

  Response2() : super();
  Response2.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Response2.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Response2 clone() => new Response2()..mergeFromMessage(this);
  Response2 copyWith(void Function(Response2) updates) => super.copyWith((message) => updates(message as Response2));
  $pb.BuilderInfo get info_ => _i;
  static Response2 create() => new Response2();
  Response2 createEmptyInstance() => create();
  static $pb.PbList<Response2> createRepeated() => new $pb.PbList<Response2>();
  static Response2 getDefault() => _defaultInstance ??= create()..freeze();
  static Response2 _defaultInstance;

  List<FactResp> get facts => $_getList(0);
}

class FactResp extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('FactResp', package: const $pb.PackageName('rw.word_breaking'))
    ..a<int>(1, 'id', $pb.PbFieldType.O3)
    ..aOS(2, 'text')
    ..pc<PosLen>(3, 'posLens', $pb.PbFieldType.PM,PosLen.create)
    ..hasRequiredFields = false
  ;

  FactResp() : super();
  FactResp.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  FactResp.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  FactResp clone() => new FactResp()..mergeFromMessage(this);
  FactResp copyWith(void Function(FactResp) updates) => super.copyWith((message) => updates(message as FactResp));
  $pb.BuilderInfo get info_ => _i;
  static FactResp create() => new FactResp();
  FactResp createEmptyInstance() => create();
  static $pb.PbList<FactResp> createRepeated() => new $pb.PbList<FactResp>();
  static FactResp getDefault() => _defaultInstance ??= create()..freeze();
  static FactResp _defaultInstance;

  int get id => $_get(0, 0);
  set id(int v) { $_setSignedInt32(0, v); }
  bool hasId() => $_has(0);
  void clearId() => clearField(1);

  String get text => $_getS(1, '');
  set text(String v) { $_setString(1, v); }
  bool hasText() => $_has(1);
  void clearText() => clearField(2);

  List<PosLen> get posLens => $_getList(2);
}

class PosLen extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('PosLen', package: const $pb.PackageName('rw.word_breaking'))
    ..a<int>(1, 'pos', $pb.PbFieldType.O3)
    ..a<int>(2, 'end', $pb.PbFieldType.O3)
    ..hasRequiredFields = false
  ;

  PosLen() : super();
  PosLen.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  PosLen.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  PosLen clone() => new PosLen()..mergeFromMessage(this);
  PosLen copyWith(void Function(PosLen) updates) => super.copyWith((message) => updates(message as PosLen));
  $pb.BuilderInfo get info_ => _i;
  static PosLen create() => new PosLen();
  PosLen createEmptyInstance() => create();
  static $pb.PbList<PosLen> createRepeated() => new $pb.PbList<PosLen>();
  static PosLen getDefault() => _defaultInstance ??= create()..freeze();
  static PosLen _defaultInstance;

  int get pos => $_get(0, 0);
  set pos(int v) { $_setSignedInt32(0, v); }
  bool hasPos() => $_has(0);
  void clearPos() => clearField(1);

  int get end => $_get(1, 0);
  set end(int v) { $_setSignedInt32(1, v); }
  bool hasEnd() => $_has(1);
  void clearEnd() => clearField(2);
}

