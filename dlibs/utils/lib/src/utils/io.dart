import 'dart:io';
import 'package:path/path.dart' as p;

class Dir {
  Dir(String _path): path = p.absolute(_path) { }
  String path;
  Iterable<String> files({RegExp filter, bool file = true}) {
    var res = Directory(path).listSync(recursive: true).where((f) {
      if (file == null) return true;
      if (file) return f is File;
      return f is Directory;
    }).map((f) => p.relative(f.path, from: path));
    return filter != null ? res.where((f) => filter.hasMatch(f)) : res;
  }
  Iterable<String> map(Iterable<String> source) {
    return source.map((f) => p.join(path, f));
  }
}
