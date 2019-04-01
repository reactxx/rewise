import 'dart:collection';
import 'package:rw_utils/utils.dart' as $ut;
import 'stat.dart';

void toMatrixes(Map<String, int> booksDir, Iterable<LangWords> langWords) {
  _writeBooksIds(booksDir);
  _writeBrackets(langWords);
  for (final lang in langWords) {}
}

//*************** BRACKETS  */
void _writeBrackets(Iterable<LangWords> langWords) {
  void write(String type, String fn) =>
      $ut.Matrix.fromData(langWords.expand((lw) => lw.brackets.values
              .where((v) => v.type == type)
              .map((b) => [b.value, b.count.toString(), b.bookIds.join(',')])))
          .save($ut.fileSystem.logParsed.absolute('$fn.csv'));
  write('[', 'bracketsWordClass');
  write('{', 'bracketsCurl');
}

//*************** BOOK IDS  */
void _writeBooksIds(Map<String, int> booksDir) {
  final entries = booksDir.entries.toList()..sort((a, b) => a.value - b.value);
  final mx = $ut.Matrix();
  mx.add(['book#', 'path']);
  mx.addAll(entries.map((e) => [e.value.toString(), e.key]));
  mx.save($ut.fileSystem.logParsed.absolute('bookIds.csv'));
}
