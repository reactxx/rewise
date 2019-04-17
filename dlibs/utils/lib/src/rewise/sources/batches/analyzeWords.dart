import 'dart:collection';
import 'package:tuple/tuple.dart';
import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_utils/langs.dart' show Langs, Unicode;
import '../consts.dart';
import '../dom.dart';

void analyzeWords(FileInfo first, List<Tuple2<FileInfo, Word>> fileWords,
    String pathFragment) {
  final map = HashMap<AnalyzeResult, W>();
  for (final fw in fileWords) {
    final res = analyzeWord(fw.item1, fw.item2);
    map.update(res, (v) {
      v.count++;
      if (v.words.length < 100) v.words.add(fw.item2.text);
      return v;
    }, ifAbsent: () => W(fw.item2.text));
  }

  final list = List<MapEntry<AnalyzeResult, W>>.from(map.entries);
  list.sort((a, b) => b.value.count - a.value.count);
  final lines = list.map((kv) =>
      '${kv.value.count}x.${kv.key.toString()}..${kv.value.words.join('|')}');
  fileSystem.edits.writeAsLines('analyzeSources\\$pathFragment.txt', lines);
}

class W {
  W(String w) {
    words.add(w);
  }
  int count = 1;
  final words = HashSet<String>();
}

AnalyzeResult analyzeWord(FileInfo file, Word word) {
  final myScript = Langs.nameToMeta[file.dataLang].scriptId,
      otherScript = file.otherLang.isEmpty
          ? ''
          : Langs.nameToMeta[file.otherLang].scriptId;
  var ok = 0, nonLetter = 0, latin = 0, other = 0, wrongLetter = 0, len = 0;
  int cldr = null;
  for (final ch in word.text.codeUnits) {
    len++;
    final uni = Unicode.item(ch);
    if (uni == null) {
      nonLetter++;
      continue;
    }
    if (myScript != 'Latn' && uni.script == 'Latn') latin++;
    //cldr
    final c = Langs.isCldrChar(file.dataLang, ch);
    if (c != null) {
      if (cldr == null) cldr = 0;
      if (c) cldr++;
    }

    if (Unicode.scriptsEq(myScript, uni.script))
      ok++;
    else if (Unicode.scriptsEq(otherScript, uni.script))
      other++;
    else
      wrongLetter++;
  }
  if (len == cldr) return AnalyzeResult.okCldr;
  if (len == ok) return AnalyzeResult.ok;
  if (len == nonLetter) return AnalyzeResult.nonLetter;
  if (len == latin) return AnalyzeResult.latin;
  if (len == other) return AnalyzeResult.other;
  if (len == wrongLetter) return AnalyzeResult.wrong;

  AnalyzeResult percent(
      AnalyzeResult gt66, AnalyzeResult gt33, AnalyzeResult any) {
    final okNum = ok / len;
    if (okNum < 0.33) return gt66;
    if (okNum < 0.66) return gt33;
    return any;
  }

  if (ok > 0 && len == ok + nonLetter)
    return percent(AnalyzeResult.nonLetterGt66, AnalyzeResult.nonLetterGt33,
        AnalyzeResult.nonLetterAny);
  if (ok > 0 && len == ok + latin)
    return percent(AnalyzeResult.latinGt66, AnalyzeResult.latinGt33,
        AnalyzeResult.latinAny);
  if (ok == 0) return AnalyzeResult.mix;
  return percent(
      AnalyzeResult.mixGt66, AnalyzeResult.mixGt33, AnalyzeResult.mixAny);
}

enum AnalyzeResult {
  ok, // all chars is lang script
  okCldr,
  other, // all chars is side lang script
  wrong, //  all chars in other script
  nonLetter, // all or less non letter
  nonLetterGt66, //
  nonLetterGt33,
  nonLetterAny,
  latin, // for non Latn script: all or less Latn
  latinGt66,
  latinGt33,
  latinAny,
  mix, // all or less non ok
  mixGt66, //
  mixGt33,
  mixAny,
}
