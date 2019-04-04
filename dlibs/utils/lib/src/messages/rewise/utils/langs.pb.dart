///
//  Generated code. Do not modify.
//  source: rewise/utils/langs.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

class CldrLangs extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('CldrLangs', package: const $pb.PackageName('rewiseDom'))
    ..pc<CldrLang>(1, 'langs', $pb.PbFieldType.PM,CldrLang.create)
    ..hasRequiredFields = false
  ;

  CldrLangs() : super();
  CldrLangs.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  CldrLangs.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  CldrLangs clone() => new CldrLangs()..mergeFromMessage(this);
  CldrLangs copyWith(void Function(CldrLangs) updates) => super.copyWith((message) => updates(message as CldrLangs));
  $pb.BuilderInfo get info_ => _i;
  static CldrLangs create() => new CldrLangs();
  CldrLangs createEmptyInstance() => create();
  static $pb.PbList<CldrLangs> createRepeated() => new $pb.PbList<CldrLangs>();
  static CldrLangs getDefault() => _defaultInstance ??= create()..freeze();
  static CldrLangs _defaultInstance;

  List<CldrLang> get langs => $_getList(0);
}

class CldrLang extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('CldrLang', package: const $pb.PackageName('rewiseDom'))
    ..aOS(1, 'id')
    ..aOS(2, 'lang')
    ..aOS(3, 'scriptId')
    ..aOS(4, 'defaultRegion')
    ..aOB(5, 'hasMoreScripts')
    ..aOB(6, 'hasStemming')
    ..aOS(7, 'alphabet')
    ..aOS(8, 'alphabetUpper')
    ..hasRequiredFields = false
  ;

  CldrLang() : super();
  CldrLang.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  CldrLang.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  CldrLang clone() => new CldrLang()..mergeFromMessage(this);
  CldrLang copyWith(void Function(CldrLang) updates) => super.copyWith((message) => updates(message as CldrLang));
  $pb.BuilderInfo get info_ => _i;
  static CldrLang create() => new CldrLang();
  CldrLang createEmptyInstance() => create();
  static $pb.PbList<CldrLang> createRepeated() => new $pb.PbList<CldrLang>();
  static CldrLang getDefault() => _defaultInstance ??= create()..freeze();
  static CldrLang _defaultInstance;

  String get id => $_getS(0, '');
  set id(String v) { $_setString(0, v); }
  bool hasId() => $_has(0);
  void clearId() => clearField(1);

  String get lang => $_getS(1, '');
  set lang(String v) { $_setString(1, v); }
  bool hasLang() => $_has(1);
  void clearLang() => clearField(2);

  String get scriptId => $_getS(2, '');
  set scriptId(String v) { $_setString(2, v); }
  bool hasScriptId() => $_has(2);
  void clearScriptId() => clearField(3);

  String get defaultRegion => $_getS(3, '');
  set defaultRegion(String v) { $_setString(3, v); }
  bool hasDefaultRegion() => $_has(3);
  void clearDefaultRegion() => clearField(4);

  bool get hasMoreScripts => $_get(4, false);
  set hasMoreScripts(bool v) { $_setBool(4, v); }
  bool hasHasMoreScripts() => $_has(4);
  void clearHasMoreScripts() => clearField(5);

  bool get hasStemming => $_get(5, false);
  set hasStemming(bool v) { $_setBool(5, v); }
  bool hasHasStemming() => $_has(5);
  void clearHasStemming() => clearField(6);

  String get alphabet => $_getS(6, '');
  set alphabet(String v) { $_setString(6, v); }
  bool hasAlphabet() => $_has(6);
  void clearAlphabet() => clearField(7);

  String get alphabetUpper => $_getS(7, '');
  set alphabetUpper(String v) { $_setString(7, v); }
  bool hasAlphabetUpper() => $_has(7);
  void clearAlphabetUpper() => clearField(8);
}

class UncRange extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('UncRange', package: const $pb.PackageName('rewiseDom'))
    ..a<int>(1, 'start', $pb.PbFieldType.O3)
    ..a<int>(2, 'end', $pb.PbFieldType.O3)
    ..a<int>(3, 'idx', $pb.PbFieldType.O3)
    ..hasRequiredFields = false
  ;

  UncRange() : super();
  UncRange.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  UncRange.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  UncRange clone() => new UncRange()..mergeFromMessage(this);
  UncRange copyWith(void Function(UncRange) updates) => super.copyWith((message) => updates(message as UncRange));
  $pb.BuilderInfo get info_ => _i;
  static UncRange create() => new UncRange();
  UncRange createEmptyInstance() => create();
  static $pb.PbList<UncRange> createRepeated() => new $pb.PbList<UncRange>();
  static UncRange getDefault() => _defaultInstance ??= create()..freeze();
  static UncRange _defaultInstance;

  int get start => $_get(0, 0);
  set start(int v) { $_setSignedInt32(0, v); }
  bool hasStart() => $_has(0);
  void clearStart() => clearField(1);

  int get end => $_get(1, 0);
  set end(int v) { $_setSignedInt32(1, v); }
  bool hasEnd() => $_has(1);
  void clearEnd() => clearField(2);

  int get idx => $_get(2, 0);
  set idx(int v) { $_setSignedInt32(2, v); }
  bool hasIdx() => $_has(2);
  void clearIdx() => clearField(3);
}

class UncBlocks extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('UncBlocks', package: const $pb.PackageName('rewiseDom'))
    ..pPS(1, 'iSO15924')
    ..pc<UncRange>(2, 'ranges', $pb.PbFieldType.PM,UncRange.create)
    ..hasRequiredFields = false
  ;

  UncBlocks() : super();
  UncBlocks.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  UncBlocks.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  UncBlocks clone() => new UncBlocks()..mergeFromMessage(this);
  UncBlocks copyWith(void Function(UncBlocks) updates) => super.copyWith((message) => updates(message as UncBlocks));
  $pb.BuilderInfo get info_ => _i;
  static UncBlocks create() => new UncBlocks();
  UncBlocks createEmptyInstance() => create();
  static $pb.PbList<UncBlocks> createRepeated() => new $pb.PbList<UncBlocks>();
  static UncBlocks getDefault() => _defaultInstance ??= create()..freeze();
  static UncBlocks _defaultInstance;

  List<String> get iSO15924 => $_getList(0);

  List<UncRange> get ranges => $_getList(1);
}

