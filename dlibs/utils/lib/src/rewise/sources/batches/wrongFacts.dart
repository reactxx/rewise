import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/threading.dart';
import '../dom.dart';
import '../filer.dart';
import '../consts.dart';

Future exportWrongFacts(/*String bookNamee.g. '#lingea'*/) async {
  // for #lingea book: get all files grouped via leftLang
  final allGroups = Linq.group<FileInfo, String, String>(
      Filer.files, (f) => '${f.bookName}\\${f.leftLang}',
      //Filer.files.where((f) => f.bookName == bookName), (f) => '${f.bookName}\\${f.leftLang}',
      valuesAs: (v) => v.fileName);
  final arrayPars =
      allGroups.map((group) => group.key.split('\\').followedBy(group.values).toList());

  if (fileSystem.desktop) {
    final tasks = arrayPars.map((m) => ArrayMsg.encode(m));
    await Parallel(tasks, 4, _entryPoint, taskLen: allGroups.length)
        //.run(traceMsg: (count, msg) => print('$count/${allGroups.length} ${msg[0]} ${msg[1]}'));
        .run(traceMsg: (count, msg) => {});
  } else {
    var count = 0;
    for (final msg in arrayPars.map((m) => ArrayMsg(m))) {
      print('${++count}/${allGroups.length} ${msg.listValue[0]} ${msg.listValue[1]}');
      await _exportWrongFacts(msg);
    }
  }
}

void _entryPoint(List workerInitMsg) =>
    parallelEntryPoint<ArrayMsg>(workerInitMsg, _exportWrongFacts);

Future<List> _exportWrongFacts(ArrayMsg msg) {
  final errorCodeToMatrix = Map<int, Matrix>();
  for (var errorCode in Flags.factErrors)
    errorCodeToMatrix[errorCode] = Matrix(header: ['fact', 'file', 'id', 'crc']);

  for (final fn in msg.listValue.skip(2)) {
    try {
    final file = File.fromPath(fn);
    for (var errorCode in errorCodeToMatrix.keys) {
      final m = errorCodeToMatrix[errorCode];
      //var wrongs = file.factss.where((d) =>d.facts.any((dd) => d.facts!=0)).toList();
      String txt;
      for (final fact in file.factss
          .where((f) => f.facts.any((ff) => (ff.flags & errorCode) != 0)))
        m.add([txt = fact.toText(), fn, fact.id.toString(), txt.hashCode.toRadixString(32)]);
    }
    } catch (msg) {
      print('** ERROR in $fn');
      rethrow;
    }
  }

  for (var errorCode in errorCodeToMatrix.keys) {
    final m = errorCodeToMatrix[errorCode];
    if (m.rows.length == 1) continue;
    final resultFn =
        '\wrongFacts\\${msg.listValue[0]}\\${Flags.toText(errorCode)}\\${msg.listValue[1]}.csv';
    m.save(fileSystem.edits.absolute(resultFn));
  }

  return Parallel.workerReturnFuture;
}
