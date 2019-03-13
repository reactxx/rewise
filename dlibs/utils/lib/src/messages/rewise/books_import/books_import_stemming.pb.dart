///
//  Generated code. Do not modify.
//  source: rewise/books_import/books_import_stemming.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

import '../book/book.pb.dart' as $0;

class RawFacts extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('RawFacts', package: const $pb.PackageName('rewiseDom'))
    ..pc<RawFact>(1, 'facts', $pb.PbFieldType.PM,RawFact.create)
    ..hasRequiredFields = false
  ;

  RawFacts() : super();
  RawFacts.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  RawFacts.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  RawFacts clone() => new RawFacts()..mergeFromMessage(this);
  RawFacts copyWith(void Function(RawFacts) updates) => super.copyWith((message) => updates(message as RawFacts));
  $pb.BuilderInfo get info_ => _i;
  static RawFacts create() => new RawFacts();
  RawFacts createEmptyInstance() => create();
  static $pb.PbList<RawFacts> createRepeated() => new $pb.PbList<RawFacts>();
  static RawFacts getDefault() => _defaultInstance ??= create()..freeze();
  static RawFacts _defaultInstance;

  List<RawFact> get facts => $_getList(0);
}

class RawFact extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('RawFact', package: const $pb.PackageName('rewiseDom'))
    ..aOS(1, 'stemmText')
    ..a<$0.Fact>(2, 'fact', $pb.PbFieldType.OM, $0.Fact.getDefault, $0.Fact.create)
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

  String get stemmText => $_getS(0, '');
  set stemmText(String v) { $_setString(0, v); }
  bool hasStemmText() => $_has(0);
  void clearStemmText() => clearField(1);

  $0.Fact get fact => $_getN(1);
  set fact($0.Fact v) { setField(2, v); }
  bool hasFact() => $_has(1);
  void clearFact() => clearField(2);
}

