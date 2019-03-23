import 'dart:collection';
import 'dart:io' as io;
import 'package:rw_utils/utils.dart' as utils;
import 'package:rw_utils/toBinary.dart' as bin;
import 'cacheObjs.dart';

enum _FileTypes {
  groups,
}

class WordProxy {
  WordProxy(this.id, this.group);
  final int id;
  final GroupProxy group;
}

class GroupProxy {
  GroupProxy(Group grp)
      : id = grp.id,
        pos = grp.position,
        key = grp.key;
  final int id;
  final int pos;
  final String key;
}

// result of stemming
class StemmResult {
  List<String> words;
  int ownLen; // words[0..ownLen-1] are words, which stemming produces words
}

class StemmCache {
  String lang;
  int groupsCount; // number of groups in file
  // for ever word: return its ID and position of its stemm group in file
  HashMap<String, WordProxy> words;
  // for ever group: return its ID, position and unique key.
  HashMap<String, GroupProxy> groups;

  StemmCache(this.lang) {
    final groupFn = _getFileName(lang, _FileTypes.groups);
    bin.StreamReader.fromPath(groupFn, mode: io.FileMode.append).use((rdr) =>
        groups = HashMap<String, GroupProxy>.fromIterable(_readGroups(rdr),
            key: (h) => h.key, value: (h) => h));
  }

  Iterable<GroupProxy> _readGroups(bin.StreamReader rdr) sync* {
    var groupsCount = 0;
    words = HashMap<String, WordProxy>();
    while (rdr.position < rdr.length) {
      final group = Group.fromReader(rdr);
      assert(group.id == groupsCount++);
      final proxy = GroupProxy(group);
      for (final w in group.ownWords) {
        assert(!words.containsKey(w.word)); // unique word
        words[w.word] = WordProxy(w.id, proxy);
      }
      yield proxy;
    }
    //check word IDS
    assert((() {
      final ids = HashSet<int>();
      for (final w in words.values) ids.add(w.id);
      // continuous ID values
      for (var i = 0; i < ids.length; i++) if (!ids.contains(i)) return false;
      // no ID duplicity
      return ids.length == words.length;
    })());
  }

  void importStemmResults(List<StemmResult> stRess) {
    // import stemming results
    for (final stRes in stRess) {
      final newGrp = Group.fromStemmResult(stRes);
      // existing stemm group:
      if (groups.containsKey(newGrp.key)) continue;
      // new stemm group:
      final proxy = GroupProxy(newGrp);
      groups[newGrp.key] = proxy;
      // assign group ID
      newGrp.id = groupsCount++;
      // fill words
      for (final w in newGrp.ownWords) {
        assert(!words.containsKey(w.word));
        w.id = words.length;
        words[w.word] = WordProxy(w.id, proxy);
      }
      newGrp.write(null /*TODO*/); // fill POSITION
    }
  }

  String _getFileName(String lang, _FileTypes type) =>
      utils.fileSystem.stemmCache.absolute('$lang.$type.bin');
}
