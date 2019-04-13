import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/threading.dart';
import '../dom.dart';
import '../filer.dart';
import '../consts.dart';

Future exportWrongFacts(/*String bookNamee.g. '#lingea'*/) async {
  // for #lingea book: get all files grouped via leftLang
  final allGroups = Linq.group<FileInfo, String, String>(
      Filer.files, (f) => '${f.leftLang}\\${f.bookName}',
      //Filer.files.where((f) => f.bookName == bookName), (f) => '${f.leftLang}\\${f.bookName}',
      valuesAs: (v) => v.fileName);
  final stringPars =
      allGroups.map((group) => '${group.key}.csv|' + group.values.join(','));

  if (fileSystem.desktop) {
    final tasks = stringPars.map((m) => StringMsg.encode(m));
    await Parallel(tasks, 4, _entryPoint, taskLen: allGroups.length)
        .run(traceMsg: (count, msg) => print('$count/${allGroups.length}'));
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
  final idx = msg.strValue.indexOf('|');
  final fns = msg.strValue.substring(idx + 1).split(',');

  final errorCodeToMatrix = Map<int, Matrix>();
  for (var errorCode in Flags.factErrors)
    errorCodeToMatrix[errorCode] = Matrix(header: ['fact', 'file', 'id', 'crc']);

  for (final fn in fns) {
    final file = File.fromPath(fn);
    for (var errorCode in errorCodeToMatrix.keys) {
      final m = errorCodeToMatrix[errorCode];
      String txt;
      for (final fact in file.factss
          .where((f) => f.facts.any((ff) => (ff.flags & errorCode) != 0)))
        m.add([txt = fact.toText(), fn, fact.id.toString(), txt.hashCode.toRadixString(32)]);
    }
  }

  for (var errorCode in errorCodeToMatrix.keys) {
    final m = errorCodeToMatrix[errorCode];
    if (m.rows.length == 1) continue;
    final resultFn =
        '${Flags.toText(errorCode)}\\${msg.strValue.substring(0, idx)}';
    m.save(fileSystem.edits.absolute(resultFn));
  }

  return Parallel.workerReturnFuture;
}
