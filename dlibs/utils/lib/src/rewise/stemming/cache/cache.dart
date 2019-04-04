import 'dart:collection';
import 'package:path/path.dart' as p;
import 'package:rw_utils/toBinary.dart' as bin;
import 'package:rw_utils/dom/stemming.dart' as stemm;
import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_utils/langs.dart' show Langs;

import 'cacheObjs.dart';

class WordProxy {
  WordProxy(this.id, this.group);
  final int id; //-1 => alias
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
  int ownLen; // words[0..ownLen-1] are words, which stemming produces stemms
}

class StemmCache {
  String fileName;
  String lang;
  // for ever word: return its ID and position of its stemm group in file
  HashMap<String, WordProxy> words;
  // for ever group: return its ID, position and unique key.
  HashMap<String, GroupProxy> groups;

  factory StemmCache.fromLang(String lang) {
    final fn = fileSystem.stemmCache.adjustExists('$lang.bin');
    StemmCache cache;
    bin.StreamReader.fromPath(fn).use((rdr) => cache = StemmCache(rdr));
    cache.fileName = fn;
    return cache;
  }

  StemmCache(bin.StreamReader rdr) {
    groups = HashMap<String, GroupProxy>();
    for (final grp in _readGroups(rdr)) groups[grp.key] = grp;
    // groups = HashMap<String, GroupProxy>.fromIterable(_readGroups(rdr),
    //     key: (h) => h.key, value: (h) => h);
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

  static Iterable<String> get stemmLangs =>
      Langs.meta.where((m) => m.hasStemming).map((m) => m.id);
  static Iterable<String> get existingCachesLangs => fileSystem.stemmCache
      .list(regExp: r'.*\.bin')
      .map((f) => p.withoutExtension(f));

  Iterable<GroupProxy> _readGroups(bin.StreamReader rdr) sync* {
    words = HashMap<String, WordProxy>();
    while (rdr.position < rdr.length) {
      final group = Group.fromReader(rdr);
      if (group.alias != null) {
        final myGroup = groups[group.key];
        assert(myGroup != null);
        assert(!words.containsKey(group.alias));
        words[group.alias] = WordProxy(-1, myGroup);
        continue;
      }
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

  static Iterable<Group> iterateGroups(String lang) sync* {
    final fn = fileSystem.stemmCache.absolute('$lang.bin');
    final rdr = bin.StreamReader.fromPath(fn);
    try {
      while (rdr.position < rdr.length) yield Group.fromReader(rdr);
    } finally {
      rdr.close();
    }
  }

  void importStemmResults(Iterable<stemm.Word> stRess, bin.StreamWriter wr) {
    // import stemming results
    for (final stRes in stRess) {
      final newGrp = Group.fromStemmResult(stRes);
      if (newGrp.key.isEmpty) continue;
      // not existing stemm group:
      if (!groups.containsKey(newGrp.key)) {
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
        newGrp.write(wr);
      }
      // stemm source is not in stemms
      if (stRes.source.isNotEmpty) {
        if (!words.containsKey(stRes.source)) {
          final aliasGrp = Group.fromAlias(newGrp, stRes.source);
          aliasGrp.write(wr);
        }
      }
    }
  }
}
