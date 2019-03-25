//import 'dart:typed_data';
import 'dart:io' as io;

import 'reader.dart';
import 'streamWriter.dart';

class StreamFile {
  StreamFile(this._source) {
    rdr = StreamReader(_source);
    wr = StreamWriter(_source);
  }
  StreamFile.fromPath(String path, {io.FileMode mode = io.FileMode.write}) {
    _source = io.File(path).openSync(mode: mode);
    rdr = StreamReader(_source);
    wr = StreamWriter(_source);
  }
  io.RandomAccessFile _source;
  StreamReader rdr;
  StreamWriter wr;
}

class StreamReader extends Reader {
  // CONSTRUCTORS
  StreamReader(this._source);
  StreamReader.fromPath(String path, {io.FileMode mode = io.FileMode.read})
      : _source = io.File(path).openSync(mode: mode);

  void use(void run(StreamReader rdr)) {
    try {
      run(this);
    } finally {
      close();
    }
  }

  // ABSTRACT
  int readByte({int pos}) => setPos(pos)._read(1)[0];

  List<int> readBytesLow(int len) => _read(len);

  StreamReader setPos(int pos) =>
      pos == null ? this : (_source..setPositionSync(pos));

  // ByteBuffer readToBuffer(int len, {int pos}) =>
  //     Uint8List.fromList(setPos(pos)._read(len)).buffer;

  // OTHERS
  io.RandomAccessFile _source;

  close() => _source.closeSync();

  List<int> _read(int len) => _source.readSync(len);

  int get length => _length == null ? _source.lengthSync() : _length;
  int _length;

  int get position => _source.positionSync();
  set position(int value) => _source.setPositionSync(value);
}
