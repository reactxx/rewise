import 'dart:collection';
import 'package:rw_utils/utils.dart' as $ut;
import 'package:rw_utils/rewise.dart' as rew;
import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_utils/langs.dart' show Langs;

stemmStat() {
  final langs = Set<String>.from(rew.StemmCache.stemmLangs)
      .intersection(Set<String>.from(rew.StemmCache.existingCachesLangs))
      .toList();
  for (final lang in ['cs-CZ']) { //langs) {
    final cache = rew.StemmCache.fromLang(lang);
    final alphabetStems = HashSet<int>();
    final alphabetSingle = HashSet<int>();
    final matrix = $ut.Matrix();
    for (final kv in cache.words.entries) {
      (kv.value == null ? alphabetSingle : alphabetStems)
          .addAll(kv.key.codeUnits);
      final wrongs = Langs.wrongAlphabetChars(lang, kv.key);
      if (wrongs!=null) matrix.add([kv.key, wrongs]);
    }

    fileSystem.statStemmed.writeAsLines('alphabet.$lang.txt', [
      String.fromCharCodes(alphabetStems.toList()),
      String.fromCharCodes(alphabetSingle.toList()),
    ]);
    matrix.sort(0);
    matrix.save(fileSystem.statStemmed.absolute('$lang.csv'), noSaveRowLimit: 1);
  }
}
