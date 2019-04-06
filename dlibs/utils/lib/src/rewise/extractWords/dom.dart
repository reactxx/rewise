import 'dart:math';

class Flags {
  static const wInBr = 0x1; // word is in () bracket
  static const wInOtherWord = 0x2; // word is part of another word
  static const wBrCurl = 0x4; // word contains whole content of {} brackets
  static const wIsLatin = 0x8; // latin word in non latin text

  static const feDelimInBracket = 0x10; // ^ or | in brackets
  static const feMissingBr = 0x20; // missing ) bracket
  static const feMissingCurlBr = 0x40; // missing } bracket
  static const feMissingSqBr = 0x80; // missing ] bracket
  static const feUnexpectedBr = 0x100; // unexpected ) bracket
  static const feUnexpectedCurlBr = 0x200; // unexpected } bracket
  static const feUnexpectedSqBr = 0x400; // unexpected ] bracket
  static const feNoWordInFact = 0x800; // no word in fact
  static const feMixingBrs = 0x1000; // mixing different brakets
  static const feSingleWClassAllowed = 0x2000; // more than single []
  static const feMissingWClass = 0x4000; // missing []
  static const feWClassNotInFirstFact = 0x8000; // [] not in first fact

  static const weOtherScript = 0x10000; // e.g. left word is in right script
  static const weWrongUnicode = 0x20000;
  static const weWrongCldr = 0x40000;

}

class Word {
  Word._();
  Word(this.text);
  String text = '';
  String before = '';
  String after = ''; // for last word in the Fact
  int flags = 0; // flags and errors, see bellow
  String flagsData = ''; // flags data, e.g wrong chars etc.

  bool get isPartOf => flags & Flags.wInOtherWord != 0;

  void toText(StringBuffer buf) {
    if (!isPartOf) buf..write(before)..write(text)..write(after);
  }

  void toRow(List<String> row, bool isLeft) {
    final idx = isLeft ? 0 : _rowLen;
    row[idx] = before;
    row[idx + 1] = text;
    row[idx + 2] = after;
    row[idx + 3] = flags.toString();
    row[idx + 4] = flagsData;
  }

  Word.fromRow(List<String> row, bool isLeft) {
    final idx = isLeft ? 0 : _rowLen;
    text = row[idx + 1];
    if (text == _blank) return;
    before = row[idx];
    after = row[idx + 2];
    flags = int.parse(row[idx + 3]);
    flagsData = row[idx + 4];
  }

  String get dump => '$before#$text#$after#$flags#$flagsData';

  static const _rowLen = 5;

  // static const inBr = '|('; // word is in () bracket
  // static const inOtherWord = '|I'; // word is part of another word
  // static const brCurl = '|{'; // word contains whole content of {} brackets
  // static const latin = '|L'; // latin word in non latin text

  // static const otherScript = '*O'; // e.g. left word is in right script
  // static const wrongUnicode = '*U';
  // static const wrongCldr = '*C';
}

class Fact {
  final left = List<Word>();
  List<Word> right;
  String wClassLeft = '';
  String wClassRight;
  String flagsLeft = '';
  String flagsRight;
  String errorLeft = '';
  String errorRight;

  void toText(StringBuffer buf, bool isLeft) {
    if (isLeft) {
      if (wClassLeft.isNotEmpty) buf.write('[$wClassLeft]');
      for (final w in left) w.toText(buf);
    } else {
      if (wClassRight.isNotEmpty) buf.write('[$wClassRight]');
      for (final w in right) w.toText(buf);
    }
  }

  Fact.fromRows(Iterator<List<String>> iter, bool leftOnly)
      : right = leftOnly ? null : List<Word>() {
    assert(iter.current[0] == _ctrlFact);
    final r = iter.current;
    flagsLeft = r[1];
    errorLeft = r[3];
    if (!leftOnly) {
      flagsRight = r[Word._rowLen + 1];
      errorRight = r[Word._rowLen + 3];
    }
    while (iter.moveNext() &&
        iter.current[0] != _ctrlFact &&
        iter.current[0] != _ctrlFacts) {
      final l = Word.fromRow(iter.current, true);
      if (l.text != _blank) left.add(l);
      if (!leftOnly) {
        final r = Word.fromRow(iter.current, false);
        if (r.text != _blank) right.add(r);
      }
    }
  }
  Iterable<List<String>> toRows(bool leftOnly) sync* {
    var row = _createRow(leftOnly);
    row[0] = _ctrlFact;
    row[1] = flagsLeft;
    row[3] = errorLeft;
    if (leftOnly) {
      row[Word._rowLen + 1] = flagsRight;
      row[Word._rowLen + 3] = errorRight;
    }
    yield row;
    final len = max(left.length, leftOnly ? 0 : right.length);
    for (var i = 0; i < len; i++) {
      row = _createRow(leftOnly);
      (left.length > i ? left[i] : _emptyWord).toRow(row, true);
      if (!leftOnly)
        (right.length > i ? right[i] : _emptyWord).toRow(row, false);
      yield row;
    }
  }

  static final _emptyWord = Word._()..text = _blank;
}

class Facts {
  final facts = List<Fact>();

  String toText(StringBuffer buf, bool isLeft) {
    for (final f in facts) f.toText(buf, isLeft);
    var res = buf.toString();
    buf.clear();
    return res;
  }

  Facts.fromRows(Iterator<List<String>> iter, bool leftOnly) {
    assert(iter.current[0] == _ctrlFacts);

    iter.moveNext();
    while (iter.current != null && iter.current[0] != _ctrlFacts)
      facts.add(Fact.fromRows(iter, leftOnly));
  }

  Iterable<List<String>> toRows(bool leftOnly) sync* {
    var row = _createRow(leftOnly);
    row[0] = _ctrlFacts;
    final buf = StringBuffer();
    row[Word._rowLen - 1] = toText(buf, true);
    if (!leftOnly) row[Word._rowLen * 2 - 1] = toText(buf, false);
    yield row;
    for (final fact in facts) {
      yield* fact.toRows(leftOnly);
    }
  }
}

class Book {
  String leftLang;
  String leftScript;
  String rightLang;
  String rightScript;
  String fileName;
  bool get leftOnly => rightLang != null;
  final factss = List<Facts>();

  Book(this.fileName, this.leftLang, this.leftScript,
      {this.rightLang, this.rightScript});

  Book.fromRows(Iterator<List<String>> iter) {
    assert(iter.moveNext());
    assert(iter.current[0] == _ctrlBook);
    var r = iter.current;
    leftLang = r[1];
    leftScript = r[2];
    final tempLeftOnly = r[3] == '+';
    if (tempLeftOnly) {
      fileName = r[Word._rowLen - 1];
    } else {
      rightLang = r[Word._rowLen + 1];
      rightScript = r[Word._rowLen + 2];
      fileName = r[Word._rowLen * 2 - 1];
    }
    iter.moveNext();
    while (iter.current != null) factss.add(Facts.fromRows(iter, leftOnly));
  }

  Iterable<List<String>> toRows() sync* {
    var row = _createRow(leftOnly);
    row[0] = _ctrlBook;
    row[1] = leftLang;
    row[2] = leftScript;
    row[3] = leftOnly ? '+' : '-';
    if (leftOnly) {
      row[Word._rowLen - 1] = fileName;
    } else {
      row[Word._rowLen + 1] = rightLang;
      row[Word._rowLen + 2] = rightScript;
      row[Word._rowLen * 2 - 1] = fileName;
    }
    yield row;
    for (final facts in factss) yield* facts.toRows(leftOnly);
  }
}

List<String> _createRow(bool leftOnly) =>
    List<String>.filled(leftOnly ? Word._rowLen : Word._rowLen * 2, '');

const _spaces = '\u{FEFF} \u{3000}';
const _blank = '-$_spaces-';
const _ctrlBook = '###$_spaces#';
const _ctrlFacts = '##$_spaces#';
const _ctrlFact = '#$_spaces#';
