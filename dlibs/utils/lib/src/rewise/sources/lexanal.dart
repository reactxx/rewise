import 'dart:collection';
import 'dart:math';
import 'package:rw_utils/dom/word_breaking.dart' as br;

class Flags {
  // word is in () bracket
  static final wInBr = _r('wInBr', 0);
  // word is part of another word
  static final wIsPartOf = _r('wInOtherWord', 1);
  // word has parts, don't use it for stemming
  static final wHasParts = _r('wHasParts', 2);
  // word contains whole content of [] brackets
  static final wBrSq = _r('wBrSq', 3);
  // word contains whole content of {} brackets
  static final wBrCurl = _r('wBrCurl', 4);
  // latin word in non latin text
  static final wIsLatin = _r('wIsLatin', 5);

  // ^ or | in brackets
  static final feDelimInBracket = _r('feDelimInBracket', 6);
  // missing ) bracket
  static final feMissingBr = _r('feMissingBr', 7);
  // missing } bracket
  static final feMissingCurlBr = _r('feMissingCurlBr', 8);
  // missing ] bracket
  static final feMissingSqBr = _r('feMissingSqBr', 9);
  // unexpected ) bracket
  static final feUnexpectedBr = _r('feUnexpectedBr', 10);
  // unexpected } bracket
  static final feUnexpectedCurlBr = _r('feUnexpectedCurlBr', 11);
  // unexpected ] bracket
  static final feUnexpectedSqBr = _r('feUnexpectedSqBr', 12);
  // no word in fact
  static final feNoWordInFact = _r('feNoWordInFact', 13);
  // mixing different brakets
  static final feMixingBrs = _r('feMixingBrs', 14);
  // // more than single []
  // static final feSingleWClassAllowed = _r('feSingleWClassAllowed', 0x2000);
  // // missing []
  // static final feMissingWClass = _r('feMissingWClass', 0x4000);
  // // [] not in first fact
  // static final feWClassNotInFirstFact = _r('feWClassNotInFirstFact', 0x8000);

  // e.g. left word is in right script
  static final weOtherScript = _r('weOtherScript', 15);
  static final weWrongUnicode = _r('weWrongUnicode', 16);
  static final weWrongCldr = _r('weWrongCldr', 17);

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

  static int _r(String id, int s) {
    final f = 1 << s;
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
    if (isPartOf) return;
    buf.write(before);
    //escape(buf, before);
    buf.write(text);
    //escape(buf, after);
    buf.write(after);
  }

  //String get dump => '$before#$text#$after#$flags#$flagsData';
}

class LexFact {
  //String wordClass = '';
  int flags = 0;
  final words = List<LexWord>();

  String get flagsText => Flags.toText(flags);

  void toText(StringBuffer buf) {
    //if (wordClass.isNotEmpty) buf.write('[$wordClass]');
    for (final w in words) w.toText(buf);
  }

  bool get isEmpty => words.length == 0 && flags == 0; // && wordClass.isEmpty;

  //bool canHaveWordClass = false; // true: must, false: never, null: option
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
    return buf.toString();
    // var res = buf.toString();
    // buf.clear();
    // return res;
  }
}

class Token {
  Token(this.type, this.pos, this.end, this.idx,
      {String wordText, int wordFlags}) {
    if (this.type != 'w') return;
    word = LexWord(wordText.substring(pos, end));
    word.flags = wordFlags;
  }
  final int idx; // order in token list
  final String type; // tw[]{}()|^,
  LexWord word;
  // pos in source text input
  final int pos;
  final int end;
}

void overlappedBreaks(
    List<br.PosLen> brs, void onBrGroup(int idxPos, int idxEnd, int endPos)) {
  if (brs.length == 0) return;
  brs.sort((a, b) =>
      a.pos == b.pos ? b.end - b.pos - (a.end - a.pos) : a.pos - b.pos);
  var idxPos = 0, idxEnd = 1, end = brs[0].end;
  while (true) {
    var lastRun = idxEnd >= brs.length;
    if (lastRun || end <= brs[idxEnd].pos) {
      onBrGroup(idxPos, idxEnd, end);
      if (lastRun) break;
      idxPos = idxEnd;
    }
    if (idxEnd < brs.length) end = max(end, brs[idxEnd].end);
    idxEnd++;
  }
}

List<Token> lexanal(String srcText, List<br.PosLen> breaks) {
  final res = List<Token>();
  int textPos; // text token start (could be null)

  flushText(int end) {
    if (textPos == null) return;
    if (end == null) end = srcText.length;
    if (textPos == end) return;
    res.add(Token('t', textPos, end, res.length));
    textPos = null;
  }

  startText(int pos) {
    if (textPos == null) textPos = pos;
  }

  getTokensBetweenBreaks(int pos, int end) {
    if (end == null) end = srcText.length;
    var escaped = false;
    for (var i = pos; i < end; i++) {
      final ch = srcText[i];
      if (escaped) {
        startText(i);
        assert(ch == r'\' || _tokenTypes.contains(ch));
        escaped = false;
      } else if (ch == r'\') {
        escaped = true;
        flushText(i);
      } else if (_tokenTypes.contains(ch)) {
        flushText(i);
        res.add(Token(ch, i, i + 1, res.length));
      } else
        startText(i);
    }
    assert(!escaped);
    flushText(end);
  }

  // *********** lexanal
  var pos = 0;
  overlappedBreaks(breaks, (idxBeg, idxEnd, endPos) {
    getTokensBetweenBreaks(pos, breaks[idxBeg].pos);
    // first word
    final begBr = breaks[idxBeg];
    res.add(Token('w', begBr.pos, endPos, res.length,
        wordText: srcText,
        wordFlags: begBr.end < endPos ? Flags.wHasParts : 0));
    // other words
    for (var i = idxBeg + 1; i < idxEnd; i++)
      res.add(Token('w', breaks[i].pos, breaks[i].end, res.length,
          wordText: srcText, wordFlags: Flags.wIsPartOf));
    pos = endPos;
  });
  getTokensBetweenBreaks(pos, null);

  return res;
}

void escape(StringBuffer buf, String src, int pos, int end) {
  for (var i = pos; i < end; i++) {
    var ch = src[i];
    if (_tokenTypes.contains(ch) || ch == r'\') buf.write(r'\');
    buf.write(ch);
  }
}

String tokensToString(Iterable<Token> tokens, String src) {
  final buf = StringBuffer();
  for (final t in tokens) {
    if (t.type == 'w') {
      if (!t.word.isPartOf) buf.write(t.word.text);
    } else if (t.type == 't')
      escape(buf, src, t.pos, t.end);
    else
      buf.write(t.type);
  }
  return buf.toString();
}

final _tokenTypes =
    HashSet<String>.from(['(', ')', '[', ']', '{', '}', '|', '^', ',']);
