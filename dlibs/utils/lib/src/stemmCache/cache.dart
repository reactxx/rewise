import 'dart:math';
import 'dart:collection';
import 'dart:typed_data';
import 'dart:io' as io;
import 'package:rw_utils/utils.dart' as utils;
import 'package:rw_utils/toBinary.dart' as bin;
import 'groups.dart';
import 'words.dart';

enum _FileTypes {
  groups,
  words,
}

const fileLenSize = 3;

class StemmCache {
  String lang;
  int groupsCount; // number of groups in file
  // for ever word: return its ID and position of its stemm group in file
  HashMap<String, WordDisk> words;
  // hashTable[(MD5 hash) % hashTable.length] => [id, position, md5Hash, mpd5First, ...]
  List<Uint32List> groups;

  StemmCache(this.lang) {
    final groupFn = _getFileName(lang, _FileTypes.groups);
    final wordsFn = _getFileName(lang, _FileTypes.words);

    if (!io.File(groupFn).existsSync() || !io.File(wordsFn).existsSync()) {
      bin.StreamWriter.fromPath(groupFn)
          .use((wr) => wr.writeSizedInt(0, fileLenSize));
      bin.StreamWriter.fromPath(wordsFn)
          .use((wr) => wr.writeSizedInt(0, fileLenSize));
    }

    bin.StreamReader.fromPath(groupFn).use((rdr) {
      final headers = readGroups(rdr);
      buildHashTable(headers);
    });
    bin.StreamReader.fromPath(wordsFn).use((rdr) => words = readWords(rdr));
  }

  void buildHashTable(List<GroupHeader> headers, {int len}) {
    // hash table length is 1.5x num of groups, min 1800.
    if (len == null) len = (max(1000, headers.length) * 1.8).round();
    groups = List<Uint32List>(len);
    for (final hdr in headers) proxyToHashTable(hdr);
  }

  void importStemmResults(List<StemmResult> stRess) {
    // rebuild hash table?
    final headers = List<GroupHeader>.from(groups.where((g) => g != null));
    assert(headers.length == groupsCount);
    if (groups.length < (headers.length + stRess.length) * 1.3)
      buildHashTable(headers, len: headers.length + stRess.length);
    // import stemming results
    for (final stRes in stRess) {
      // new stemm group
      final newGrp = GroupDisk.fromStemmResult(stRes, groupsCount++);
      // use newGrp's md5Hash and mpd5First for finding existing grps
      final existingHeaders = GroupHeader.checkExistingHeaders(
          groups[getHeaderIdx(newGrp.header)], newGrp.header);
      // final check: via md5 (readed from disk)
      var actGroup = checkExistingMD5(null/*TODO*/, newGrp.md5, existingHeaders);
      if (actGroup != null) {
        // group already exists
      } else if (existingHeaders != null) {
        // OR hash conflict: add new group with the same hash
        // OR new group with new hash
        newGrp.write(null/*TODO*/);
        proxyToHashTable(actGroup = newGrp.header);
      }
      /*TODO*/ //use actGroup for WORD's dictionary
    }
  }

  int getHeaderIdx(GroupHeader header) => header.md5Hash % groups.length;

  String _getFileName(String lang, _FileTypes type) =>
      utils.fileSystem.stemmCache.absolute('$lang.$type.bin');

  void proxyToHashTable(GroupHeader header) {
    final proxy = header.proxy;
    final idx = getHeaderIdx(header);
    final item = groups[idx];
    groups[idx] =
        item == null ? proxy : Uint32List.fromList(item.followedBy(proxy));
  }
}

// result of stemming
class StemmResult {
  List<String> groupStemms;
  List<String> forWords; // words with the same stemm-group
}

bool eqMD5(List<int> md51, List<int> md52) {
  for (var i = 0; i < md51.length; i++) if (md51[i] != md52[i]) return false;
  return true;
}

GroupHeader checkExistingMD5(
    bin.StreamReader rdr, List<int> md5, List<GroupHeader> hdrs) {
  if (hdrs == null) return null;
  return hdrs.singleWhere((hdr) => eqMD5(md5, readMD5(rdr, hdr.position)),
      orElse: () => null);
}

List<int> readMD5(bin.StreamReader rdr, int position) =>
    rdr.readBytesLow(16, pos: position + GroupHeader.headerLen);
