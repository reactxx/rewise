///
//  Generated code. Do not modify.
//  source: rewise/books_import_fromrj.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

class FileNamesRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('FileNamesRequest', package: const $pb.PackageName('rewiseDom'))
    ..pPS(1, 'fileNames')
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

  List<String> get fileNames => $_getList(0);
}

class BookOut extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('BookOut', package: const $pb.PackageName('rewiseDom'))
    ..pPS(1, 'errorWrongLangs')
    ..pc<FactOut>(2, 'facts', $pb.PbFieldType.PM,FactOut.create)
    ..p<int>(3, 'lessons', $pb.PbFieldType.P3)
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

  List<String> get errorWrongLangs => $_getList(0);

  List<FactOut> get facts => $_getList(1);

  List<int> get lessons => $_getList(2);
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

