///
//  Generated code. Do not modify.
//  source: rewise/books_import/books_import_stemming.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

class StemmRequest extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('StemmRequest', package: const $pb.PackageName('rewiseDom'))
    ..aOS(1, 'lang')
    ..pPS(2, 'words')
    ..hasRequiredFields = false
  ;

  StemmRequest() : super();
  StemmRequest.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  StemmRequest.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  StemmRequest clone() => new StemmRequest()..mergeFromMessage(this);
  StemmRequest copyWith(void Function(StemmRequest) updates) => super.copyWith((message) => updates(message as StemmRequest));
  $pb.BuilderInfo get info_ => _i;
  static StemmRequest create() => new StemmRequest();
  StemmRequest createEmptyInstance() => create();
  static $pb.PbList<StemmRequest> createRepeated() => new $pb.PbList<StemmRequest>();
  static StemmRequest getDefault() => _defaultInstance ??= create()..freeze();
  static StemmRequest _defaultInstance;

  String get lang => $_getS(0, '');
  set lang(String v) { $_setString(0, v); }
  bool hasLang() => $_has(0);
  void clearLang() => clearField(1);

  List<String> get words => $_getList(1);
}

