import 'dart:collection';
import 'dart:convert' as conv;
import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
import 'package:tuple/tuple.dart';
import '../sources/filer.dart';
import '../sources/dom.dart';
import '../sources/consts.dart';
import 'cache.dart';
import 'spellCheck.dart';

void dumpSpellCaches() {
  String alphabetFromCaches(Iterable<String> words) => String.fromCharCodes(
      HashSet<int>.from(words.expand((k) => k.codeUnits)).toList()..sort());

  void saveAlphabets(Map<String, String> alpha, String fn) =>
      fileSystem.spellCheckDump
          .writeAsString('alphabet.$fn.json', conv.jsonEncode(alpha));

  final alphaOK = Map<String, String>(),
      alphaWrong = Map<String, String>(),
      stat = Matrix(header: ['lang', 'OK', 'WRONG'], delim: ';');
  for (final lang in SCCache.cacheLangs()) {
    final cache = SCCache.fromMemory(lang),
        ok = cache.correctWords().toList()..sort(),
        wrongs = cache.wrongWords().toList()..sort();
    fileSystem.spellCheckDump.writeAsLines('$lang.ok.txt', ok);
    fileSystem.spellCheckDump.writeAsLines('$lang.wrong.txt', wrongs);
    alphaOK[cache.lang] = alphabetFromCaches(ok);
    alphaWrong[cache.lang] = alphabetFromCaches(wrongs);
    stat.add([lang, ok.length.toString(), wrongs.length.toString()]);
  }
  saveAlphabets(alphaOK, 'ok');
  saveAlphabets(alphaWrong, 'wrong');
  stat.save(fileSystem.spellCheckDump.absolute('stat.csv'));
}

void dumpSpellCheckFiles({bool filter(FileInfo fi)}) {
  for (final grp
      in Filer.groups(GroupByType.fileNameDataLang, filter: filter)) {
    final first = grp.values.first;
    final cache = SCCache.fromMemory(first.dataLang);
    if (cache == null) continue;
    final words = cache.toCheckDump(HashSet<String>.from(scanFilesLow(
            grp.values)
        .expand((file) => scanFile(file, wordCondition: defaultWordCondition))
        .map((w) => w.item2.text))).toList();
    //final stat = Matrix(header: ['lang', 'OK', 'WRONG'], delim: ';');
    dumpSpellCheckFile(words, true, first);
    dumpSpellCheckFile(words, false, first);
    //stat.add([fi.dataLang, ok.toString(), wrong.toString()]);
    //stat.save(fileSystem.spellCheckDump.absolute('${grp.key}\\stat.csv'));
  }
}

int dumpSpellCheckFile(
    Iterable<Tuple2<String, bool>> words, bool isOK, FileInfo fi) {
  var count = 0;
  final str = wordsToHTML(
      fi.dataLang,
      words.where((w) => w.item2 == isOK).map((w) {
        count++;
        return w.item1;
      }),
      toTable: true);
  if (count > 0)
    fileSystem.spellCheckDump.writeAsString(
        '${fi.bookName}\\${fi.dataLang}.${isOK ? 'ok' : 'wrong'}.html', str);
  return count;
}
