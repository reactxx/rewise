///
//  Generated code. Do not modify.
//  source: rewise/stemming/stemming_service.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

class Request extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Request', package: const $pb.PackageName('rw.stemming'))
    ..aOS(1, 'lang')
    ..pPS(2, 'words')
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

  String get lang => $_getS(0, '');
  set lang(String v) { $_setString(0, v); }
  bool hasLang() => $_has(0);
  void clearLang() => clearField(1);

  List<String> get words => $_getList(1);
}

class Response extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Response', package: const $pb.PackageName('rw.stemming'))
    ..pc<Word>(1, 'words', $pb.PbFieldType.PM,Word.create)
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

  List<Word> get words => $_getList(0);
}

class Word extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Word', package: const $pb.PackageName('rw.stemming'))
    ..aOS(1, 'word')
    ..pPS(2, 'stemms')
    ..a<int>(3, 'ownLen', $pb.PbFieldType.O3)
    ..hasRequiredFields = false
  ;

  Word() : super();
  Word.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Word.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Word clone() => new Word()..mergeFromMessage(this);
  Word copyWith(void Function(Word) updates) => super.copyWith((message) => updates(message as Word));
  $pb.BuilderInfo get info_ => _i;
  static Word create() => new Word();
  Word createEmptyInstance() => create();
  static $pb.PbList<Word> createRepeated() => new $pb.PbList<Word>();
  static Word getDefault() => _defaultInstance ??= create()..freeze();
  static Word _defaultInstance;

  String get word => $_getS(0, '');
  set word(String v) { $_setString(0, v); }
  bool hasWord() => $_has(0);
  void clearWord() => clearField(1);

  List<String> get stemms => $_getList(1);

  int get ownLen => $_get(2, 0);
  set ownLen(int v) { $_setSignedInt32(2, v); }
  bool hasOwnLen() => $_has(2);
  void clearOwnLen() => clearField(3);
}

