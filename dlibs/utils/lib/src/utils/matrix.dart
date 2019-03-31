import 'dart:io' as io;

abstract class RowTyped {
  RowTyped([this.r]) {
    if (r == null)
      r = Row.blank(len);
    else
      assert(r.data.length == len);
  }
  Row r;
  bool get isHeader => r.isHeader;
  set isHeader(v) => r.isHeader = v;
  int getInt(int idx) => int.parse(r.getData(idx));
  void setInt(int idx, int v) => r.setData(idx, v.toString());
  String get(int idx) => r.getData(idx);
  void set(int idx, String v) => r.setData(idx, v);

  int get len;
}

typedef CreateRow<T> = T Function([Row r]);

class MatrixTyped<T extends RowTyped> extends Matrix {
  MatrixTyped(this._createRow);
  MatrixTyped.fromFile(String path, this._createRow) {
    final lines = io.File(path).readAsLinesSync();
    var lineCount = 0;
    tRows.addAll(lines.map((l) => _createRow(Row(l, lineCount++ == 0)..v = 1)));
  }
  CreateRow<T> _createRow;
  T newRow() {
    final res = _createRow(null);
    tRows.add(res);
    return res;
  }

  save(String path) => Matrix.writeRows(path, tRows.map((r) => r.r));
  Row get header => tRows[0].r;
  final tRows = List<T>();
}

class Matrix {
  Matrix() : rows = List<Row>();
  Matrix.fromFile(String path)
      : rows = io.File(path).readAsLinesSync().map((l) => Row(l)).toList();
  Matrix.fromData(Iterable<List<String>> data)
      : rows = data.map((d) => Row.fromData(d)).toList();

  save(String path) => writeRows(path, rows);

  static writeRows(String path, Iterable<Row> rows) {
    final wr = io.File(path).openSync(mode: io.FileMode.writeOnly);
    try {
      for (final rw in rows) writeRow(wr, rw);
    } finally {
      wr.closeSync();
    }
  }

  static writeRow(io.RandomAccessFile wr, Row rw) {
    wr.writeStringSync(rw.isHeader ? '[${rw.str}]' : rw.str);
    wr.writeStringSync('\r\n');
  }

  List<Row> rows;
}

class Row {
  Row([this._str = '', bool checkHeader]) {
    if (checkHeader != true) return;
    isHeader = _str.startsWith('[') && _str.endsWith(']');
  }
  Row.fromData(this._data) : _v = 1;
  Row.blank(int len) : this.fromData(List<String>()..length = len);
  bool isHeader = false;
  // v0
  String get str => (this..v = 0)._str;
  String _str;
  // v1
  List<String> get data => (this..v = 1)._data;
  set data(d) => this
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
          _str = _data.join(';');
          break;
        case 2:
          _str = '$_lang;$_raw';
          break;
        case 3:
          _str = '$_lang;${_langData.join(';')}';
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
        _data = _str.split(';');
        _raw = null;
        break;
      case 2:
        final idx = _str.indexOf(';');
        _lang = _str.substring(0, idx);
        _raw = _str.substring(idx + 1);
        _str = null;
        break;
      case 3:
        v = 2;
        _langData = _raw.split(';');
        _raw = null;
        break;
    }
    _v = nv;
  }

  int _v = 0;
}
