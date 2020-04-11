///
//  Generated code. Do not modify.
//  source: google/api/field_behavior.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore: UNUSED_SHOWN_NAME
import 'dart:core' show int, bool, double, String, List, Map, override;

import 'package:protobuf/protobuf.dart' as $pb;

import 'field_behavior.pbenum.dart';

export 'field_behavior.pbenum.dart';

class Field_behavior {
  static final $pb.Extension fieldBehavior = new $pb.Extension<FieldBehavior>.repeated('google.protobuf.FieldOptions', 'fieldBehavior', 1052, $pb.PbFieldType.PE, $pb.getCheckFunction($pb.PbFieldType.PE), null, FieldBehavior.valueOf, FieldBehavior.values);
  static void registerAllExtensions($pb.ExtensionRegistry registry) {
    registry.add(fieldBehavior);
  }
}

