// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'data_langs.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

CldrLang _$CldrLangFromJson(Map<String, dynamic> json) {
  return CldrLang()
    ..id = json['id'] as String
    ..lang = json['lang'] as String
    ..scriptId = json['scriptId'] as String
    ..defaultRegion = json['defaultRegion'] as String
    ..hasMoreScripts = json['hasMoreScripts'] as bool
    ..hasStemming = json['hasStemming'] as bool;
}

Map<String, dynamic> _$CldrLangToJson(CldrLang instance) {
  final val = <String, dynamic>{};

  void writeNotNull(String key, dynamic value) {
    if (value != null) {
      val[key] = value;
    }
  }

  writeNotNull('id', instance.id);
  writeNotNull('lang', instance.lang);
  writeNotNull('scriptId', instance.scriptId);
  writeNotNull('defaultRegion', instance.defaultRegion);
  writeNotNull('hasMoreScripts', instance.hasMoreScripts);
  writeNotNull('hasStemming', instance.hasStemming);
  return val;
}
