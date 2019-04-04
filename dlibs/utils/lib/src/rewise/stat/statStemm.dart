import 'dart:collection';
import 'package:rw_utils/utils.dart' as $ut;
import 'package:rw_utils/rewise.dart' as rew;
import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_utils/langs.dart' show Langs;

stemmStat() {
  final langs = Set<String>.from(rew.StemmCache.stemmLangs)
      .intersection(Set<String>.from(rew.StemmCache.existingCachesLangs))
      .toList();
  final matrixAllStat = $ut.Matrix();
  matrixAllStat.add([
    'LANG',
    'ownOwn#',
    'ownOther#',
    'emptyGroups#',
    'aliasGroups',
    'ownOwn',
    'ownOther',
    'emptyGroups',
    'aliasGroups'
  ]);
  for (final lang in langs) {
    final alphabetOwnOwn = HashSet<int>(); 
    final alphabetOwnOther = HashSet<int>();
    final alphabetEmpty = HashSet<int>();
    final alphabetAlias = HashSet<int>();
    //final matrixWrong = $ut.Matrix();
    //final matrixOK = $ut.Matrix();

    int ownOwn = 0;
    int ownOther = 0;
    int emptyGroups = 0;
    int aliasGroups = 0;
    for (final group in rew.StemmCache.iterateGroups(lang)) {
      if (group.alias != null) {
        aliasGroups++;
        alphabetAlias.addAll(group.alias.codeUnits);
        continue;
      }
      if (group.ownWords == null) {
        emptyGroups++;
        alphabetEmpty.addAll(group.key.codeUnits);
        continue;
      }
      for (final own in group.ownWords) {
        ownOwn++;
        alphabetOwnOwn.addAll(own.word.codeUnits);
      }
      if (group.words != null)
        for (final other in group.words) {
          ownOther++;
          alphabetOwnOther.addAll(other.codeUnits);
        }
    }

    final aown = Langs.wrongAlphabetCodes(lang, alphabetOwnOwn);
    final aother = Langs.wrongAlphabetCodes(lang, alphabetOwnOther);
    final aempty = Langs.wrongAlphabetCodes(lang, alphabetEmpty);
    final aalias = Langs.wrongAlphabetCodes(lang, alphabetAlias);

    matrixAllStat.add([
      lang,
      ownOwn.toString(),
      ownOther.toString(),
      emptyGroups.toString(),
      aliasGroups.toString(),
      aown,
      aother == aown ? '' : aother,
      aempty == aown ? '' : aempty,
      aalias == aown ? '' : aalias,
    ]);
    // fileSystem.statStemmed.writeAsLines('alphabet.$lang.txt', [
    //   Langs.wrongAlphabetChars(
    //       lang, String.fromCharCodes(alphabetStems.toList()..sort())),
    //   Langs.wrongAlphabetChars(
    //       lang, String.fromCharCodes(alphabetSingle.toList()..sort())),
    // ]);
    // matrixWrong.sort(0);
    // matrixWrong.save(fileSystem.statStemmed.absolute('wrongs.$lang.csv'),
    //     noSaveRowLimit: 1);
    // matrixOK.sort(0);
    // matrixOK.save(fileSystem.statStemmed.absolute('ok.$lang.csv'),
    //     noSaveRowLimit: 1);
  }
  matrixAllStat.sort(0);
  matrixAllStat.save(fileSystem.statStemmed.absolute('all.csv'));
}
