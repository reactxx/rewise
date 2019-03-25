import 'dart:io' as io;
import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/langs.dart' show Langs;
import 'package:rw_utils/dom/stemming.dart' as stemm;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/toBinary.dart' as bin;

import 'cache/cache.dart';

Future toStemmCache() async {
  final stemmLangs =
      Set.from(Langs.meta.where((m) => m.hasStemming).map((m) => m.id));
  return Future.wait(
    stemmLangs.map((lang) => toStemmCacheLang(lang))
  );
}

Future toStemmCacheLang(String lang) async {
  final fn = fileSystem.stemmCache.adjustExists('$lang\\cache.bin');

  StemmCache cache;
  bin.StreamReader.fromPath(fn).use((rdr) => cache = StemmCache(rdr));

  for (var bookFn in fileSystem.parsedLang.list(regExp: r'msg$', from: lang)) {
    final book =
        toPars.ParsedBook.fromBuffer(fileSystem.parsedLang.readAsBytes(bookFn));
    final texts = Linq.distinct(book.facts.expand((f) => f.childs.expand((sf) {
          final txt = sf.breakText.isEmpty ? sf.text : sf.breakText;
          return _wordsTostemm(txt, sf.breaks);
        }))).toList();

    final req = stemm.Request()
      ..lang = book.lang
      ..words.addAll(texts);
    final bookStemms = await client.Stemming_Stemm(req);

    bin.StreamWriter.fromPath(fn, mode: io.FileMode.append)
        .use((wr) => cache.importStemmResults(bookStemms.words, wr));
  }

  return Future.value();
}

Iterable<String> _wordsTostemm(String text, List<int> breaks) sync* {
  if (breaks == null || breaks.length == 0) return;
  for (var i = 0; i < breaks.length; i += 2)
    yield text.substring(breaks[i], breaks[i] + breaks[i + 1]);
  // var lastPos = 0;
  // for (var i = 0; i < breaks.length; i += 2) {
  //   final pos = lastPos + breaks[i];
  //   final end = pos + breaks[i + 1];
  //   final t = text.substring(pos, end);
  //   yield t;
  //   lastPos = end;
  // }
}

main() async {
  //await toStemmCacheLang('cs-CZ');
  await toStemmCache();
}
