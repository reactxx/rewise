import 'dart:collection';
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;

class Flags {
  // word is in () bracket
  static final wInBr = _r('wInBr', 0x1);
  // word is part of another word
  static final wIsPartOf = _r('wInOtherWord', 0x2);
  // word contains whole content of {} brackets
  static final wBrCurl = _r('wBrCurl', 0x4);
  // latin word in non latin text
  static final wIsLatin = _r('wIsLatin', 0x8);

  // ^ or | in brackets
  static final feDelimInBracket = _r('feDelimInBracket', 0x10);
  // missing ) bracket
  static final feMissingBr = _r('feMissingBr', 0x20);
  // missing } bracket
  static final feMissingCurlBr = _r('feMissingCurlBr', 0x40);
  // missing ] bracket
  static final feMissingSqBr = _r('feMissingSqBr', 0x80);
  // unexpected ) bracket
  static final feUnexpectedBr = _r('feUnexpectedBr', 0x100);
  // unexpected } bracket
  static final feUnexpectedCurlBr = _r('feUnexpectedCurlBr', 0x200);
  // unexpected ] bracket
  static final feUnexpectedSqBr = _r('feUnexpectedSqBr', 0x400);
  // no word in fact
  static final feNoWordInFact = _r('feNoWordInFact', 0x800);
  // mixing different brakets
  static final feMixingBrs = _r('feMixingBrs', 0x1000);
  // more than single []
  static final feSingleWClassAllowed = _r('feSingleWClassAllowed', 0x2000);
  // missing []
  static final feMissingWClass = _r('feMissingWClass', 0x4000);
  // [] not in first fact
  static final feWClassNotInFirstFact = _r('feWClassNotInFirstFact', 0x8000);

  // e.g. left word is in right script
  static final weOtherScript = _r('weOtherScript', 0x10000);
  static final weWrongUnicode = _r('weWrongUnicode', 0x20000);
  static final weWrongCldr = _r('weWrongCldr', 0x40000);

  // word has parts
  static final wHasParts = _r('wInOtherWord', 0x80000);

  static String toText(int f) {
    _buf.clear();
    for (var i = 0; i < 16; i++)
      if (((f >> i) & 0x1) != 0) {
        if (_buf.length > 0) _buf.write(', ');
        _buf.write(_codes[1 << i]);
      }
    return _buf.toString();
  }

  static final _buf = StringBuffer();

  static int _r(String id, int f) {
    _codes[f] = id;
    return f;
  }

  static final _codes = Map<int, String>();
}

class LexWord {
  String text = '';
  String before = '';
  String after = ''; // for last word in the Fact
  int flags = 0; // flags and errors, see bellow
  String flagsData = ''; // flags data, e.g wrong chars etc.

  LexWord(this.text);

  bool get isPartOf => flags & Flags.wIsPartOf != 0;
  void toText(StringBuffer buf) {
    if (!isPartOf) buf..write(before)..write(text)..write(after);
  }

  String get dump => '$before#$text#$after#$flags#$flagsData';
}

class LexFact {
  String wordClass = '';
  int flags = 0;
  final words = List<LexWord>();

  String get flagsText => Flags.toText(flags);

  void toText(StringBuffer buf) {
    if (wordClass.isNotEmpty) buf.write('[$wordClass]');
    for (final w in words) w.toText(buf);
  }

  bool get isEmpty => words.length == 0 && flags != 0 && wordClass.isEmpty;

  bool canHaveWordClass = false; // true: must, false: never, null: option
}

class LexFacts {
  //int crc;
  //String asString;
  final List<LexFact> facts;

  LexFacts.empty() : facts = List<LexFact>();
  LexFacts(this.facts);
  String toText() {
    final buf = StringBuffer();
    for (final f in facts) f.toText(buf);
    var res = buf.toString();
    buf.clear();
    return res;
  }
}

class Token {
  Token(this.type, this.pos, this.end, this.idx, {this.word});
  final int idx; // order in token list
  final String type; // tw[]{}()|^,
  final LexWord word;
  // pos in source text input
  final int pos;
  final int end;
}

sortBreaks(List<wbreak.PosLen> breaks) =>
    breaks.sort((a, b) => a.pos == b.pos ? b.len - a.len : a.pos - b.pos);
bool breakIn(wbreak.PosLen br, wbreak.PosLen ins) {
  if (ins.pos >= br.pos + br.len) return false;
  if (ins.pos >= br.pos && ins.pos + ins.len <= br.pos + br.len) return true;
  return null;
}

Iterable<Token> lexanal(String srcText, List<wbreak.PosLen> breaks) sync* {
  sortBreaks(breaks);

  var idx = 0; // last break end position
  var counter = 0; // token's counter
  wbreak.PosLen lastBr; // last break (unless last is not in other)

  // *********** text token processing

  int textStart; // text token start (could be null)

  Iterable<Token> flushText(int end) sync* {
    if (textStart == null) return;
    if (end == null) end = srcText.length;
    if (textStart == end) return;
    yield Token('t', textStart, end, counter++);
    textStart = null;
  }

  startText(int pos) {
    if (textStart == null) textStart = pos;
  }

  Iterable<Token> noBreakChars(int pos, int end) sync* {
    if (end == null) end = srcText.length;
    for (var i = pos; i < end; i++) {
      final ch = srcText[i];
      if (_tokenTypes.contains(ch)) {
        yield* flushText(i);
        yield Token(ch, i, i + 1, counter++);
      } else
        startText(i);
    }
  }

  // *********** lexanal
  for (final br in breaks) {
    // preprocess break (check if some break is not withon other)
    bool isIn = false;
    if (lastBr != null) {
      isIn = breakIn(lastBr, br);
      if (isIn == null) isIn = true;// in ko-KR happens throw Exception();
      else if (!isIn) lastBr = br;
    } else
      lastBr = br;

    // process chars between last and current break
    if (!isIn) yield* noBreakChars(idx, br.pos);

    // return word token
    yield* flushText(br.pos);
    yield Token('w', br.pos, br.pos + br.len, counter++,
        word: LexWord(srcText.substring(br.pos, br.pos + br.len))
          ..flags |= (isIn ? Flags.wIsPartOf : 0));
    idx = br.pos + br.len;
  }
  // final no break chars
  yield* noBreakChars(idx, null);
  yield* flushText(null);
}

breaksGroup() {
  
}

String tokensToString(Iterable<Token> tokens, String src) {
  final buf = StringBuffer();
  for (final t in tokens) {
    if (t.type == 'w') {
      if (!t.word.isPartOf) buf.write(t.word.text);
    } else if (t.type == 't')
      buf.write(src.substring(t.pos, t.end));
    else
      buf.write(t.type);
  }
  return buf.toString();
}

final _tokenTypes =
    HashSet<String>.from(['(', ')', '[', ']', '{', '}', '|', '^', ',']);
