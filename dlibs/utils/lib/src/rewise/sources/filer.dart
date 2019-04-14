import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/threading.dart';

import 'consts.dart';
import 'dom.dart';

class Filer {
  static List<FileInfo> get files =>
      _files ??
      (_files = fileSystem.source
          .list(regExp: r'\.csv$')
          .map((f) => FileInfo.infoFromPath(f))
          .toList());

  static List<FileInfo> _files;
}

Future useSources(WorkerEntryPoint entryPoint, Future<Msg> action(DataMsg msg),
    {bool groupByDataLang = false,
    bool emptyPrint = true,
    void processFile(File file),
    bool doParallel}) async {
  final allGroups = Linq.group<FileInfo, String, String>(Filer.files,
      (f) => '${f.bookName}\\${groupByDataLang ? f.dataLang : f.leftLang}',
      valuesAs: (v) => v.fileName);
  final tasks = allGroups.map((group) =>
      DataMsg(group.key.split('\\').followedBy(group.values).toList())).toList();

  return processTasks(entryPoint, action, tasks,
      emptyPrint: emptyPrint, doParallel: doParallel, printDetail: (l) => '${l.listValue[0]}.${l.listValue[1]}');

}

Iterable<File> scanFiles(DataMsg msg) sync* {
  for (final fn in msg.listValue.skip(2)) {
    try {
      yield File.fromPath(fn);
    } catch (msg) {
      print('** ERROR in $fn');
      rethrow;
    }
  }
}

/*
import 'package:rw_utils/threading.dart';
import '../dom.dart';
import '../filer.dart';

Future exportWrongFacts({bool emptyPrint = true}) async =>
    useSources(_entryPoint, _action, emptyPrint: emptyPrint);

void _entryPoint(List workerInitMsg) =>
    parallelEntryPoint<ArrayMsg>(workerInitMsg, _action);

Future<List> _action(ArrayMsg msg) {
  for (final fn in msg.listValue.skip(2)) {
    try {
      final file = File.fromPath(fn);
    } catch (msg) {
      print('** ERROR in $fn');
      rethrow;
    }
  }
  return Parallel.workerReturnFuture;
}

 */
