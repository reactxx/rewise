import 'package:test/test.dart' as test;
import 'package:rw_utils/dom/to_parsed.dart' as $pars;
import 'package:rw_utils/rewise.dart' as rw;

main() {
  test.group("PARSE BOOK", () {
    test.test('parse text', () async {
      final bk = $pars.RawBooks()
        ..name = 'book'
        ..lessons.addAll([0,1])
        ..books.add($pars.RawBook()
          ..lang = 'cs-CZ'
          ..facts.addAll([
            'd^Kolik? Hodně i více...',
            '[w]a(?)',
          ]));
      var res = rw.parsebook(bk);
      test.expect(res.book.name, test.equals('book'));
      var book = res.book.books[0];
      test.expect(book.lang, test.equals('cs-CZ'));
      var fact = book.facts[1];
      test.expect(fact.idx, test.equals(1));
      test.expect(fact.lessonId, test.equals(2));
      var subFact = fact.childs[0];
      test.expect(subFact.text, test.equals('a(?)'));
      test.expect(subFact.breakText, test.equals('a   '));
      test.expect(subFact.wordClass, test.equals('w'));
      res = await rw.wordBreaking(res);
      subFact = res.book.books[0].facts[0].childs[1];
      var breaks = subFact.breaks.join(',');
      test.expect(breaks, test.equals('5,2,5,1,1,1,4'));
    });
  });
}
