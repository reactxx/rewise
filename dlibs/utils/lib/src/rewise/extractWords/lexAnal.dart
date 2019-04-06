import 'dart:collection';
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;

import 'dom.dart';

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
  final String type; // tw[]{}()|^,;$
  final Word word;
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

Iterable<Token> lexAnal(Breaked breaked) sync* {
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
        word: Word(breaked.src.substring(br.pos, br.pos + br.len))
          ..flags += (isIn ? Word.inOtherWord : ''));
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
      if (t.word.flags.indexOf(Word.inOtherWord) < 0) buf.write(t.word.text);
    } else if (t.type == 't')
      buf.write(src.substring(t.pos, t.end));
    else
      buf.write(t.type);
  }
  return buf.toString();
}

final _tokenTypes =
    HashSet<String>.from(['(', ')', '[', ']', '{', '}', '|', '^', ',']);
