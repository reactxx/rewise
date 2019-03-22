import 'dart:typed_data';
import 'dart:io' as io;

class StreamWriter {
  StreamWriter(this._source);
  StreamWriter.fromPath(String path)
      : _source = io.File(path).openSync(mode: io.FileMode.writeOnly);

  // ABSTRACTS
  void writeByte(int byte, {int pos}) =>
      setPos(pos)._source.writeByteSync(byte);

  void writeBytes(Uint8List data, {int pos}) =>
      setPos(pos)._source.writeFromSync(data);

  StreamWriter setPos(int pos) =>
      pos == null ? this : (_source..setPositionSync(pos));

  void writeToBuffer(int len, void fillData(ByteData data), {int pos}) {
    final toWrite = Uint8List(len);
    fillData(ByteData.view(toWrite.buffer));
    writeBytes(toWrite, pos: pos);
  }

  // OTHERS
  io.RandomAccessFile _source;

  close() => _source.closeSync();

  int get position => _source.positionSync();
  set position(int value) => _source.setPositionSync(value);
}
