import 'dart:collection';
import 'package:rw_utils/dom/word_breaking.dart' as br;
import 'package:rw_utils/langs.dart' show CldrLang;
import 'lexanal.dart';
import 'dom.dart';
import 'consts.dart';

Facts parser(CldrLang langMeta, String srcText, List<br.PosLen> breaks) {
  final tokens = lexanal(srcText, breaks);
  return _parser(tokens, srcText);
}

Facts reparseFact(
    CldrLang langMeta, Facts old, String srcText, List<br.PosLen> breaks) {
  assert(srcText != null);
  final res = parser(langMeta, srcText, breaks)
    ..id = old.id
    ..lesson = old.lesson;
  final txt = res.toText();
  if (txt != srcText) {
    //print('$txt != ${old.asString}');
    throw Exception('${old.id}: $txt != ${old.asString}');
  }
  res.crc = txt.hashCode.toRadixString(32);
  return res;
}

String toRefresh(Facts fact, CldrLang langMeta, {bool force = false}) {
  final hasFacts = fact.facts.length > 0;
  final asStringEmpty = fact.asString.isEmpty;

  // !hasFacts
  if (!hasFacts && asStringEmpty) return null; // empty csv cell
  if (!hasFacts && fact.crc.isEmpty)
    return beforeBreaking(langMeta, fact.asString); //first import from CVS

  if (hasFacts && force) return beforeBreaking(langMeta, fact.toText());

  // crc
  assert(fact.crc.isNotEmpty);
  final crcNum = int.parse(fact.crc, radix: 32);

  // asString changed
  if (!asStringEmpty && crcNum != fact.asString.hashCode)
    return beforeBreaking(langMeta, fact.asString);

  assert(hasFacts); // something wrong

  // facts changed
  final txt = fact.toText();
  return beforeBreaking(langMeta, txt.hashCode != crcNum ? txt : null);
}

// after user input, not used yet
String validateText(CldrLang langMeta, String txt) {
  if (txt == null || txt.isEmpty) return txt;
  _buf.clear();
  for (var ch in txt.codeUnits) {
    switch (ch) {
      case 0xa:
      case 0xd:
      case 0x9:
        _buf.write(' ');
        continue;
    }
  }
  return _buf.toString();
}

String beforeBreaking(CldrLang langMeta, String txt) {
  if (txt == null || txt.isEmpty) return txt;

  var fillStarted = false;
  final units = txt.codeUnits;
  void fill(int code, int idx) {
    if (!fillStarted) {
      _buf.clear();
      fillStarted = true;
      for (var i = 0; i < idx; i++) _buf.writeCharCode(units[i]);
    }
    if (code != null) _buf.writeCharCode(code);
  }

  for (var i = 0; i < units.length; i++) {
    final ch = units[i];
    switch (langMeta.scriptId) {
      case 'Cyrl':
        switch (ch) {
          //?? After normalization: can we remove chars from https://en.wikipedia.org/wiki/Combining_Diacritical_Marks 
          case 0x301:
            fill(null, i);
            continue;
        }
        break;
    }
    switch (ch) {
      case 0x2018:
      case 0x2019:
        fill(0x27, i);
        continue;
      case 0x201C:
      case 0x201D:
        fill(0x22, i);
        continue;
    }
    if (fillStarted) _buf.writeCharCode(ch);
  }
  return fillStarted ? _buf.toString() : txt;
}

final _buf = StringBuffer();

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
  else if (curlBrStart != null) {
    txt.write(source.substring(curlBrStart.pos, source.length));
    addError(Flags.feMissingCurlBr);
  } else if (sqBrStart != null) {
    txt.write(source.substring(sqBrStart.pos, source.length));
    addError(Flags.feMissingSqBr);
  }

  // if (lastFact.words.isNotEmpty) {
  //   lastFact.words.last.after = useText();
  // }

  processSpliter(null);

  return Facts.fromFacts(facts);
}

final _allBrakets = HashSet<String>.from(['(', ')', '[', ']', '{', '}']);
final _allSplitters = HashSet<String>.from(['|', ';', '^']);
