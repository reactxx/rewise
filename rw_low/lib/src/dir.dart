import 'dart:io';
import 'package:path/path.dart' as p;

Iterable<String> _empty() sync* {
  if ('' == ' ') yield '';
}

class Dir {
  Dir(String _path) : path = p.absolute(_path) {}
  String path;
  Iterable<String> list(
      {String regExp,
      bool filter(String relPage),
      bool file = true,
      String from,
      bool isAbsolute = false,
      String relTo}) {
    final src = from == null ? path : p.join(path, from);
    if (!Directory(src).existsSync()) return _empty();
    var files = Directory(src).listSync(recursive: true).where((f) {
      if (file == null) return true;
      return file ? f is File : f is Directory;
    });
    String relToPath = relTo == null
        ? path
        : (p.isAbsolute(relTo) ? relTo : p.join(path, relTo));
    Iterable<String> res =
        files.map((f) => p.relative(f.path, from: relToPath));
    if (filter != null) res = res.where(filter);
    if (regExp != null) {
      final rx = RegExp(regExp, caseSensitive: false);
      res = res.where((f) => rx.hasMatch(f));
    }
    if (isAbsolute) res = res.map((f) => absolute(f));
    return res;
  }

  String adjustExists(String relPath, {String ext}) {
    final fn = absolute(relPath, ext: ext);
    final file = File(fn);
    if (!file.existsSync()) file.createSync(recursive: true);
    return fn;
  }

  void use(String relPath, void run(RandomAccessFile rdr),
      {String ext, FileMode mode = FileMode.write}) {
    final fn = adjustExists(relPath, ext: ext);
    final rdr = File(fn).openSync(mode: mode);
    try {
      run(rdr);
    } finally {
      rdr.closeSync();
    }
  }

  Iterable<String> toAbsolute(Iterable<String> relSource) {
    return relSource.map((f) => p.join(path, f));
  }

  Iterable<String> changeExtension(Iterable<String> relSource, String ext) {
    return relSource.map((f) => p.setExtension(f, ext));
  }

  String absolute(String relPath, {String ext}) =>
      p.join(path, ext == null ? relPath : p.setExtension(relPath, ext));

  String readAsString(String relPath, {String ext}) =>
      File(absolute(relPath, ext: ext)).readAsStringSync();
  void writeAsString(String relPath, String content, {String ext}) =>
      File(absolute(relPath, ext: ext))
        ..createSync(recursive: true)
        ..writeAsStringSync(content);

  List<String> readAsLines(String relPath, {String ext}) =>
      File(absolute(relPath, ext: ext)).readAsLinesSync();
  void writeAsLines(String relPath, Iterable<String> lines, {String ext}) {
    final sb = StringBuffer();
    for (final l in lines) sb.writeln(l);
    File(absolute(relPath, ext: ext))
      ..createSync(recursive: true)
      ..writeAsStringSync(sb.toString());
  }

  List<int> readAsBytes(String relPath, {String ext}) =>
      File(absolute(relPath, ext: ext)).readAsBytesSync();
  void writeAsBytes(String relPath, List<int> content, {String ext}) =>
      File(absolute(relPath, ext: ext))
        ..createSync(recursive: true)
        ..writeAsBytesSync(content);
}

adjustFileDir(String fn) {
  final dir = Directory(p.dirname(fn));
  if(dir.existsSync()) return;
  dir.createSync(recursive:true);
}