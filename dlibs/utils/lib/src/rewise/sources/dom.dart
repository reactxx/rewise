import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
import 'package:path/path.dart' as p;
import 'package:rw_utils/dom/word_breaking.dart' as br;
import 'package:rw_low/code.dart' show Dir;
import 'parser.dart';
import 'lexanal.dart';

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

class File {
  //extendsN d.FileMsg {

  String leftLang = '';
  String bookName = '';
  String lang = '';
  int bookType = 0;
  int fileType = 0;
  final factss = List<Facts>();

  File();
  //File.fromBuffer(List<int> i) : super.fromBuffer(i);
  factory File.fromPath(String relPath, [Dir dir]) {
    final m = Matrix.fromFile((dir ?? fileSystem.source).absolute(relPath));
    final rows = m.rows.map((r) => r.data);
    
    return File.fromRows(rows.iterator);
  } // : this.fromBuffer(fileSystem.source.readAsBytes(fn));

  // void save([Dir dir]) =>
  //     (dir ?? fileSystem.source).writeAsBytes(fileName, writeToBuffer());
  void save([Dir dir]) => Matrix.fromData(toRows())
      .save((dir ?? fileSystem.source).absolute(fileName));

  String get fileName => Filer.getFileNameLow(this);

  Iterable<List<String>> toRows() sync* {
    var row = _createRow();
    row[0] = _ctrlBook;
    row[1] = bookName;
    row[2] = leftLang;
    row[3] = lang;
    row[4] = '${bookType.toString()}.${fileType.toString()}';
    yield row;
    yield* factss.expand((f) => f.toRows());
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

  String get dataLang => fileType == FileType.LANG ? lang : leftLang;
}

class Facts {
  int id = 0;
  String crc = '';
  String asString = '';
  final facts = List<Fact>();
  String lesson = '';

  Facts();
  factory Facts.fromParser(Facts old, String srcText, List<br.PosLen> breaks) {
    assert(srcText!=null);
    final lex = parser(srcText, breaks);
    final res = Facts()
      ..id = old.id
      ..lesson = old.lesson
      ..facts.addAll(lex.facts.map((lf) {
        return Fact()
          ..wordClass = lf.wordClass
          ..flags = lf.flags
          ..words.addAll(lf.words.map((lw) {
            return Word(lw.before, lw.text, lw.after, lw.flags, lw.flagsData);
          }));
      }));
    final txt = res.toText();
    res.crc = txt.hashCode.toRadixString(32);
    return res;
  }

  String toText() {
    if (facts.length == 0) return asString;
    final buf = StringBuffer();
    for (final f in facts) f.toText(buf);
    return buf.toString();
  }

  Iterable<List<String>> toRows() sync* {
    var row = _createRow();
    final txt = toText();
    row[0] = _ctrlFacts;
    row[1] = crc;
    row[2] = lesson;
    row[3] = id.toString();
    row[4] = facts.length > 0 ? '' : txt;
    yield row;
    yield* facts.expand((f) => f.toRows());
  }

  factory Facts.fromRows(Iterator<List<String>> iter) {
    assert(iter.current[0] == _ctrlFacts);

    final r = iter.current;
    final res = Facts()
      ..crc = r[1]
      ..lesson = r[2]
      ..id = int.parse(r[3])
      ..asString = r[4];

    iter.moveNext();
    while (iter.current != null && iter.current[0] != _ctrlFacts)
      res.facts.add(Fact.fromRows(iter));

    return res;
  }

  String toRefresh() {
    final hasFacts = facts.length > 0;
    final asStringEmpty = asString.isEmpty;

    // !hasFacts
    if (!hasFacts && asStringEmpty) return null; // empty csv cell
    if (!hasFacts && crc.isEmpty) return asString; //first import from CVS

    // crc
    assert(crc.isNotEmpty);
    final crcNum = int.parse(crc, radix: 32);

    // asString changed
    if (!asStringEmpty && crcNum != asString.hashCode) return asString;

    assert(hasFacts); // something wrong

    // facts changed
    final txt = toText();
    return txt.hashCode != crcNum ? txt : null;
  }
}

class Fact {
  String wordClass = '';
  int flags = 0;
  final words = List<Word>();

  Fact();
  toText(StringBuffer buf) {
    if (wordClass.isNotEmpty) buf.write('[${wordClass}]');
    for (final w in words) w.toText(buf);
  }

  Iterable<List<String>> toRows() sync* {
    var row = _createRow();
    row[0] = _ctrlFact;
    row[1] = wordClass;
    row[2] = flags.toString();
    yield row;
    yield* words.map((w) => w.toRow());
  }

  factory Fact.fromRows(Iterator<List<String>> iter) {
    assert(iter.current[0] == _ctrlFact);
    final r = iter.current;
    final res = Fact()
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
  String text = '';
  String before = '';
  String after = '';
  int flags = 0;
  String flagsData = '';

  Word(this.before, this.text, this.after, this.flags, this.flagsData);
  Word.fromRow(List<String> row)
      : this(row[0], row[1], row[2], int.parse(row[3]), row[4]);

  bool isPartOf() => flags & Flags.wInOtherWord != 0;
  void toText(StringBuffer buf) {
    if (!isPartOf()) buf..write(before)..write(text)..write(after);
  }

  List<String> toRow() {
    var row = _createRow();
    row[0] = before;
    row[1] = text;
    row[2] = after;
    row[3] = flags.toString();
    row[4] = flagsData;
    return row;
  }
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

List<String> _createRow() => List<String>.filled(_rowLen, '');
const _rowLen = 5;
const _spaces = '\u{FEFF}';
const _ctrlBook = '###$_spaces';
const _ctrlFacts = '##$_spaces';
const _ctrlFact = '#$_spaces';
