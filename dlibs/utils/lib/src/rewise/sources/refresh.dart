import 'dom.dart';
import 'package:rw_utils/dom/word_breaking.dart' as wb;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_utils/threading.dart';
import 'package:rw_utils/utils.dart' show fileSystem;
import 'filer.dart';
import 'parser.dart';

const reparse = true;

Future refreshFiles() async {
  //final all = Filer.files.where((f) => f.bookName=='#dictcc').toList();
  final all = Filer.files;
  if (fileSystem.desktop) {
    final tasks = all.map((f) => StringMsg.encode(f.fileName));
    await Parallel(tasks, 4, _entryPoint, taskLen: all.length).run(
        //traceMsg: (count, msg) => print('$count/${all.length} - ${msg[1]}'));
        traceMsg: (count, msg) => {});
  } else {
    var count = 0;
    for (final f in all) {
      print('${++count}/${all.length} - ${f.fileName}');
      await _refreshFile(StringMsg(f.fileName));
    }
  }
  return Future.value();
}

void _entryPoint(List workerInitMsg) =>
    parallelEntryPoint<StringMsg>(workerInitMsg, _refreshFile);

Future<int> refreshFileLow(File file) async {
  final req = wb.Request2()
    ..lang = file.dataLang
    ..path = file.fileName;
  var modifiedCount = 0;
  
  for (var i = 0; i < file.factss.length; i++) {
    var f = file.factss[i];
    final txt = f.toRefresh(reparse: reparse);
    if (txt == null || txt.isEmpty) continue;
    req.facts.add(wb.FactReq()
      ..text = txt
      ..id = f.id);
    final lastFact = i == file.factss.length - 1;
    if (lastFact || req.facts.length >= maxFacts) {
      //print(req.facts.length.toString());

      final resp = await client.WordBreaking_Run2(req);

      for (final f in resp.facts) {
        final oldFacts = file.factss[f.id];
        assert(oldFacts.id == f.id);
        try {
          file.factss[f.id] = fromNewText(oldFacts, f.text, f.posLens);
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

Future<List> _refreshFile(StringMsg msg) async {
  final file = File.fromPath(msg.strValue);
  final modified = await refreshFileLow(file);
  if (modified > 0) file..save();

  return Parallel.workerReturnFuture;
}

const maxFacts = 40000;
//const maxFacts = 1;
