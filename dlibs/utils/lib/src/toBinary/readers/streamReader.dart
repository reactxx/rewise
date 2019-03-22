import 'dart:typed_data';
import 'dart:io' as io;

import 'reader.dart';

class StreamReader extends Reader {
  // CONSTRUCTORS
  StreamReader(this._source);
  StreamReader.fromPath(String path) : _source = io.File(path).openSync();

  void use(void run(StreamReader rdr)) {
    try {
      run(this);
    } finally {
      close();
    }
  }

  // ABSTRACT
  int readByte({int pos}) => setPos(pos)._read(1)[0];

  List<int> readBytesLow(int len, {int pos}) => setPos(pos)._read(len);

  StreamReader setPos(int pos) =>
      pos == null ? this : (_source..setPositionSync(pos));

  ByteBuffer readToBuffer(int len, {int pos}) =>
      Uint8List.fromList(setPos(pos)._read(len)).buffer;

  // OTHERS
  io.RandomAccessFile _source;

  close() => _source.closeSync();

  List<int> _read(int len) => _source.readSync(len);

  int get length => _length == 0 ? _source.lengthSync() : _length;
  int _length;

  int get position => _source.positionSync();
  set position(int value) => _source.setPositionSync(value);
}
