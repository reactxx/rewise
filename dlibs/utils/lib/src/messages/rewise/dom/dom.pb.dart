///
//  Generated code. Do not modify.
//  source: rewise/dom/dom.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

import 'dom.pbenum.dart';

export 'dom.pbenum.dart';

class Fact extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Fact', package: const $pb.PackageName('rw.dom'))
    ..a<int>(1, 'id', $pb.PbFieldType.O3)
    ..aOS(2, 'text')
    ..a<List<int>>(3, 'breaks', $pb.PbFieldType.OY)
    ..p<int>(4, 'rightIds', $pb.PbFieldType.P3)
    ..aOS(5, 'wordClass')
    ..a<int>(6, 'lessonId', $pb.PbFieldType.O3)
    ..hasRequiredFields = false
  ;

  Fact() : super();
  Fact.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Fact.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Fact clone() => new Fact()..mergeFromMessage(this);
  Fact copyWith(void Function(Fact) updates) => super.copyWith((message) => updates(message as Fact));
  $pb.BuilderInfo get info_ => _i;
  static Fact create() => new Fact();
  Fact createEmptyInstance() => create();
  static $pb.PbList<Fact> createRepeated() => new $pb.PbList<Fact>();
  static Fact getDefault() => _defaultInstance ??= create()..freeze();
  static Fact _defaultInstance;

  int get id => $_get(0, 0);
  set id(int v) { $_setSignedInt32(0, v); }
  bool hasId() => $_has(0);
  void clearId() => clearField(1);

  String get text => $_getS(1, '');
  set text(String v) { $_setString(1, v); }
  bool hasText() => $_has(1);
  void clearText() => clearField(2);

  List<int> get breaks => $_getN(2);
  set breaks(List<int> v) { $_setBytes(2, v); }
  bool hasBreaks() => $_has(2);
  void clearBreaks() => clearField(3);

  List<int> get rightIds => $_getList(3);

  String get wordClass => $_getS(4, '');
  set wordClass(String v) { $_setString(4, v); }
  bool hasWordClass() => $_has(4);
  void clearWordClass() => clearField(5);

  int get lessonId => $_get(5, 0);
  set lessonId(int v) { $_setSignedInt32(5, v); }
  bool hasLessonId() => $_has(5);
  void clearLessonId() => clearField(6);
}

class BookMeta extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('BookMeta', package: const $pb.PackageName('rw.dom'))
    ..aOS(1, 'id')
    ..aOS(2, 'title')
    ..hasRequiredFields = false
  ;

  BookMeta() : super();
  BookMeta.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  BookMeta.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  BookMeta clone() => new BookMeta()..mergeFromMessage(this);
  BookMeta copyWith(void Function(BookMeta) updates) => super.copyWith((message) => updates(message as BookMeta));
  $pb.BuilderInfo get info_ => _i;
  static BookMeta create() => new BookMeta();
  BookMeta createEmptyInstance() => create();
  static $pb.PbList<BookMeta> createRepeated() => new $pb.PbList<BookMeta>();
  static BookMeta getDefault() => _defaultInstance ??= create()..freeze();
  static BookMeta _defaultInstance;

  String get id => $_getS(0, '');
  set id(String v) { $_setString(0, v); }
  bool hasId() => $_has(0);
  void clearId() => clearField(1);

  String get title => $_getS(1, '');
  set title(String v) { $_setString(1, v); }
  bool hasTitle() => $_has(1);
  void clearTitle() => clearField(2);
}

class WordMsg extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('WordMsg', package: const $pb.PackageName('rw.dom'))
    ..aOS(1, 'text')
    ..aOS(2, 'before')
    ..aOS(3, 'after')
    ..a<int>(4, 'flags', $pb.PbFieldType.O3)
    ..aOS(5, 'flagsData')
    ..hasRequiredFields = false
  ;

  WordMsg() : super();
  WordMsg.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  WordMsg.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  WordMsg clone() => new WordMsg()..mergeFromMessage(this);
  WordMsg copyWith(void Function(WordMsg) updates) => super.copyWith((message) => updates(message as WordMsg));
  $pb.BuilderInfo get info_ => _i;
  static WordMsg create() => new WordMsg();
  WordMsg createEmptyInstance() => create();
  static $pb.PbList<WordMsg> createRepeated() => new $pb.PbList<WordMsg>();
  static WordMsg getDefault() => _defaultInstance ??= create()..freeze();
  static WordMsg _defaultInstance;

  String get text => $_getS(0, '');
  set text(String v) { $_setString(0, v); }
  bool hasText() => $_has(0);
  void clearText() => clearField(1);

  String get before => $_getS(1, '');
  set before(String v) { $_setString(1, v); }
  bool hasBefore() => $_has(1);
  void clearBefore() => clearField(2);

  String get after => $_getS(2, '');
  set after(String v) { $_setString(2, v); }
  bool hasAfter() => $_has(2);
  void clearAfter() => clearField(3);

  int get flags => $_get(3, 0);
  set flags(int v) { $_setSignedInt32(3, v); }
  bool hasFlags() => $_has(3);
  void clearFlags() => clearField(4);

  String get flagsData => $_getS(4, '');
  set flagsData(String v) { $_setString(4, v); }
  bool hasFlagsData() => $_has(4);
  void clearFlagsData() => clearField(5);
}

class FactMsg extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('FactMsg', package: const $pb.PackageName('rw.dom'))
    ..aOS(1, 'wordClass')
    ..a<int>(2, 'flags', $pb.PbFieldType.O3)
    ..pc<WordMsg>(3, 'words', $pb.PbFieldType.PM,WordMsg.create)
    ..hasRequiredFields = false
  ;

  FactMsg() : super();
  FactMsg.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  FactMsg.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  FactMsg clone() => new FactMsg()..mergeFromMessage(this);
  FactMsg copyWith(void Function(FactMsg) updates) => super.copyWith((message) => updates(message as FactMsg));
  $pb.BuilderInfo get info_ => _i;
  static FactMsg create() => new FactMsg();
  FactMsg createEmptyInstance() => create();
  static $pb.PbList<FactMsg> createRepeated() => new $pb.PbList<FactMsg>();
  static FactMsg getDefault() => _defaultInstance ??= create()..freeze();
  static FactMsg _defaultInstance;

  String get wordClass => $_getS(0, '');
  set wordClass(String v) { $_setString(0, v); }
  bool hasWordClass() => $_has(0);
  void clearWordClass() => clearField(1);

  int get flags => $_get(1, 0);
  set flags(int v) { $_setSignedInt32(1, v); }
  bool hasFlags() => $_has(1);
  void clearFlags() => clearField(2);

  List<WordMsg> get words => $_getList(2);
}

class FactsMsg extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('FactsMsg', package: const $pb.PackageName('rw.dom'))
    ..a<int>(1, 'crc', $pb.PbFieldType.O3)
    ..aOS(2, 'asString')
    ..pc<FactMsg>(3, 'facts', $pb.PbFieldType.PM,FactMsg.create)
    ..aOS(4, 'lesson')
    ..hasRequiredFields = false
  ;

  FactsMsg() : super();
  FactsMsg.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  FactsMsg.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  FactsMsg clone() => new FactsMsg()..mergeFromMessage(this);
  FactsMsg copyWith(void Function(FactsMsg) updates) => super.copyWith((message) => updates(message as FactsMsg));
  $pb.BuilderInfo get info_ => _i;
  static FactsMsg create() => new FactsMsg();
  FactsMsg createEmptyInstance() => create();
  static $pb.PbList<FactsMsg> createRepeated() => new $pb.PbList<FactsMsg>();
  static FactsMsg getDefault() => _defaultInstance ??= create()..freeze();
  static FactsMsg _defaultInstance;

  int get crc => $_get(0, 0);
  set crc(int v) { $_setSignedInt32(0, v); }
  bool hasCrc() => $_has(0);
  void clearCrc() => clearField(1);

  String get asString => $_getS(1, '');
  set asString(String v) { $_setString(1, v); }
  bool hasAsString() => $_has(1);
  void clearAsString() => clearField(2);

  List<FactMsg> get facts => $_getList(2);

  String get lesson => $_getS(3, '');
  set lesson(String v) { $_setString(3, v); }
  bool hasLesson() => $_has(3);
  void clearLesson() => clearField(4);
}

class FileMsg extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('FileMsg', package: const $pb.PackageName('rw.dom'))
    ..aOS(1, 'leftLang')
    ..aOS(2, 'bookName')
    ..aOS(3, 'lang')
    ..e<FileMsg_BookType>(4, 'bookType', $pb.PbFieldType.OE, FileMsg_BookType.KDICT, FileMsg_BookType.valueOf, FileMsg_BookType.values)
    ..e<FileMsg_FileType>(5, 'fileType', $pb.PbFieldType.OE, FileMsg_FileType.LEFT, FileMsg_FileType.valueOf, FileMsg_FileType.values)
    ..pc<FactsMsg>(6, 'factss', $pb.PbFieldType.PM,FactsMsg.create)
    ..hasRequiredFields = false
  ;

  FileMsg() : super();
  FileMsg.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  FileMsg.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  FileMsg clone() => new FileMsg()..mergeFromMessage(this);
  FileMsg copyWith(void Function(FileMsg) updates) => super.copyWith((message) => updates(message as FileMsg));
  $pb.BuilderInfo get info_ => _i;
  static FileMsg create() => new FileMsg();
  FileMsg createEmptyInstance() => create();
  static $pb.PbList<FileMsg> createRepeated() => new $pb.PbList<FileMsg>();
  static FileMsg getDefault() => _defaultInstance ??= create()..freeze();
  static FileMsg _defaultInstance;

  String get leftLang => $_getS(0, '');
  set leftLang(String v) { $_setString(0, v); }
  bool hasLeftLang() => $_has(0);
  void clearLeftLang() => clearField(1);

  String get bookName => $_getS(1, '');
  set bookName(String v) { $_setString(1, v); }
  bool hasBookName() => $_has(1);
  void clearBookName() => clearField(2);

  String get lang => $_getS(2, '');
  set lang(String v) { $_setString(2, v); }
  bool hasLang() => $_has(2);
  void clearLang() => clearField(3);

  FileMsg_BookType get bookType => $_getN(3);
  set bookType(FileMsg_BookType v) { setField(4, v); }
  bool hasBookType() => $_has(3);
  void clearBookType() => clearField(4);

  FileMsg_FileType get fileType => $_getN(4);
  set fileType(FileMsg_FileType v) { setField(5, v); }
  bool hasFileType() => $_has(4);
  void clearFileType() => clearField(5);

  List<FactsMsg> get factss => $_getList(5);
}

