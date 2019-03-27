import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/rewise.dart' as rew;
import 'package:rw_utils/utils.dart' show fileSystem, hackToJson;
import 'package:path/path.dart' as p;
import 'package:rw_utils/threading.dart';
//import 'package:server_dart/utils.dart' as utilss;

Future toParsed() async {
  final relPaths = fileSystem.raw.list(regExp: fileSystem.devFilter + r'msg$').toList();

  final tasks = relPaths.map((rel) => ParseBook.encode(rel));
  await _Parallel.START(tasks, relPaths.length, 4);

  //for (final relPath in relPaths) await _toParsedBook(relPath);

  return Future.value();
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

class _Parallel extends Parallel<ParseBook, ContinueMsg> {
  _Parallel(Iterable<List> tasks, this.len, int workersNum)
      : super(tasks, (p) => createProxies(workersNum, p)) {
    initThreadingTest();
    len = tasks.length;
  }

  @override
  callback(ContinueMsg msg) => print('${count++} / $len');
  int len;
  int count = 1;
  static List<Worker> createProxies(int workers, WorkerPool p) =>
      List<_Worker>.generate(workers, (i) => _Worker.proxy(p));

  static Future<List> START(tasks, int len, int parallels) async {
    final parallel = _Parallel(tasks, len, parallels);
    return await parallel.runParallel();
  }
}

class _Worker extends Worker {
  _Worker.proxy(pool) : super.proxy(pool) {}
  _Worker.worker(List list) : super.worker(list) {
    initThreadingTest();
  }
  @override
  Future workerMsg(Worker worker, Msg input) async {
    if (input is ParseBook) {
      await _toParsedBook(input.relPath);
      worker.sendMsg(ContinueMsg.encode());
    } else
      return super.workerMsg(worker, input);
  }

  @override
  EntryPoint get entryPoint => workerCode;
  static void workerCode(List l) => _Worker.worker(l).workerRun();
}

class ParseBook extends Msg {
  static const id = _namespace + 'ParseBook';
  String relPath;
  static List encode(String relPath) => [id, relPath];
  ParseBook.decode(List list) : super.decode(list) {
    relPath = list[3];
  }
}

const _namespace = 'rw.parsing.';
bool _called = false;
void initThreadingTest() {
  if (!_called) {
    initMessages();
    messageDecoders.addAll(<String, DecodeProc>{
      ParseBook.id: (list) => ParseBook.decode(list),
    });
    _called = true;
  }
}

main() async {
  await toParsed();  
}
