import 'dart:collection';
import 'dom.dart';
import 'lexanal.dart';

class LexFact {
  bool canHaveWordClass = false; // true: must, false: never, null: option
  String wordClass;
  int flags = 0;
  final words = List<Word>();
  bool get empty => words.length==0 && flags!=0 && wordClass==null;
}

List<LexFact> parser(Iterable<Token> tokens, String source) {
  //  *********** bracket processing
  Token sqBrStart;
  Token curlBrStart;
  Token brStart;
  var brLevel = 0;

  // *********** word with before x after text
  LexFact lastFact = LexFact()..canHaveWordClass = null;
  final text = StringBuffer();
  Token textStart;

  flushText(Token after) {
    if (textStart == null) return;
    final end = after == null ? source.length : after.pos;
    text.write(source.substring(textStart.pos, end));
    textStart = null;
  }

  startText(Token from) {
    if (textStart == null) textStart = from;
  }

  processWord(Token t, Word w) {
    flushText(t);
    w.before = text.toString();
    text.clear();
    if (brStart != null) w.flags += Flags.wInBr;
    lastFact.words.add(w);
  }

  //  *********** split to facts
  var lastToken;
  final facts = List<LexFact>()..add(lastFact);
  bool isWordClassMode = false;

  addError(int err, [LexFact fact]) {
    (fact ?? lastFact).flags |= err;
  }

  checkNoSplitter(Token t) {
    if (t.type == '^' || t.type == '|') addError(Flags.feDelimInBracket);
  }

  checkNoBracket(Token t) {
    if (_allBrakets.contains(t.type)) addError(Flags.feMixingBrs);
  }

  processSpliter(Token t) {
    if (facts.last.empty) {
      facts.removeLast(); // empty fact
      return;
    }

    // check word
    if (!lastFact.words.any((w) => w.flags & Flags.wBrCurl == 0))
      addError(Flags.feNoWordInFact);

    // after to word
    if (lastFact.words.isNotEmpty) {
      flushText(t);
      lastFact.words.last.after = text.toString();
      text.clear();
    }

    // check wordClass
    if (lastFact.canHaveWordClass == false && lastFact.wordClass != null)
      addError(Flags.feWClassNotInFirstFact);
    else if (lastFact.canHaveWordClass == true && lastFact.wordClass == null)
      addError(Flags.feMissingWClass);

    // wordClass mode
    final isWc = t?.type == '|';
    if (isWc && !isWordClassMode) {
      isWordClassMode = true;
      // check first fact
      if (facts[0].wordClass == null) addError(Flags.feMissingWClass, facts[0]);
    }

    if (t != null)
      facts.add(
          lastFact = LexFact()..canHaveWordClass = isWordClassMode && isWc);
  }

  //  *********** parsing
  for (var t in tokens) {
    lastToken = t;
    if (sqBrStart != null) /* [] */ {
      checkNoSplitter(t);
      if (t.type == ']') {
        if (lastFact.wordClass != null) addError(Flags.feSingleWClassAllowed);
        lastFact.wordClass = source.substring(sqBrStart.pos + 1, t.end - 1);
        sqBrStart = null;
      } else
        checkNoBracket(t);
    } else if (curlBrStart != null) /* {} */ {
      checkNoSplitter(t);
      if (t.type == '{')
        brLevel++;
      else if (t.type == '}') {
        brLevel--;
        if (brLevel == 0) {
          final word = Word(source.substring(curlBrStart.pos, t.end));
          word.flags += Flags.wBrCurl;
          processWord(t, word);
          curlBrStart = null;
        }
      } else
        checkNoBracket(t);
    } else if (brStart != null) /* () */ {
      startText(t);
      checkNoSplitter(t);
      if (t.type == '(')
        brLevel++;
      else if (t.type == ')') {
        brLevel--;
        if (brLevel == 0) brStart = null;
      } else if (t.type == 'w') {
        processWord(t, t.word);
      } else
        checkNoBracket(t);
    } else if (_allSplitters.contains(t.type)) {
      processSpliter(t);
    } else if (t.type == 'w') {
      processWord(t, t.word);
    } else if (t.type == '(') {
      startText(t);
      brStart = t;
      brLevel = 1;
    } else if (t.type == '{') {
      flushText(t);
      curlBrStart = t;
      brLevel = 1;
    } else if (t.type == '[') {
      flushText(t);
      sqBrStart = t;
    } else if (t.type == 't') {
      startText(t);
    } else if (t.type == ')')
      addError(Flags.feUnexpectedBr);
    else if (t.type == '}')
      addError(Flags.feUnexpectedCurlBr);
    else if (t.type == ']')
      addError(Flags.feUnexpectedSqBr);
    else
      throw Exception();
  }
  if (lastToken == null) return List<LexFact>(0); // no tokens
  if (brStart != null)
    addError(Flags.feMissingBr);
  else if (curlBrStart != null)
    addError(Flags.feMissingCurlBr);
  else if (sqBrStart != null) addError(Flags.feMissingSqBr);

  if (lastFact.words.isNotEmpty) {
    flushText(null);
    lastFact.words.last.after = text.toString();
  }

  processSpliter(null);

  return facts;
}

final _allBrakets = HashSet<String>.from(['(', ')', '[', ']', '{', '}']);
final _allSplitters = HashSet<String>.from(['|', ',', '^']);
