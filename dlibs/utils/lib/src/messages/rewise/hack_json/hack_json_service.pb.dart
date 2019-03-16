///
//  Generated code. Do not modify.
//  source: rewise/hack_json/hack_json_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

import '../utils/common.pb.dart' as $1;

class HackJsonPar extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('HackJsonPar', package: const $pb.PackageName('rw.hack_json'))
    ..aOS(1, 'qualifiedMessageName')
    ..aOB(2, 'isToJson')
    ..aOS(3, 's')
    ..a<List<int>>(4, 'b', $pb.PbFieldType.OY)
    ..hasRequiredFields = false
  ;

  HackJsonPar() : super();
  HackJsonPar.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  HackJsonPar.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  HackJsonPar clone() => new HackJsonPar()..mergeFromMessage(this);
  HackJsonPar copyWith(void Function(HackJsonPar) updates) => super.copyWith((message) => updates(message as HackJsonPar));
  $pb.BuilderInfo get info_ => _i;
  static HackJsonPar create() => new HackJsonPar();
  HackJsonPar createEmptyInstance() => create();
  static $pb.PbList<HackJsonPar> createRepeated() => new $pb.PbList<HackJsonPar>();
  static HackJsonPar getDefault() => _defaultInstance ??= create()..freeze();
  static HackJsonPar _defaultInstance;

  String get qualifiedMessageName => $_getS(0, '');
  set qualifiedMessageName(String v) { $_setString(0, v); }
  bool hasQualifiedMessageName() => $_has(0);
  void clearQualifiedMessageName() => clearField(1);

  bool get isToJson => $_get(1, false);
  set isToJson(bool v) { $_setBool(1, v); }
  bool hasIsToJson() => $_has(1);
  void clearIsToJson() => clearField(2);

  String get s => $_getS(2, '');
  set s(String v) { $_setString(2, v); }
  bool hasS() => $_has(2);
  void clearS() => clearField(3);

  List<int> get b => $_getN(3);
  set b(List<int> v) { $_setBytes(3, v); }
  bool hasB() => $_has(3);
  void clearB() => clearField(4);
}

class HackJsonFilePar extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('HackJsonFilePar', package: const $pb.PackageName('rw.hack_json'))
    ..aOS(1, 'qualifiedMessageName')
    ..aOB(2, 'isToJson')
    ..a<$1.FromToFiles>(3, 'files', $pb.PbFieldType.OM, $1.FromToFiles.getDefault, $1.FromToFiles.create)
    ..hasRequiredFields = false
  ;

  HackJsonFilePar() : super();
  HackJsonFilePar.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  HackJsonFilePar.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  HackJsonFilePar clone() => new HackJsonFilePar()..mergeFromMessage(this);
  HackJsonFilePar copyWith(void Function(HackJsonFilePar) updates) => super.copyWith((message) => updates(message as HackJsonFilePar));
  $pb.BuilderInfo get info_ => _i;
  static HackJsonFilePar create() => new HackJsonFilePar();
  HackJsonFilePar createEmptyInstance() => create();
  static $pb.PbList<HackJsonFilePar> createRepeated() => new $pb.PbList<HackJsonFilePar>();
  static HackJsonFilePar getDefault() => _defaultInstance ??= create()..freeze();
  static HackJsonFilePar _defaultInstance;

  String get qualifiedMessageName => $_getS(0, '');
  set qualifiedMessageName(String v) { $_setString(0, v); }
  bool hasQualifiedMessageName() => $_has(0);
  void clearQualifiedMessageName() => clearField(1);

  bool get isToJson => $_get(1, false);
  set isToJson(bool v) { $_setBool(1, v); }
  bool hasIsToJson() => $_has(1);
  void clearIsToJson() => clearField(2);

  $1.FromToFiles get files => $_getN(2);
  set files($1.FromToFiles v) { setField(3, v); }
  bool hasFiles() => $_has(2);
  void clearFiles() => clearField(3);
}

