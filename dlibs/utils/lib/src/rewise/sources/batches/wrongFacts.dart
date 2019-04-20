import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
import 'package:rw_utils/threading.dart';
import '../filer.dart';
import '../consts.dart';
import '../dom.dart';
import '../reparseFacts.dart';

Future exportWrongFacts({bool emptyPrint = true, bool doParallel}) async =>
    useSources(_exportWrongFactsEntryPoint, _exportWrongFacts, GroupByType.fileNameDataLang,
        emptyPrint: emptyPrint,
        doParallel: doParallel);

Future importWrongFacts({bool emptyPrint = true, bool doParallel}) async {
  final tasks = fileSystem.edits
      .list(file: false, from: 'wrongFacts', recursive: false)
      .map((f) => DataMsg([f]))
      .toList();
  return processTasks(_importWrongFactsEntryPoint, _importWrongFacts, tasks,
      emptyPrint: emptyPrint,
      doParallel: doParallel,
      printDetail: (l) => '${l.listValue[0]}');
}

void _exportWrongFactsEntryPoint(List workerInitMsg) =>
    parallelEntryPoint(workerInitMsg, _exportWrongFacts);

void _importWrongFactsEntryPoint(List workerInitMsg) =>
    parallelEntryPoint(workerInitMsg, _importWrongFacts);

Future<Msg> _exportWrongFacts(DataMsg msg, InitMsg initPar) {
  final errorCodeToMatrix = Map<int, Matrix>();
  for (var errorCode in FactFlags.factErrors)
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
        '\wrongFacts\\${firstFile.bookName}\\${FactFlags.toText(errorCode)}\\${firstFile.leftLang}.csv';
    m.save(fileSystem.edits.absolute(resultFn));
  }

  return Parallel.workerReturnFuture;
}

Future<Msg> _importWrongFacts(DataMsg msg, InitMsg initPar) async {
  final files = fileSystem.edits.list(from: msg.listValue[0]);
  final rows = files
      .map((f) => Matrix.fromFile(fileSystem.edits.absolute(f)))
      .expand((m) => m.rows
          .skip(1)
          .map((r) =>
              _Row(r.data[0], r.data[1], int.parse(r.data[2]), r.data[3]))
          .where((r) => r.fact.hashCode.toRadixString(32) != r.crc))
      .toList();
  if (rows.isEmpty) Parallel.workerReturnFuture;

  for (final grp in Linq.group<_Row, String, _Row>(rows, (r) => r.file)) {
    final file = File.fromPath(grp.key);
    for (final r in grp.values) {
      final fact = file.factss[r.id];
      if (fact.id != r.id) throw Exception('fact.id!=r.id');
      if (fact.crc != r.crc) throw Exception('fact.crc!=r.crc');
      fact.asString = r.fact;
    }
    final modified = await refreshFileLow(file, false);
    if (modified > 0) {
      file..save();
    }
  }
  //fileSystem.edits.deleteDir(msg.listValue[0]);

  return Parallel.workerReturnFuture;
}

class _Row {
  _Row(this.fact, this.file, this.id, this.crc);
  String fact;
  String file;
  int id;
  String crc;
}
