import 'dart:collection';
import 'dart:math';
import 'dart:typed_data';
import 'dart:io' as io;
import 'package:rw_utils/utils.dart' as utils;
import 'package:rw_utils/toBinary.dart' as bin;
import 'group.dart';

enum _FileTypes {
  groups,
  words,
}

class StemmCache {
  StemmCache(this.lang) {
    final groupFn = _getFileName(lang, _FileTypes.groups);
    final wordsFn = _getFileName(lang, _FileTypes.words);

    if (!io.File(groupFn).existsSync() || !io.File(wordsFn).existsSync()) {
      bin.StreamWriter.fromPath(groupFn)
          .use((wr) => wr.writeSizedInt(0, fileLenSize));
      bin.StreamWriter.fromPath(wordsFn)
          .use((wr) => wr.writeSizedInt(0, fileLenSize));
    }

    bin.StreamReader.fromPath(groupFn).use(readGroups);
    bin.StreamReader.fromPath(wordsFn).use(readWords);
  }

  String lang;
  int groupsCount; // number of groups in file
  // for ever word: return its ID and position of its stemm group in file
  HashMap<String, WordDisk> words;
  // hashTable[(MD5 hash) % hashTable.length] => [id, position, md5Hash, mpd5First, ...]
  List<Uint32List> groups;

  static const fileLenSize = 3;

  void readWords(bin.StreamReader rdr) {
    final length = rdr.readSizedInt(fileLenSize,
        pos: rdr.length - fileLenSize); // last 4 bytes is number of groups
    rdr.position = 0;
    words = HashMap<String, WordDisk>();
    for (var i = 0; i < length; i++) {
      final word = rdr.readString();
      final groupPos = rdr.readUInt32();
      words[word] = WordDisk(i, groupPos);
    }
    assert(rdr.position == rdr.length - fileLenSize);
  }

  void readGroups(bin.StreamReader rdr) {
    void proxyToHashTable(
        Uint32List proxy /* id, position, md5Hash, mpd5First */) {
      final idx = proxy[2] % groups.length;
      final item = groups[idx];
      groups[idx] =
          item == null ? proxy : Uint32List.fromList(item.followedBy(proxy));
    }

    // last 4 bytes is number of groups
    groupsCount = rdr.readSizedInt(fileLenSize, pos: rdr.length - fileLenSize);
    groups = List<Uint32List>((max(1000, groupsCount) * 1.5)
        .round()); // hash table has length 1.5x num of groups
    for (var groupId = 0; groupId < groupsCount; groupId++) {
      final header = GroupHeader.fromReader(rdr, groupId);
      rdr.position += header.numOfBytes;
      proxyToHashTable(header.proxy);
    }
    assert(rdr.position == rdr.length - fileLenSize);
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
    final item = groups[md5Hash % groups.length];
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

  static GroupDisk tryReadGroupDisk(int filePos, List<int> md5) {
    // null if md5 is different
    return null;
  }

  String _getFileName(String lang, _FileTypes type) =>
      utils.fileSystem.stemmCache.absolute('$lang.$type.bin');
}

class GroupProxy {
  GroupProxy(this.filePos, this.md5Hash, this.md5First);
  int md5Hash;
  int md5First;
  int filePos;
}

// in StreamReader: [numOfBytes, md5Hash, mpd5First]
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

class WordDisk {
  WordDisk(this.id, this.groupPos);
  final int id;
  final int groupPos;
}
