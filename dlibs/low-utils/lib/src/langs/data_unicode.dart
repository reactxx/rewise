import 'dart:convert' as convert;
import 'package:json_annotation/json_annotation.dart';
import 'data_unicodeData.dart';

part 'data_unicode.g.dart';

//pub run build_runner build
@JsonSerializable(nullable: true, explicitToJson: true, includeIfNull: false)
class UncRange {
  UncRange();
  int start;
  int end;
  int idx;
  factory UncRange.fromJson(Map<String, dynamic> json) =>
      _$UncRangeFromJson(json);
  Map<String, dynamic> toJson() => _$UncRangeToJson(this);
  factory UncRange.fromJsonStr(String json) =>
      UncRange.fromJson(convert.jsonDecode(json));
}

@JsonSerializable(nullable: true, explicitToJson: true, includeIfNull: false)
class UncBlocks {
  UncBlocks();
  List<String> ISO15924;
  List<UncRange> ranges;
  factory UncBlocks.fromJson(Map<String, dynamic> json) =>
      _$UncBlocksFromJson(json);
  factory UncBlocks.fromJsonStr(String json) =>
      UncBlocks.fromJson(convert.jsonDecode(json));
  factory UncBlocks.fromData() => UncBlocks.fromJson(getUnicodeData());
}
