import 'package:rw_utils/dom/word_breaking.dart' as wbreak;

import 'dom.dart';

class Breaked {
  String src;
  List<wbreak.PosLen> breaks;
}

Facts import(Book book, Breaked breakedLeft, {Breaked breakedRight}) {
  final wordsLeft = _lexAnal(breakedLeft);
}

List<Token> _lexAnal(Breaked breaked) {}

List<LexFact> _splitToFacts(List<Token> tokens) {
  var beg = tokens.first;

  final facts = List<LexFact>();

  // wordClass management
  LexFact firstInWordClassGroup = null;
  var factHasWordClass = false;

  for (var t in tokens) {
    if (t.type == '^' || t.type == ',' || t.type == '|') {
      if (beg == t) /*ERROR*/ continue;
      final fact = LexFact(beg, t);
      facts.add(fact);
      if (t != tokens.last) beg = tokens[t.idx + 1];
      // wordClass management
      if (firstInWordClassGroup == null) {
        firstInWordClassGroup = fact;
        fact.canHaveWordClass = factHasWordClass ? true : null;
      }
      if (t.type == '|') {
        // facts has wordClass
        firstInWordClassGroup.canHaveWordClass = true;
        firstInWordClassGroup == null;
        factHasWordClass = true;
      }
    }
  }
  return facts;
}

_processFact(LexFact fact, List<Token> tokens) {
  Token sqBrStart;
  Token curlBrStart = null;
  Token brStart = null;
  Token lastWord = null;
  var brLevel = 0;

  final text = StringBuffer();
  Token textStart;

  flushText(Token after) {
    if (textStart == null) return;
    //text.write()
    //textStart.pos-(after==null ? end : after.end)
    textStart = null;
  }

  startText(Token from) {
    if (textStart == null) textStart = from;
  }

  for (var i = fact.start.idx; i <= fact.end.idx; i++) {
    final t = tokens[i];
    if (sqBrStart != null) {
      // []
      if (t.type == ']') {
        if (fact.canHaveWordClass == false) {
          /*ERROR*/
        } else if (fact.wordClass != null) {/*ERROR*/} else
          fact.wordClass = null; /* TODO sqBrStart..t */
        sqBrStart = null;
      }
    } else if (curlBrStart != null) {
      // {}
      if (t.type == '{')
        brLevel++;
      else if (t.type == '}') {
        brLevel--;
        if (brLevel == 0) curlBrStart = null;
      }
    } else if (brStart != null) {
      // ()
      if (t.type == '(')
        brLevel++;
      else if (t.type == ')') {
        brLevel--;
        if (brLevel == 0) brStart = null;
      }
    } else if (t.type == '(') {
      startText(t);
      brStart = t;
      brLevel = 1;
    } else if (t.type == '{') {
      startText(t);
      curlBrStart = t;
      brLevel = 1;
    } else if (t.type == '[') {
      flushText(t);
      sqBrStart = t;
    } else if (t.type == 't') {
      startText(t);
    } else if (t.type == 'w') {
      flushText(t);
      fact.words.add(null /* TODO */);
      // word before is in text
      //if (brStart != null) isInBracket = true;
    } else {/* ERROR */}
  }
  if (lastWord == null) {/* ERROR */}
  flushText(null);
  // word AFTER is in text

  if (sqBrStart != null || curlBrStart != null || brStart != null) {/* ERROR */}
}

class LexFact {
  LexFact(this.start, this.end);
  bool canHaveWordClass = false; // true: must, false: never, null: option
  Token start;
  Token end;
  String wordClass;
  final words = List<Word>();
}

/*
  PRG = WC<DELIM WC>*
  DELIM = | ^ ,
  WC = <TW CB SB B>*
  TW = t w
  SB = [TW*]
  CB = {<TW [ ] ( ) CB>*}
  B = (<TW [ ] { } B>*)
*/

class Token {
  int idx; // order in list
  String type; // tw[]{}()|^,;$
  // pos in source text input
  int pos;
  int end;
  bool inBracket = false; // for word inside the bracket
}

class TokenReader {
  int pos = -1;
  final input = List<Token>();

  bool moveNext() {
    if (pos >= input.length - 1) return false;
    pos++;
    return true;
  }

  Token get current => pos >= input.length ? null : input[pos];
}

readFacts(TokenReader rdr) {
  rdr.moveNext();
  while (rdr.current != null) readFact(rdr);
}

readFact(TokenReader rdr) {
  while (rdr.current != null && rdr.current.type != '|,^') readFact(rdr);
}
