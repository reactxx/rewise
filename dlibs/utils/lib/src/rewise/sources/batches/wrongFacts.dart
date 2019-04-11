import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/threading.dart';
import '../dom.dart';
import '../filer.dart';

Future exportWrongFacts(String bookName/*e.g. '#lingea'*/) async {
  // for #lingea book: get all files grouped via leftLang
  final allGroups = Linq.group<FileInfo, String, String>(
      Filer.files.where((f) => f.bookName == bookName), (f) => f.leftLang,
      valuesAs: (v) => v.path);
  final stringPars =
      allGroups.map((group) => 'wrongFacts\\$bookName\\${group.key}.csv#' + group.values.join(','));

  if (fileSystem.desktop) {
    final tasks = stringPars.map((m) => StringMsg.encode(m));
    await Parallel(tasks, 4, _entryPoint, taskLen: allGroups.length).run(
        traceMsg: (count, msg) => print('$count/${allGroups.length}'));
        //traceMsg: (count, msg) => {});
  } else {
    var count = 0;
    for (final msg in stringPars.map((m) => StringMsg(m))) {
      print('${++count}/${allGroups.length}');
      await _exportWrongFacts(msg);
    }
  }
}

void _entryPoint(List workerInitMsg) =>
    parallelEntryPoint<StringMsg>(workerInitMsg, _exportWrongFacts);

Future<List> _exportWrongFacts(StringMsg msg) {
  final idx = msg.strValue.indexOf('#');
  final fns = msg.strValue.substring(idx + 1).split(',');

  final matrix = Matrix(header: ['fact', 'file', 'id']);
  for (final fn in fns) {
    final file = File.fromPath(fn);
    for (final fact
        in file.factss.where((f) => f.facts.any((ff) => ff.flags != 0)))
      matrix.add([fact.toText(), fn, fact.id.toString()]);
  }

  final resultFn = msg.strValue.substring(0, idx);
  matrix.save(fileSystem.edits.absolute(resultFn));

  return Parallel.workerReturnFuture;
}
