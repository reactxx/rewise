// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'serialize_test.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Person _$PersonFromJson(Map<String, dynamic> json) {
  return Person(
      firstName: json['firstName'] as String,
      lastName: json['lastName'] as String,
      subPerson: json['subPerson'] == null
          ? null
          : Person.fromJson(json['subPerson'] as Map<String, dynamic>),
      subPersons: (json['subPersons'] as List)
          ?.map((e) =>
              e == null ? null : Person.fromJson(e as Map<String, dynamic>))
          ?.toList());
}

Map<String, dynamic> _$PersonToJson(Person instance) {
  final val = <String, dynamic>{};

  void writeNotNull(String key, dynamic value) {
    if (value != null) {
      val[key] = value;
    }
  }

  writeNotNull('firstName', instance.firstName);
  writeNotNull('lastName', instance.lastName);
  writeNotNull('subPerson', instance.subPerson?.toJson());
  writeNotNull(
      'subPersons', instance.subPersons?.map((e) => e?.toJson())?.toList());
  return val;
}
