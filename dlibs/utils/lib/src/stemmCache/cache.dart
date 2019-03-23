import 'dart:math';
import 'dart:collection';
//import 'dart:typed_data';
import 'dart:io' as io;
import 'package:rw_utils/utils.dart' as utils;
import 'package:rw_utils/toBinary.dart' as bin;
import 'groups.dart';

enum _FileTypes {
  groups,
}

class Word {
  Word(this.id, this.groupPos);
  final int id;
  final int groupPos;
}

class StemmCache {
  String lang;
  int groupsCount; // number of groups in file
  // for ever word: return its ID and position of its stemm group in file
  HashMap<String, Word> words;
  // hashTable[(MD5 hash) % hashTable.length] => [id, position, md5Hash, mpd5First, ...]
  List<List<GroupHeader>> groups;

  StemmCache(this.lang) {
    final groupFn = _getFileName(lang, _FileTypes.groups);
    if (!io.File(groupFn).existsSync())
      bin.StreamWriter.fromPath(groupFn).use((wr) => {});
    bin.StreamReader.fromPath(groupFn).use((rdr) {
      final headers = _readGroups(rdr);
      _buildHashTable(headers);
    });
  }

  List<GroupHeader> _readGroups(bin.StreamReader rdr) {
    var groupId = 0;
    final headers = List<GroupHeader>();
    words = HashMap<String, Word>();
    while (rdr.position < rdr.length) {
      final group = GroupDisk.fromReader(rdr);
      assert(group.header.id == groupId++);
      headers.add(group.header);
      for (final w in group.ownWords)
        words[w.word] = Word(w.id, group.header.position);
    }
    return headers;
  }

  void _buildHashTable(List<GroupHeader> headers, {int len}) {
    // hash table length is 1.8x num of groups, min 1800.
    if (len == null) len = (max(1000, headers.length) * 1.8).round();
    groups = List<List<GroupHeader>>(len);
    for (final hdr in headers) _headerToHashTable(hdr);
  }

  void _headerToHashTable(GroupHeader header) {
    //final proxy = header.proxy;
    final idx = getHeaderIdx(header);
    final item = groups[idx];
    groups[idx] = (item ?? List<GroupHeader>())..add(header);
  }

  void importStemmResults(List<StemmResult> stRess) {
    // rebuild hash table?
    if (groups.length < (groupsCount + stRess.length) * 1.3) {
      final headers = List<GroupHeader>.from(groups.where((g) => g != null));
      assert(headers.length == groupsCount);
      _buildHashTable(headers, len: groupsCount + stRess.length);
    }
    // import stemming results
    for (final stRes in stRess) {
      // new stemm group
      final newGrp = GroupDisk.fromStemmResult(stRes);
      // use newGrp's key for finding existing grp
      final existing = groups[getHeaderIdx(newGrp.header)];
      final found = existing == null
          ? null
          : existing.singleWhere((h) => h.key == newGrp.header.key,
              orElse: () => null);
      if (found != null) continue;
      newGrp.header.id = groupsCount++;
      newGrp.write(null /*TODO*/);
      _headerToHashTable(newGrp.header);
      // fill words
      for (final w in newGrp.ownWords) {
        assert(words[w.word] == null);
        w.id = words.length;
        words[w.word] = Word(w.id, newGrp.header.position);
      }
    }
  }

  int getHeaderIdx(GroupHeader header) => header.key.hashCode % groups.length;

  String _getFileName(String lang, _FileTypes type) =>
      utils.fileSystem.stemmCache.absolute('$lang.$type.bin');
}

// result of stemming
class StemmResult {
  List<String> words;
  int ownLen; // words[0..ownLen-1] are words, which stemming produces words
  //List<String> forWords; //wordse words with the same stemm-group
}

// bool eqMD5(List<int> md51, List<int> md52) {
//   for (var i = 0; i < md51.length; i++) if (md51[i] != md52[i]) return false;
//   return true;
// }

// GroupHeader checkExistingMD5(
//     bin.StreamReader rdr, List<int> md5, List<GroupHeader> hdrs) {
//   if (hdrs == null) return null;
//   return hdrs.singleWhere((hdr) => eqMD5(md5, readMD5(rdr, hdr.position)),
//       orElse: () => null);
// }

// List<int> readMD5(bin.StreamReader rdr, int position) {
//   rdr.setPos(position + GroupHeader.headerLen);
//   return rdr.readBytesLow(16);
// }
