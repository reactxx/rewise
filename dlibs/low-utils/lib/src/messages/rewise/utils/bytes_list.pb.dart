///
//  Generated code. Do not modify.
//  source: rewise/utils/bytes_list.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

import '../../google/protobuf/wrappers.pb.dart' as $0;

class BytesList extends $pb.GeneratedMessage {
  static final $pb.BuilderInfo _i = new $pb.BuilderInfo('BytesList', package: const $pb.PackageName('rewiseDom'))
    ..pc<$0.BytesValue>(1, 'list', $pb.PbFieldType.PM,$0.BytesValue.create)
    ..hasRequiredFields = false
  ;

  BytesList() : super();
  BytesList.fromBuffer(List<int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromBuffer(i, r);
  BytesList.fromJson(String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) : super.fromJson(i, r);
  BytesList clone() => new BytesList()..mergeFromMessage(this);
  BytesList copyWith(void Function(BytesList) updates) => super.copyWith((message) => updates(message as BytesList));
  $pb.BuilderInfo get info_ => _i;
  static BytesList create() => new BytesList();
  BytesList createEmptyInstance() => create();
  static $pb.PbList<BytesList> createRepeated() => new $pb.PbList<BytesList>();
  static BytesList getDefault() => _defaultInstance ??= create()..freeze();
  static BytesList _defaultInstance;

  List<$0.BytesValue> get list => $_getList(0);
}

