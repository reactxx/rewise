///
//  Generated code. Do not modify.
//  source: google/api/field_behavior.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore_for_file: UNDEFINED_SHOWN_NAME,UNUSED_SHOWN_NAME
import 'dart:core' show int, dynamic, String, List, Map;
import 'package:protobuf/protobuf.dart' as $pb;

class FieldBehavior extends $pb.ProtobufEnum {
  static const FieldBehavior FIELD_BEHAVIOR_UNSPECIFIED = const FieldBehavior._(0, 'FIELD_BEHAVIOR_UNSPECIFIED');
  static const FieldBehavior OPTIONAL = const FieldBehavior._(1, 'OPTIONAL');
  static const FieldBehavior REQUIRED = const FieldBehavior._(2, 'REQUIRED');
  static const FieldBehavior OUTPUT_ONLY = const FieldBehavior._(3, 'OUTPUT_ONLY');
  static const FieldBehavior INPUT_ONLY = const FieldBehavior._(4, 'INPUT_ONLY');
  static const FieldBehavior IMMUTABLE = const FieldBehavior._(5, 'IMMUTABLE');

  static const List<FieldBehavior> values = const <FieldBehavior> [
    FIELD_BEHAVIOR_UNSPECIFIED,
    OPTIONAL,
    REQUIRED,
    OUTPUT_ONLY,
    INPUT_ONLY,
    IMMUTABLE,
  ];

  static final Map<int, FieldBehavior> _byValue = $pb.ProtobufEnum.initByValue(values);
  static FieldBehavior valueOf(int value) => _byValue[value];

  const FieldBehavior._(int v, String n) : super(v, n);
}

