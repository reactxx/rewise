import 'dart:collection';
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/utils.dart' show fileSystem, Matrix;
import 'package:rw_utils/langs.dart' show Langs, Unicode;
import 'package:rw_utils/threading.dart';
import '../filer.dart';
import '../consts.dart';
import '../dom.dart';

List _groupByDataLangOnly(FileInfo f) => [f.dataLang];

Future analyzeLangs({bool emptyPrint = false, bool doParallel}) async =>
    useSources(_analyzeLangsEntryPoint, _analyzeLangs,
        groupBy: _groupByDataLangOnly,
        emptyPrint: emptyPrint,
        printDetail: (l) => '${l.listValue[0]}',
        doParallel: doParallel);

void _analyzeLangsEntryPoint(List workerInitMsg) =>
    parallelEntryPoint(workerInitMsg, _analyzeLangs);

Future<Msg> _analyzeLangs(DataMsg msg) {
  FileInfo anyFile;
  final words = scanFiles(msg)
      .expand((f) => (anyFile = f).factss)
      .expand((fs) => fs.facts)
      .expand((f) => f.words)
      .map((w) => w.text)
      .where((w) => w != null && w.isNotEmpty)
      .toList();
  final wordsChars = Linq.sum(words.map((w) => w.length));
  final uniqueWords = HashSet<String>.from(words);
  final uniqueWordsChars = Linq.sum(uniqueWords.map((w) => w.length));
  final chars = HashSet<int>.from(uniqueWords.expand((w) => w.codeUnits));

  fileSystem.edits.writeAsString('analyzeLangs\\${anyFile.dataLang}.txt',
      'words=${words.length}, wordsChars=$wordsChars, uniqueWords=${uniqueWords.length}, uniqueWordsChars=$uniqueWordsChars, chars=${chars.length}');

  return Parallel.workerReturnFuture;
}
