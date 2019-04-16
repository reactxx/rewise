import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/threading.dart';
import 'package:tuple/tuple.dart';

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

Future useSources(WorkerEntryPoint entryPoint,
    Future<Msg> action(DataMsg msg, InitMsg initPar), GroupByType groupByType,
    {bool emptyPrint = true,
    String printDetail(DataMsg msg),
    void processFile(File file),
    bool doParallel}) async {
  final allGroups = Linq.group<FileInfo, String, FileInfo>(
      Filer.files, (f) => groupBy(f, groupByType, null));
  final tasks = allGroups
      .map((group) =>
          DataMsg(group.values.expand((f) => f.toDataMsg()).toList()))
      .toList();

  return processTasks(entryPoint, action, tasks,
      emptyPrint: emptyPrint,
      doParallel: doParallel,
      initPar: InitMsg([groupByType]),
      printDetail: printDetail ?? (l) => '${l.listValue[0]}.${l.listValue[1]}');
}

enum GroupByType {
  dataLang,
  fileNameDataLang,
  fileNameLeftLang,
}

String groupBy(FileInfo f, GroupByType type, String subPath) {
  switch (type) {
    case GroupByType.dataLang:
      return f.dataLang;
    case GroupByType.fileNameDataLang:
      return '${f.bookName}${subPath == null ? '' : '\\' + subPath}\\${f.dataLang}';
    case GroupByType.fileNameLeftLang:
      return '${f.bookName}\\${f.leftLang}';
    default:
      throw Exception();
  }
}

// ************ SCANS *******************

Iterable<FileInfo> scanFileInfos(DataMsg msg) sync* {
  final iter = msg.listValue.iterator;
  while (true) {
    final fi = FileInfo.fromDataMsg(iter);
    if (fi.bookName == null) break;
    yield fi;
  }
}

Iterable<File> scanFiles(DataMsg msg) =>
    scanFileInfos(msg).map((fi) => File.fromFileInfo(fi));

Iterable<Tuple2<FileInfo, Word>> scanFileWords(DataMsg msg,
        {bool wordCondition(Word w)}) =>
    scanFiles(msg).expand((file) => file.factss.expand((fs) => fs.facts).expand(
        (f) => f.words
                .where(wordCondition ??
                    (w) =>
                        (w.flags == 0 || w.flags == Flags.wIsPartOf) &&
                        w.text != null &&
                        w.text.isNotEmpty)
                .map((w) {
              return Tuple2(FileInfo.infoFromPath(file.fileName), w);
            })));

Iterable<Word> scanWords(DataMsg msg) => scanFiles(msg)
    .expand((f) => f.factss)
    .expand((fs) => fs.facts)
    .expand((f) => f.words);
