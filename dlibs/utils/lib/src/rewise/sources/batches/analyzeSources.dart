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

  final fileWords = scanFileWords(msg).toList();

  // Words
  analyzeWords(first, fileWords);

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

  _numOfWordsAndChars(first, words, uniqueWords);
  _nonLetterChars(first, uniqueWords);
  _nonWordsChars(first, fileWords);
  _wordsLetters(first, uniqueWords);

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
  _writeCharStat(first, map, 'nonLetterChars');
}

void _nonWordsChars(FileInfo first, List<Tuple2<FileInfo, Word>> fw) {
  final map = HashMap<int, int>();
  for (final u in fw
      .expand((w) => [w.item2.before, w.item2.after])
      .expand((s) => s.codeUnits))
    map.update(u, (v) => v + 1, ifAbsent: () => 1);
  _writeCharStat(first, map, 'nonWordChars');
}

void _wordsLetters(FileInfo first, HashSet<String> uniqueWords) {
  final map = HashMap<int, int>();
  for (final u in uniqueWords
      .expand((w) => w.codeUnits.where((ch) => Unicode.isLetter(ch))))
    map.update(u, (v) => v + 1, ifAbsent: () => 1);
  _writeCharStat(first, map, 'wordLetters');
}

void _writeCharStat(FileInfo first, HashMap<int, int> map, String name) {
  final list = List<MapEntry<int, int>>.from(map.entries);
  list.sort((a, b) => b.value - a.value);
  final lines = list.map((kv) =>
      '${kv.value}x: ${String.fromCharCode(kv.key)} 0x${kv.key.toRadixString(16)}');
  fileSystem.edits
      .writeAsLines('analyzeSources\\$name\\${first.dataLang}.txt', lines);
}
