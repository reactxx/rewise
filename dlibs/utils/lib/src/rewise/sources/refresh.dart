import 'dom.dart';
import 'package:rw_utils/dom/word_breaking.dart' as wb;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/threading.dart';
import 'package:rw_utils/utils.dart' show fileSystem;

Future refreshFiles() async {
  final all = Filer.files;
  if (fileSystem.desktop) {
    final tasks = all.map((f) => StringMsg.encode(f.path));
    await Parallel(tasks, 4, _entryPoint, taskLen: all.length).run(
        traceMsg: (count, msg) => print('$count/${all.length} - ${msg[1]}'));
  } else {
    for (final f in all) await _refreshFile(StringMsg(f.path));
  }
  return Future.value();
}

void _entryPoint(List workerInitMsg) =>
    parallelEntryPoint<StringMsg>(workerInitMsg, _refreshFile);

Future<List> _refreshFile(StringMsg msg) async {
  while (true) {
    final file = File.fromPath(msg.strValue);
    final req = wb.Request2()
      ..lang = file.dataLang
      ..path = file.fileName;

    var factsRemaining = false;
    for(final f in file.factss) {
      final txt = Facts.toRefresh(f);
      if (txt == null) continue;
      if (req.facts.length>=maxFacts) {
        factsRemaining = true;
        break;
      }
      req.facts.add(wb.FactReq()
            ..text = txt
            ..id = f.id);
    }

    if (req.facts.length == 0) return Parallel.workerReturnFuture;

    final resp = await client.WordBreaking_Run2(req);

    for (final f in resp.facts) {
      final src = file.factss[f.id];
      assert(src.id == f.id);
      file.factss[f.id] = Facts.fromParser(src, f.text, f.posLens);
    }
    file..save()..toCSV(fileSystem.sourceCsv);

    if (!factsRemaining) break;
  }

  return Parallel.workerReturnFuture;
}

const maxFacts = 40000;
//const maxFacts = 1;
