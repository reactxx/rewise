import 'dart:collection';
import 'package:rw_utils/dom/spellCheck.dart' as dom;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/threading.dart';
import '../sources/filer.dart';
import '../sources/consts.dart';
import 'cache.dart';

Future spellCheck({bool doParallel, bool emptyPrint}) async =>
    useSources(_spellCheckEntryPoint, _spellCheck, GroupByType.dataLang,
        emptyPrint: emptyPrint, doParallel: doParallel);

Future spellCheckLow(String lang, Iterable<String> words) async {
  final cache = SCCache.fromLang(lang);
  final checkReq = dom.Request()..lang = lang;
  checkReq.words.addAll(cache.toCheck(words));
  if (checkReq.words.length == 0) return Future.value();
  final resp = await client.Spellcheck_Spellcheck(checkReq);
  cache.addWords(checkReq.words, resp.wrongIdxs);
}

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
  print('${first.dataLang}: ${words.length}');

  await spellCheckLow(first.dataLang, words);
  return Parallel.workerReturnFuture;
}

/*
Wrong lang: bo-CN
Wrong lang: br-FR
Wrong lang: co-FR
Wrong lang: km-KH
Wrong lang: mn-MN
Wrong lang: oc-FR
Wrong lang: qu-PE
Wrong lang: be-BY
Wrong lang: eo-001
 */
