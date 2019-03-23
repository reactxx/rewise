import 'package:rw_utils/rewise.dart' as rewise;
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;

Future<rewise.ParseBookResult> wordBreaking (rewise.ParseBookResult parsed) async {
    // word breaking
    final futures = parsed.book.books.map((book) => client.WordBreaking_Run(
        wbreak.Request()
          ..lang = book.lang
          ..facts.addAll(
              rewise.forBreaking(book).map((ch) => ch.breakText.isNotEmpty ? ch.breakText : ch.text))));
    final booksBreaks = await Future.wait(futures);
    for (var pair in Linq.zip(parsed.book.books, booksBreaks))
      rewise.megreBreaking(pair.item1, pair.item2);
    return Future.value(parsed);
}

