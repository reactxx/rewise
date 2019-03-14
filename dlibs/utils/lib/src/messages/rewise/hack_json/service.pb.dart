///
//  Generated code. Do not modify.
//  source: rewise/hack_json/service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

import '../../google/protobuf/wrappers.pb.dart' as $0;

class HackJsonString extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('HackJsonString', package: const $pb.PackageName('rw.hack_json'))
    ..aOS(1, 'qualifiedMessageName')
    ..a<$0.StringValue>(2, 'value', $pb.PbFieldType.OM, $0.StringValue.getDefault, $0.StringValue.create)
    ..hasRequiredFields = false
  ;

  HackJsonString() : super();
  HackJsonString.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  HackJsonString.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  HackJsonString clone() => new HackJsonString()..mergeFromMessage(this);
  HackJsonString copyWith(void Function(HackJsonString) updates) => super.copyWith((message) => updates(message as HackJsonString));
  $pb.BuilderInfo get info_ => _i;
  static HackJsonString create() => new HackJsonString();
  HackJsonString createEmptyInstance() => create();
  static $pb.PbList<HackJsonString> createRepeated() => new $pb.PbList<HackJsonString>();
  static HackJsonString getDefault() => _defaultInstance ??= create()..freeze();
  static HackJsonString _defaultInstance;

  String get qualifiedMessageName => $_getS(0, '');
  set qualifiedMessageName(String v) { $_setString(0, v); }
  bool hasQualifiedMessageName() => $_has(0);
  void clearQualifiedMessageName() => clearField(1);

  $0.StringValue get value => $_getN(1);
  set value($0.StringValue v) { setField(2, v); }
  bool hasValue() => $_has(1);
  void clearValue() => clearField(2);
}

class HackJsonBytes extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('HackJsonBytes', package: const $pb.PackageName('rw.hack_json'))
    ..aOS(1, 'qualifiedMessageName')
    ..a<$0.BytesValue>(2, 'value', $pb.PbFieldType.OM, $0.BytesValue.getDefault, $0.BytesValue.create)
    ..hasRequiredFields = false
  ;

  HackJsonBytes() : super();
  HackJsonBytes.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  HackJsonBytes.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  HackJsonBytes clone() => new HackJsonBytes()..mergeFromMessage(this);
  HackJsonBytes copyWith(void Function(HackJsonBytes) updates) => super.copyWith((message) => updates(message as HackJsonBytes));
  $pb.BuilderInfo get info_ => _i;
  static HackJsonBytes create() => new HackJsonBytes();
  HackJsonBytes createEmptyInstance() => create();
  static $pb.PbList<HackJsonBytes> createRepeated() => new $pb.PbList<HackJsonBytes>();
  static HackJsonBytes getDefault() => _defaultInstance ??= create()..freeze();
  static HackJsonBytes _defaultInstance;

  String get qualifiedMessageName => $_getS(0, '');
  set qualifiedMessageName(String v) { $_setString(0, v); }
  bool hasQualifiedMessageName() => $_has(0);
  void clearQualifiedMessageName() => clearField(1);

  $0.BytesValue get value => $_getN(1);
  set value($0.BytesValue v) { setField(2, v); }
  bool hasValue() => $_has(1);
  void clearValue() => clearField(2);
}

