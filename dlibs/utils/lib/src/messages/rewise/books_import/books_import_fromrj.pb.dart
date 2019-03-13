///
//  Generated code. Do not modify.
//  source: rewise/books_import/books_import_fromrj.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

class ImportRJRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('ImportRJRequest', package: const $pb.PackageName('rewiseDom'))
    ..pc<RJFileNames>(1, 'fileNames', $pb.PbFieldType.PM,RJFileNames.create)
    ..hasRequiredFields = false
  ;

  ImportRJRequest() : super();
  ImportRJRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  ImportRJRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  ImportRJRequest clone() => new ImportRJRequest()..mergeFromMessage(this);
  ImportRJRequest copyWith(void Function(ImportRJRequest) updates) => super.copyWith((message) => updates(message as ImportRJRequest));
  $pb.BuilderInfo get info_ => _i;
  static ImportRJRequest create() => new ImportRJRequest();
  ImportRJRequest createEmptyInstance() => create();
  static $pb.PbList<ImportRJRequest> createRepeated() => new $pb.PbList<ImportRJRequest>();
  static ImportRJRequest getDefault() => _defaultInstance ??= create()..freeze();
  static ImportRJRequest _defaultInstance;

  List<RJFileNames> get fileNames => $_getList(0);
}

class RJFileNames extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('RJFileNames', package: const $pb.PackageName('rewiseDom'))
    ..aOS(1, 'matrix')
    ..aOS(2, 'bin')
    ..hasRequiredFields = false
  ;

  RJFileNames() : super();
  RJFileNames.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  RJFileNames.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  RJFileNames clone() => new RJFileNames()..mergeFromMessage(this);
  RJFileNames copyWith(void Function(RJFileNames) updates) => super.copyWith((message) => updates(message as RJFileNames));
  $pb.BuilderInfo get info_ => _i;
  static RJFileNames create() => new RJFileNames();
  RJFileNames createEmptyInstance() => create();
  static $pb.PbList<RJFileNames> createRepeated() => new $pb.PbList<RJFileNames>();
  static RJFileNames getDefault() => _defaultInstance ??= create()..freeze();
  static RJFileNames _defaultInstance;

  String get matrix => $_getS(0, '');
  set matrix(String v) { $_setString(0, v); }
  bool hasMatrix() => $_has(0);
  void clearMatrix() => clearField(1);

  String get bin => $_getS(1, '');
  set bin(String v) { $_setString(1, v); }
  bool hasBin() => $_has(1);
  void clearBin() => clearField(2);
}

class BooksFromRJ extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('BooksFromRJ', package: const $pb.PackageName('rewiseDom'))
    ..aOS(1, 'name')
    ..pPS(4, 'errorWrongLangs')
    ..pc<FactFromRJ>(5, 'facts', $pb.PbFieldType.PM,FactFromRJ.create)
    ..p<int>(6, 'lessons', $pb.PbFieldType.P3)
    ..hasRequiredFields = false
  ;

  BooksFromRJ() : super();
  BooksFromRJ.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  BooksFromRJ.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  BooksFromRJ clone() => new BooksFromRJ()..mergeFromMessage(this);
  BooksFromRJ copyWith(void Function(BooksFromRJ) updates) => super.copyWith((message) => updates(message as BooksFromRJ));
  $pb.BuilderInfo get info_ => _i;
  static BooksFromRJ create() => new BooksFromRJ();
  BooksFromRJ createEmptyInstance() => create();
  static $pb.PbList<BooksFromRJ> createRepeated() => new $pb.PbList<BooksFromRJ>();
  static BooksFromRJ getDefault() => _defaultInstance ??= create()..freeze();
  static BooksFromRJ _defaultInstance;

  String get name => $_getS(0, '');
  set name(String v) { $_setString(0, v); }
  bool hasName() => $_has(0);
  void clearName() => clearField(1);

  List<String> get errorWrongLangs => $_getList(1);

  List<FactFromRJ> get facts => $_getList(2);

  List<int> get lessons => $_getList(3);
}

class FactFromRJ extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('FactFromRJ', package: const $pb.PackageName('rewiseDom'))
    ..aOS(1, 'lang')
    ..pPS(2, 'words')
    ..hasRequiredFields = false
  ;

  FactFromRJ() : super();
  FactFromRJ.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  FactFromRJ.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  FactFromRJ clone() => new FactFromRJ()..mergeFromMessage(this);
  FactFromRJ copyWith(void Function(FactFromRJ) updates) => super.copyWith((message) => updates(message as FactFromRJ));
  $pb.BuilderInfo get info_ => _i;
  static FactFromRJ create() => new FactFromRJ();
  FactFromRJ createEmptyInstance() => create();
  static $pb.PbList<FactFromRJ> createRepeated() => new $pb.PbList<FactFromRJ>();
  static FactFromRJ getDefault() => _defaultInstance ??= create()..freeze();
  static FactFromRJ _defaultInstance;

  String get lang => $_getS(0, '');
  set lang(String v) { $_setString(0, v); }
  bool hasLang() => $_has(0);
  void clearLang() => clearField(1);

  List<String> get words => $_getList(1);
}

