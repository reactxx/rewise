///
//  Generated code. Do not modify.
//  source: rewise/to_parsed/to_parsed_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

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
    ..pc<ParsedFact>(2, 'facts', $pb.PbFieldType.PM,ParsedFact.create)
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
    ..a<int>(2, 'lessonId', $pb.PbFieldType.O3)
    ..pc<ParsedSubFact>(3, 'childs', $pb.PbFieldType.PM,ParsedSubFact.create)
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

  int get lessonId => $_get(1, 0);
  set lessonId(int v) { $_setSignedInt32(1, v); }
  bool hasLessonId() => $_has(1);
  void clearLessonId() => clearField(2);

  List<ParsedSubFact> get childs => $_getList(2);
}

class ParsedSubFact extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('ParsedSubFact', package: const $pb.PackageName('rw.to_parsed'))
    ..aOS(1, 'text')
    ..aOS(2, 'breakText')
    ..aOS(3, 'wordClass')
    ..a<List<int>>(4, 'breaks', $pb.PbFieldType.OY)
    ..hasRequiredFields = false
  ;

  ParsedSubFact() : super();
  ParsedSubFact.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  ParsedSubFact.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  ParsedSubFact clone() => new ParsedSubFact()..mergeFromMessage(this);
  ParsedSubFact copyWith(void Function(ParsedSubFact) updates) => super.copyWith((message) => updates(message as ParsedSubFact));
  $pb.BuilderInfo get info_ => _i;
  static ParsedSubFact create() => new ParsedSubFact();
  ParsedSubFact createEmptyInstance() => create();
  static $pb.PbList<ParsedSubFact> createRepeated() => new $pb.PbList<ParsedSubFact>();
  static ParsedSubFact getDefault() => _defaultInstance ??= create()..freeze();
  static ParsedSubFact _defaultInstance;

  String get text => $_getS(0, '');
  set text(String v) { $_setString(0, v); }
  bool hasText() => $_has(0);
  void clearText() => clearField(1);

  String get breakText => $_getS(1, '');
  set breakText(String v) { $_setString(1, v); }
  bool hasBreakText() => $_has(1);
  void clearBreakText() => clearField(2);

  String get wordClass => $_getS(2, '');
  set wordClass(String v) { $_setString(2, v); }
  bool hasWordClass() => $_has(2);
  void clearWordClass() => clearField(3);

  List<int> get breaks => $_getN(3);
  set breaks(List<int> v) { $_setBytes(3, v); }
  bool hasBreaks() => $_has(3);
  void clearBreaks() => clearField(4);
}

class BracketBooks extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('BracketBooks', package: const $pb.PackageName('rw.to_parsed'))
    ..aOS(1, 'name')
    ..pc<BracketBook>(2, 'books', $pb.PbFieldType.PM,BracketBook.create)
    ..hasRequiredFields = false
  ;

  BracketBooks() : super();
  BracketBooks.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  BracketBooks.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  BracketBooks clone() => new BracketBooks()..mergeFromMessage(this);
  BracketBooks copyWith(void Function(BracketBooks) updates) => super.copyWith((message) => updates(message as BracketBooks));
  $pb.BuilderInfo get info_ => _i;
  static BracketBooks create() => new BracketBooks();
  BracketBooks createEmptyInstance() => create();
  static $pb.PbList<BracketBooks> createRepeated() => new $pb.PbList<BracketBooks>();
  static BracketBooks getDefault() => _defaultInstance ??= create()..freeze();
  static BracketBooks _defaultInstance;

  String get name => $_getS(0, '');
  set name(String v) { $_setString(0, v); }
  bool hasName() => $_has(0);
  void clearName() => clearField(1);

  List<BracketBook> get books => $_getList(1);
}

class BracketBook extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('BracketBook', package: const $pb.PackageName('rw.to_parsed'))
    ..aOS(1, 'lang')
    ..pc<Bracket>(2, 'brackets', $pb.PbFieldType.PM,Bracket.create)
    ..aOS(3, 'alphabetAll')
    ..m<String, String>(4, 'alphabetScripts', 'BracketBook.AlphabetScriptsEntry',$pb.PbFieldType.OS, $pb.PbFieldType.OS, null, null, null , const $pb.PackageName('rw.to_parsed'))
    ..pPS(5, 'oKWords')
    ..pPS(6, 'wrongUnicodeWords')
    ..pPS(7, 'wrongCldrWords')
    ..hasRequiredFields = false
  ;

  BracketBook() : super();
  BracketBook.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  BracketBook.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  BracketBook clone() => new BracketBook()..mergeFromMessage(this);
  BracketBook copyWith(void Function(BracketBook) updates) => super.copyWith((message) => updates(message as BracketBook));
  $pb.BuilderInfo get info_ => _i;
  static BracketBook create() => new BracketBook();
  BracketBook createEmptyInstance() => create();
  static $pb.PbList<BracketBook> createRepeated() => new $pb.PbList<BracketBook>();
  static BracketBook getDefault() => _defaultInstance ??= create()..freeze();
  static BracketBook _defaultInstance;

  String get lang => $_getS(0, '');
  set lang(String v) { $_setString(0, v); }
  bool hasLang() => $_has(0);
  void clearLang() => clearField(1);

  List<Bracket> get brackets => $_getList(1);

  String get alphabetAll => $_getS(2, '');
  set alphabetAll(String v) { $_setString(2, v); }
  bool hasAlphabetAll() => $_has(2);
  void clearAlphabetAll() => clearField(3);

  Map<String, String> get alphabetScripts => $_getMap(3);

  List<String> get oKWords => $_getList(4);

  List<String> get wrongUnicodeWords => $_getList(5);

  List<String> get wrongCldrWords => $_getList(6);
}

class Bracket extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Bracket', package: const $pb.PackageName('rw.to_parsed'))
    ..aOS(1, 'type')
    ..aOS(2, 'value')
    ..a<int>(3, 'factIdx', $pb.PbFieldType.O3)
    ..hasRequiredFields = false
  ;

  Bracket() : super();
  Bracket.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Bracket.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Bracket clone() => new Bracket()..mergeFromMessage(this);
  Bracket copyWith(void Function(Bracket) updates) => super.copyWith((message) => updates(message as Bracket));
  $pb.BuilderInfo get info_ => _i;
  static Bracket create() => new Bracket();
  Bracket createEmptyInstance() => create();
  static $pb.PbList<Bracket> createRepeated() => new $pb.PbList<Bracket>();
  static Bracket getDefault() => _defaultInstance ??= create()..freeze();
  static Bracket _defaultInstance;

  String get type => $_getS(0, '');
  set type(String v) { $_setString(0, v); }
  bool hasType() => $_has(0);
  void clearType() => clearField(1);

  String get value => $_getS(1, '');
  set value(String v) { $_setString(1, v); }
  bool hasValue() => $_has(1);
  void clearValue() => clearField(2);

  int get factIdx => $_get(2, 0);
  set factIdx(int v) { $_setSignedInt32(2, v); }
  bool hasFactIdx() => $_has(2);
  void clearFactIdx() => clearField(3);
}

