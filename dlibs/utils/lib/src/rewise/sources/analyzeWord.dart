import 'package:rw_utils/langs.dart' show Langs, Unicode;
import 'consts.dart';


int analyzeWord(String dataLang, List<int> wordCodeUnits) {
  final myScript = Langs.nameToMeta[dataLang].scriptId;
  var ok = 0, nonLetter = 0, latin = 0, wrongLetter = 0, len = 0;
  int cldr = null;
  for (final ch in wordCodeUnits) {
    len++;
    final uni = Unicode.item(ch);
    if (uni == null) {
      nonLetter++;
      continue;
    }
    if (myScript != 'Latn' && uni.script == 'Latn') latin++;
    //cldr
    final c = Langs.isCldrChar(dataLang, ch);
    if (c != null) {
      if (cldr == null) cldr = 0;
      if (c) cldr++;
    }

    if (Unicode.scriptsEq(myScript, uni.script))
      ok++;
    else
      wrongLetter++;
  }
  if (len == cldr) return Flags.okCldr;
  if (len == ok) return Flags.ok;
  if (len == nonLetter) return Flags.nonLetter;
  if (len == latin) return Flags.latin;
  //if (len == other) return Flags.other;
  if (len == wrongLetter) return Flags.wrong;

  // Flags percent(
  //     Flags gt66, Flags gt33, Flags any) {
  //   final okNum = ok / len;
  //   if (okNum < 0.33) return gt66;
  //   if (okNum < 0.66) return gt33;
  //   return any;
  // }

  if (ok > 0 && len == ok + nonLetter)
    return Flags.nonLetterAny;
  if (ok > 0 && len == ok + latin)
    return Flags.latinAny;
  if (ok == 0) return Flags.mix;
  return Flags.mixAny;
}