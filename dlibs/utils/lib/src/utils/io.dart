import 'dart:io';
import 'package:path/path.dart' as p;

class Dir {
  Dir(String _path) : path = p.absolute(_path) {}
  String path;
  Iterable<String> list(
      {String regExp,
      bool file = true,
      String from,
      bool isAbsolute = false,
      String relTo}) {
    final src = from == null ? path : p.join(path, from);
    var files = Directory(src).listSync(recursive: true).where((f) {
      if (file == null) return true;
      if (file) return f is File;
      return f is Directory;
    });
    String relToPath = relTo == null
        ? path
        : (p.isAbsolute(relTo) ? relTo : p.join(path, relTo));
    var res = files.map((f) => p.relative(f.path, from: relToPath));
    if (regExp != null) {
      final rx = RegExp(regExp, caseSensitive: false);
      res = res.where((f) => rx.hasMatch(f));
    }
    if (isAbsolute) res = res.map((f) => absolute(f));
    res = res;
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
      File(absolute(relPath))..createSync(recursive: true)..writeAsStringSync(content);

  List<String> readAsLines(String relPath) =>
      File(absolute(relPath)).readAsLinesSync();
  void writeAsLines(String relPath, Iterable<String> lines) {
    final sb = StringBuffer();
    for (final l in lines) sb.writeln(l);
    var s = sb.toString();
    File(absolute(relPath))..createSync(recursive: true)..writeAsStringSync(sb.toString());
  }

  List<int> readAsBytes(String relPath) =>
      File(absolute(relPath)).readAsBytesSync();
  void writeAsBytes(String relPath, List<int> content) =>
      File(absolute(relPath))..createSync(recursive: true)..writeAsBytesSync(content);
}
