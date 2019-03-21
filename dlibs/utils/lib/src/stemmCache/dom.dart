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

import 'streamReader.dart' show StreamReader;

class StemmCache {
  int length; // number of groups in file
  FileHeader header; //
  // returns position of group in file
  HashMap<String, WordDisk> wordsDisk;
  // hashTable[(MD5 hash) % hashTable.length] => [id, position, md5Hash, mpd5First, ...]
  List<Uint32List> hashTable;

  StemmCache._();
  factory StemmCache.fromDisk(io.RandomAccessFile source) {
    final rdr = StreamReader(source);
    try {
      var res = StemmCache._();
      final length = rdr.readUInt32(
          pos: rdr.length - 4,
          setPos: false); // last 4 bytes is number of groups
      res.header = FileHeader.fromReader(rdr);
      res.hashTable = List<Uint32List>((rdr.length * 1.5)
          .round()); // hash table has length 1.5x num of groups
      for (var groupId = 0; groupId < length; groupId++) {
        final header = GroupHeader.fromReader(rdr, groupId, setPos: false);
        rdr.position += header.numOfBytes;
        _insertProxyToHashTable(res.hashTable, header.proxy);
      }
      assert(rdr.position == rdr.length - 4);
      return res;
    } finally {
      rdr.close();
    }
  }

  static void _insertProxyToHashTable(List<Uint32List> hashTable,
      Uint32List proxy /* id, position, md5Hash, mpd5First */) {
    final idx = proxy[2] % hashTable.length;
    final item = hashTable[idx];
    hashTable[idx] =
        item == null ? proxy : Uint32List.fromList(item.followedBy(proxy));
  }

  // fill group.GroupHeader, 3 scenary exist:
  // - new group, to in hashTable
  // - new group, add to existing hashTable item
  // - existing group
  GroupDisk adjustGroup(GroupDisk group /* */) {
    final md5 = group.md5;
    // index in hashTable
    var md5Hash = 0;
    for (final b in md5) md5Hash |= b;
    final mpd5First = md5[0];
    // array of [id, position, md5Hash, mpd5First]
    final item = hashTable[md5Hash % hashTable.length];
    if (item == null) {
      //new group, no collision
    } else {
      //
      for (var i = 0; i < item.length; i += 4) {
        if (item[i + 2] == md5Hash && item[i + 3] == mpd5First) {
          final res = tryReadGroupDisk(item[i + 1], md5);
          if (res != null) return res;
        }
      }
      // add new collision group
    }
    return group;
  }

  void addGroup(GroupDisk group) {}

  static GroupDisk tryReadGroupDisk(int filePos, Uint32List md5) {
    // null if md5 is different
  }
}

class GroupProxy {
  GroupProxy(this.filePos, this.md5Hash, this.md5First);
  int md5Hash;
  int md5First;
  int filePos;
}

// in StreamReader: [numOfBytes, md5Hash, mpd5First]
class GroupHeader {
  GroupHeader.fromReader(StreamReader rdr, int id,
      {int pos = -1, bool setPos = true}) {
    final data = rdr.readUInt32s(3, pos: pos, setPos: setPos);
    numOfBytes = data[0];
    proxy = Uint32List.fromList(
        [id, rdr.position].followedBy(data.skip(1) /*md5Hash, mpd5First*/));
  }
  int numOfBytes;
  Uint32List proxy; // id, position, md5Hash, mpd5First
}

class FileHeader {
  FileHeader.fromReader(StreamReader rdr) {
    toCodeUnit = rdr.readUInt16s(256, pos: 0); // byte to uint16
    fromCodeUnit = Map();
    for (var i = 0; i < toCodeUnit.length; i++) {
      final key = toCodeUnit[i];
      if (key == 0) break;
      fromCodeUnit[key] = i;
    }
    numOfBytes = 256 << 1; // 256*2
  }
  int codeToByte(int code) => fromCodeUnit.putIfAbsent(code, () {
        if (fromCodeUnit.length >= 256)
          throw Exception('Too many alphabet codes');
        toCodeUnit[fromCodeUnit.length] = code;
        return fromCodeUnit.length;
      });
  int byteToCode(int byte) => toCodeUnit[byte];
  int numOfBytes;
  List<int> toCodeUnit; // byte to uint16
  Map<int, int> fromCodeUnit; // uint16 to byte
}

class GroupDisk {
  GroupDisk.fromReader(StreamReader rdr, int id, Uint16List toCodeUnit,
      {int pos = -1}) {
    header = GroupHeader.fromReader(rdr, id, pos: pos);
    md5 = rdr.readUInt32s(16);
    final wordsCount = rdr.readByte();
    words = List<GroupDiskWord>(wordsCount);
    for (var i = 0; i < wordsCount; i++)
      words[i] = GroupDiskWord.fromReader(rdr, toCodeUnit);
  }
  GroupHeader header; // 3 bytes: numOfBytes, md5Hash, mpd5First
  Uint32List md5; // 16 bytes MD5
  // 1 bit: isStemmSource, 7 bits: num of words.
  // word: 1 bit: isStemmSource, 7 bits: num of chars, byte* as chars
  List<GroupDiskWord> words;
}

class GroupDiskWord {
  GroupDiskWord.fromReader(StreamReader rdr, Uint16List toCodeUnit) {
    var flag = rdr.readByte();
    isStemmSource = (flag & 0x1) != 0;
    word = rdr.readEncodedString(toCodeUnit, len: flag >> 1);
  }
  String word;
  bool isStemmSource; // this group is result of this.word's stemming
}

class WordDisk {
  WordDisk(this.id, this.groupPos);
  final int id;
  final int groupPos;

  static HashMap<String, WordDisk> fromReader(
      StreamReader rdr, Uint16List toCodeUnit) {
    final length =
        rdr.readUInt32(pos: rdr.length - 4); // last 4 bytes is number of groups
    rdr.position = 0;
    final res = HashMap<String, WordDisk>();
    for (var i = 0; i < length; i++) {
      final word = rdr.readEncodedString(toCodeUnit);
      final groupPos = rdr.readUInt32();
      res[word] = WordDisk(i, groupPos);
    }
    assert(rdr.position==rdr.length-4);
  }
}
