import 'dart:collection';
import 'package:rw_utils/rewise.dart' as rew;
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;
import 'package:rw_utils/dom/to_parsed.dart' as toPars;

const maxFactCount = 100000;

Future<rew.ParseBookResult> wordBreaking(rew.ParseBookResult parsed) async {
  // word breaking
  final futures = parsed.book.books.map((book) async {
    final facts = forBreaking(book)
        .map((ch) => ch.breakText.isNotEmpty ? ch.breakText : ch.text)
        .toList();

    final factl = facts.length;
    if (factl > maxFactCount) {
      final subBooks = List<wbreak.Response>();
      // split books to smaller subbooks
      final chunkSize = factl ~/ (factl ~/ maxFactCount + 1) + 1;
      var pos = 0;
      while (pos < factl) {
        final req = wbreak.Request()
          ..lang = book.lang
          ..facts.addAll(facts.skip(pos).take(chunkSize));
        pos += chunkSize;
        final resp = await client.WordBreaking_Run(req);
        subBooks.add(resp);
      }
      // join subbooks
      final resp = wbreak.Response();
      resp.facts.addAll(subBooks.expand((b) => b.facts));
      return Future.value(resp);
    } else {
      final req = wbreak.Request()
        ..lang = book.lang
        ..facts.addAll(facts);
      return client.WordBreaking_Run(req);
    }
  });

  // wait for result
  final booksBreaks = await Future.wait(futures);

  // merge with source
  for (var pair in Linq.zip(parsed.book.books, booksBreaks)) {
    toPars.ParsedBook book = pair.item1;
    wbreak.Response breaks = pair.item2;
    final error = parsed.errors[book.lang];
    final brackets =
        parsed.brakets.books.firstWhere((bk) => bk.lang == book.lang);
    final wordStat = rew.WordsStat();

    for (final pair in Linq.zip(forBreaking(book), breaks.facts))
      mergeBreaking(book.lang, pair.item1, pair.item2.posLens, error, wordStat);

    brackets.latinWords.addAll(wordStat.latinWords);
    brackets.okWords.addAll(wordStat.okWords);
    brackets.wrongWords.addAll(wordStat.wrongWords);
    brackets.latinWords.addAll(wordStat.latinWords);
  }
  //megreBreaking(pair.item1, pair.item2, parsed.errors);
  return Future.value(parsed);
}

/****** pair of fuctions, which:
 * - gets data for word-breaking
 * - pair resulted breaks with original data
*/
Iterable<toPars.ParsedSubFact> forBreaking(toPars.ParsedBook book) =>
    book.facts.expand((toPars.ParsedFact f) => f.childs);
//Linq.selectMany(book.facts, (toPars.ParsedFact f) => f.childs);

// megreBreaking(toPars.ParsedBook book, wbreak.Response breaks,
//     Map<String, StringBuffer> errors) {
//   for (final pair in Linq.zip(forBreaking(book), breaks.facts)) {
//     mergeBreakingLow(
//         book.lang, pair.item1, pair.item2.posLens, errors[book.lang]);
//   }
// }

mergeBreaking(String lang, toPars.ParsedSubFact sf, List<wbreak.PosLen> posLens,
    StringBuffer error, rew.WordsStat wordStat) {
  final okPosLens = rew.alphabetTest(lang, sf, posLens, error, wordStat);
  try {
    sf.breaks = rew.BreaksLib.oldToNew(sf.text, okPosLens) ??
        [0, 0] /* empty breaks => whole sf.text for stemming, which is wrong */;
  } catch (err) {
    error.writeln('** $err: ${sf.text}');
  }
}
