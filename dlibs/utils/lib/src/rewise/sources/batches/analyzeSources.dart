import 'dart:collection';
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
import 'package:rw_utils/langs.dart' show Langs, Unicode;
import 'package:rw_utils/threading.dart';
import '../filer.dart';
import '../consts.dart';
import '../dom.dart';

List _groupByDataLangOnly(FileInfo f) => [f.dataLang];

Future analyzeSources({bool doParallel}) async =>
    useSources(_analyzeSourcesEntryPoint, _analyzeSources,
        groupBy: _groupByDataLangOnly,
        emptyPrint: true,
        doParallel: doParallel);

void _analyzeSourcesEntryPoint(List workerInitMsg) =>
    parallelEntryPoint(workerInitMsg, _analyzeSources);

Future<Msg> _analyzeSources(DataMsg msg) {
  FileInfo first = scanFileInfos(msg).first;
  print(first.dataLang);
  
  final wordsLow = scanFiles(msg)
      .expand((f) => f.factss)
      .expand((fs) => fs.facts)
      .expand((f) => f.words)
      .toList();

  final words = wordsLow
      .where((w) =>
          (w.flags == 0 || w.flags == Flags.wHasParts) &&
          w.text != null &&
          w.text.isNotEmpty)
      .map((w) => w.text)
      .toList();

  final uniqueWords = HashSet<String>.from(words);

  _numOfWordsAndChars(first, words, uniqueWords);
  _nonLetterChars(first, uniqueWords);
  _nonWordsChars(first, wordsLow);

  return Parallel.workerReturnFuture;
}

void _numOfWordsAndChars(
    FileInfo first, List<String> words, HashSet<String> uniqueWords) {
  final wordsChars = Linq.sum(words.map((w) => w.length));
  final uniqueWordsChars = Linq.sum(uniqueWords.map((w) => w.length));
  final chars = HashSet<int>.from(uniqueWords.expand((w) => w.codeUnits));

  fileSystem.edits.writeAsString(
      'analyzeSources\\numOfWordsAndChars\\${first.dataLang}.txt',
      'words=${words.length}, wordsChars=$wordsChars, uniqueWords=${uniqueWords.length}, uniqueWordsChars=$uniqueWordsChars, chars=${chars.length}');
}

void _nonLetterChars(FileInfo first, HashSet<String> uniqueWords) {
  final map = HashMap<int, int>();
  for (final u in uniqueWords
      .expand((w) => w.codeUnits.where((ch) => !Unicode.isLetter(ch))))
    map.update(u, (v) => v + 1, ifAbsent: () => 1);
  final list = List<MapEntry<int, int>>.from(map.entries);
  list.sort((a, b) => b.value - a.value);
  final lines = list.map((kv) =>
      '${String.fromCharCode(kv.key)} (${kv.key.toRadixString(16)}): ${kv.value}');
  fileSystem.edits.writeAsLines(
      'analyzeSources\\nonLetterChars\\${first.dataLang}.txt', lines);
}

void _nonWordsChars(FileInfo first, List<Word> words) {
  final map = HashMap<int, int>();
  for (final u in words
      .expand((w) => [w.before, w.after]).expand((s) => s.codeUnits))
    map.update(u, (v) => v + 1, ifAbsent: () => 1);
  final list = List<MapEntry<int, int>>.from(map.entries);
  list.sort((a, b) => b.value - a.value);
  final lines = list.map((kv) =>
      '${String.fromCharCode(kv.key)} (${kv.key.toRadixString(16)}): ${kv.value}');
  fileSystem.edits.writeAsLines(
      'analyzeSources\\nonWordChars\\${first.dataLang}.txt', lines);
}
