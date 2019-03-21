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

  _read(int len, {bool setPos = true}) {
    final res = _source.readSync(len);
    if (setPos) position += len;
    return res;
  }
  _setPos(int pos) =>
      pos >= 0 ? (_source..setPositionSync(pos)) : _source;

  ByteBuffer _getBuf(int len, {int pos = -1, bool setPos = true}) =>
      Uint8List.fromList(_setPos(pos).readSync(len, setPos = setPos)).buffer;

  int readByte({int pos = -1, bool setPos = true}) => _setPos(pos).readSync(1, setPos = setPos)[0];
  List<int> readBytes(int len, {int pos = -1, bool setPos = true}) => _setPos(pos).readSync(len, setPos = setPos);
  int readUInt32({int pos = -1, bool setPos = true}) =>
      ByteData.view(_getBuf(4, pos: pos, setPos: setPos)).getUint32(0, _endian);
  List<int> readUInt32s(int length, {int pos = -1, bool setPos = true}) {
    final bd = ByteData.view(_getBuf(length << 2, pos: pos, setPos: setPos));
    return List.from(
        Linq.range(0, length).map((i) => bd.getUint32(i << 2, _endian)));
  }

  List<int> readUInt16s(int length, {int pos = -1, bool setPos = true}) {
    final bd = ByteData.view(_getBuf(length << 1, pos: pos, setPos: setPos));
    return List.from(
        Linq.range(0, length).map((i) => bd.getUint16(i << 1, _endian)));
  }

  String readEncodedString(Uint16List toCodeUnit,
      {int len = -1, int pos = -1, bool setPos = true}) {
    if (len < 0) {
      len = readByte(pos: pos);
      position += 1;
    }
    return String.fromCharCodes(readBytes(len, setPos: setPos).map((b) => toCodeUnit[b]));
  }

  int get length => _length < 0 ? _source.lengthSync() : _length;
  int _length = -1;

  int get position => _source.positionSync();
  set position(int value) => _source.setPositionSync(value);
}
