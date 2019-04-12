import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/langs.dart' show Langs, Unicode;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;
import 'package:rw_utils/dom/utils.dart';
import 'dart:collection';
import 'package:tuple/tuple.dart';

class WordsStat {
  final okWords = HashSet<String>();
  final wrongWords = HashSet<String>();
  final latinWords = HashSet<String>();
}

List<wbreak.PosLen> alphabetTest(String lang, toPars.ParsedSubFact fact,
    Iterable<wbreak.PosLen> posLens, StringBuffer errors, WordsStat wordStat) {
  final meta = Langs.nameToMeta[lang];
  bool isError = false;

  var res = posLens.where((pl) {
    final word = fact.text.substring(pl.pos, pl.end);
    final err = _latinOrScript(meta, word, wordStat);
    if (err == null) return true;
    if (err.item3) return false;
    if (!isError) {
      errors.writeln('FACT: ${meta.scriptId} expected in "${fact.text}"');
      errors.write('  ');
      isError = true;
    }
    errors.write('$word:');
    if (err.item1.isNotEmpty) errors.write('(${err.item1})');
    if (err.item2.isNotEmpty) errors.write('(*${err.item2})');
    return false;
  }).toList();

  if (isError) errors.writeln('');
  return res;
}

Tuple3<String, String, bool> _latinOrScript(
    CldrLang meta, String word, WordsStat stat) {
  if (word == null || word.isEmpty) return null;

  // characters, allowed for lang (from cldr source)
  // cached in cldrAlphabets
  final cldrAlphabet = _cldrAlphabets.putIfAbsent(
      meta.id,
      () => meta.alphabet == null || meta.alphabet.isEmpty
          ? null
          : HashSet<int>.from((meta.alphabet + meta.alphabetUpper).codeUnits));

  String latins = '';
  String wrongCldr = '';
  String wrongUnicode = '';
  String noLetter = '';
  String ok = '';

  for (final ch in word.codeUnits) {
  //for (final ch in Langs.netToLower(word).codeUnits) {
    final it = Unicode.item(ch);
    // prepare cldr
    final okCldr = cldrAlphabet != null && cldrAlphabet.contains(ch);

    // NO LETTER
    if (it == null) {
      if (okCldr)
        ok += String.fromCharCode(ch);
      else
        noLetter += String.fromCharCode(ch);
      continue;
    }
    // OK
    if (Unicode.scriptsEq(meta.scriptId, it.script)) {
      if (!okCldr && it.script == meta.scriptId) {
        wrongCldr += String.fromCharCode(ch);
      } else
        ok += String.fromCharCode(ch);
      continue;
    }
    // LATIN in nonLatin
    if (meta.scriptId != 'Latn' && it.script == 'Latn') {
      latins += String.fromCharCode(ch);
      continue;
    }
    wrongUnicode += String.fromCharCode(ch);
  }

  if (ok.length == word.length) {
    stat.okWords.add(word);
    return null;
  } else if (latins.length == word.length) {
    stat.latinWords.add(word);
    return Tuple3(null, null, true);
  }
  if (noLetter.length == word.length) return Tuple3(null, null, true);
  stat.wrongWords.add('$word|${wrongUnicode + latins}|${wrongCldr + noLetter}');
  return Tuple3(wrongUnicode + latins, wrongCldr + noLetter, false);
}

final _cldrAlphabets = Map<String, HashSet<int>>();
/**
cs_cz;ru_ru
čekal;д
č6;д6
čü;дa
čд;д6a
č6ü;aaa
čдü;д
čд6;д
čü6;д
čü6д;д
123;123
 */
/*
CODE to stemming:
Latin: Replace 0x2018:0x27, 0x2019:0x27, 0x201C:0x22, 0x201D:0x22 (apostrof etc.)
After normalization: can we remove chars from https://en.wikipedia.org/wiki/Combining_Diacritical_Marks 
*/
void specialCheck(String txt, int beg, int end, StringBuffer b1,
    StringBuffer b2, CldrLang meta) {
  if (meta == null) return; // in tests: lang is null

  void fill(int code) {
    b1.writeCharCode(code);
    b2.writeCharCode(code);
  }

  for (var i = beg; i < end; i++) {
    final ch = txt.codeUnitAt(i);
    if (meta.scriptId == 'Latn') {
      switch (ch) {
        case 0x2018:
        case 0x2019:
          fill(0x27);
          continue;
        case 0x201C:
        case 0x201D:
          fill(0x22);
          continue;
      }
    } else if (meta.scriptId == 'Cyrl') {
      switch (ch) {
        case 0x301: continue;
      }
    }
    fill(ch);
  }
}

