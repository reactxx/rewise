import 'dart:collection';
import 'package:rw_utils/utils.dart' as $ut;
import 'stat.dart';

void toMatrixes(Stats stats) {
  _writeBooksIds(stats.bookIds);
  _writeBrackets(stats);
  _writeAlphabets(stats.stats.values);
  for (final lang in stats.stats.values) {
    _writeWordsWrong(lang);
    _writeWordsOther(lang, true);
    _writeWordsOther(lang, false);
  }
}

//*************** WORDS  */
void _writeWordsWrong(StatLang words) =>
  $ut.Matrix.fromData(
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
          sortColumn: 0)
      .save($ut.fileSystem.logParsed.absolute('wordsWrong.${words.lang}.csv'));

void _writeWordsOther(StatLang words, bool isOK) =>
  $ut.Matrix.fromData(
          (isOK ? words.ok : words.latin).values.map((w) => [
                w.text,
                w.count.toString(),
                w.bookIds.length.toString(),
                w.bookIds.take(100).join(',')
              ]),
          header: [
            'WORD',
            '#',
            '# BOOKS',
            'BOOOK IDS'
          ],
          sortColumn: 0)
      .save($ut.fileSystem.logParsed.absolute('words${isOK ? 'OK' : 'Latn'}.${words.lang}.csv'));
//*************** ALPHABETS  */
void _writeAlphabets(Iterable<StatLang> langWords) => $ut.Matrix.fromData(
        langWords.map((lw) => [
              lw.lang,
              String.fromCharCodes(lw.okAlpha),
              String.fromCharCodes(lw.wrongsUnicodeAlpha),
              String.fromCharCodes(lw.wrongsCldrAlpha)
            ]),
        header: ['LANG', 'OK', 'WRONG UNICODE', 'WRONG CLDR'],
        sortColumn: 0)
    .save($ut.fileSystem.logParsed.absolute('alphabets.csv'));

//*************** BRACKETS  */
void _writeBrackets(Stats stats) {
  void write(String type, HashMap<String, Bracket> brs, String fn) =>
      $ut.Matrix.fromData(
              brs.values.map((b) => [
                    b.value,
                    b.count.toString(),
                    b.bookIds.length.toString(),
                    b.bookIds.take(100).join(',')
                  ]),
              header: ['content', '#', '#books', 'book ids'],
              sortColumn: 0)
          .save($ut.fileSystem.logParsed.absolute('$fn.csv'));
  write('[', stats.bracketsSq, 'bracketsSq');
  write('{', stats.bracketsCurl, 'bracketsCurl');
  write('{', stats.bracketsCurlIndex, 'bracketsCurlIndex');
}

//*************** BOOK IDS  */
void _writeBooksIds(Map<String, int> booksDir) {
  final entries = booksDir.entries.toList()..sort((a, b) => a.value - b.value);
  $ut.Matrix.fromData(entries.map((e) => [e.value.toString(), e.key]),
          header: ['book#', 'path'])
      .save($ut.fileSystem.logParsed.absolute('bookIds.csv'));
}
