// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'data_unicode.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UncRange _$UncRangeFromJson(Map<String, dynamic> json) {
  return UncRange()
    ..start = json['start'] as int
    ..end = json['end'] as int
    ..idx = json['idx'] as int;
}

Map<String, dynamic> _$UncRangeToJson(UncRange instance) {
  final val = <String, dynamic>{};

  void writeNotNull(String key, dynamic value) {
    if (value != null) {
      val[key] = value;
    }
  }

  writeNotNull('start', instance.start);
  writeNotNull('end', instance.end);
  writeNotNull('idx', instance.idx);
  return val;
}

UncBlocks _$UncBlocksFromJson(Map<String, dynamic> json) {
  return UncBlocks()
    ..ISO15924 = (json['ISO15924'] as List)?.map((e) => e as String)?.toList()
    ..ranges = (json['ranges'] as List)
        ?.map((e) =>
            e == null ? null : UncRange.fromJson(e as Map<String, dynamic>))
        ?.toList();
}

Map<String, dynamic> _$UncBlocksToJson(UncBlocks instance) {
  final val = <String, dynamic>{};

  void writeNotNull(String key, dynamic value) {
    if (value != null) {
      val[key] = value;
    }
  }

  writeNotNull('ISO15924', instance.ISO15924);
  writeNotNull('ranges', instance.ranges?.map((e) => e?.toJson())?.toList());
  return val;
}
