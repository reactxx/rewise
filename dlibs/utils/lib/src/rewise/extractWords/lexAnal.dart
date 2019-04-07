import 'dart:collection';
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;

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

class LexWord {
  String text = '';
  String before = '';
  String after = ''; // for last word in the Fact
  int flags = 0; // flags and errors, see bellow
  String flagsData = ''; // flags data, e.g wrong chars etc.

  LexWord(this.text);

  bool get isPartOf => flags & Flags.wInOtherWord != 0;
  void toText(StringBuffer buf) {
    if (!isPartOf) buf..write(before)..write(text)..write(after);
  }

  String get dump => '$before#$text#$after#$flags#$flagsData';
}

class LexFact {
  String wordClass = '';
  int flags = 0;
  final words = List<LexWord>();

  void toText(StringBuffer buf) {
    if (wordClass.isNotEmpty) buf.write('[$wordClass]');
    for (final w in words) w.toText(buf);
  }
  bool get isEmpty => words.length == 0 && flags != 0 && wordClass.isEmpty;

  bool canHaveWordClass = false; // true: must, false: never, null: option
}

class LexFacts {
  final facts;

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

class Breaked {
  Breaked(this.src, this.breaks);
  Breaked.dev(this.src, [List<int> posLens]) : breaks = List<wbreak.PosLen>() {
    if (posLens == null)
      breaks.add(wbreak.PosLen()
        ..pos = 0
        ..len = src.length);
    else
      for (var i = 0; i < posLens.length; i += 2)
        breaks.add(wbreak.PosLen()
          ..pos = posLens[i]
          ..len = posLens[i + 1]);
  }
  final String src;
  List<wbreak.PosLen> breaks;
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

Iterable<Token> lexanal(Breaked breaked) sync* {
  sortBreaks(breaked.breaks);

  var idx = 0; // last break end position
  var counter = 0; // token's counter
  wbreak.PosLen lastBr; // last break (unless last is not in other)

  // *********** text token processing

  int textStart; // text token start (could be null)

  Iterable<Token> flushText(int end) sync* {
    if (textStart == null) return;
    if (end == null) end = breaked.src.length;
    if (textStart == end) return;
    yield Token('t', textStart, end, counter++);
    textStart = null;
  }

  startText(int pos) {
    if (textStart == null) textStart = pos;
  }

  Iterable<Token> noBreakChars(int pos, int end) sync* {
    if (end == null) end = breaked.src.length;
    for (var i = pos; i < end; i++) {
      final ch = breaked.src[i];
      if (_tokenTypes.contains(ch)) {
        yield* flushText(i);
        yield Token(ch, i, i + 1, counter++);
      } else
        startText(i);
    }
  }

  // *********** lexanal
  for (final br in breaked.breaks) {
    // preprocess break (check if some break is not withon other)
    bool isIn = false;
    if (lastBr != null) {
      isIn = breakIn(lastBr, br);
      if (isIn == null) throw Exception();
      if (!isIn) lastBr = br;
    } else
      lastBr = br;

    // process chars between last and current break
    if (!isIn) yield* noBreakChars(idx, br.pos);

    // return word token
    yield* flushText(br.pos);
    yield Token('w', br.pos, br.pos + br.len, counter++,
        word: LexWord(breaked.src.substring(br.pos, br.pos + br.len))
          ..flags |= (isIn ? Flags.wInOtherWord : 0));
    idx = br.pos + br.len;
  }
  // final no break chars
  yield* noBreakChars(idx, null);
  yield* flushText(null);
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
