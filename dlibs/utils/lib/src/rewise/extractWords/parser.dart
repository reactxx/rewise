import 'dart:collection';
import 'dom.dart';
import 'lexAnal.dart';

class LexFact {
  bool canHaveWordClass = false; // true: must, false: never, null: option
  Token start;
  Token end;
  String wordClass;
  String flags = '';
  final words = List<Word>();
}

List<LexFact> parser(Iterable<Token> tokens, String source) {
  //  *********** split to facts
  var begFactToken;
  var lastToken;
  LexFact lastFact = LexFact()..canHaveWordClass = null;
  final facts = List<LexFact>()..add(lastFact);
  bool isWordClassMode = false;

  addError(String msg) => lastFact.flags += msg;

  checkNoSplitter(Token t) {
    if (t.type == '^' || t.type == '|') addError(Fact.e1);
  }

  checkNoBracket(Token t) {
    if (_allBrakets.contains(t.type)) addError(Fact.ea);
  }

  processSpliter(Token t) {
    if (t == null) t = lastToken;
    if (begFactToken == t) {
      facts.removeLast(); // empty fact
      return;
    }
    lastFact
      ..start = begFactToken
      ..end = t;
    begFactToken = null;

    // check word
    if (!lastFact.words.any((w) => w.flags.indexOf(Word.brCurl) < 0))
      addError(Fact.e9);

    // check wordClass
    if (lastFact.canHaveWordClass == false && lastFact.wordClass != null)
      addError(Fact.ed);
    else if (lastFact.canHaveWordClass == true && lastFact.wordClass == null)
      addError(Fact.ec);

    // wordClass mode
    final isWc = t.type == '|';
    if (isWc && !isWordClassMode) {
      isWordClassMode = true;
      // check first fact
      if (facts[0].wordClass == null) addError(Fact.ec);
    }

    if (t != null)
      facts.add(
          lastFact = LexFact()..canHaveWordClass = isWordClassMode && isWc);
  }

  //  *********** bracket processing
  Token sqBrStart;
  Token curlBrStart;
  Token brStart;
  var brLevel = 0;

  // *********** word with before x after text
  Word lastWord;
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
    if (brStart != null) w.flags += Word.inBr;
    lastFact.words.add(w);
    text.clear();
    lastWord = w;
  }

  //  *********** parsing
  for (var t in tokens) {
    if (begFactToken == null) begFactToken = t;
    lastToken = t;
    if (t.type == '^' || t.type == ',' || t.type == '|') {
      processSpliter(t);
    } else if (sqBrStart != null) /* [] */ {
      checkNoSplitter(t);
      if (t.type == ']') {
        if (lastFact.wordClass != null) addError(Fact.eb);
        lastFact.wordClass = source.substring(sqBrStart.pos + 1, t.end - 1);
        sqBrStart = null;
      }
      checkNoBracket(t);
    } else if (curlBrStart != null) /* {} */ {
      checkNoSplitter(t);
      if (t.type == '{')
        brLevel++;
      else if (t.type == '}') {
        brLevel--;
        if (brLevel == 0) {
          final word = Word(source.substring(curlBrStart.pos + 1, t.end - 1));
          word.flags += Word.brCurl;
          processWord(t, word);
          curlBrStart = null;
        }
      }
      checkNoBracket(t);
    } else if (brStart != null) /* () */ {
      checkNoSplitter(t);
      if (t.type == '(')
        brLevel++;
      else if (t.type == ')') {
        brLevel--;
        if (brLevel == 0) brStart = null;
      } else if (t.type == 'w') {
        processWord(t, t.word);
      }
      checkNoBracket(t);
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
    } else if (t.type == 'w') {
      processWord(t, t.word);
    } else if (t.type == ')')
      addError(Fact.e3);
    else if (t.type == '}')
      addError(Fact.e4);
    else if (t.type == ']')
      addError(Fact.e5);
    else
      throw Exception();
  }
  if (brStart != null)
    addError(Fact.e6);
  else if (curlBrStart != null)
    addError(Fact.e7);
  else if (sqBrStart != null) addError(Fact.e8);

  processSpliter(null);

  if (lastWord != null) {
    flushText(null);
    lastWord.after = text.toString();
  }
  return facts;
}

final _allBrakets = HashSet<String>.from(['(', ')', '[', ']', '{', '}']);
