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
  final brs = langWords
      .expand((lw) =>
          lw.brackets.values.where((v) => v.type == '[').map((b) => _Br()
            ..value = b.value
            ..count = b.count
            ..bookIds = b.bookIds))
      .toList();
  brs.sort((a,b) => a.r.getData(0).compareTo(b.r.getData(0)));
  final mx = $ut.MatrixTyped<_Br>(null);
  mx.tRows.addAll(brs);
  mx.save($ut.fileSystem.logParsed.absolute('wordClasses.csv'));
}

abstract class _IdsCount extends $ut.RowTyped {
  _IdsCount(this._idx, [r]) : super(r);
  set bookIds(HashSet<int> v) => set(_idx+1, v.join(','));
  set count(int v) => setInt(_idx, v);
  int _idx;
}

class _Br extends _IdsCount {
  _Br([r]) : super(2, r);
  int get len => 3;
  set value(v) => set(0, v);
}

//*************** BOOK IDS  */
void _writeBooksIds(Map<String, int> booksDir) {
  final entries = booksDir.entries.toList()..sort((a, b) => a.value - b.value);
  final rows = entries.map((e) => _WBIds()
    ..bookId = e.value
    ..path = e.key);
  final mx = $ut.MatrixTyped<_WBIds>(null);
  mx.tRows.add(_WBIds()
    ..isHeader = true
    ..r.data = ['book#', 'path']);
  mx.tRows.addAll(rows);
  mx.save($ut.fileSystem.logParsed.absolute('bookIds.csv'));
}

class _WBIds extends $ut.RowTyped {
  _WBIds([r]) : super(r);
  set bookId(int v) => setInt(0, v);
  set path(v) => set(1, v);
  int get len => 2;
}
