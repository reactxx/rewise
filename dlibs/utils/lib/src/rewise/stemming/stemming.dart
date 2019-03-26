import 'dart:io' as io;
import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/langs.dart' show Langs;
import 'package:rw_utils/dom/stemming.dart' as stemm;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/rewise.dart' show BreakConverter;
import 'package:rw_utils/toBinary.dart' as bin;

import 'cache/cache.dart';

Future toStemmCache() async {
  final stemmLangs =
      Set.from(Langs.meta.where((m) => m.hasStemming).map((m) => m.id));
  return Future.wait(stemmLangs.map((lang) => toStemmCacheLang(lang)));
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
          return BreakConverter.getStemms(txt, sf.breaks)
              .where((w) => !cache.words.containsKey(w.toLowerCase()));
        }))).toList();

    // all used words stemmed => return
    if (texts.length == 0) return Future.value();

    final req = stemm.Request()
      ..lang = book.lang
      ..words.addAll(texts);
    final bookStemms = await client.Stemming_Stemm(req);

    bin.StreamWriter.fromPath(fn, mode: io.FileMode.append)
        .use((wr) => cache.importStemmResults(bookStemms.words, wr));
  }

  return Future.value();
}

main() async {
  //await toStemmCacheLang('cs-CZ');
  await toStemmCache();
}
