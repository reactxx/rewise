import 'dart:io' as io;
import 'dart:convert' as conv;
import 'package:rw_low/code.dart' show adjustFileDir;

const _defaultDelim = '\t';
//const _defaultDelim = ';';

class Matrix {
  Matrix({List<String> header}) : rows = List<Row>() {
    if (header != null) rows.add(Row.fromData(this, header));
  }
  Matrix.fromFile(String path, {this.delim = _defaultDelim}) {
     rows = io.File(path).readAsLinesSync().map((l) => Row(this, l)).toList();
  }
  Matrix.fromData(Iterable<List<String>> data,
      {List<String> header, int sortColumn})
      : rows = List<Row>() {
    if (header != null) rows.add(Row.fromData(this, header));
    rows.addAll(data.map((d) => Row.fromData(this, d)));
    if (sortColumn != null) {
      if (header != null) header[0] = '\u{0000}' + header[0];
      sort(sortColumn);
      if (header != null) header[0] = header[0].substring(1);
    }
  }

  List<Row> rows;
  var delim = _defaultDelim;

  Iterable<int> checkRowLen() sync* {
    final len = rows[0].data.length;
    for (var i = 0; i < rows.length; i++)
      if (rows[i].data.length != len) 
        yield i;
  }

  sort(int sortColumn) => rows.sort((a, b) =>
      (a..v = 1)._data[sortColumn].compareTo((b..v = 1)._data[sortColumn]));

  save(String path, {int noSaveRowLimit = 0}) {
    if (rows.length < noSaveRowLimit) return;
    final sb = StringBuffer();
    sb.writeCharCode(conv.unicodeBomCharacterRune);
    for (final rw in rows) writeRow(sb, rw);
    adjustFileDir(path);
    io.File(path).writeAsStringSync(sb.toString());
  }

  static writeRow(StringBuffer sb, Row rw) =>
      sb.writeln(rw.isHeader ? '[${rw.str}]' : rw.str);

  add(List<String> data) => rows.add(Row(this)..data = data);
  addAll(Iterable<List<String>> data) =>
      rows.addAll(data.map((d) => Row(this)..data = d));
}

class Row {
  Row(Matrix owner, [this._str = '', bool checkHeader]): _delim = owner.delim {
    if (checkHeader != true) return;
    isHeader = _str.startsWith('[') && _str.endsWith(']');
  }
  Row.fromData(Matrix owner, this._data) : _delim = owner.delim, _v = 1;
  Row.blank(Matrix owner, int len) : this.fromData(owner, List<String>()..length = len);

  String _delim;
  bool isHeader = false;
  // v0
  String get str => (this..v = 0)._str;
  String _str;
  // v1
  List<String> get data => (this..v = 1)._data;
  set data(List<String> d) => this
    .._reset(1)
    .._data = d;
  setData(int idx, String v) => (this..v = 1)._data[idx] = v;
  String getData(int idx) => (this..v = 1)._data[idx];
  int get dataLength => (this..v = 1)._data.length;
  List<String> _data;
  // v2, v3
  //String lang;
  String get lang => (this..v = _v == 3 ? 3 : 2)._lang;
  set lang(String v) => (this..v = _v == 3 ? 3 : 2)._lang = v;
  String _lang;
  // v2
  //String raw;
  String get raw => (this..v = 2)._raw;
  set raw(String v) => (this..v = 2)._raw = v;
  String _raw;
  // v3
  List<String> get langData => (this..v = 3)._langData;
  set langData(List<String> d) => this
    .._reset(3)
    .._langData = d;
  String getLangData(int idx) => (this..v = 3)._langData[idx];
  setLangData(int idx, String v) => (this..v = 3)._langData[idx] = v;
  int get langDataLength => (this..v = 3)._langData.length;
  List<String> _langData;

  void _reset([int vOnly]) {
    if (vOnly == null)
      switch (_v) {
        case 0:
          break;
        case 1:
          _str = _data.join(_delim);
          break;
        case 2:
          _str = '$_lang;$_raw';
          break;
        case 3:
          _str = '$_lang;${_langData.join(_delim)}';
          break;
      }
    _data = null;
    _lang = null;
    _raw = null;
    _v = vOnly ?? 0;
  }

  // v
  set v(int nv) {
    if (nv == _v) return;
    if (nv < _v || (_v > 1 && nv == 1)) _reset();
    switch (nv) {
      case 1:
        _data = _str.split(_delim);
        _raw = null;
        break;
      case 2:
        final idx = _str.indexOf(_delim);
        _lang = _str.substring(0, idx);
        _raw = _str.substring(idx + 1);
        _str = null;
        break;
      case 3:
        v = 2;
        _langData = _raw.split(_delim);
        _raw = null;
        break;
    }
    _v = nv;
  }

  int _v = 0;
}
