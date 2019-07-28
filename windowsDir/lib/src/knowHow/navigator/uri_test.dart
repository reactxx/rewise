// https://github.com/google/uri.dart
import 'package:uri/uri.dart';
import 'package:json_annotation/json_annotation.dart';

part 'uri_test.g.dart';

@JsonSerializable()  
class Par {
  Par({this.name, this.id});
  final String name;
  final int id;
}

abstract class RouteLow<T> {
  RouteLow(String templateStr) {
    _parser = UriParser(_template = UriTemplate(templateStr));
  }
  
  UriMatch match(Uri uri) => _parser.match(uri);
  bool matches(Uri uri) => _parser.matches(uri);
  
  T fromUrl(String url) => fromUri(Uri.parse(url));
  T fromUri(Uri uri) {
    final m = match(uri);
    return m == null || m.parameters == null ? null : fromMap(m.parameters);
  }
  String toUrl(T obj) => _template.expand(toMap(obj));

  T fromMap(Map<String, dynamic> map);
  Map<String, dynamic> toMap(T obj);

  UriTemplate _template;
  UriParser _parser;
}

class RouterPar extends RouteLow<Par> {
  RouterPar(String templateStr) : super(templateStr); 
  @override
  Par fromMap(Map<String, dynamic> map) => _$ParFromJson(map); 
  @override
  Map<String, dynamic> toMap(Par obj) => _$ParToJson(obj);
}

void test() {
  final t = UriTemplate('/{+rval}/b/{?d,e}');
  var m = UriParser(t, queryParamsAreOptional: true)
      .match(Uri.parse('/a/aa/b/c?e=2'));

  final t2 = UriTemplate('/aa/');
  final t3 = UriTemplate('b/');
  final t4 = UriTemplate('c{?d}');
  final t5 = UriTemplate('{?d}');

  final uri = Uri.parse('/aa/b/c?d=1');
  m = UriParser(t2).match(uri);
  m = UriParser(t3).match(m.rest);
  m = UriParser(t5).match(m.rest);

  final uri2 = Uri.parse('x/y/c?d=1');
  m = UriParser(t4).match(uri2);
  m = UriParser(t5).match(uri2);

  // does not work for parser
  final t6 = UriTemplate('/aa{x}');
  final uri3 = Uri.parse('/aa.x=1');
  m = UriParser(t6).match(uri3);

  m = null;
}
