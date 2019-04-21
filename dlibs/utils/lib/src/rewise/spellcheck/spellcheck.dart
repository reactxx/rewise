import 'dart:collection';
import 'package:rw_utils/dom/spellCheck.dart' as dom;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/threading.dart';
import '../sources/filer.dart';
import '../sources/consts.dart';
import 'cache.dart';

Future spellCheck(
        {bool doParallel,
        GroupByType groupBy = GroupByType.fileNameDataLang,
        bool emptyPrint}) async =>
    useSources(_spellCheckEntryPoint, _spellCheck, groupBy,
        emptyPrint: emptyPrint, doParallel: doParallel);

void _spellCheckEntryPoint(List workerInitMsg) =>
    parallelEntryPoint(workerInitMsg, _spellCheck);

Future<Msg> _spellCheck(DataMsg msg, InitMsg initPar) async {
  final allWords = scanFileWords(msg,
      wordCondition: (w) =>
          w.text.isNotEmpty &&
          (w.flags & WordFlags.wInBr == 0) &&
          (w.flags & WordFlags.wHasParts == 0) &&
          (w.flags & WordFlags.wBrSq == 0) &&
          (w.flags & WordFlags.wBrCurl == 0) &&
          (w.flags & WordFlags.nonLetter == 0));

  final words = HashSet<String>.from(allWords.map((w) => w.item3.text));

  FileInfo first = scanFileInfos(msg).first;
  await spellCheckLow(first.dataLang, words);
  return Parallel.workerReturnFuture;
}

Future spellCheckLow(String lang, Iterable<String> words) async {
  final cache = SCCache.fromLang(lang);
  final checkReq = dom.Request()..lang = lang;
  checkReq.words.addAll(cache.toCheck(words));
  if (checkReq.words.length == 0) return Future.value();
  final resp = await client.Spellcheck_Spellcheck(checkReq);
  cache.addWords(checkReq.words, resp.wrongIdxs);
}
