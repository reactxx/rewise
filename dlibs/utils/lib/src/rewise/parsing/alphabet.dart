import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/langs.dart' show Langs, Unicode;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;

List<wbreak.PosLen> alphabetTest(String lang, toPars.ParsedSubFact fact,
    Iterable<wbreak.PosLen> posLens, StringBuffer errors) {

  final langScript = Langs.nameToMeta[lang].scriptId;
  bool isError = false;

  var res = posLens.where((pl) {
    final word = fact.text.substring(pl.pos, pl.pos + pl.len);
    final err = Unicode.latinOrScript(langScript, word);
    if (err==null) return true;
    if (!isError) {
      errors.writeln('FACT: $langScript expected in "${fact.text}"');
      errors.write('  ');
      isError = true;
    }
    errors.write('$word ($err), ');
    return false;
  }).toList();

  if (isError) errors.writeln('');
  return res;
}
