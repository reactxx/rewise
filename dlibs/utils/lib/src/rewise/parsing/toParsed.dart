import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/rewise.dart' as rewise;
import 'package:rw_utils/utils.dart' show fileSystem, hackToJson;
import 'package:path/path.dart' as p;
//import 'package:server_dart/utils.dart' as utilss;

Future<String> toParsed() async {
  for (final fn in fileSystem.raw.list(regExp: fileSystem.devFilter + r'msg$')) {
    // READING
    final rawBooks = toPars.RawBooks.fromBuffer(fileSystem.raw.readAsBytes(fn));

    // PARSING, CHECKING
    var res = rewise.parsebook(rawBooks);

    // BREAKING
    res = await rewise.wordBreaking(res);

    final relDir = p.setExtension(fn, '') + r'\';
    // SPLIT TO LANGS
    for (final book in res.book.books)
      fileSystem.parsed.writeAsBytes('$relDir/${book.lang}.msg', book.writeToBuffer());

    // WRITING BOOK
    //fileSystem.parsed.writeAsBytes(fn, res.book.writeToBuffer());
    fileSystem.parsed.writeAsBytes('$relDir/stat.msg', res.brakets.writeToBuffer());
    fileSystem.parsed.writeAsString('$relDir/stat.json', await hackToJson(res.brakets));
    for (final key in res.errors.keys)
      if (res.errors[key].length > 0)
        fileSystem.parsed.writeAsString('$relDir/$key.log', res.errors[key].toString());
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
