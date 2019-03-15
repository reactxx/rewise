///
//  Generated code. Do not modify.
//  source: rewise/to_parsed/to_parsed_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

import '../dom/dom.pb.dart' as $0;

class RawBooks extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('RawBooks', package: const $pb.PackageName('rw.to_parsed'))
    ..aOS(1, 'name')
    ..pc<RawBook>(2, 'books', $pb.PbFieldType.PM,RawBook.create)
    ..p<int>(3, 'lessons', $pb.PbFieldType.P3)
    ..hasRequiredFields = false
  ;

  RawBooks() : super();
  RawBooks.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  RawBooks.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  RawBooks clone() => new RawBooks()..mergeFromMessage(this);
  RawBooks copyWith(void Function(RawBooks) updates) => super.copyWith((message) => updates(message as RawBooks));
  $pb.BuilderInfo get info_ => _i;
  static RawBooks create() => new RawBooks();
  RawBooks createEmptyInstance() => create();
  static $pb.PbList<RawBooks> createRepeated() => new $pb.PbList<RawBooks>();
  static RawBooks getDefault() => _defaultInstance ??= create()..freeze();
  static RawBooks _defaultInstance;

  String get name => $_getS(0, '');
  set name(String v) { $_setString(0, v); }
  bool hasName() => $_has(0);
  void clearName() => clearField(1);

  List<RawBook> get books => $_getList(1);

  List<int> get lessons => $_getList(2);
}

class RawBook extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('RawBook', package: const $pb.PackageName('rw.to_parsed'))
    ..aOS(1, 'lang')
    ..pPS(2, 'facts')
    ..hasRequiredFields = false
  ;

  RawBook() : super();
  RawBook.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  RawBook.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  RawBook clone() => new RawBook()..mergeFromMessage(this);
  RawBook copyWith(void Function(RawBook) updates) => super.copyWith((message) => updates(message as RawBook));
  $pb.BuilderInfo get info_ => _i;
  static RawBook create() => new RawBook();
  RawBook createEmptyInstance() => create();
  static $pb.PbList<RawBook> createRepeated() => new $pb.PbList<RawBook>();
  static RawBook getDefault() => _defaultInstance ??= create()..freeze();
  static RawBook _defaultInstance;

  String get lang => $_getS(0, '');
  set lang(String v) { $_setString(0, v); }
  bool hasLang() => $_has(0);
  void clearLang() => clearField(1);

  List<String> get facts => $_getList(1);
}

class ParsedBooks extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('ParsedBooks', package: const $pb.PackageName('rw.to_parsed'))
    ..aOS(1, 'name')
    ..pc<ParsedBook>(2, 'books', $pb.PbFieldType.PM,ParsedBook.create)
    ..hasRequiredFields = false
  ;

  ParsedBooks() : super();
  ParsedBooks.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  ParsedBooks.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  ParsedBooks clone() => new ParsedBooks()..mergeFromMessage(this);
  ParsedBooks copyWith(void Function(ParsedBooks) updates) => super.copyWith((message) => updates(message as ParsedBooks));
  $pb.BuilderInfo get info_ => _i;
  static ParsedBooks create() => new ParsedBooks();
  ParsedBooks createEmptyInstance() => create();
  static $pb.PbList<ParsedBooks> createRepeated() => new $pb.PbList<ParsedBooks>();
  static ParsedBooks getDefault() => _defaultInstance ??= create()..freeze();
  static ParsedBooks _defaultInstance;

  String get name => $_getS(0, '');
  set name(String v) { $_setString(0, v); }
  bool hasName() => $_has(0);
  void clearName() => clearField(1);

  List<ParsedBook> get books => $_getList(1);
}

class ParsedBook extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('ParsedBook', package: const $pb.PackageName('rw.to_parsed'))
    ..aOS(1, 'lang')
    ..pc<ParsedFact>(3, 'facts', $pb.PbFieldType.PM,ParsedFact.create)
    ..hasRequiredFields = false
  ;

  ParsedBook() : super();
  ParsedBook.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  ParsedBook.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  ParsedBook clone() => new ParsedBook()..mergeFromMessage(this);
  ParsedBook copyWith(void Function(ParsedBook) updates) => super.copyWith((message) => updates(message as ParsedBook));
  $pb.BuilderInfo get info_ => _i;
  static ParsedBook create() => new ParsedBook();
  ParsedBook createEmptyInstance() => create();
  static $pb.PbList<ParsedBook> createRepeated() => new $pb.PbList<ParsedBook>();
  static ParsedBook getDefault() => _defaultInstance ??= create()..freeze();
  static ParsedBook _defaultInstance;

  String get lang => $_getS(0, '');
  set lang(String v) { $_setString(0, v); }
  bool hasLang() => $_has(0);
  void clearLang() => clearField(1);

  List<ParsedFact> get facts => $_getList(1);
}

class ParsedFact extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('ParsedFact', package: const $pb.PackageName('rw.to_parsed'))
    ..a<int>(1, 'idx', $pb.PbFieldType.O3)
    ..aOS(2, 'stemmText')
    ..a<$0.Fact>(3, 'fact', $pb.PbFieldType.OM, $0.Fact.getDefault, $0.Fact.create)
    ..hasRequiredFields = false
  ;

  ParsedFact() : super();
  ParsedFact.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  ParsedFact.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  ParsedFact clone() => new ParsedFact()..mergeFromMessage(this);
  ParsedFact copyWith(void Function(ParsedFact) updates) => super.copyWith((message) => updates(message as ParsedFact));
  $pb.BuilderInfo get info_ => _i;
  static ParsedFact create() => new ParsedFact();
  ParsedFact createEmptyInstance() => create();
  static $pb.PbList<ParsedFact> createRepeated() => new $pb.PbList<ParsedFact>();
  static ParsedFact getDefault() => _defaultInstance ??= create()..freeze();
  static ParsedFact _defaultInstance;

  int get idx => $_get(0, 0);
  set idx(int v) { $_setSignedInt32(0, v); }
  bool hasIdx() => $_has(0);
  void clearIdx() => clearField(1);

  String get stemmText => $_getS(1, '');
  set stemmText(String v) { $_setString(1, v); }
  bool hasStemmText() => $_has(1);
  void clearStemmText() => clearField(2);

  $0.Fact get fact => $_getN(2);
  set fact($0.Fact v) { setField(3, v); }
  bool hasFact() => $_has(2);
  void clearFact() => clearField(3);
}

