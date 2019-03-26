import 'dart:collection';
import 'package:rw_utils/toBinary.dart' as bin;
import 'package:rw_utils/dom/stemming.dart' as stemm;

import 'cacheObjs.dart';

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
  List<String> stemms;
  int ownLen; // words[0..ownLen-1] are words, which stemming produces words
}

class StemmCache {
  String lang;
  // for ever word: return its ID and position of its stemm group in file
  HashMap<String, WordProxy> words;
  // for ever group: return its ID, position and unique key.
  HashMap<String, GroupProxy> groups;

  StemmCache(bin.StreamReader rdr) {
    groups = HashMap<String, GroupProxy>.fromIterable(_readGroups(rdr),
        key: (h) => h.key, value: (h) => h);
    //check word and group IDS
    assert((() {
      // words
      var ids = HashSet<int>();
      for (final w in words.values.where((w) => w != null))
        if (!ids.add(w.id)) throw Exception();
      // groups
      ids = HashSet<int>();
      for (final w in groups.values) if (!ids.add(w.id)) throw Exception();
      // continuous ID values
      for (var i = 0; i < ids.length; i++)
        if (!ids.contains(i)) throw Exception();
      return true;
    })());
  }

  Iterable<GroupProxy> _readGroups(bin.StreamReader rdr) sync* {
    words = HashMap<String, WordProxy>();
    while (rdr.position < rdr.length) {
      final group = Group.fromReader(rdr);
      //assert(group.id == groups.length);
      final proxy = GroupProxy(group);
      if (group.ownWords == null) /* no stemms */ {
        assert(!words.containsKey(group.key));
        words[group.key] = null;
      } else
        for (final w in group.ownWords) {
          assert(!words.containsKey(w.word)); // unique word
          words[w.word] = WordProxy(w.id, proxy);
        }
      yield proxy;
    }
  }

  void importStemmResults(Iterable<stemm.Word> stRess, bin.StreamWriter wr) {
    // import stemming results
    for (final stRes in stRess) {
      final newGrp = Group.fromStemmResult(stRes);
      if (newGrp.key.isEmpty) continue;
      // existing stemm group:
      if (groups.containsKey(newGrp.key)) continue;
      // new stemm group:
      final proxy = GroupProxy(newGrp);
      // assign group ID
      newGrp.id = groups.length;
      groups[newGrp.key] = proxy;
      // fill words
      if (newGrp.ownWords == null) /* no stemms */ {
        assert(!words.containsKey(newGrp.key));
        words[newGrp.key] = null;
      } else
        for (final w in newGrp.ownWords) {
          assert(!words.containsKey(w.word));
          w.id = words.length;
          words[w.word] = WordProxy(w.id, proxy);
        }
      newGrp.write(wr /*TODO*/); // fill POSITION
    }
  }
}
