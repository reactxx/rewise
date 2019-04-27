import 'dart:collection';
import 'dart:convert' as conv;
import 'package:rw_utils/dom/spellCheck.dart' as dom;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/threading.dart';
import 'package:rw_utils/utils.dart' as ut;
import '../sources/filer.dart';
import '../sources/consts.dart';
import '../sources/dom.dart';
import 'cache.dart';

Future spellCheck({bool doParallel, bool emptyPrint}) async =>
    useSources(_spellCheckEntryPoint, _spellCheck, GroupByType.dataLang,
        emptyPrint: emptyPrint, doParallel: doParallel);

Future spellCheckLow(SCCache cache, Iterable<String> words) async {
  final checkReq = dom.Request()..lang = cache.lang;
  final toCheck = cache.toCheck(words);
  if (toCheck.length == 0) return Future.value();

  Future runChunk(Iterable<String> seq) async {
    checkReq.html = wordsToHTML(cache.lang, seq);
    final resp = await client.Spellcheck_Spellcheck(checkReq);
    cache.addWords(seq, resp.wrongIdxs);
    return Future.value();
  }

  if (toCheck.length <= _maxLen) return runChunk(toCheck);

  for (var intl in ut.Interval.intervals(toCheck.length, _maxLen))
    await runChunk(toCheck.skip(intl.skip).take(intl.take).toList());
  return Future.value();
}

const _maxLen = 5000;
const _tableHtmlRowLen = 0xff;

String wordsToHTML(String lang, Iterable<String> words,
    {bool toTable = false}) {
  final res = StringBuffer();
  // for TABLE:
  var tableCount = 0;
  void row(bool inCycle) {
    if (!toTable) return;
    if (inCycle) {
      if (tableCount == 0)
        res.write('<tr>');
      else if (tableCount & _tableHtmlRowLen == 0) res.write('</tr><tr>');
      tableCount++;
    } else {
      for (var i = 0; i < _tableHtmlRowLen - (tableCount & _tableHtmlRowLen); i++) res.write('<td> </td>');
      res.write('</tr>');
    }
  }

  var isEmpty = true;
  res.write("<html lang=\"");
  res.write(lang);
  res.write("\"><head><meta charset=\"UTF-8\"></head><body>");
  if (toTable) res.write('<table>');
  for (final w in words) {
    isEmpty = false;
    row(true);
    res.write(toTable ? "<td>" : "<p>");
    res.write(conv.htmlEscape.convert(w));
    res.write(toTable ? "</td>" : "</p>");
  }
  row(false);
  if (toTable) res.write('</table>');
  res.write("</body></html>");
  return isEmpty ? '' : res.toString();
}

bool defaultWordCondition(Word w) =>
    w.text.isNotEmpty &&
    (w.flags & WordFlags.wInBr == 0) &&
    (w.flags & WordFlags.wHasParts == 0) &&
    (w.flags & WordFlags.wBrSq == 0) &&
    (w.flags & WordFlags.wBrCurl == 0) &&
    (w.flags & WordFlags.nonLetter == 0);
//(w.flags & WordFlags.ok != 0)); // !!! only OK words are checked !!!

void _spellCheckEntryPoint(List workerInitMsg) =>
    parallelEntryPoint(workerInitMsg, _spellCheck);

Future<Msg> _spellCheck(DataMsg msg, InitMsg initPar) async {
  final allWords = scanFileWords(msg, wordCondition: defaultWordCondition);

  final words = HashSet<String>.from(allWords.map((w) => w.item3.text));

  FileInfo first = scanFileInfos(msg).first;
  print('${first.dataLang}: ${words.length}');

  final cache = SCCache.fromLang(first.dataLang);

  await spellCheckLow(cache, words);

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
