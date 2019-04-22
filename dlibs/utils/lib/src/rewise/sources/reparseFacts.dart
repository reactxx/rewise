import 'dom.dart';
import 'package:rw_utils/dom/word_breaking.dart' as wb;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/threading.dart';
import 'package:rw_utils/langs.dart' show Langs;
import '../spellcheck/cache.dart' show SCCache;
import '../spellcheck/spellcheck.dart' show spellCheckLow, defaultWordCondition;
import 'filer.dart';
import 'parser.dart';
import 'consts.dart';

Future refreshFiles(
        {bool force = false, bool doParallel, bool emptyPrint}) async =>
    useSources(_entryPoint, _refreshFile, GroupByType.dataLang,
        initPar: [force], emptyPrint: emptyPrint, doParallel: doParallel);

void _entryPoint(List workerInitMsg) =>
    parallelEntryPoint(workerInitMsg, _refreshFile);

Future<int> refreshFileLow(File file, bool force) async {
  final req = wb.Request2()
    ..lang = file.dataLang
    ..path = file.fileName;
  var modifiedCount = 0;

  final langMeta = Langs.nameToMeta[file.dataLang];

  for (var i = 0; i < file.factss.length; i++) {
    var f = file.factss[i];
    final txt = toRefresh(f, langMeta, force: force);
    if (txt != null && txt.isNotEmpty)
      req.facts.add(wb.FactReq()
        ..text = txt
        ..id = f.id);
    final lastFact = i == file.factss.length - 1;
    if (lastFact && req.facts.length > 0 || req.facts.length >= maxFacts) {
      // server side word breaking
      final resp = await client.WordBreaking_Run2(req);

      for (final breakedFact in resp.facts) {
        final oldFact = file.factss[breakedFact.id];
        assert(oldFact.id == breakedFact.id);
        try {
          file.factss[breakedFact.id] = reparseFact(
              langMeta, oldFact, breakedFact.text, breakedFact.posLens);
        } catch (e) {
          print('** ERROR in ${file.fileName}');
          rethrow;
        }
      }
      modifiedCount += req.facts.length;
      req.facts.clear();
    }
  }
  return Future.value(modifiedCount);
}

Future<Msg> _refreshFile(DataMsg msg, InitMsg initPar) async {
  final bool force = msg.listValue[0];
  FileInfo first = scanFileInfos(msg, skip: 1).first;
  final spell = SCCache.fromLang(first.dataLang);
  for (final file in scanFiles(msg, skip: 1)) {
    final modified = await refreshFileLow(file, force);
    if (modified == 0) continue;
    // fill spellCheck word flag
    final words = file.factss.expand(
        (fs) => fs.facts.expand((f) => f.words.where(defaultWordCondition))).toList();
    await spellCheckLow(spell, words.map((w) => w.text));
    for (final word in words)
      word.flags |= spell.words[word.text] ? WordFlags.okSpell : 0;
    file..save();
  }

  return Parallel.workerReturnFuture;
}

const maxFacts = 40000;
//const maxFacts = 1;
