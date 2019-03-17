import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/rewise.dart' as rewise;
import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/dom/word_breaking.dart' as wbreak;
import 'package:path/path.dart' as p;
//import 'package:server_dart/utils.dart' as utilss;

const _devFilter = r'goetheverlag\.msg';

Future<String> toParsed() async {
  for (final fn in fileSystem.raw.list(regExp: _devFilter)) {
    final rawBooks = toPars.RawBooks.fromBuffer(fileSystem.raw.readAsBytes(fn));
    // parsing and checking facts
    final res = rewise.parsebook(rawBooks);
    // word breaking
    final futures = res.book.books.map((book) => client.WordBreaking_RunEx(
        wbreak.Request()
          ..lang = book.lang
          ..facts.addAll(
              rewise.forBreaking(book).map((ch) => ch.breakText ?? ch.text))));
    final booksBreaks = await Future.wait(futures);
    for (var pair in Linq.zip(res.book.books, booksBreaks))
      rewise.megreBreaking(pair.item1, pair.item2);
    fileSystem.parsed.writeAsBytes(fn, res.book.writeToBuffer());
    fileSystem.parsed.writeAsBytes(
        p.setExtension(fn, '.br.msg'), res.brakets.writeToBuffer());
    for (final key in res.errors.keys) 
      if (res.errors[key].length > 0)
        fileSystem.parsed.writeAsString(
            p.setExtension(fn, '.$key.log'), res.errors[key].toString());
    // fileSystem.parsed
    //     .writeAsString(p.setExtension(fn, '.log'), res.errors.toString());
    // // JSON file na serveru
    // await utilss.hackJsonFile(
    //     parsedBooks.info_.qualifiedMessageName,
    //     fileSystem.parsed.absolute(fn),
    //     fileSystem.parsed.absolute(fn, ext: '.json'),
    //     true);
  }
  return Future.value('');
}
