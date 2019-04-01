import 'dart:collection';
import 'package:rw_utils/utils.dart' as $ut;
import 'stat.dart';

void toMatrixes(Map<String, int> booksDir, Iterable<LangWords> langWords) {
  _writeBooksIds(booksDir);
  _writeBrackets(langWords);
  _writeAlphabets(langWords);
  for (final lang in langWords) {}
}

//*************** ALPHABETS  */
void _writeAlphabets(Iterable<LangWords> langWords) => $ut.Matrix.fromData(
        langWords.map((lw) => [
              lw.lang,
              String.fromCharCodes(lw.okAlpha),
              String.fromCharCodes(lw.wrongsUnicodeAlpha),
              String.fromCharCodes(lw.wrongsCldrAlpha)
            ]),
        header: ['LANG', 'OK', 'WRONG UNICODE', 'WRONG CLDR'], sortColumn: 0)
    .save($ut.fileSystem.logParsed.absolute('alphabets.csv'));

//*************** BRACKETS  */
void _writeBrackets(Iterable<LangWords> langWords) {
  void write(String type, HashMap<String, Bracket> brs(LangWords lw), String fn) =>
      $ut.Matrix.fromData(langWords.expand((lw) => brs(lw).values
              .map((b) => [b.value, b.count.toString(), b.bookIds.join(',')])), header: ['content','#of','book ids'], sortColumn: 0)
          .save($ut.fileSystem.logParsed.absolute('$fn.csv'));
  write('[', (lw) => lw.bracketsSq, 'bracketsSq');
  write('{', (lw) => lw.bracketsCurl, 'bracketsCurl');
}

//*************** BOOK IDS  */
void _writeBooksIds(Map<String, int> booksDir) {
  final entries = booksDir.entries.toList()..sort((a, b) => a.value - b.value);
  $ut.Matrix.fromData(entries.map((e) => [e.value.toString(), e.key]),
          header: ['book#', 'path'])
      .save($ut.fileSystem.logParsed.absolute('bookIds.csv'));
}
