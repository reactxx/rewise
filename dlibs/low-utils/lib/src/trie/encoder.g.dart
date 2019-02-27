// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'encoder.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

InputNode _$InputNodeFromJson(Map<String, dynamic> json) {
  return InputNode(json['key'] as String,
      json['data'] == null ? null : base64Decode(json['data'] as String));
}

Map<String, dynamic> _$InputNodeToJson(InputNode instance) {
  final val = <String, dynamic>{};

  void writeNotNull(String key, dynamic value) {
    if (value != null) {
      val[key] = value;
    }
  }

  writeNotNull('key', instance.key);
  writeNotNull(
      'data', instance.data == null ? null : base64Encode(instance.data));
  return val;
}
