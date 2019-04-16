import 'dart:collection';
import 'data_langsData.dart' show getLangsData;
import 'package:rw_utils/dom/utils.dart';
//import 'netToLower.dart' show netToLowerChar;
import 'netToLower2.dart' show netToLowerChar2;

export 'package:rw_utils/dom/utils.dart';

class Langs {
  static List<CldrLang> get meta => _meta ?? (_meta = getLangsData().langs);
  static List<CldrLang> _meta;
  static HashMap<String, CldrLang> get nameToMeta {
    return _nameToMeta ??
        (_nameToMeta = HashMap<String, CldrLang>.fromIterable(meta,
            key: (item) => item.id, value: (item) => item));
  }

  static HashMap<String, CldrLang> _nameToMeta;
  static String netToLower(String str) => str == null || str.isEmpty
      ? str
      : String.fromCharCodes(str.codeUnits.map((c) => netToLowerChar2(c)));

  static bool isCldrChar(String lang, int code) {
    code = netToLowerChar2(code);
    final chars = _alphabetChars.putIfAbsent(
        lang, () => HashSet<int>.from(nameToMeta[lang].alphabet.codeUnits));
    return chars.length == 0 ? null : chars.contains(code);
  }

  static String wrongAlphabetCodes(String lang, HashSet<int> wordChars) {
    final chars = _alphabetChars.putIfAbsent(
        lang, () => HashSet<int>.from(nameToMeta[lang].alphabet.codeUnits));
    final res =
        String.fromCharCodes(wordChars.difference(chars).toList()..sort());
    return res.isEmpty ? '' : res;
  }

  static String wrongAlphabetChars(String lang, String word) {
    final chars = _alphabetChars.putIfAbsent(
        lang, () => HashSet<int>.from(nameToMeta[lang].alphabet.codeUnits));
    final wordChars = HashSet<int>.from(word.codeUnits);
    final res =
        String.fromCharCodes(wordChars.difference(chars).toList()..sort());
    return res.isEmpty ? '' : res;
  }

  static final _alphabetChars = HashMap<String, HashSet<int>>();
}
