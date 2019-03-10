///
//  Generated code. Do not modify.
//  source: rewise/books_import/books_import_wordbreak.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

class WordBreakRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('WordBreakRequest', package: const $pb.PackageName('rewiseDom'))
    ..aOS(1, 'lang')
    ..pPS(2, 'words')
    ..hasRequiredFields = false
  ;

  WordBreakRequest() : super();
  WordBreakRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  WordBreakRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  WordBreakRequest clone() => new WordBreakRequest()..mergeFromMessage(this);
  WordBreakRequest copyWith(void Function(WordBreakRequest) updates) => super.copyWith((message) => updates(message as WordBreakRequest));
  $pb.BuilderInfo get info_ => _i;
  static WordBreakRequest create() => new WordBreakRequest();
  WordBreakRequest createEmptyInstance() => create();
  static $pb.PbList<WordBreakRequest> createRepeated() => new $pb.PbList<WordBreakRequest>();
  static WordBreakRequest getDefault() => _defaultInstance ??= create()..freeze();
  static WordBreakRequest _defaultInstance;

  String get lang => $_getS(0, '');
  set lang(String v) { $_setString(0, v); }
  bool hasLang() => $_has(0);
  void clearLang() => clearField(1);

  List<String> get words => $_getList(1);
}

