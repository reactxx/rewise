///
//  Generated code. Do not modify.
//  source: rewise/books_import/books_import_raw_books.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

import '../book/book.pb.dart' as $0;

class RawBook extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('RawBook', package: const $pb.PackageName('rewiseDom'))
    ..aOS(1, 'lang')
    ..aOS(2, 'bookId')
    ..pc<RawFact>(3, 'facts', $pb.PbFieldType.PM,RawFact.create)
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

  String get bookId => $_getS(1, '');
  set bookId(String v) { $_setString(1, v); }
  bool hasBookId() => $_has(1);
  void clearBookId() => clearField(2);

  List<RawFact> get facts => $_getList(2);
}

class RawFact extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('RawFact', package: const $pb.PackageName('rewiseDom'))
    ..a<int>(1, 'id', $pb.PbFieldType.O3)
    ..aOS(2, 'stemmText')
    ..a<$0.Fact>(3, 'fact', $pb.PbFieldType.OM, $0.Fact.getDefault, $0.Fact.create)
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

