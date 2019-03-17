import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;
import 'parseFact.dart';

class ParseBookResult {
  ParseBookResult(this.book, this.brakets, this.errors);
  toPars.ParsedBooks book;
  toPars.BracketBooks brakets;
  Map<String, StringBuffer> errors;
}

ParseBookResult parsebook(toPars.RawBooks rawBooks) {
  // all langs
  final parsedBooks = toPars.ParsedBooks()..name = rawBooks.name;
  final bracketBooks = toPars.BracketBooks()..name = rawBooks.name;
  final errorsBooks = Map<String, StringBuffer>();
  // for each lang
  for (final rawBook in rawBooks.books) {
    // create msg version
    final parsedBook = toPars.ParsedBook()..lang = rawBook.lang;
    final bracketBook = toPars.BracketBook()..lang = rawBook.lang;
    parsedBooks.books.add(parsedBook);
    bracketBooks.books.add(bracketBook);
    final errors = StringBuffer();
    errorsBooks[rawBook.lang] = errors;
    //  for each fact
    for (var idx = 0; idx < rawBook.facts.length; idx++) {
      // create msg version of fact
      // devCount++;
      // if (devCount & 0x4ff == 0) print(devCount);
      final msgFact = toPars.ParsedFact()
        ..lessonId =
            rawBooks.lessons.length > 0 ? rawBooks.lessons[idx] + 1 : 0;
      parsedBook.facts.add(msgFact);
      // MAIN PROC: parse single source fact text and fill msg by parsed fact
      ParsedFact(rawBook.facts[idx]).toMsg(idx, msgFact, bracketBook, errors);
    }
  }
  return ParseBookResult(parsedBooks, bracketBooks, errorsBooks);
}

/****** pair of fuctions, which:
 * - gets data for word-breaking
 * - pair resulted breaks with original data
*/
Iterable<toPars.ParsedSubFact> forBreaking(toPars.ParsedBook book) =>
    Linq.selectMany(book.facts, (toPars.ParsedFact f) => f.childs);

megreBreaking(toPars.ParsedBook book, wbreak.Response breaks) {
  for (final pair in Linq.zip(forBreaking(book), breaks.facts))
    pair.item1.breaks = pair.item2.breaks;
}

