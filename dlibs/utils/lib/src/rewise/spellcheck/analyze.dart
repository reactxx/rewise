import 'dart:collection';
import 'dart:convert' as conv;
import 'package:rw_utils/utils.dart' show fileSystem;
import 'cache.dart';

void dump() {
  String alphabetFromCaches(Iterable<String> words) =>
      String.fromCharCodes(HashSet<int>.from(words.expand((k) => k.codeUnits)).toList()..sort());

  void saveAlphabets(Map<String, String> alpha, String fn) =>
      fileSystem.spellCheckCache.writeAsString('alphabet.$fn.json', conv.jsonEncode(alpha));

  final alphaOK = Map<String, String>();
  final alphaWrong = Map<String, String>();
  for (final c in caches()) {
    fileSystem.spellCheckCache
        .writeAsLines('${c.lang}.ok.txt', c.correctWords().toList()..sort());
    fileSystem.spellCheckCache
        .writeAsLines('${c.lang}.wrong.txt', c.wrongWords().toList()..sort());
    alphaOK[c.lang] = alphabetFromCaches(c.correctWords());
    alphaWrong[c.lang] = alphabetFromCaches(c.wrongWords());
  }
  saveAlphabets(alphaOK, 'ok');
  saveAlphabets(alphaWrong, 'wrong');
}
