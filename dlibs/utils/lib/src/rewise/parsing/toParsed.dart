import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/rewise.dart' as rew;
import 'package:rw_utils/utils.dart' show fileSystem, hackToJson;
import 'package:path/path.dart' as p;
import 'package:rw_utils/threading.dart';

import '../parallel.dart';

Future toParsed() async {
  final relPaths =
      fileSystem.raw.list(regExp: fileSystem.devFilter + r'msg$').toList();

  if (fileSystem.desktop) {
    final tasks = relPaths.map((rel) => StringMsg.encode(rel));
    await ParallelString.START(
        tasks, relPaths.length, (p) => _Worker.proxy(p), 1);
  } else {
    for (final relPath in relPaths) await _toParsedBook(relPath);
  }

  return Future.value();
}

class _Worker extends ParallelStringWorker {
  _Worker.proxy(pool) : super.proxy(pool) {}
  _Worker.worker(List list) : super.worker(list) {

  }
  @override
  Future workerRun3(String par) => _toParsedBook(par);
  @override
  EntryPoint get entryPoint => workerCode;
  static void workerCode(List l) => _Worker.worker(l).workerRun0();
}

Future _toParsedBook(String relPath) async {
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
  fileSystem.parsed
      .writeAsString('$relDir/stat.json', await hackToJson(res.brakets));
  for (final key in res.errors.keys)
    if (res.errors[key].length > 0)
      fileSystem.parsed
          .writeAsString('$relDir/$key.log', res.errors[key].toString());
  print(relPath);
  return Future.value();
}
