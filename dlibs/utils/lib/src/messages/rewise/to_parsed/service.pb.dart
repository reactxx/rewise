///
//  Generated code. Do not modify.
//  source: rewise/to_parsed/service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

import '../dom/dom.pb.dart' as $0;

class Request extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Request', package: const $pb.PackageName('rw.to_parsed'))
    ..a<RawBook>(1, 'book', $pb.PbFieldType.OM, RawBook.getDefault, RawBook.create)
    ..hasRequiredFields = false
  ;

  Request() : super();
  Request.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Request.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Request clone() => new Request()..mergeFromMessage(this);
  Request copyWith(void Function(Request) updates) => super.copyWith((message) => updates(message as Request));
  $pb.BuilderInfo get info_ => _i;
  static Request create() => new Request();
  Request createEmptyInstance() => create();
  static $pb.PbList<Request> createRepeated() => new $pb.PbList<Request>();
  static Request getDefault() => _defaultInstance ??= create()..freeze();
  static Request _defaultInstance;

  RawBook get book => $_getN(0);
  set book(RawBook v) { setField(1, v); }
  bool hasBook() => $_has(0);
  void clearBook() => clearField(1);
}

class RawBook extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('RawBook', package: const $pb.PackageName('rw.to_parsed'))
    ..aOS(1, 'name')
    ..pc<RawFact>(5, 'facts', $pb.PbFieldType.PM,RawFact.create)
    ..p<int>(6, 'lessons', $pb.PbFieldType.P3)
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

  String get name => $_getS(0, '');
  set name(String v) { $_setString(0, v); }
  bool hasName() => $_has(0);
  void clearName() => clearField(1);

  List<RawFact> get facts => $_getList(1);

  List<int> get lessons => $_getList(2);
}

class RawFact extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('RawFact', package: const $pb.PackageName('rw.to_parsed'))
    ..aOS(1, 'lang')
    ..pPS(2, 'words')
    ..hasRequiredFields = false
  ;

  RawFact() : super();
  RawFact.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  RawFact.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  RawFact clone() => new RawFact()..mergeFromMessage(this);
  RawFact copyWith(void Function(RawFact) updates) => super.copyWith((message) => updates(message as RawFact));
  $pb.BuilderInfo get info_ => _i;
  static RawFact create() => new RawFact();
  RawFact createEmptyInstance() => create();
  static $pb.PbList<RawFact> createRepeated() => new $pb.PbList<RawFact>();
  static RawFact getDefault() => _defaultInstance ??= create()..freeze();
  static RawFact _defaultInstance;

  String get lang => $_getS(0, '');
  set lang(String v) { $_setString(0, v); }
  bool hasLang() => $_has(0);
  void clearLang() => clearField(1);

  List<String> get words => $_getList(1);
}

class Response extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Response', package: const $pb.PackageName('rw.to_parsed'))
    ..a<ParsedBook>(1, 'book', $pb.PbFieldType.OM, ParsedBook.getDefault, ParsedBook.create)
    ..aOS(2, 'error')
    ..hasRequiredFields = false
  ;

  Response() : super();
  Response.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Response.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Response clone() => new Response()..mergeFromMessage(this);
  Response copyWith(void Function(Response) updates) => super.copyWith((message) => updates(message as Response));
  $pb.BuilderInfo get info_ => _i;
  static Response create() => new Response();
  Response createEmptyInstance() => create();
  static $pb.PbList<Response> createRepeated() => new $pb.PbList<Response>();
  static Response getDefault() => _defaultInstance ??= create()..freeze();
  static Response _defaultInstance;

  ParsedBook get book => $_getN(0);
  set book(ParsedBook v) { setField(1, v); }
  bool hasBook() => $_has(0);
  void clearBook() => clearField(1);

  String get error => $_getS(1, '');
  set error(String v) { $_setString(1, v); }
  bool hasError() => $_has(1);
  void clearError() => clearField(2);
}

class ParsedBook extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('ParsedBook', package: const $pb.PackageName('rw.to_parsed'))
    ..aOS(1, 'lang')
    ..aOS(2, 'bookId')
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

  String get bookId => $_getS(1, '');
  set bookId(String v) { $_setString(1, v); }
  bool hasBookId() => $_has(1);
  void clearBookId() => clearField(2);

  List<ParsedFact> get facts => $_getList(2);
}

class ParsedFact extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('ParsedFact', package: const $pb.PackageName('rw.to_parsed'))
    ..a<int>(1, 'id', $pb.PbFieldType.O3)
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

  int get id => $_get(0, 0);
  set id(int v) { $_setSignedInt32(0, v); }
  bool hasId() => $_has(0);
  void clearId() => clearField(1);

  String get stemmText => $_getS(1, '');
  set stemmText(String v) { $_setString(1, v); }
  bool hasStemmText() => $_has(1);
  void clearStemmText() => clearField(2);

  $0.Fact get fact => $_getN(2);
  set fact($0.Fact v) { setField(3, v); }
  bool hasFact() => $_has(2);
  void clearFact() => clearField(3);
}

