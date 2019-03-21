import 'dart:collection';
import 'dart:convert';
import 'dart:math';
import 'dart:typed_data';
import 'dart:io' as io;

import 'package:test/test.dart' as test;

import 'package:grpc/grpc.dart' as grpc;
import 'package:protobuf/protobuf.dart' as pb;
import 'package:recase/recase.dart' show ReCase;

import 'package:rw_utils/utils.dart' as utils;
import 'package:rw_utils/rewise.dart' as rewise;
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/client.dart' as client;

final _endian = Endian.big;

class StreamWriter {
  io.RandomAccessFile _source;

  StreamWriter(this._source);
  StreamWriter.fromPath(String path)
      : _source = io.File(path).openSync(mode: io.FileMode.writeOnly);
  close() => _source.closeSync();

  void _write(int len, void fillData(ByteData data), int pos) {
    final toWrite = Uint8List(4);
    fillData(ByteData.view(toWrite.buffer));
    _setPos(pos)._source.writeFromSync(toWrite);
  }

  StreamWriter _setPos(int pos) =>
      pos >= 0 ? (_source..setPositionSync(pos)) : this;

  void _writeBuf(Uint8List data, pos) =>
      _setPos(pos)._source.writeFromSync(data);

  void writeByte(int byte, {int pos = -1}) =>
      _setPos(pos)._source.writeByteSync(byte);
  void writeBytes(Uint8List data, {int pos = -1}) => _writeBuf(data, pos);

  void writeUInt32(int value, {int pos = -1}) =>
      _write(4, (bd) => bd.setUint32(0, value, _endian), pos);
  void writeUInt32s(List<int> data, {int pos = -1}) =>
      _write(data.length << 2, (bd) {
        for (final i in data) bd.setUint32(i << 2, i, _endian);
      }, pos);
  void writeUInt16s(List<int> data, {int pos = -1}) =>
      _write(data.length << 1, (bd) {
        for (final i in data) bd.setUint16(i << 1, i, _endian);
      }, pos);

  void writeDecodedString(String str, Map<int, int> fromCodeUnit,
      {bool writeLen = true, int pos = -1}) {
    if (str==null || str.isEmpty) return;
    if (writeLen) writeByte(str.length, pos: pos);
    final list = str.codeUnits.map((c) => fromCodeUnit[c]);
    writeBytes(Uint8List.fromList(list));
  }

  int get position => _source.positionSync();
  set position(int value) => _source.setPositionSync(value);
}
