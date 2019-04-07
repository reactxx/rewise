import 'dart:collection';
import 'package:rw_utils/utils.dart' show fileSystem, Matrix;

class Lang {
  String lang;
  List<String> data;
}

class LeftLang {
  String lang;
  List<String> left;
  List<String> data;
}

class LangDatas {
  LangDatas(this.type, this.path);
  final int type;
  final String path;
  // except etalk
  String lang;
  // for book
  String newName; // name in brackets
  // for book and KDict
  List<String> left;
  // for book
  List<int> lessons;
  // for book and dict
  List<LeftLang> leftLangs;
  // for etalk and KDict
  List<Lang> langs;
}

class SrcFiles {
  static const kdict = 0; // kdict
  static const dict = 1; // lingea and other dicts
  static const etalk = 2; // goethe, eurotalk
  static const book = 3; // templates and local dicts
  static final filters = <String>[
    r'^dictionaries\\kdictionaries\\.*',
    r'^dictionaries\\.*',
    r'^local_dictionaries\\all\\.*',
    r'^templates\\.*'
  ];
  static final files = filters
      .map((f) =>
          HashSet<String>.from(fileSystem.csv.list(regExp: f + r'\.csv$')))
      .toList();

  static final oldLangs = HashSet<String>.from([0, 1, 2, 3].expand((type) =>
      getFiles(type)
          .map((fn) => fileSystem.csv.readAsLines(fn).first.split(';')).expand((l) => l))).toList();

  static Iterable<String> getFiles(int type) {
    switch (type) {
      case kdict:
      case etalk:
      case book:
        return files[type];
      case dict:
        return files[dict].difference(files[kdict]);
      default:
        throw Exception();
    }
  }

  static Iterable<LangDatas> getMatrix(int type) sync* {
    List<String> getColumn(Matrix matrix, int colIdx) =>
        matrix.rows.skip(1).map((r) => r.data[colIdx]).toList();

    for (final fn in getFiles(type)) {
      final res = LangDatas(type, fn);
      final matrix = Matrix.fromFile(fileSystem.csv.absolute(fn));
      final firstRow = matrix.rows[0].data;
      if (type == book) {
        assert(firstRow.length == 2);
        assert(firstRow[0] == '_lesson');
        res.lang = firstRow[1];
        res.left = getColumn(matrix, 1);
        res.lessons = getColumn(matrix, 0).map((s) => int.parse(s)).toList();
      }
      yield res;
    }
  }

  static final newNameRx = RegExp(r'\((.*?)\)\.csv$');
}
