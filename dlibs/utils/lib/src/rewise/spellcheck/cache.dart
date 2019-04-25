import 'dart:collection';
import 'dart:io' as io;
import 'package:tuple/tuple.dart';
import 'package:path/path.dart' as p;
import 'package:rw_utils/toBinary.dart' as bin;
import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_utils/langs.dart' show Langs;

class SCCache {
  final words = HashMap<String, bool>(); // ok x wrong
  String lang;

  String fileName(String lang) =>
      fileSystem.spellCheckCache.absolute('$lang.bin');

  factory SCCache.fromLang(String lang) {
    if (Langs.nameToMeta[lang].wordSpellCheckLcid==0) return null;
    final fn = fileSystem.spellCheckCache.adjustExists('$lang.bin');
    SCCache cache;
    bin.StreamReader.fromPath(fn).use((rdr) => cache = SCCache._(lang, rdr));
    return cache;
  }

  factory SCCache.fromPath(String relPath) {
    SCCache cache;
    bin.StreamReader.fromPath(fileSystem.spellCheckCache.absolute(relPath)).use(
        (rdr) => cache = SCCache._(p.basenameWithoutExtension(relPath), rdr));
    return cache;
  }

  SCCache._(this.lang, bin.StreamReader rdr) {
    while (rdr.position < rdr.length) {
      final b = rdr.readByte();
      words[rdr.readString()] = b != 0;
    }
  }

  Iterable<String> toCheck(Iterable<String> ws) =>
      HashSet<String>.from(ws.where((w) => !words.containsKey(w)));

  Iterable<Tuple2<String, bool>> toCheckDump(Iterable<String> ws) =>
      ws.map((w) => Tuple2(w, words[w]));

  addWords(List<String> ws, List<int> wrongIdxs) {
    bin.StreamWriter.fromPath(fileName(lang), mode: io.FileMode.append)
        .use((wr) {
      final wrongs = HashSet<int>.from(wrongIdxs);
      for (var i = 0; i < ws.length; i++) {
        final w = ws[i];
        assert(!words.containsKey(w));
        final ok = !wrongs.contains(i);
        words[w] = ok;
        wr.writeByte(ok ? 1 : 0);
        wr.writeString(w);
      }
    });
  }

  Iterable<String> correctWords() => words.keys.where((k) => words[k]);
  Iterable<String> wrongWords() => words.keys.where((k) => !words[k]);

  static SCCache fromMemory(String lang) =>
    _fromMemory.putIfAbsent(lang, () => SCCache.fromLang(lang));
  static final _fromMemory = Map<String, SCCache>();
}

Iterable<SCCache> caches() => fileSystem.spellCheckCache
    .list(regExp: r'\.bin$')
    .map((fn) => SCCache.fromPath(fn));

Iterable<String> cacheLangs() => fileSystem.spellCheckCache
    .list(regExp: r'\.bin$')
    .map((fn) => p.basenameWithoutExtension(fn));
