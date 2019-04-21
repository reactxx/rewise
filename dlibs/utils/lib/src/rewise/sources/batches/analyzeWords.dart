import 'dart:collection';
import 'package:tuple/tuple.dart';
import 'package:rw_utils/utils.dart' show fileSystem;
import '../consts.dart';
import '../dom.dart';

void analyzeWords(FileInfo first, List<Tuple3<FileInfo, Facts, Word>> fileWords,
    String pathFragment) {
  final list = _toFrequentList(fileWords);
  final lines = list.map((kv) =>
      '${kv.value.count}x.${WordFlags.toText(kv.key)}..${kv.value.words.join('|')}');
  fileSystem.edits.writeAsLines('analyzeSources\\$pathFragment.txt', lines);
}

class W {
  W(String w) {
    words.add(w);
  }
  int count = 1;
  final words = HashSet<String>();
}

List<MapEntry<int, W>> _toFrequentList(List<Tuple3<FileInfo, Facts, Word>> fileWords) {
  final map = HashMap<int, W>();
  for (final fw in fileWords) {
    final res = fw.item3.flags & WordFlags.wordsFlagsFlag; // analyzeWord(fw.item1.dataLang, fw.item3.text.codeUnits, otherLang: fw.item1.otherLang);
    map.update(res, (v) {
      v.count++;
      if (v.words.length < 100) v.words.add(fw.item3.text);
      return v;
    }, ifAbsent: () => W(fw.item3.text));
  }
  final list = List<MapEntry<int, W>>.from(map.entries);
  list.sort((a, b) => b.value.count - a.value.count);
  return list;
}

