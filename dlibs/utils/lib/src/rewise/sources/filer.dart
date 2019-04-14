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
    {List groupBy(FileInfo f),
    bool emptyPrint = true,
    void processFile(File file),
    bool doParallel}) async {
  final allGroups = Linq.group<FileInfo, String, FileInfo>(
      Filer.files, (f) => groupBy(f).join('|'));
  final tasks = allGroups
      .map((group) => DataMsg(group.values.expand((f) => f.toDataMsg()).toList()))
      .toList();

  return processTasks(entryPoint, action, tasks,
      emptyPrint: emptyPrint,
      doParallel: doParallel,
      printDetail: (l) => '${l.listValue[0]}.${l.listValue[1]}');
}

List groupByDataLang(FileInfo f) => [f.fileName, f.dataLang];
List groupByLeftLang(FileInfo f) => [f.fileName, f.leftLang];

Iterable<File> scanFiles(DataMsg msg) sync* {
  final iter = msg.listValue.iterator;
  //iter.moveNext();
  while (true) {
    FileInfo fi;
    try {
      fi = FileInfo.fromDataMsg(iter);
      yield File.fromFileInfo(fi);
      if (!iter.moveNext()) break;
    } catch (msg) {
      print('** ERROR in ${fi.fileName}');
      rethrow;
    }
  }
}
