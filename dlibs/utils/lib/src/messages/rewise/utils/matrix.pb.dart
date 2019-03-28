///
//  Generated code. Do not modify.
//  source: rewise/utils/matrix.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

class Matrix extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Matrix', package: const $pb.PackageName('rw.common'))
    ..pc<Row>(1, 'rows', $pb.PbFieldType.PM,Row.create)
    ..pPS(2, 'cols')
    ..hasRequiredFields = false
  ;

  Matrix() : super();
  Matrix.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Matrix.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Matrix clone() => new Matrix()..mergeFromMessage(this);
  Matrix copyWith(void Function(Matrix) updates) => super.copyWith((message) => updates(message as Matrix));
  $pb.BuilderInfo get info_ => _i;
  static Matrix create() => new Matrix();
  Matrix createEmptyInstance() => create();
  static $pb.PbList<Matrix> createRepeated() => new $pb.PbList<Matrix>();
  static Matrix getDefault() => _defaultInstance ??= create()..freeze();
  static Matrix _defaultInstance;

  List<Row> get rows => $_getList(0);

  List<String> get cols => $_getList(1);
}

class Row extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('Row', package: const $pb.PackageName('rw.common'))
    ..pPS(1, 'langs')
    ..pPS(2, 'values')
    ..hasRequiredFields = false
  ;

  Row() : super();
  Row.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  Row.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  Row clone() => new Row()..mergeFromMessage(this);
  Row copyWith(void Function(Row) updates) => super.copyWith((message) => updates(message as Row));
  $pb.BuilderInfo get info_ => _i;
  static Row create() => new Row();
  Row createEmptyInstance() => create();
  static $pb.PbList<Row> createRepeated() => new $pb.PbList<Row>();
  static Row getDefault() => _defaultInstance ??= create()..freeze();
  static Row _defaultInstance;

  List<String> get langs => $_getList(0);

  List<String> get values => $_getList(1);
}

