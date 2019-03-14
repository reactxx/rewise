///
//  Generated code. Do not modify.
//  source: rewise/dom/dom.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

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

