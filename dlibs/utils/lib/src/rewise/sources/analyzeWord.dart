import 'package:rw_utils/langs.dart' show Langs, Unicode;
import 'consts.dart';

int analyzeWord(String dataLang, List<int> wordCodeUnits) {
  final myScript = Langs.nameToMeta[dataLang].scriptId;
  var ok = 0, nonLetter = 0, latin = 0, wrongLetter = 0, len = 0;
  int cldr = 0;
  for (final ch in wordCodeUnits) {
    len++;
    // high prio: cldr alphabet
    final c = Langs.isCldrChar(dataLang, ch);
    if (c) {
      cldr++;
      ok++;
      continue;
    }

    final uni = Unicode.item(ch);
    if (uni == null) {
      nonLetter++;
      continue;
    }
    if (myScript != 'Latn' && uni.script == 'Latn') latin++;

    if (Unicode.scriptsEq(myScript, uni.script))
      ok++;
    else
      wrongLetter++;
  }
  if (dataLang == 'ja-JP' || dataLang == 'ko-KR' || dataLang == 'zh-Hans')
    cldr = ok;
  if (len == cldr) return WordFlags.cldr;
  if (len == ok) return WordFlags.ok;
  if (len == nonLetter) return WordFlags.nonLetter;
  if (len == latin) return WordFlags.latin;
  //if (len == other) return Flags.other;
  if (len == wrongLetter) return WordFlags.wrong;

  if (ok > 0 && len == ok + nonLetter) return WordFlags.nonLetterAny;
  if (ok > 0 && len == ok + latin) return WordFlags.latinAny;
  if (ok == 0) return WordFlags.mix;
  return WordFlags.mixAny;
}

String analyzeWordMark(String lang, int char) {
  final flag = analyzeWord(lang, [char]);
  switch (flag) {
    case WordFlags.spell:
      return '§';
    case WordFlags.cldr:
      return '*';
    case WordFlags.ok:
      return '+';
    case WordFlags.latin:
      return 'L';
    case WordFlags.nonLetter:
      return '-';
    default:
      return '?';
  }
}

