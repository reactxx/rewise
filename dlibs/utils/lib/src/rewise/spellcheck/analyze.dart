import 'dart:collection';
import 'dart:convert' as conv;
import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
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

  final alphaOK = Map<String, String>();
  final alphaWrong = Map<String, String>();
  for (final lang in cacheLangs()) {
    final c = SCCache.fromMemory(lang);
    fileSystem.spellCheckDump
        .writeAsLines('${c.lang}.ok.txt', c.correctWords().toList()..sort());
    fileSystem.spellCheckDump
        .writeAsLines('${c.lang}.wrong.txt', c.wrongWords().toList()..sort());
    alphaOK[c.lang] = alphabetFromCaches(c.correctWords());
    alphaWrong[c.lang] = alphabetFromCaches(c.wrongWords());
  }
  saveAlphabets(alphaOK, 'ok');
  saveAlphabets(alphaWrong, 'wrong');
}

void dumpSpellCheckFiles({String bookName}) {
  for (final grp in Filer.groups(GroupByType.fileName)) {
    if (bookName != null && bookName != grp.key) continue;
  final stat = Matrix(header: ['lang', 'OK', 'WRONG'], delim: ';');
    for (final fi in grp.values) {
      final ok = dumpSpellCheckFile(fi, true);
      final wrong = dumpSpellCheckFile(fi, false);
      stat.add([fi.dataLang, ok.toString(), wrong.toString()]);
    }
    stat.save(fileSystem.spellCheckDump.absolute('${grp.key}\\stat.csv'));
  }
}

int dumpSpellCheckFile(FileInfo fi, bool isOK) {
  var count = 0;
  final file = File.fromFileInfo(fi),
      cache = SCCache.fromMemory(fi.dataLang),
      words = cache.toCheckDump(
          scanFile(file, wordCondition: defaultWordCondition)
              .map((f) => f.item2.text)),
      str = wordsToHTML(
          file.dataLang,
          words.where((w) => w.item2 == isOK).map((w) {
            count++;
            return w.item1;
          }));
  if (count > 0)
    fileSystem.spellCheckDump.writeAsString(
        '${file.bookName}\\${file.dataLang}.${isOK ? 'ok' : 'wrong'}.html',
        str);
  return count;
}
