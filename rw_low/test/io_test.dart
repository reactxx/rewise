import 'package:test/test.dart' as test;
import 'package:rw_low/code.dart';
import 'package:path/path.dart' as p;
import 'dart:io' as io;

main() {
  test.test('diff-test', () {
    var dir = Dir(r'\temp');
    var mapDir = Dir(r'\new');
    List<String> list = List.from(dir.list());
    list = list = List.from(dir.list(isAbsolute: true));
    list = list = List.from(dir.list(from: 'obj'));
    list = List.from(dir.list(regExp: r'\.zip$'));
    list =List.from(mapDir.toAbsolute(list));
    list = List.from(dir.list(file: null));
    list = List.from(dir.list(file: false));
    test.expect(list, test.equals(list));
  });

  test.test('path', () {
    var c = p.current;
    c = p.relative(r'c:\x.y');
    io.Directory.current = r'c:\temp';
    c = p.current;
    c = p.relative(r'\x.y');
    c = p.absolute(r'\x.y');
    c = p.absolute(r'x.y');
    c = p.relative(r'/temp/x.y');
    c = p.absolute(r'x.y');
    c = p.join(r'x.y');
    c = p.relative(r'/temp/a/b\c/x.y');
    p.split(r'C:\path\to\foo');
    test.expect(c, test.equals(c));
  });

}
