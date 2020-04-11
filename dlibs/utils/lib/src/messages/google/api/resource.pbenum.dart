///
//  Generated code. Do not modify.
//  source: google/api/resource.proto
///
// ignore_for_file: non_constant_identifier_names,library_prefixes,unused_import

// ignore_for_file: UNDEFINED_SHOWN_NAME,UNUSED_SHOWN_NAME
import 'dart:core' show int, dynamic, String, List, Map;
import 'package:protobuf/protobuf.dart' as $pb;

class ResourceDescriptor_History extends $pb.ProtobufEnum {
  static const ResourceDescriptor_History HISTORY_UNSPECIFIED = const ResourceDescriptor_History._(0, 'HISTORY_UNSPECIFIED');
  static const ResourceDescriptor_History ORIGINALLY_SINGLE_PATTERN = const ResourceDescriptor_History._(1, 'ORIGINALLY_SINGLE_PATTERN');
  static const ResourceDescriptor_History FUTURE_MULTI_PATTERN = const ResourceDescriptor_History._(2, 'FUTURE_MULTI_PATTERN');

  static const List<ResourceDescriptor_History> values = const <ResourceDescriptor_History> [
    HISTORY_UNSPECIFIED,
    ORIGINALLY_SINGLE_PATTERN,
    FUTURE_MULTI_PATTERN,
  ];

  static final Map<int, ResourceDescriptor_History> _byValue = $pb.ProtobufEnum.initByValue(values);
  static ResourceDescriptor_History valueOf(int value) => _byValue[value];

  const ResourceDescriptor_History._(int v, String n) : super(v, n);
}

