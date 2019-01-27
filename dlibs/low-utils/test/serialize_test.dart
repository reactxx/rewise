import 'package:test/test.dart' as test;
import 'package:json_annotation/json_annotation.dart';
import 'dart:convert' as convert;
//import 'dart:html' as html;

part 'serialize_test.g.dart';

//pub run build_runner build
//https://github.com/dart-lang/json_serializable/tree/master/example

@JsonSerializable(nullable: true, explicitToJson: true, includeIfNull: false)
class Person {
  final String firstName;
  final String lastName;
  final Person subPerson;
  final List<Person> subPersons;
  Person({this.firstName, this.lastName, this.subPerson, this.subPersons});
  factory Person.fromJson(Map<String, dynamic> json) => _$PersonFromJson(json);
  Map<String, dynamic> toJson() => _$PersonToJson(this);
}

main() {
  test.test('serialization', () {
    var person = Person(
      firstName: 'fn', 
      subPerson: Person(firstName: 'subFn'),
      subPersons: [Person(firstName: 'subFns')]
    );  
    var json = person.toJson();
    final firstJsonStr = convert.jsonEncode(json);
    test.expect(firstJsonStr, test.equals('{"firstName":"fn","subPerson":{"firstName":"subFn"},"subPersons":[{"firstName":"subFns"}]}'));
    person = Person.fromJson(convert.jsonDecode(firstJsonStr));
    //person = Person.fromJson(convert.jsonDecode(convert.JsonEncoder.withIndent('  ').convert(json)));
    json = person.toJson();
    var jsonStr = convert.jsonEncode(json);
    test.expect(jsonStr, test.equals(firstJsonStr));
    // final root = html.querySelector('#root');
    // root.text = 'rootx';
    // test.expect(root.text, test.equals('rootx'));
    // print(root.text);
  });

}
