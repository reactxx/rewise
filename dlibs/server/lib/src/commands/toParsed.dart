import 'package:rw_dom/to_parsed.dart' as ToParsed;
import 'package:rw_dom/dom.dart' as dom;
import 'package:rewise_low_utils/rewise.dart' as rewise;
import 'package:rewise_low_utils/utils.dart' show Linq;
import 'package:rewise_low_utils/client.dart' as client;
import 'package:rewise_low_utils/designTime.dart';
import 'package:rw_dom/word_breaking.dart' as wbreak;
import 'package:server_dart/utils.dart' as utilss;

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
      for (var idx = 0; idx < rawBook.facts.length; idx++) {
        final parsed = rewise
            .parseFactTextFormat /*MAIN PROC*/ (rawBook.facts[idx])
            .map((f) {
          final res = ToParsed.ParsedFact()
            ..idx = idx
            ..fact = (dom.Fact()
              ..lessonId =
                  rawBooks.lessons.length > 0 ? rawBooks.lessons[idx] + 1 : 0);
          if (f.breakText != null) res.breakText = f.breakText;
          return res;
        });
        parsedBook.facts.addAll(parsed);
      }
    }
    // word breaking
    final futures = parsedBooks.books
        .map((book) => client.WordBreaking_RunEx(wbreak.Request()
          ..lang = book.lang
          ..facts.addAll(book.facts.map((f) => f.breakText))));
    final booksBreaks = await Future.wait(futures);
    assert(booksBreaks.where((bk) => bk != null).length ==
        parsedBooks.books.length);
    for (var book in Linq.zip(parsedBooks.books, booksBreaks)) {
      for (var fact in Linq.zip(book.item1.facts, book.item2.facts))
        fact.item1.fact.breaks = fact.item2.breaks;
    }
    fileSystem.parsed.writeAsBytes(fn, parsedBooks.writeToBuffer());
    // JSON file na serveru
    await utilss.hackJsonFile(
        parsedBooks.info_.qualifiedMessageName,
        fileSystem.parsed.absolute(fn),
        fileSystem.parsed.absolute(fn, ext: '.json'),
        true);
  }
  return Future.value('');
}
