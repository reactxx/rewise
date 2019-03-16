///
//  Generated code. Do not modify.
//  source: rewise/hallo_world/hello_world_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

class HelloRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('HelloRequest', package: const $pb.PackageName('rw.hallo_world'))
    ..aOB(1, 'noRecursion')
    ..a<int>(2, 'dartCount', $pb.PbFieldType.O3)
    ..a<int>(3, 'dartId', $pb.PbFieldType.O3)
    ..a<int>(4, 'csharpId', $pb.PbFieldType.O3)
    ..hasRequiredFields = false
  ;

  HelloRequest() : super();
  HelloRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  HelloRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  HelloRequest clone() => new HelloRequest()..mergeFromMessage(this);
  HelloRequest copyWith(void Function(HelloRequest) updates) => super.copyWith((message) => updates(message as HelloRequest));
  $pb.BuilderInfo get info_ => _i;
  static HelloRequest create() => new HelloRequest();
  HelloRequest createEmptyInstance() => create();
  static $pb.PbList<HelloRequest> createRepeated() => new $pb.PbList<HelloRequest>();
  static HelloRequest getDefault() => _defaultInstance ??= create()..freeze();
  static HelloRequest _defaultInstance;

  bool get noRecursion => $_get(0, false);
  set noRecursion(bool v) { $_setBool(0, v); }
  bool hasNoRecursion() => $_has(0);
  void clearNoRecursion() => clearField(1);

  int get dartCount => $_get(1, 0);
  set dartCount(int v) { $_setSignedInt32(1, v); }
  bool hasDartCount() => $_has(1);
  void clearDartCount() => clearField(2);

  int get dartId => $_get(2, 0);
  set dartId(int v) { $_setSignedInt32(2, v); }
  bool hasDartId() => $_has(2);
  void clearDartId() => clearField(3);

  int get csharpId => $_get(3, 0);
  set csharpId(int v) { $_setSignedInt32(3, v); }
  bool hasCsharpId() => $_has(3);
  void clearCsharpId() => clearField(4);
}

class HelloReply extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('HelloReply', package: const $pb.PackageName('rw.hallo_world'))
    ..a<int>(1, 'dartId', $pb.PbFieldType.O3)
    ..a<int>(2, 'csharpId', $pb.PbFieldType.O3)
    ..a<int>(3, 'dartCount', $pb.PbFieldType.O3)
    ..hasRequiredFields = false
  ;

  HelloReply() : super();
  HelloReply.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  HelloReply.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  HelloReply clone() => new HelloReply()..mergeFromMessage(this);
  HelloReply copyWith(void Function(HelloReply) updates) => super.copyWith((message) => updates(message as HelloReply));
  $pb.BuilderInfo get info_ => _i;
  static HelloReply create() => new HelloReply();
  HelloReply createEmptyInstance() => create();
  static $pb.PbList<HelloReply> createRepeated() => new $pb.PbList<HelloReply>();
  static HelloReply getDefault() => _defaultInstance ??= create()..freeze();
  static HelloReply _defaultInstance;

  int get dartId => $_get(0, 0);
  set dartId(int v) { $_setSignedInt32(0, v); }
  bool hasDartId() => $_has(0);
  void clearDartId() => clearField(1);

  int get csharpId => $_get(1, 0);
  set csharpId(int v) { $_setSignedInt32(1, v); }
  bool hasCsharpId() => $_has(1);
  void clearCsharpId() => clearField(2);

  int get dartCount => $_get(2, 0);
  set dartCount(int v) { $_setSignedInt32(2, v); }
  bool hasDartCount() => $_has(2);
  void clearDartCount() => clearField(3);
}

