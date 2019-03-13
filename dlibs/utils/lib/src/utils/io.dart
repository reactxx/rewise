import 'dart:io';
import 'package:path/path.dart' as p;

class Dir {
  Dir(String _path) : path = p.absolute(_path) {}
  String path;
  Iterable<String> list(
      {String regExp, bool file = true, String from, bool isAbsolute = false}) {
    final src = from == null ? path : p.join(path, from);
    var res = Directory(src).listSync(recursive: true).where((f) {
      if (file == null) return true;
      if (file) return f is File;
      return f is Directory;
    }).map((f) => p.relative(f.path, from: path));
    if (regExp != null) {
      final rx = RegExp(regExp, caseSensitive: false);
      res = res.where((f) => rx.hasMatch(f));
    }
    res = isAbsolute ? res.map((f) => absolute(f)) : res;
    return res;
  }

  Iterable<String> toAbsolute(Iterable<String> relSource) {
    return relSource.map((f) => p.join(path, f));
  }

  Iterable<String> changeExtension(Iterable<String> relSource, String ext) {
    return relSource.map((f) => p.setExtension(f, ext));
  }

  String absolute(String relPath) => p.join(path, relPath);

  String readAsString(String relPath) =>
      File(absolute(relPath)).readAsStringSync();
  void writeAsString(String relPath, String content) =>
      File(absolute(relPath)).writeAsStringSync(content);

  List<String> readAsLines(String relPath) =>
      File(absolute(relPath)).readAsLinesSync();
  void writeAsLines(String relPath, Iterable<String> lines) {
    final file = File(absolute(relPath)).openWrite();
    try {
      for (final l in lines) file.writeln(l);
    } finally {
      file.close();
    }
  }

  List<int> readAsBytes(String relPath) =>
      File(absolute(relPath)).readAsBytesSync();
  void writeAsBytes(String relPath, List<int> content) =>
      File(absolute(relPath)).writeAsBytesSync(content);

}
