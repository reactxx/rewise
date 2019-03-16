///
//  Generated code. Do not modify.
//  source: rewise/utils/common.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

import '../../google/protobuf/wrappers.pb.dart' as $0;

class BytesList extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('BytesList', package: const $pb.PackageName('rw.common'))
    ..pc<$0.BytesValue>(1, 'list', $pb.PbFieldType.PM,$0.BytesValue.create)
    ..hasRequiredFields = false
  ;

  BytesList() : super();
  BytesList.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  BytesList.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  BytesList clone() => new BytesList()..mergeFromMessage(this);
  BytesList copyWith(void Function(BytesList) updates) => super.copyWith((message) => updates(message as BytesList));
  $pb.BuilderInfo get info_ => _i;
  static BytesList create() => new BytesList();
  BytesList createEmptyInstance() => create();
  static $pb.PbList<BytesList> createRepeated() => new $pb.PbList<BytesList>();
  static BytesList getDefault() => _defaultInstance ??= create()..freeze();
  static BytesList _defaultInstance;

  List<$0.BytesValue> get list => $_getList(0);
}

class FromToFiles extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('FromToFiles', package: const $pb.PackageName('rw.common'))
    ..aOS(1, 'src')
    ..aOS(2, 'dest')
    ..hasRequiredFields = false
  ;

  FromToFiles() : super();
  FromToFiles.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  FromToFiles.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  FromToFiles clone() => new FromToFiles()..mergeFromMessage(this);
  FromToFiles copyWith(void Function(FromToFiles) updates) => super.copyWith((message) => updates(message as FromToFiles));
  $pb.BuilderInfo get info_ => _i;
  static FromToFiles create() => new FromToFiles();
  FromToFiles createEmptyInstance() => create();
  static $pb.PbList<FromToFiles> createRepeated() => new $pb.PbList<FromToFiles>();
  static FromToFiles getDefault() => _defaultInstance ??= create()..freeze();
  static FromToFiles _defaultInstance;

  String get src => $_getS(0, '');
  set src(String v) { $_setString(0, v); }
  bool hasSrc() => $_has(0);
  void clearSrc() => clearField(1);

  String get dest => $_getS(1, '');
  set dest(String v) { $_setString(1, v); }
  bool hasDest() => $_has(1);
  void clearDest() => clearField(2);
}

