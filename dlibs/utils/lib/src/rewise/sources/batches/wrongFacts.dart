import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
import 'package:rw_utils/threading.dart';
import '../filer.dart';
import '../consts.dart';

Future exportWrongFacts({bool emptyPrint = true, bool doParallel}) async =>
    useSources(_entryPoint, _exportWrongFacts,
        groupBy: groupByLeftLang,
        emptyPrint: emptyPrint,
        doParallel: doParallel);

void _entryPoint(List workerInitMsg) =>
    parallelEntryPoint(workerInitMsg, _exportWrongFacts);

Future<Msg> _exportWrongFacts(DataMsg msg) {
  final errorCodeToMatrix = Map<int, Matrix>();
  for (var errorCode in Flags.factErrors)
    errorCodeToMatrix[errorCode] =
        Matrix(header: ['fact', 'file', 'id', 'crc']);

  FileInfo firstFile;
  for (final file in scanFiles(msg)) {
    firstFile = file;
    for (var errorCode in errorCodeToMatrix.keys) {
      final m = errorCodeToMatrix[errorCode];
      //var wrongs = file.factss.where((d) =>d.facts.any((dd) => d.facts!=0)).toList();
      String txt;
      for (final fact in file.factss
          .where((f) => f.facts.any((ff) => (ff.flags & errorCode) != 0)))
        m.add([
          txt = fact.toText(),
          file.fileName,
          fact.id.toString(),
          txt.hashCode.toRadixString(32)
        ]);
    }
  }

  for (var errorCode in errorCodeToMatrix.keys) {
    final m = errorCodeToMatrix[errorCode];
    if (m.rows.length == 1) continue;
    final resultFn =
        '\wrongFacts\\${firstFile.bookName}\\${Flags.toText(errorCode)}\\${firstFile.leftLang}.csv';
    m.save(fileSystem.edits.absolute(resultFn));
  }

  return Parallel.workerReturnFuture;
}
