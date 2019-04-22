import 'dart:collection';
import 'package:tuple/tuple.dart';
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_utils/langs.dart' show Unicode;
import 'package:rw_utils/threading.dart';
import '../filer.dart';
import '../consts.dart';
import '../dom.dart';
import '../analyzeWord.dart';

Future analyzeSources(
        {bool doParallel,
        GroupByType groupBy = GroupByType.fileNameDataLang,
        bool emptyPrint}) async =>
    useSources(_analyzeSourcesEntryPoint, _analyzeSources, groupBy,
        emptyPrint: emptyPrint, doParallel: doParallel);

void _analyzeSourcesEntryPoint(List workerInitMsg) =>
    parallelEntryPoint(workerInitMsg, _analyzeSources);

Future<Msg> _analyzeSources(DataMsg msg, InitMsg initPar) {
  FileInfo first = scanFileInfos(msg).first;

  String gBy(String name) => groupBy(first, initPar.listValue[0], name);
  print(gBy(null));

  final fileWords = scanFileWords(msg).toList();
  final wordCharsList = _charsLow(fileWords);

  _words(first, fileWords, gBy('&words'));
  _dumpCharFacts(first, fileWords, gBy('&dumpFacts'));
  _chars(first, wordCharsList, gBy('&wordChars'), false);
  _chars(first, wordCharsList, gBy('&extendAlphabet'), true);

  return Parallel.workerReturnFuture;
}

class W {
  W(String w) {
    words.add(w);
  }
  int count = 1;
  final words = HashSet<String>();
}

void _dumpCharFacts(FileInfo first, List<Tuple3<FileInfo, Facts, Word>> fw,
    String pathFragment) {
  final facts = fw
      .map((fw) => '${fw.item1.fileName}: ${fw.item2.toText()}')
      .where((f) => f.contains('\u{fffd}'));
  fileSystem.edits.writeAsLines('analyzeSources\\$pathFragment.txt', facts);
}

List<MapEntry<int, W>> _charsLow(
    List<Tuple3<FileInfo, Facts, Word>> fileWords) {
  final map = HashMap<int, W>();
  for (final w in fileWords.map((fw) => fw.item3.text)) {
    for (final u in w.codeUnits)
      map.update(u, (v) {
        v.count++;
        if (v.words.length < 100) v.words.add(w);
        return v;
      }, ifAbsent: () => W(w));
  }
  final list = List<MapEntry<int, W>>.from(map.entries);
  list.sort((a, b) => b.value.count - a.value.count);
  return list;
}

void _chars(FileInfo first, List<MapEntry<int, W>> list, String pathFragment,
    bool forAlphabet) {
  Iterable<String> lines;
  if (!forAlphabet)
    lines = list.map(
        (kv) => '${analyzeWordMark(first.dataLang, kv.key)}.${kv.value.count}x:'
            '.${String.fromCharCode(kv.key)}.0x${kv.key.toRadixString(16)}'
            '.${kv.value.words.join('|')}');
  else
    lines = list.map((kv) {
      final cht = analyzeWordMark(first.dataLang, kv.key);
      if (cht == '*' || Unicode.isDigit(kv.key)) return null;
      return '\\x${kv.key.toRadixString(16)}#'
          '$cht.${kv.value.count}x:'
          '.${String.fromCharCode(kv.key)}'
          '.${kv.value.words.join('|')}';
    }).where((l) => l != null);

  fileSystem.edits.writeAsLines('analyzeSources\\$pathFragment.txt', lines);
}

void _words(FileInfo first, List<Tuple3<FileInfo, Facts, Word>> fileWords,
    String pathFragment) {
  final map = HashMap<int, W>();
  void update(String w, int flag) => map.update(flag, (v) {
        v.count++;
        if (v.words.length < 100) v.words.add(w);
        return v;
      }, ifAbsent: () => W(w));

  for (final w in fileWords.map((f) => f.item3)) update(w.text, w.flags);
  // var res = w.flags & WordFlags.wordsFlagsFlag;
  // if (res & WordFlags.okCldr!=0 && res & WordFlags.okSpell!=0)
  //   update(w.text, WordFlags.okCldrSpell);
  // else
  //   update(w.text, res);

  final list = List<MapEntry<int, W>>.from(map.entries);
  list.sort((a, b) => b.value.count - a.value.count);
  final lines = list.map((kv) =>
      '${kv.value.count}x.${WordFlags.toTextFirst(kv.key) ?? ''}..${kv.value.words.join('|')}');
  fileSystem.edits.writeAsLines('analyzeSources\\$pathFragment.txt', lines);
}
