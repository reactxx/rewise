import 'dom.dart';
import 'package:rw_utils/dom/word_breaking.dart' as wb;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/threading.dart';
import 'package:rw_utils/langs.dart' show Langs;
import 'filer.dart';
import 'parser.dart';

Future refreshFiles(
    {bool force = false, bool emptyPrint = true, bool doParallel}) async {
  final tasks = Filer.files.map((f) => DataMsg([f.fileName, force])).toList();
  return processTasks(_entryPoint, _refreshFile, tasks,
      emptyPrint: emptyPrint,
      doParallel: doParallel,
      printDetail: (l) => l.listValue[0]);
}

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

      for (final breaked in resp.facts) {
        final oldFact = file.factss[breaked.id];
        assert(oldFact.id == breaked.id);
        try {
          file.factss[breaked.id] =
              reparseFact(langMeta, oldFact, breaked.text, breaked.posLens);
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
  //print(msg.listValue[0]);
  final file = File.fromPath(msg.listValue[0]);
  final modified = await refreshFileLow(file, msg.listValue[1]);
  if (modified > 0) file..save();

  return Parallel.workerReturnFuture;
}

const maxFacts = 40000;
//const maxFacts = 1;
