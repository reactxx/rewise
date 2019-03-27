import 'package:rw_utils/rewise.dart' as rewise;
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;

const maxFactCount = 100000;

Future<rewise.ParseBookResult> wordBreaking(
    rewise.ParseBookResult parsed) async {
  // word breaking
  final futures = parsed.book.books.map((book) async {
    final facts = rewise
        .forBreaking(book)
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
        pos+=chunkSize;
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
  final booksBreaks = await Future.wait(futures);
  for (var pair in Linq.zip(parsed.book.books, booksBreaks))
    rewise.megreBreaking(pair.item1, pair.item2, parsed.errors);
  return Future.value(parsed);
}
