import 'dart:collection';
import 'package:tuple/tuple.dart';
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
import 'package:rw_utils/langs.dart' show Langs, Unicode;
import 'package:rw_utils/threading.dart';
import '../filer.dart';
import '../consts.dart';
import '../dom.dart';

void analyzeWords(FileInfo first, List<Tuple2<FileInfo, Word>> fileWords) {
  final map = HashMap<AnalyzeResult, int>();
  for (final fw in fileWords.where((w) =>
      (w.item2.flags & Flags.wInBr == 0) &&
      (w.item2.flags & Flags.wHasParts == 0) &&
      w.item2.text != null &&
      w.item2.text.isNotEmpty)) {
    final res = analyzeWord(fw.item1, fw.item2);
    map.update(res, (v) => v + 1, ifAbsent: () => 1);
  }

  final list = List<MapEntry<AnalyzeResult, int>>.from(map.entries);
  list.sort((a, b) => b.value - a.value);
  final lines = list.map((kv) => '${kv.value}x: ${kv.key.toString()}');
  fileSystem.edits
      .writeAsLines('analyzeSources\\words\\${first.dataLang}.txt', lines);
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
