import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
import 'package:rw_low/code.dart' show Dir;
import 'consts.dart';

class File extends FileInfo {
  final factss = List<Facts>();

  File();

  factory File.fromMatrix(Matrix m) {
    final rows = m.rows.map((r) => r.data);
    return File.fromRows(rows.iterator);
  }
  factory File.fromPath(String relPath, {Dir dir}) {
    return File.fromMatrix(
        Matrix.fromFile((dir ?? fileSystem.source).absolute(relPath)));
  }
  factory File.fromFileInfo(FileInfo info) {
    return File.fromPath(info.fileName);
  }

  File.fromRows(Iterator<List<String>> iter) {
    final m = iter.moveNext();
    assert(m);
    assert(iter.current[0] == _ctrlBook);
    var r = iter.current;
    bookName = r[1];
    leftLang = r[2];
    lang = r[3];
    final f = r[4].split('.');
    bookType = int.parse(f[0]);
    fileType = int.parse(f[1]);
    final editMode = int.parse(f[2]);
    iter.moveNext();
    while (iter.current != null)
      factss.add(Facts.fromRows(iter, editMode: editMode));
  }

  Iterable<List<String>> toRows(
      {int editMode = EditMode.NONE, Iterable<Facts> subFactss}) sync* {
    var row = _createRow();
    row[0] = _ctrlBook;
    row[1] = bookName;
    row[2] = leftLang;
    row[3] = lang;
    row[4] =
        '${bookType.toString()}.${fileType.toString()}.${editMode.toString()}';
    yield row;
    yield* (subFactss ?? factss).expand((f) => f.toRows(editMode: editMode));
  }

  void save(
          {Dir dir, int editMode = EditMode.NONE, Iterable<Facts> subFactss}) =>
      Matrix.fromData(toRows(editMode: editMode, subFactss: subFactss))
          .save((dir ?? fileSystem.source).absolute(fileName));
}

class Facts {
  int id = 0;
  String crc = '';
  String asString = '';
  List<Fact> facts;
  String lesson = '';

  Facts() : facts = List<Fact>();
  Facts.fromFacts(this.facts);

  factory Facts.fromRows(Iterator<List<String>> iter, {int editMode}) {
    assert(iter.current[0] == _ctrlFacts);
    final r = iter.current;
    Facts res;
    var withFacts = true;
    switch (editMode) {
      case EditMode.NONE:
        res = Facts()
          ..crc = r[1]
          ..lesson = r[2]
          ..id = int.parse(r[3])
          ..asString = r[4];
        break;
      case EditMode.ASSTRING:
        res = Facts()
          ..id = int.parse(r[1])
          ..asString = r[2];
        withFacts = false;
        break;
      default:
        throw Exception();
    }

    iter.moveNext();
    while (iter.current != null && iter.current[0] != _ctrlFacts)
      if (withFacts)
        res.facts.add(Fact.fromRows(iter));
      else
        iter.moveNext();

    return res;
  }

  Iterable<List<String>> toRows({int editMode}) sync* {
    var row = _createRow();
    row[0] = _ctrlFacts;
    var withFacts = true;
    switch (editMode) {
      case EditMode.NONE:
        row[1] = crc;
        row[2] = lesson;
        row[3] = id.toString();
        row[4] = facts.length > 0 ? '' : toText();
        break;
      case EditMode.ASSTRING:
        withFacts = false;
        row[1] = id.toString();
        row[2] = toText();
        break;
      default:
        throw Exception();
    }
    yield row;
    if (!withFacts || facts.length == 0) return;
    yield* facts.expand((f) => f.toRows());
  }

  String toText() {
    if (facts.length == 0) return asString;
    final buf = StringBuffer();
    for (final f in facts) f.toText(buf);
    return buf.toString();
  }

}

class Fact {
  String wordClass = '';
  int flags = 0;
  final words = List<Word>();

  var canHaveWordClass = false;

  Fact();

  String get flagsText => Flags.toText(flags);

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

  Iterable<List<String>> toRows() sync* {
    var row = _createRow();
    row[0] = _ctrlFact;
    row[1] = wordClass;
    row[2] = flags.toString();
    yield row;
    yield* words.map((w) => w.toRow());
  }

  toText(StringBuffer buf) {
    //if (wordClass.isNotEmpty) buf.write('[${wordClass}]');
    for (final w in words) w.toText(buf);
  }

  bool get isEmpty => words.length == 0 && flags == 0; // && wordClass.isEmpty;
}

class Word {
  String text = '';
  String before = '';
  String after = ''; 
  int flags = 0;
  String flagsData = '';

  Word(this.before, this.text, this.after, this.flags, this.flagsData);
  Word.fromText(this.text);
  Word.fromRow(List<String> row)
      : this(row[0], row[1], row[2], int.parse(row[3]), row[4]);

  bool get isPartOf => flags & Flags.wIsPartOf != 0;
  void toText(StringBuffer buf) {
    buf.write(before);
    if (!isPartOf) buf.write(text);
    buf.write(after);
    //buf..write(before)..write(text)..write(after);
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

List<String> _createRow() => List<String>.filled(_rowLen, '');
const _rowLen = 5;
const _spaces = '\u{3000}';
const _ctrlBook = '###$_spaces';
const _ctrlFacts = '##$_spaces';
const _ctrlFact = '#$_spaces';
