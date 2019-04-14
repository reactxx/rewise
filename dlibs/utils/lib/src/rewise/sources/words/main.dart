import 'package:rw_utils/langs.dart' show CldrLang;

import 'package:rw_utils/threading.dart';
import '../filer.dart';

Future exportWrongFacts({bool emptyPrint = true}) async =>
    useSources(_entryPoint, _action, emptyPrint: emptyPrint, groupBy: groupByDataLang);

void _entryPoint(List workerInitMsg) =>
    parallelEntryPoint(workerInitMsg, _action);

Future<Msg> _action(DataMsg msg) {
  for (final file in scanFiles(msg)) {
  }
  return Parallel.workerReturnFuture;
}


analyzeWord(String word, CldrLang lang, CldrLang langOther, bool inBracket) {

}
//https://en.wikipedia.org/wiki/Combining_Diacritical_Marks