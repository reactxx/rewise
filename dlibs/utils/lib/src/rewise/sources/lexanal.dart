import 'dart:collection';
import 'dart:math';
import 'package:rw_utils/dom/word_breaking.dart' as br;

import 'dom.dart';
import 'consts.dart';

class Token {
  Token(this.type, this.pos, this.end, this.idx,
      {String wordText, int wordFlags}) {
    if (this.type != 'w') return;
    word = Word.fromText(wordText.substring(pos, end));
    word.flags = wordFlags;
  }
  final int idx; // order in token list
  final String type; // tw[]{}()|^,
  Word word;
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
    HashSet<String>.from(['(', ')', '[', ']', '{', '}', '|', '^', ';']);
