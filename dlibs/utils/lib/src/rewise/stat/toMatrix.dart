import 'dart:collection';
import 'package:rw_utils/utils.dart' as $ut;
import 'package:rw_low/code.dart' show adjustFileDir;
import 'stat.dart';

void toMatrixes(Stats stats, [String resultSubPath = '']) {
  void save($ut.Matrix matrix, String relFn) async {
    final fn = $ut.fileSystem.statParsed.absolute('${resultSubPath.isEmpty ? '' : resultSubPath + '\\'}$relFn.csv');
    adjustFileDir(fn);
    //print(fn);
    matrix.save(fn, noSaveRowLimit: 2);
  }

  save(_writeBooksIds(stats.bookIds), 'bookIds');
  save(_writeBrackets('[', stats.bracketsSq), 'bracketsSq');
  save(_writeBrackets('{', stats.bracketsCurl), 'bracketsCurl');
  save(_writeBrackets('{', stats.bracketsCurlIndex), 'bracketsCurlIndex');
  //save(_writeAlphabets(stats.stats.values), 'alphabets');
  for (final words in stats.stats.values) {
    $ut.fileSystem.statParsed.writeAsString('wrong.${words.lang}.txt', _writeWordsWrongStr(words));
    save(_writeWordsWrong(words), 'wrong.${words.lang}');
    save(_writeWordsOther(words, true), 'ok.${words.lang}');
    save(_writeWordsOther(words, false), 'latn.${words.lang}');
  }
}

//*************** WORDS  */
String _writeWordsWrongStr(StatLang words) => words.wrongs.values.map((w) => w.text).join(',');

$ut.Matrix _writeWordsWrong(StatLang words) => $ut.Matrix.fromData(
    words.wrongs.values.map((w) => [
          w.text,
          w.wrongUnicode,
          w.wrongCldr,
          w.count.toString(),
          w.bookIds.length.toString(),
          w.bookIds.take(100).join(',')
        ]),
    header: [
      'WORD',
      'WRONG UNICODE',
      'WRONG CLDR',
      '#',
      '# BOOKS',
      'BOOOK IDS'
    ],
    sortColumn: 0);

$ut.Matrix _writeWordsOther(StatLang words, bool isOK) => $ut.Matrix.fromData(
    (isOK ? words.ok : words.latin).values.map((w) => [
          w.text,
          w.count.toString(),
          w.bookIds.length.toString(),
          w.bookIds.take(100).join(',')
        ]),
    header: ['WORD', '#', '# BOOKS', 'BOOOK IDS'],
    sortColumn: 0);

//*************** ALPHABETS  */
$ut.Matrix _writeAlphabets(Iterable<StatLang> langWords) => $ut.Matrix.fromData(
    langWords.map((lw) => [
          lw.lang,
          String.fromCharCodes(lw.okAlpha.toList()..sort()),
          String.fromCharCodes(lw.wrongsUnicodeAlpha.toList()..sort()),
          String.fromCharCodes(lw.wrongsCldrAlpha.toList()..sort())
        ]),
    header: ['LANG', 'OK', 'WRONG UNICODE', 'WRONG CLDR'],
    sortColumn: 0);

//*************** BRACKETS  */
$ut.Matrix _writeBrackets(String type, HashMap<String, Bracket> brs) =>
    $ut.Matrix.fromData(
        brs.values.map((b) => [
              b.value,
              b.count.toString(),
              b.bookIds.length.toString(),
              b.bookIds.take(100).join(',')
            ]),
        header: ['content', '#', '#books', 'book ids'],
        sortColumn: 0);

//*************** BOOK IDS  */
$ut.Matrix _writeBooksIds(Map<String, int> booksDir) {
  final entries = booksDir.entries.toList()..sort((a, b) => a.value - b.value);
  return $ut.Matrix.fromData(entries.map((e) => [e.value.toString(), e.key]),
      header: ['book#', 'path']);
}
