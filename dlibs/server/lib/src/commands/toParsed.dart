import 'package:rewise_low_utils/rw/to_parsed.dart' as ToParsed;
import 'package:rewise_low_utils/rw/client.dart' as client;
import 'package:rewise_low_utils/rewise.dart' as rewise;
import 'package:rewise_low_utils/utils.dart' show Linq;
import 'package:rewise_low_utils/designTime.dart';
//import 'package:server_dart/utils.dart';

const _devFilter = r'goetheverlag\.msg';

Future<String> toParsed() async {
  final relFiles = fileSystem.raw.list(regExp: _devFilter);
  for (final fn in relFiles) {
    // parsing and checking facts
    final rawBooks =
        ToParsed.RawBooks.fromBuffer(fileSystem.raw.readAsBytes(fn));
    final parsedBooks = ToParsed.ParsedBooks()..name = rawBooks.name;
    for (final rawBook in rawBooks.books) {
      final parsedBook = ToParsed.ParsedBook()..lang = rawBook.lang;
      parsedBooks.books.add(parsedBook);
      for (var idx = 0; idx < rawBook.facts.length; idx++)
        parsedBook.facts.addAll(rewise.parseFactTextFormat/*MAIN PROC*/(rawBook.facts[idx],
            idx, rawBooks.lessons.length > 0 ? rawBooks.lessons[idx] : 0));
    }
    // word breaking
    final futures = parsedBooks.books
        .map((f) => rewise.wordBreak/*MAIN PROC*/(f, f.lang, client.WordBreaking_Run));
    final booksBreaks = await Future.wait(futures);
    for (var book in Linq.zip(parsedBooks.books, booksBreaks)) {
      for (var fact in Linq.zip(book.item1.facts, book.item2)) {
        if (fact.item2!=null) fact.item1.fact.breaks.addAll(fact.item2);
      }
    }
  }
  return Future.value('');
}
