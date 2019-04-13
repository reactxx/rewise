import 'dart:collection';
import 'package:rw_utils/dom/word_breaking.dart' as br;
import 'lexanal.dart';
import 'dom.dart';
import 'consts.dart';

Facts parser(String srcText, List<br.PosLen> breaks) {
  final tokens = lexanal(srcText, breaks);
  return _parser(tokens, srcText);
}

Facts fromNewText(Facts old, String srcText, List<br.PosLen> breaks) {
  assert(srcText != null);
  final res = parser(srcText, breaks)
    ..id = old.id
    ..lesson = old.lesson;
  // final res = Facts()
  //   ..id = old.id
  //   ..lesson = old.lesson
  //   ..facts.addAll(lex.facts.map((lf) {
  //     return Fact()
  //       //..wordClass = lf.wordClass
  //       ..flags = lf.flags
  //       ..words.addAll(lf.words.map((lw) {
  //         return Word(lw.before, lw.text, lw.after, lw.flags, lw.flagsData);
  //       }));
  //   }));
  final txt = res.toText();
  res.crc = txt.hashCode.toRadixString(32);
  return res;
}

Facts _parser(List<Token> tokens, String source) {
  if (tokens.length == 0) return Facts();

  //  *********** bracket processing
  Token sqBrStart;
  Token curlBrStart;
  Token brStart;
  var brLevel = 0;
  Fact lastFact = Fact()..canHaveWordClass = null;
  final txt = StringBuffer();
  final facts = List<Fact>()..add(lastFact);
  bool isWordClassMode = false;

  // *********** word with before x after text

  toText(Token t) {
    if (t.type == 't')
      escape(txt, source, t.pos, t.end);
    else
      txt.write(source.substring(t.pos, t.end));
  }

  String useText() {
    final res = txt.toString();
    txt.clear();
    return res;
  }

  processWord(Word w) {
    w.before = useText();
    if (brStart != null) w.flags += Flags.wInBr;
    lastFact.words.add(w);
  }

  //  *********** split to facts

  addError(int err, [Fact fact]) {
    (fact ?? lastFact).flags |= err;
  }

  checkNoSplitter(Token t) {
    if (t.type == '^' || t.type == '|') {
      addError(Flags.feDelimInBracket);
    }
  }

  checkNoBracket(Token t) {
    if (_allBrakets.contains(t.type)) addError(Flags.feMixingBrs);
  }

  processSpliter(Token splitter) {
    if (facts.last.isEmpty && txt.isEmpty) {
      facts.removeLast(); // empty fact
      return;
    }

    // check word
    if (lastFact.words.every((w) =>
        (w.flags & Flags.wBrCurl != 0) ||
        (w.flags & Flags.wInBr != 0) ||
        w.text.isEmpty)) addError(Flags.feNoWordInFact);

    // after to word
    if (lastFact.words.isEmpty && txt.isNotEmpty)
      lastFact.words.add(Word.fromText('')); // fake word
    if (lastFact.words.isNotEmpty)
      lastFact.words.last.after = useText(); // + (splitter?.type ?? '');

    //check wordClass
    if (lastFact.canHaveWordClass == false && lastFact.wordClass.isNotEmpty)
      addError(Flags.feWClassNotInFirstFact);
    else if (lastFact.canHaveWordClass == true && lastFact.wordClass.isEmpty)
      addError(Flags.feMissingWClass);

    // wordClass mode
    final isWc = splitter?.type == '|';
    if (isWc && !isWordClassMode) {
      isWordClassMode = true;
      // check first fact
      if (facts[0].wordClass.isEmpty) addError(Flags.feMissingWClass, facts[0]);
    }

    if (splitter != null)
      facts.add(lastFact = Fact()..canHaveWordClass = isWordClassMode && isWc);
  }

  //  *********** parsing
  for (var i = 0; i < tokens.length; i++) {
    final t = tokens[i];
    if (sqBrStart != null) /* [] */ {
      checkNoSplitter(t);
      if (t.type == ']') {
        final word = Word.fromText(source.substring(sqBrStart.pos, t.end));
        word.flags += Flags.wBrSq;
        processWord(word);
        if (lastFact.wordClass.isNotEmpty)
          addError(Flags.feSingleWClassAllowed);
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
          final word = Word.fromText(source.substring(curlBrStart.pos, t.end));
          word.flags += Flags.wBrCurl;
          processWord(word);
          curlBrStart = null;
        }
      }
    } else if (brStart != null) /* () */ {
      checkNoSplitter(t);
      if (t.type != 'w') toText(t);
      if (t.type == '(')
        brLevel++;
      else if (t.type == ')') {
        brLevel--;
        if (brLevel == 0) brStart = null;
      } else if (t.type == 'w') processWord(t.word);
    } else {
      if (!const ['w', '{', '['].contains(t.type)) toText(t);

      if (_allSplitters.contains(t.type)) {
        processSpliter(t);
      } else if (t.type == 'w') {
        processWord(t.word);
      } else if (t.type == '(') {
        brStart = t;
        brLevel = 1;
      } else if (t.type == '{') {
        curlBrStart = t;
        brLevel = 1;
      } else if (t.type == '[') {
        sqBrStart = t;
      } else if (t.type == 't') {
      } else if (t.type == ')') {
        addError(Flags.feUnexpectedBr);
      } else if (t.type == '}') {
        addError(Flags.feUnexpectedCurlBr);
      } else if (t.type == ']') {
        addError(Flags.feUnexpectedSqBr);
      } else
        throw Exception();
    }
  }
  if (brStart != null)
    addError(Flags.feMissingBr);
  else if (curlBrStart != null)
    addError(Flags.feMissingCurlBr);
  else if (sqBrStart != null) addError(Flags.feMissingSqBr);

  // if (lastFact.words.isNotEmpty) {
  //   lastFact.words.last.after = useText();
  // }

  processSpliter(null);

  return Facts.fromFacts(facts);
}

final _allBrakets = HashSet<String>.from(['(', ')', '[', ']', '{', '}']);
final _allSplitters = HashSet<String>.from(['|', ';', '^']);
