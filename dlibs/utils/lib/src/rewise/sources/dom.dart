import 'package:rw_utils/dom/dom.dart' as d;
import 'package:rw_utils/utils.dart' show fileSystem, Matrix;

class FileMsg_BookType {
  static const ETALK = 0;
}

class FileMsg_FileType {
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

  String get fileName {
    if (bookType == FileMsg_BookType.ETALK) return '$bookName\\$lang.msg';
    String path = '$leftLang\\$bookName\\';
    switch (fileType) {
      case FileMsg_FileType.LEFT:
        return '$path.left.msg';
      case FileMsg_FileType.LANG:
        return '$path$lang.msg';
      case FileMsg_FileType.LANGLEFT:
        return '$path$lang.left.msg';
      default:
        throw Exception();
    }
  }

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
  static String toText(d.FactsMsg f) {
    final buf = StringBuffer();
    for (final f in f.facts) Fact.toText(f, buf);
    return buf.toString();
  }

  static Iterable<List<String>> toRows(d.FactsMsg f) sync* {
    var row = _createRow();
    final noFacts = f.facts.length == 0;
    final txt = noFacts ? toText(f) : null;
    row[0] = _ctrlFacts;
    row[1] = noFacts ? '' : txt.hashCode.toString();
    row[2] = f.lesson;
    row[4] = noFacts ? f.asString : txt;
    yield row;
    yield* f.facts.expand((f) => Fact.toRows(f));
  }

  static d.FactsMsg fromRows(Iterator<List<String>> iter) {
    assert(iter.current[0] == _ctrlFacts);

    final r = iter.current;
    final crc = r[1].isEmpty ? 0 : int.parse(r[1]);
    final res = d.FactsMsg()..lesson = r[2];

    iter.moveNext();
    final noFacts = iter.current == null || iter.current[0] == _ctrlFacts;

    // no facts or modified r[3]
    var txt = r[3];
    final useText = noFacts || txt.hashCode != crc;

    // read facts
    while (iter.current != null && iter.current[0] != _ctrlFacts)
      if (useText)
        iter.moveNext();
      else
        res.facts.add(Fact.fromRows(iter));

    if (useText)
      res.asString = txt;
    else {
      txt = res.toString();
      if (txt.hashCode != crc) {
        // modified facts
        res.facts.clear();
        res.asString = txt;
      } else
        res.crc = crc; // not zero crc => fact is upto date
    }
    return res;
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

List<String> _createRow() => List<String>.filled(_rowLen, '');
const _rowLen = 5;
const _spaces = '\u{FEFF} \u{3000}';
//const _blank = '-$_spaces-';
const _ctrlBook = '###$_spaces';
const _ctrlFacts = '##$_spaces';
const _ctrlFact = '#$_spaces';
