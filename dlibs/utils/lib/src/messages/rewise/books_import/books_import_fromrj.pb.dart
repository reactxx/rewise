///
//  Generated code. Do not modify.
//  source: rewise/books_import/books_import_fromrj.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

class FileNamesRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('FileNamesRequest', package: const $pb.PackageName('rewiseDom'))
    ..pc<FileNames>(1, 'fileNames', $pb.PbFieldType.PM,FileNames.create)
    ..hasRequiredFields = false
  ;

  FileNamesRequest() : super();
  FileNamesRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  FileNamesRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  FileNamesRequest clone() => new FileNamesRequest()..mergeFromMessage(this);
  FileNamesRequest copyWith(void Function(FileNamesRequest) updates) => super.copyWith((message) => updates(message as FileNamesRequest));
  $pb.BuilderInfo get info_ => _i;
  static FileNamesRequest create() => new FileNamesRequest();
  FileNamesRequest createEmptyInstance() => create();
  static $pb.PbList<FileNamesRequest> createRepeated() => new $pb.PbList<FileNamesRequest>();
  static FileNamesRequest getDefault() => _defaultInstance ??= create()..freeze();
  static FileNamesRequest _defaultInstance;

  List<FileNames> get fileNames => $_getList(0);
}

class FileNames extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('FileNames', package: const $pb.PackageName('rewiseDom'))
    ..aOS(1, 'matrix')
    ..aOS(2, 'bin')
    ..hasRequiredFields = false
  ;

  FileNames() : super();
  FileNames.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  FileNames.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  FileNames clone() => new FileNames()..mergeFromMessage(this);
  FileNames copyWith(void Function(FileNames) updates) => super.copyWith((message) => updates(message as FileNames));
  $pb.BuilderInfo get info_ => _i;
  static FileNames create() => new FileNames();
  FileNames createEmptyInstance() => create();
  static $pb.PbList<FileNames> createRepeated() => new $pb.PbList<FileNames>();
  static FileNames getDefault() => _defaultInstance ??= create()..freeze();
  static FileNames _defaultInstance;

  String get matrix => $_getS(0, '');
  set matrix(String v) { $_setString(0, v); }
  bool hasMatrix() => $_has(0);
  void clearMatrix() => clearField(1);

  String get bin => $_getS(1, '');
  set bin(String v) { $_setString(1, v); }
  bool hasBin() => $_has(1);
  void clearBin() => clearField(2);
}

class BookOut extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('BookOut', package: const $pb.PackageName('rewiseDom'))
    ..aOS(1, 'name')
    ..pPS(4, 'errorWrongLangs')
    ..pc<FactOut>(5, 'facts', $pb.PbFieldType.PM,FactOut.create)
    ..p<int>(6, 'lessons', $pb.PbFieldType.P3)
    ..hasRequiredFields = false
  ;

  BookOut() : super();
  BookOut.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  BookOut.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  BookOut clone() => new BookOut()..mergeFromMessage(this);
  BookOut copyWith(void Function(BookOut) updates) => super.copyWith((message) => updates(message as BookOut));
  $pb.BuilderInfo get info_ => _i;
  static BookOut create() => new BookOut();
  BookOut createEmptyInstance() => create();
  static $pb.PbList<BookOut> createRepeated() => new $pb.PbList<BookOut>();
  static BookOut getDefault() => _defaultInstance ??= create()..freeze();
  static BookOut _defaultInstance;

  String get name => $_getS(0, '');
  set name(String v) { $_setString(0, v); }
  bool hasName() => $_has(0);
  void clearName() => clearField(1);

  List<String> get errorWrongLangs => $_getList(1);

  List<FactOut> get facts => $_getList(2);

  List<int> get lessons => $_getList(3);
}

class FactOut extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('FactOut', package: const $pb.PackageName('rewiseDom'))
    ..aOS(1, 'lang')
    ..pPS(2, 'words')
    ..hasRequiredFields = false
  ;

  FactOut() : super();
  FactOut.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  FactOut.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  FactOut clone() => new FactOut()..mergeFromMessage(this);
  FactOut copyWith(void Function(FactOut) updates) => super.copyWith((message) => updates(message as FactOut));
  $pb.BuilderInfo get info_ => _i;
  static FactOut create() => new FactOut();
  FactOut createEmptyInstance() => create();
  static $pb.PbList<FactOut> createRepeated() => new $pb.PbList<FactOut>();
  static FactOut getDefault() => _defaultInstance ??= create()..freeze();
  static FactOut _defaultInstance;

  String get lang => $_getS(0, '');
  set lang(String v) { $_setString(0, v); }
  bool hasLang() => $_has(0);
  void clearLang() => clearField(1);

  List<String> get words => $_getList(1);
}

