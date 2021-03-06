//import 'dart:typed_data';
import 'dart:io' as io;

import 'writer.dart';

class StreamWriter extends Writer {
  StreamWriter(this._source);
  StreamWriter.fromPath(String path, {io.FileMode mode = io.FileMode.writeOnly})
      : _source = io.File(path).openSync(mode: mode);

  use(void run(StreamWriter wr)) {
    try {
      run(this);
    } finally {
      close();
    }
  }

  // ABSTRACTS
  void writeByte(int byte) =>
      _source.writeByteSync(byte);

  void writeBytesLow(List<int> data) =>
      _source.writeFromSync(data);

  StreamWriter setPos(int pos) =>
      pos == null ? this : (_source..setPositionSync(pos));

  // void writeToBuffer(int len, void fillData(ByteData data), {int pos}) {
  //   final toWrite = Uint8List(len);
  //   fillData(ByteData.view(toWrite.buffer));
  //   writeBytesLow(toWrite, pos: pos);
  // }

  // OTHERS
  io.RandomAccessFile _source;

  close() => _source.closeSync();

  int get length => _source.lengthSync();
  int get position => _source.positionSync();
  set position(int value) => _source.setPositionSync(value);
}
