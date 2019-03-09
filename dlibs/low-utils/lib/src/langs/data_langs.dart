import 'dart:convert' as convert;
import 'package:json_annotation/json_annotation.dart';
import 'data_langsData.dart';

part 'data_langs.g.dart';

//pub run build_runner build
@JsonSerializable(nullable: true, explicitToJson: true, includeIfNull: false)
class CldrLang {
  CldrLang();
  // CldrLang(this.id, this.lang, this.scriptId, this.defaultRegion,
  //     this.hasMoreScripts, this.regions, this.googleTransId);
  String id; // e.g. cs-CZ, sr-Latn, 1 for invariant locale
  String lang;
  String scriptId; // unicode script, e.g. Latn, Arab etc.
  String defaultRegion;
  bool hasMoreScripts;
  bool hasStemming;
  factory CldrLang.fromJson(Map<String, dynamic> json) =>
      _$CldrLangFromJson(json);
  factory CldrLang.fromJsonStr(String json) =>
      CldrLang.fromJson(convert.jsonDecode(json));
}

List<CldrLang> fromJsonStr() {
  Iterable l = getLangsData();
  return l.map((map) => CldrLang.fromJson(map)).toList();
}
