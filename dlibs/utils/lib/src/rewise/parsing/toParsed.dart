import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/rewise.dart' as rew;
import 'package:rw_utils/utils.dart' show fileSystem, hackToJson;
import 'package:path/path.dart' as p;
import 'package:rw_utils/threading.dart';

Future toParsed() async {
  final relPaths =
      fileSystem.raw.list(regExp: fileSystem.devFilter + r'msg$').toList();

  final tasks = relPaths.map((rel) => _ParseBook.encode(rel));
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

class _Parallel extends Parallel<_ParseBook, ContinueMsg> {
  _Parallel(Iterable<List> tasks, this._len, int workersNum)
      : super(tasks, (p) => _createProxies(workersNum, p)) {
    _initThreadingTest();
    _len = tasks.length;
  }

  static Future<List> START(tasks, int len, int parallels) async {
    final parallel = _Parallel(tasks, len, parallels);
    return await parallel.runParallel();
  }

  int _len;
  int _count = 1;
  @override
  callback(ContinueMsg msg) => print('${_count++} / $_len');

  static List<Worker> _createProxies(int workers, WorkerPool p) =>
      List<_Worker>.generate(workers, (i) => _Worker.proxy(p));

}

class _Worker extends Worker {
  _Worker.proxy(pool) : super.proxy(pool) {}
  _Worker.worker(List list) : super.worker(list) {
    _initThreadingTest();
  }
  @override
  Future workerMsg(Worker worker, Msg input) async {
    if (input is _ParseBook) {
      await _toParsedBook(input.relPath);
      worker.sendMsg(ContinueMsg.encode());
    } else
      return super.workerMsg(worker, input);
  }

  @override
  EntryPoint get entryPoint => workerCode;
  static void workerCode(List l) => _Worker.worker(l).workerRun();
}

class _ParseBook extends Msg {
  static const id = _namespace + 'ParseBook';
  String relPath;
  static List encode(String relPath) => [id, relPath];
  _ParseBook.decode(List list) : super.decode(list) {
    relPath = list[3];
  }
}

const _namespace = 'rw.parsing.';
bool _called = false;
void _initThreadingTest() {
  if (!_called) {
    initMessages();
    messageDecoders.addAll(<String, DecodeProc>{
      _ParseBook.id: (list) => _ParseBook.decode(list),
    });
    _called = true;
  }
}
