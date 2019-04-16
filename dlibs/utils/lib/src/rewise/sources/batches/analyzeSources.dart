import 'dart:collection';
import 'package:tuple/tuple.dart';
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
import 'package:rw_utils/langs.dart' show Langs, Unicode;
import 'package:rw_utils/threading.dart';
import '../filer.dart';
import '../consts.dart';
import '../dom.dart';
import 'analyzeWords.dart';

Future analyzeSources({bool doParallel, GroupByType groupBy = GroupByType.fileNameDataLang}) async => useSources(
    _analyzeSourcesEntryPoint, _analyzeSources, groupBy,
    emptyPrint: true, doParallel: doParallel);

void _analyzeSourcesEntryPoint(List workerInitMsg) =>
    parallelEntryPoint(workerInitMsg, _analyzeSources);

Future<Msg> _analyzeSources(DataMsg msg, InitMsg initPar) {
  FileInfo first = scanFileInfos(msg).first;
  print(initPar.listValue);
  print(first.dataLang);

  final fileWords = scanFileWords(msg).toList();

  // Words
  analyzeWords(first, fileWords, groupBy(first, initPar.listValue[0], 'words'));

  // Chars etc.
  final words = fileWords
      .map((fw) => fw.item2)
      .where((w) =>
          (w.flags & Flags.wInBr == 0) &&
          (w.flags & Flags.wIsPartOf == 0) &&
          w.text != null &&
          w.text.isNotEmpty)
      .map((w) => w.text)
      .toList();

  final uniqueWords = HashSet<String>.from(words);

  _numOfWordsAndChars(first, words, uniqueWords,
      groupBy(first, initPar.listValue[0], 'numOfWordsAndChars'));
  _nonLetterChars(first, uniqueWords,
      groupBy(first, initPar.listValue[0], 'nonLetterChars'));
  _nonWordsChars(
      first, fileWords, groupBy(first, initPar.listValue[0], 'nonWordChars'));
  _wordsLetters(
      first, uniqueWords, groupBy(first, initPar.listValue[0], 'wordLetters'));
  _wordsChars(first, words, groupBy(first, initPar.listValue[0], 'wordChars'));

  return Parallel.workerReturnFuture;
}

void _numOfWordsAndChars(FileInfo first, List<String> words,
    HashSet<String> uniqueWords, String pathFragment) {
  final wordsChars = Linq.sum(words.map((w) => w.length));
  final uniqueWordsChars = Linq.sum(uniqueWords.map((w) => w.length));
  final chars = HashSet<int>.from(uniqueWords.expand((w) => w.codeUnits));

  fileSystem.edits.writeAsString('analyzeSources\\$pathFragment.txt',
      'words=${words.length}, wordsChars=$wordsChars, uniqueWords=${uniqueWords.length}, uniqueWordsChars=$uniqueWordsChars, chars=${chars.length}');
}

void _nonLetterChars(
    FileInfo first, HashSet<String> uniqueWords, String pathFragment) {
  final map = HashMap<int, int>();
  for (final u in uniqueWords
      .expand((w) => w.codeUnits.where((ch) => !Unicode.isLetter(ch))))
    map.update(u, (v) => v + 1, ifAbsent: () => 1);
  _writeCharStat(first, map, pathFragment);
}

void _nonWordsChars(
    FileInfo first, List<Tuple2<FileInfo, Word>> fw, String pathFragment) {
  final map = HashMap<int, int>();
  for (final u in fw
      .expand((w) => [w.item2.before, w.item2.after])
      .expand((s) => s.codeUnits))
    map.update(u, (v) => v + 1, ifAbsent: () => 1);
  _writeCharStat(first, map, pathFragment);
}

void _wordsLetters(
    FileInfo first, HashSet<String> uniqueWords, String pathFragment) {
  final map = HashMap<int, int>();
  for (final u in uniqueWords
      .expand((w) => w.codeUnits.where((ch) => Unicode.isLetter(ch))))
    map.update(u, (v) => v + 1, ifAbsent: () => 1);
  _writeCharStat(first, map, pathFragment);
}

void _wordsChars(FileInfo first, List<String> words, String pathFragment) {
  final map = HashMap<int, int>();
  for (final u in words.expand((w) => w.codeUnits))
    map.update(u, (v) => v + 1, ifAbsent: () => 1);
  _writeCharStat(first, map, pathFragment);
}

void _writeCharStat(
    FileInfo first, HashMap<int, int> map, String pathFragment) {
  final myScript = Langs.nameToMeta[first.dataLang].scriptId;
  final list = List<MapEntry<int, int>>.from(map.entries);
  list.sort((a, b) => b.value - a.value);
  final lines = list.map((kv) =>
      '${charType(kv.key, myScript, first.dataLang)} ${kv.value}x: ${String.fromCharCode(kv.key)} 0x${kv.key.toRadixString(16)}');
  fileSystem.edits.writeAsLines('analyzeSources\\$pathFragment.txt', lines);
}

String charType(int ch, String myScript, String dataLang) {
  final uni = Unicode.item(ch);
  if (uni == null) return '-';
  if (myScript != 'Latn' && uni.script == 'Latn') return 'L';
  if (Langs.isCldrChar(dataLang, ch) == true) return '*';
  if (Unicode.scriptsEq(myScript, uni.script)) return '+';
  return '?';
}
