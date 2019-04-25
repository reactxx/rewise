import 'dart:collection';
import 'dart:convert' as conv;
import 'package:rw_utils/utils.dart' show fileSystem;
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
  for (final c in caches()) {
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
    for (final fi in grp.values) dumpSpellCheckFile(fi);
  }
}

void dumpSpellCheckFile(FileInfo fi) {
  //Filer.groups(GroupByType.fileName)
  final file = File.fromFileInfo(fi),
      cache = SCCache.fromLang(file.dataLang),
      words = cache.toCheckDump(
          scanFile(file, wordCondition: defaultWordCondition)
              .map((f) => f.item2.text));
  void toHTML(bool isOK) {
    final str = wordsToHTML(
        file.dataLang, words.where((w) => w.item2 == isOK).map((w) => w.item1));
    fileSystem.spellCheckDump.writeAsString(
        '${file.bookName}\\${file.dataLang}.${isOK ? 'ok' : 'wrong'}.html',
        str);
  }

  toHTML(true);
  toHTML(false);
}
