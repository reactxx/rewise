import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/rewise.dart' as rew;
import 'package:rw_utils/utils.dart' show fileSystem, hackToJson;
import 'package:path/path.dart' as p;
import 'package:rw_utils/threading.dart';

//import '../parallel.dart';

Future toParsed() async {
  final relPaths =
      fileSystem.raw.list(regExp: fileSystem.devFilter + r'msg$').toList();

  if (fileSystem.desktop) {
    final tasks = relPaths.map((rel) => StringMsg.encode(rel));
    await Parallel(tasks, 3, _entryPoint, taskLen: relPaths.length).run();
  } else {
    for (final relPath in relPaths) await _toParsedBook(StringMsg(relPath));
  }

  return Future.value();
}

void _entryPoint(List workerInitMsg) =>
    parallelEntryPoint<StringMsg>(workerInitMsg, _toParsedBook);

Future<List> _toParsedBook(StringMsg msg) async {
  final relPath = msg.strValue;
  final rawBooks =
      toPars.RawBooks.fromBuffer(fileSystem.raw.readAsBytes(relPath));

  // PARSING, CHECKING
  var res = rew.parsebook(rawBooks);

  // BREAKING
  res = await rew.wordBreaking(res);

  final relDir = p.setExtension(relPath, '') + r'\';

  // SPLIT TO LANGS
  for (final book in res.book.books)
    fileSystem.parsed
        .writeAsBytes('$relDir/${book.lang}.msg', book.writeToBuffer());

  // WRITING BOOK
  fileSystem.parsed
      .writeAsBytes('$relDir/stat.msg', res.brakets.writeToBuffer());

  if (fileSystem.ntb) {
    for (final bk in res.brakets.books) bk.brackets.clear();
    fileSystem.parsed
        .writeAsString('$relDir/stat.json', await hackToJson(res.brakets));
  }
  
  for (final key in res.errors.keys)
    if (res.errors[key].length > 0)
      fileSystem.parsed
          .writeAsString('$relDir/$key.log', res.errors[key].toString());
  print(relPath);
  return Parallel.workerReturnFuture;
}
