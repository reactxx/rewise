import 'package:rw_utils/dom/dom.dart' as d;
import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
import 'package:path/path.dart' as p;
import 'package:rw_utils/dom/word_breaking.dart' as br;
import 'parser.dart';

class BookType {
  static const KDICT = 0; // kdict
  static const DICT = 1; // lingea and other dicts
  static const ETALK = 2; // goethe, eurotalk
  static const BOOK = 3; // templates and local dicts
}

class FileType {
  static const LEFT = 0;
  static const LANG = 1;
  static const LANGLEFT = 2;
}

class File extends d.FileMsg {
  File();
  File.fromBuffer(List<int> i) : super.fromBuffer(i);
  File.fromPath(String fn) : this.fromBuffer(fileSystem.source.readAsBytes(fn));

  void save() => fileSystem.source.writeAsBytes(fileName, writeToBuffer());
  void toCSV() => Matrix.fromData(toRows())
      .save(fileSystem.source.absolute(fileName, ext: '.csv'));

  String get fileName => Filer.getFileNameLow(this);

  Iterable<List<String>> toRows() sync* {
    var row = _createRow();
    row[0] = _ctrlBook;
    row[1] = bookName;
    row[2] = leftLang;
    row[3] = lang;
    row[4] = '${bookType.toString()}.${fileType.toString()}';
    yield row;
    yield* factss.expand((f) => Facts.toRows(f));
  }

  File.fromRows(Iterator<List<String>> iter) {
    assert(iter.moveNext());
    assert(iter.current[0] == _ctrlBook);
    var r = iter.current;
    bookName = r[1];
    leftLang = r[2];
    lang = r[3];
    final f = r[4].split('.');
    bookType = int.parse(f[0]);
    fileType = int.parse(f[1]);
    iter.moveNext();
    while (iter.current != null) factss.add(Facts.fromRows(iter));
  }
}

class Facts {
  static d.FactsMsg fromParser(d.FactsMsg old, String srcText, List<br.PosLen> breaks) {
    final lex = parser(srcText, breaks);
    final res = d.FactsMsg()..id = old.id..lesson = old.lesson..facts.addAll(lex.facts.map((lf) {
      return d.FactMsg()..wordClass = lf.wordClass..flags = lf.flags..words.addAll(lf.words.map((lw) {
        return d.WordMsg()..text = lw.text..before = lw.before..after = lw.after..flags = lw.flags..flagsData = lw.flagsData;
      }));
    }));
    final txt = Facts.toText(res);
    res.crc = txt.hashCode.toString();
    return res;
  }

  static String toText(d.FactsMsg f) {
    if (f.facts.length == 0) return f.asString;
    final buf = StringBuffer();
    for (final f in f.facts) Fact.toText(f, buf);
    return buf.toString();
  }

  static Iterable<List<String>> toRows(d.FactsMsg f) sync* {
    var row = _createRow();
    final txt = toText(f);
    row[0] = _ctrlFacts;
    row[1] = f.crc;
    row[2] = f.lesson;
    row[3] = f.id.toString();
    row[4] = txt;
    yield row;
    yield* f.facts.expand((f) => Fact.toRows(f));
  }

  static d.FactsMsg fromRows(Iterator<List<String>> iter) {
    assert(iter.current[0] == _ctrlFacts);

    final r = iter.current;
    final res = d.FactsMsg()
      ..crc = r[1]
      ..lesson = r[2]
      ..id = int.parse(r[3])
      ..asString = r[4];

    iter.moveNext();
    while (iter.current != null && iter.current[0] != _ctrlFacts)
      res.facts.add(Fact.fromRows(iter));

    return res;
  }

  static String toRefresh(d.FactsMsg f) {
    final hasFacts = f.facts.length > 0;
    final asStringEmpty = f.asString.isEmpty;

    // !hasFacts
    if (!hasFacts && asStringEmpty) return null; // empty csv cell
    if (!hasFacts && f.crc.isEmpty) return f.asString; //first import from CVS

    // crc
    assert(f.crc.isNotEmpty);
    final crc = int.parse(f.crc) & 0xffffffff;

    // asString changed
    if (!asStringEmpty && crc != f.asString.hashCode) return f.asString;

    assert(hasFacts); // something wrong

    // facts changed
    final txt = toText(f);
    return txt.hashCode != crc ? txt : null;
  }

}

class Fact {
  static toText(d.FactMsg f, StringBuffer buf) {
    if (f.wordClass.isNotEmpty) buf.write('[${f.wordClass}]');
    for (final w in f.words) Word.toText(w, buf);
  }

  static Iterable<List<String>> toRows(d.FactMsg f) sync* {
    var row = _createRow();
    row[0] = _ctrlFact;
    row[1] = f.wordClass;
    row[2] = f.flags.toString();
    yield row;
    yield* f.words.map((w) => Word.toRow(w));
  }

  static d.FactMsg fromRows(Iterator<List<String>> iter) {
    assert(iter.current[0] == _ctrlFact);
    final r = iter.current;
    final res = d.FactMsg()
      ..wordClass = r[1]
      ..flags = int.parse(r[2]);
    while (iter.moveNext() &&
        iter.current[0] != _ctrlFact &&
        iter.current[0] != _ctrlFacts)
      res.words.add(Word.fromRow(iter.current));
    return res;
  }
}

class Word {
  static bool isPartOf(d.WordMsg w) =>
      w.flags & 0 != 0; //Flags.wInOtherWord != 0;
  static toText(d.WordMsg w, StringBuffer buf) {
    if (!isPartOf(w)) buf..write(w.before)..write(w.text)..write(w.after);
  }

  static List<String> toRow(d.WordMsg f) {
    var row = _createRow();
    row[0] = f.before;
    row[1] = f.text;
    row[2] = f.after;
    row[3] = f.flags.toString();
    row[4] = f.flagsData;
    return row;
  }

  static d.WordMsg fromRow(List<String> row) => d.WordMsg()
    ..before = row[0]
    ..text = row[1]
    ..after = row[2]
    ..flags = int.parse(row[3])
    ..flagsData = row[4];
}

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
}

class Filer {
  static String getFileNameLow(dynamic fOrMsg) {
    String path =
        '${fOrMsg.bookType == BookType.ETALK ? 'all' : fOrMsg.leftLang}\\${fOrMsg.bookName}\\';
    switch (fOrMsg.fileType) {
      case FileType.LEFT:
        return '$path.left.msg';
      case FileType.LANG:
        return '$path${fOrMsg.lang}.msg';
      case FileType.LANGLEFT:
        return '$path${fOrMsg.lang}.left.msg';
      default:
        throw Exception();
    }
  }

  static String getFileName(FileInfo f) => getFileNameLow(f);

  static List<FileInfo> get files =>
      _files ??
      (_files = fileSystem.source
          .list(regExp: r'\.msg$')
          .map((f) => FileInfo.fromPath(f))
          .toList());

  static int bookNameToType(List<String> parts) {
    if (parts[0] == 'all') return BookType.ETALK;
    if (parts[1] == 'kdictionaries') return BookType.KDICT;
    return _allDicts.contains(parts[1]) ? BookType.KDICT : BookType.BOOK;
  }

  static const _allDicts = <String>{
    'bangla',
    'bdword',
    'cambridge',
    'collins',
    'dictcc',
    'enacademic',
    'google',
    'handpicked',
    'indirect',
    //'kdictionaries',
    'lingea',
    'lm',
    'memrise',
    'reverso',
    'shabdosh',
    'vdict',
    'wiktionary',
  };

  static List<FileInfo> _files;
}

List<String> _createRow() => List<String>.filled(_rowLen, '');
const _rowLen = 5;
const _spaces = '\u{FEFF} \u{3000}';
//const _blank = '-$_spaces-';
const _ctrlBook = '###$_spaces';
const _ctrlFacts = '##$_spaces';
const _ctrlFact = '#$_spaces';
