import 'dart:collection';
import 'package:tuple/tuple.dart';
import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_low/code.dart' show Linq, Group;
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

  static List<Group<String, FileInfo>> groups(GroupByType groupByType,
          {bool filter(FileInfo fi)}) =>
      Linq.group<FileInfo, String, FileInfo>(
          Filer.files.where(filter ?? (fi) => true),
          (f) => groupBy(f, groupByType, null));

  static String groupBy(FileInfo f, GroupByType type, String subPath) {
    subPath = subPath == null ? '' : subPath + '\\';
    switch (type) {
      case GroupByType.dataLang:
        return '$subPath${f.dataLang}';
      case GroupByType.fileNameDataLang:
        return '${f.bookName}\\$subPath${f.dataLang}';
      case GroupByType.fileNameLeftLang:
        return '${f.bookName}\\$subPath${f.leftLang}';
      case GroupByType.fileName:
        return '$subPath${f.bookName}';
      default:
        throw Exception();
    }
  }
}

Future useSources(WorkerEntryPoint entryPoint,
    Future<Msg> action(DataMsg msg, InitMsg initPar), GroupByType groupByType,
    {bool emptyPrint = true,
    List initPar = const [],
    String printDetail(DataMsg msg),
    bool doParallel,
    bool filter(FileInfo fi)}) async {
  final groups = Filer.groups(groupByType, filter: filter),
      tasks = groups
          .map((group) => DataMsg(initPar
              .followedBy(group.values.expand((f) => f.toDataMsg()))
              .toList()))
          .toList();

  return processTasks(entryPoint, action, tasks,
      emptyPrint: emptyPrint ?? false,
      doParallel: doParallel,
      initPar: InitMsg([groupByType]),
      printDetail: printDetail ?? (l) => '${l.listValue[0]}.${l.listValue[1]}');
}

enum GroupByType {
  dataLang,
  fileNameDataLang,
  fileNameLeftLang,
  fileName,
}

// ************ SCANS *******************

Iterable<FileInfo> scanFileInfos(DataMsg msg, {int skip = 0}) sync* {
  final iter = msg.listValue.iterator;
  for (var i = 0; i < skip; i++) iter.moveNext();
  while (true) {
    final fi = FileInfo.fromDataMsg(iter);
    if (fi.bookName == null) break;
    yield fi;
  }
}

Iterable<File> scanFiles(DataMsg msg, {int skip = 0}) =>
    scanFileInfos(msg, skip: skip).map((fi) => File.fromFileInfo(fi));

HashSet<String> uniqueFilesWords(Iterable<FileInfo> infos, {bool wordCondition(Word w)}) =>
    HashSet<String>.from(scanFilesLow(infos).expand((file) => scanFile(file, wordCondition: wordCondition))
        .map((w) => w.item2.text));

Iterable<File> scanFilesLow(Iterable<FileInfo> infos) =>
    infos.map((fi) => File.fromFileInfo(fi));

Iterable<Tuple2<Facts, Word>> scanFile(File file,
        {bool wordCondition(Word w)}) =>
    file.factss.expand((fs) => fs.facts
        .expand((f) => f.words.where(wordCondition ??
            (w) =>
                w.text.isNotEmpty &&
                (w.flags & WordFlags.wInBr == 0) &&
                (w.flags & WordFlags.wIsPartOf == 0) &&
                (w.flags & WordFlags.wBrSq == 0) &&
                (w.flags & WordFlags.wBrCurl == 0)))
        .map((w) => Tuple2(fs, w)));

Iterable<Tuple3<FileInfo, Facts, Word>> scanFileWords(DataMsg msg,
        {bool wordCondition(Word w)}) =>
    scanFiles(msg).expand(
        (file) => scanFile(file, wordCondition: wordCondition).map((fw) {
              return Tuple3(
                  FileInfo.infoFromPath(file.fileName), fw.item1, fw.item2);
            }));
