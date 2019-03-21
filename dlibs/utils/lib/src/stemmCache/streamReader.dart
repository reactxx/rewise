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

class StreamReader {
  io.RandomAccessFile _source;

  StreamReader(this._source);
  StreamReader.fromPath(String path) : _source = io.File(path).openSync();
  close() => _source.closeSync();

  List<int> _read(int len) => _source.readSync(len);
  StreamReader _setPos(int pos) =>
      pos == null ? this : (_source..setPositionSync(pos));

  ByteBuffer _getBuf(int len, {int pos}) =>
      Uint8List.fromList(_setPos(pos)._read(len)).buffer;

  int readByte({int pos}) => _setPos(pos)._read(1)[0];
  List<int> readBytes(int len, {int pos}) => _setPos(pos)._read(len);
  int readUInt32({int pos}) =>
      ByteData.view(_getBuf(4, pos: pos)).getUint32(0, _endian);
  List<int> readUInt32s(int length, {int pos}) {
    final bd = ByteData.view(_getBuf(length << 2, pos: pos));
    return List.from(
        Linq.range(0, length).map((i) => bd.getUint32(i << 2, _endian)));
  }

  List<int> readUInt16s(int length, {int pos}) {
    final bd = ByteData.view(_getBuf(length << 1, pos: pos));
    return List.from(
        Linq.range(0, length).map((i) => bd.getUint16(i << 1, _endian)));
  }

  String readEncodedString(Uint16List toCodeUnit, {int len, int pos}) {
    if (len == null) len = readByte(pos: pos);
    return String.fromCharCodes(readBytes(len).map((b) => toCodeUnit[b]));
  }

  int get length => _length == 0 ? _source.lengthSync() : _length;
  int _length;

  int get position => _source.positionSync();
  set position(int value) => _source.setPositionSync(value);
}
