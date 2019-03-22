import 'dart:collection';
import 'dart:math';
import 'dart:typed_data';
import 'dart:io' as io;
import 'package:rw_utils/utils.dart' as utils;
import 'package:rw_utils/toBinary.dart' as bin;

class GroupHeader {
  GroupHeader.fromReader(bin.StreamReader rdr, int id, {int pos = -1}) {
    final oldPos = pos < 0 ? rdr.position : pos;
    final data = rdr.readUInt32s(3, pos: pos);
    numOfBytes = data[0];
    proxy = Uint32List.fromList(
        [id, oldPos].followedBy(data.skip(1) /*md5Hash, mpd5First*/));
  }
  int numOfBytes;
  Uint32List proxy; // id, position, md5Hash, mpd5First
}

class GroupDisk {
  GroupDisk.fromReader(bin.StreamReader rdr, int id, {int pos = -1}) {
    header = GroupHeader.fromReader(rdr, id, pos: pos);
    md5 = rdr.readSizedInts(2);
    words = rdr.readStrings();
  }
  GroupHeader header; // 3 bytes: numOfBytes, md5Hash, mpd5First
  List<int> md5; // 16 bytes MD5
  // 1 bit: isStemmSource, 7 bits: num of words.
  // word: 1 bit: isStemmSource, 7 bits: num of chars, byte* as chars
  List<String> words;
}
