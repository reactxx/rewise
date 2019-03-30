import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/langs.dart' show Langs, Unicode;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;
import 'package:rw_utils/dom/utils.dart';
import 'dart:collection';
import 'package:tuple/tuple.dart';

class WordsStat {
  final okWords = HashSet<String>();
  final wrongUnicodeWords = HashSet<String>();
  final wrongCldrWords = HashSet<String>();
  final latinWords = HashSet<String>();
}

List<wbreak.PosLen> alphabetTest(String lang, toPars.ParsedSubFact fact,
    Iterable<wbreak.PosLen> posLens, StringBuffer errors, WordsStat wordStat) {
  final meta = Langs.nameToMeta[lang];
  bool isError = false;

  var res = posLens.where((pl) {
    final word = fact.text.substring(pl.pos, pl.pos + pl.len);
    final err = _latinOrScript(meta, word, wordStat);
    if (err == null) return true;
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

Tuple2<String, String> _latinOrScript(
    CldrLang meta, String word, WordsStat wordStat) {
  if (word == null || word.isEmpty) return null;
  final chars = _alphaCache.putIfAbsent(
      meta.id,
      () => meta.alphabet == null
          ? null
          : HashSet<int>.from(meta.alphabet.codeUnits));
  bool isLatn;
  bool isError = false;
  String noCldrScript = '';
  String otherScript = '';
  for (final ch in word.toLowerCase().codeUnits) {
    final it = Unicode.item(ch);
    if (it == null) continue;
    if (chars != null && meta.scriptId == it.script && !chars.contains(ch))
      noCldrScript += String.fromCharCode(ch);
    if (it.script != meta.scriptId) otherScript += String.fromCharCode(ch);
    if (isLatn == null) /* set isLatn based on first char */ {
      isLatn = it.script == 'Latn';
      continue;
    } else if (isLatn) {
      if (it.script == 'Latn') /* continued Latn*/ continue;
    } else {
      if (Unicode.scriptOK(
          meta.scriptId, it.script)) /* continued OK script*/ continue;
    }
    isError = true; // script error
  }
  if (isError) {
    if (noCldrScript.isNotEmpty) wordStat.wrongCldrWords.add(word);
    if (isError) wordStat.wrongUnicodeWords.add(word);
    return Tuple2(isError ? otherScript : '', noCldrScript);
  } else {
    (isLatn ? wordStat.latinWords : wordStat.okWords).add(word);
    return null;
  }
}

final _alphaCache = Map<String, HashSet<int>>();
