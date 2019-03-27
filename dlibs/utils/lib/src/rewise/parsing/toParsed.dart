import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/rewise.dart' as rew;
import 'package:rw_utils/utils.dart' show fileSystem, hackToJson;
import 'package:path/path.dart' as p;
import 'package:rw_utils/threading.dart';
//import 'package:server_dart/utils.dart' as utilss;

Future<String> toParsed() async {

  
  
  for (final fn in fileSystem.raw.list(regExp: fileSystem.devFilter + r'msg$')) {
    // READING
    final rawBooks = toPars.RawBooks.fromBuffer(fileSystem.raw.readAsBytes(fn));

    // PARSING, CHECKING
    var res = rew.parsebook(rawBooks);

    // BREAKING
    res = await rew.wordBreaking(res);

    final relDir = p.setExtension(fn, '') + r'\';
    
    // SPLIT TO LANGS
    for (final book in res.book.books)
      fileSystem.parsed.writeAsBytes('$relDir/${book.lang}.msg', book.writeToBuffer());

    // WRITING BOOK
    fileSystem.parsed.writeAsBytes('$relDir/stat.msg', res.brakets.writeToBuffer());
    fileSystem.parsed.writeAsString('$relDir/stat.json', await hackToJson(res.brakets));
    for (final key in res.errors.keys)
      if (res.errors[key].length > 0)
        fileSystem.parsed.writeAsString('$relDir/$key.log', res.errors[key].toString());
  }
  return Future.value('');
}

class _Parallel extends Parallel<ParseBook, Msg> {
  _Parallel(Iterable<List> tasks, int workersNum)
      : super(
            tasks,
            (p) => List<_Worker>.generate(
                workersNum, (idx) => _Worker.proxy(p))) {
    initThreadingTest();
  }

  static Future<List> START(int taskNum, num parallels) async {
    final tasks = List.generate(taskNum, (idx) => ParseBook.encode());
    final parallel = _Parallel(tasks, parallels);
    return await parallel.runParallel();
  }
}

class _Worker extends Worker {
  _Worker.proxy(pool, {List initPar, WorkerMsg workerMsg})
      : super.proxy(pool) {}
  _Worker.worker(List list) : super.worker(list) {
    initThreadingTest();
  }
  @override
  Future workerMsg(Worker worker, Msg input) async {
    if (input is ParseBook) {
      await Future.delayed(Duration(milliseconds: 500));
      worker.sendMsg(ParseBook.encode());
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
  static List encode(String relPath) => [id,relPath];
  ParseBook.decode(List list) : super.decode(list) {
    relPath = list[3];
  }
}

const _namespace = 'rw.parsing.';
void initThreadingTest() {
  if (_called) return;
  initMessages();
  messageDecoders.addAll(<String, DecodeProc>{
    ParseBook.id: (list) => ParseBook.decode(list),
  });
  _called = true;
}
bool _called = false;
