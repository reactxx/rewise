import 'dart:io' as io;
import 'package:test/test.dart' as test;
import 'package:path/path.dart' as p;

main() {
  test.test('path', () {
    var c = p.current;
    c = p.relative(r'c:\x.y');
    io.Directory.current = r'c:\temp';
    c = p.current;
    c = p.relative(r'\x.y');
    c = p.relative(r'/temp/x.y');
    c = p.absolute(r'x.y');
    c = p.join(r'x.y');
    c = p.relative(r'/temp/a/b\c/x.y');
    p.split(r'C:\path\to\foo');
    test.expect(c, test.equals(c));
  });
}
