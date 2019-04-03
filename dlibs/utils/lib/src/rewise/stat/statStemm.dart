import 'dart:collection';
import 'package:rw_utils/utils.dart' as $ut;
import 'package:rw_utils/rewise.dart' as rew;
import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_utils/langs.dart' show Langs;

stemmStat() {
  final langs = Set<String>.from(rew.StemmCache.stemmLangs)
      .intersection(Set<String>.from(rew.StemmCache.existingCachesLangs))
      .toList();
  for (final lang in langs) {
    final alphabetStems = HashSet<int>();
    final alphabetSingle = HashSet<int>();
    final matrixWrong = $ut.Matrix();
    final matrixOK = $ut.Matrix();
    for(final group in rew.StemmCache.iterateGroups(lang)) {
    //for (final kv in cache.words.entries) {
      // (kv.value == null ? alphabetSingle : alphabetStems)
      //     .addAll(kv.key.codeUnits);
      // final wrongs = Langs.wrongAlphabetChars(lang, kv.key);
      // if (wrongs.isNotEmpty)
      //   matrixWrong.add([kv.key, wrongs]);
      // else {
      //   matrixOK.add([kv.key, kv.value == null ? '-' : '+']);
      // }
    }

    fileSystem.statStemmed.writeAsLines('alphabet.$lang.txt', [
      Langs.wrongAlphabetChars(
          lang, String.fromCharCodes(alphabetStems.toList()..sort())),
      Langs.wrongAlphabetChars(lang, String.fromCharCodes(alphabetSingle.toList()..sort())),
    ]);
    matrixWrong.sort(0);
    matrixWrong.save(fileSystem.statStemmed.absolute('wrongs.$lang.csv'),
        noSaveRowLimit: 1);
    matrixOK.sort(0);
    matrixOK.save(fileSystem.statStemmed.absolute('ok.$lang.csv'),
        noSaveRowLimit: 1);
  }
}
