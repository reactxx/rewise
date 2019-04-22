import 'dart:collection';
import 'dart:io' as io;
import 'package:path/path.dart' as p;
import 'package:rw_utils/toBinary.dart' as bin;
import 'package:rw_utils/utils.dart' show fileSystem;

class SCCache {
  final words = HashMap<String, bool>(); // ok x wrong
  String lang;

  String fileName(String lang) =>
      fileSystem.spellCheckCache.absolute('$lang.bin');

  factory SCCache.fromLang(String lang) {
    final fn = fileSystem.spellCheckCache.adjustExists('$lang.bin');
    SCCache cache;
    bin.StreamReader.fromPath(fn).use((rdr) => cache = SCCache(lang, rdr));
    return cache;
  }

  factory SCCache.fromPath(String relPath) {
    SCCache cache;
    bin.StreamReader.fromPath(fileSystem.spellCheckCache.absolute(relPath)).use(
        (rdr) => cache = SCCache(p.basenameWithoutExtension(relPath), rdr));
    return cache;
  }

  SCCache(this.lang, bin.StreamReader rdr) {
    while (rdr.position < rdr.length) {
      final b = rdr.readByte();
      words[rdr.readString()] = b != 0;
    }
  }

  Iterable<String> toCheck(Iterable<String> ws) =>
      ws.where((w) => !words.containsKey(w));

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
}

Iterable<SCCache> caches() =>
    fileSystem.spellCheckCache.list(regExp: r'\.bin$').map((fn) => SCCache.fromPath(fn));
