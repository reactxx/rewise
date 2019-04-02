import 'dart:collection';
import 'package:rw_utils/rewise.dart' as rew;
import 'package:rw_utils/utils.dart' show fileSystem;

stemmStat() {
  final langs = Set<String>.from(rew.StemmCache.stemmLangs)
      .intersection(Set<String>.from(rew.StemmCache.existingCachesLangs))
      .toList();
  for (final lang in langs) {
    final cache = rew.StemmCache.fromLang(lang);
    final alphabetStems = HashSet<int>();
    final alphabetWord = HashSet<int>();
    for (final kv in cache.words.entries)
      (kv.value == null ? alphabetWord : alphabetStems)
          .addAll(kv.key.codeUnits);
    fileSystem.statStemmed.writeAsLines('alphabet.$lang.txt', [
      String.fromCharCodes(alphabetStems.toList()),
      String.fromCharCodes(alphabetWord.toList()),
    ]);
  }
}
