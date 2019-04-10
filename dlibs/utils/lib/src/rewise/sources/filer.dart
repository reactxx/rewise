import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:path/path.dart' as p;
import 'dom.dart';

class FileInfo {
  factory FileInfo.fromPath(String path) {
    final parts = p.split(path);
    assert(parts.length == 3);
    final np = parts[2].split('.');
    assert(np.length == 3 || np.length == 2);
    return FileInfo._(
        parts[0],
        parts[1],
        np[0],
        np.length == 2
            ? FileType.LANG
            : (np[0].isEmpty ? FileType.LEFT : FileType.LANGLEFT),
        Filer.bookNameToType(parts));
  }
  FileInfo._(
      this.leftLang, this.bookName, this.lang, this.fileType, this.bookType);

  final String leftLang;
  final String lang;
  final String bookName;
  final int fileType;
  final int bookType;

  String get path => Filer.getFileName(this);
  File get readFile => File.fromPath(path);
  String get dataLang => fileType == FileType.LANG ? lang : leftLang;
}

class Filer {
  static String getFileNameLow(dynamic fileOrMsg) {
    String path =
        '${fileOrMsg.bookType == BookType.ETALK ? 'all' : fileOrMsg.leftLang}\\${fileOrMsg.bookName}\\';
    switch (fileOrMsg.fileType) {
      case FileType.LEFT:
        return '$path.left.csv';
      case FileType.LANG:
        return '$path${fileOrMsg.lang}.csv';
      case FileType.LANGLEFT:
        return '$path${fileOrMsg.lang}.left.csv';
      default:
        throw Exception();
    }
  }

  static String getFileName(FileInfo f) => getFileNameLow(f);

  static List<FileInfo> get files =>
      _files ??
      (_files = fileSystem.source
          .list(regExp: r'\.csv$')
          .map((f) => FileInfo.fromPath(f))
          .toList());

  static int bookNameToType(List<String> parts) {
    if (parts[0] == 'all') return BookType.ETALK;
    if (parts[1] == '#kdictionaries') return BookType.KDICT;
    return parts[1].startsWith('#') ? BookType.DICT : BookType.BOOK;
  }

  static List<FileInfo> _files;
}
