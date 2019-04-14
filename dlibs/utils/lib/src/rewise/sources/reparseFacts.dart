import 'dom.dart';
import 'package:rw_utils/dom/word_breaking.dart' as wb;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/threading.dart';
import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_utils/langs.dart' show Langs;
import 'filer.dart';
import 'parser.dart';

Future refreshFiles({force = false}) async {
  //final all = Filer.files.where((f) => f.bookName=='#dictcc').toList();
  final all = Filer.files;
  if (fileSystem.desktop) {
    final tasks = all.map((f) => ArrayMsg.encode([f.fileName, force]));
    await Parallel(tasks, 4, _entryPoint, taskLen: all.length).run(
        //traceMsg: (count, msg) => print('$count/${all.length} - ${msg[1]}'));
        traceMsg: (count, msg) => {});
  } else {
    var count = 0;
    for (final f in all) {
      print('${++count}/${all.length} - ${f.fileName}');
      await _refreshFile(ArrayMsg([f.fileName, force]));
    }
  }
  return Future.value();
}

void _entryPoint(List workerInitMsg) =>
    parallelEntryPoint<ArrayMsg>(workerInitMsg, _refreshFile);

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
      //print(req.facts.length.toString());

      final resp = await client.WordBreaking_Run2(req);

      for (final breaked in resp.facts) {
        final oldFact = file.factss[breaked.id];
        assert(oldFact.id == breaked.id);
        try {
          file.factss[breaked.id] = reparseFact(langMeta, oldFact, breaked.text, breaked.posLens);
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

Future<List> _refreshFile(ArrayMsg msg) async {
  final file = File.fromPath(msg.listValue[0]);
  final modified = await refreshFileLow(file, msg.listValue[1]);
  if (modified > 0) file..save();

  return Parallel.workerReturnFuture;
}

const maxFacts = 40000;
//const maxFacts = 1;
