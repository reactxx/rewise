import 'dart:convert' as conv;
import 'package:rw_utils/utils.dart' as ut;
import 'package:rw_utils/threading.dart';
import '../sources/filer.dart';
import '../sources/consts.dart';
import 'package:rw_utils/utils.dart' show fileSystem;


Future toTrans({bool doParallel, bool emptyPrint}) async =>
    useSources(_toTransEntryPoint, _toTrans, GroupByType.dataLang,
        emptyPrint: emptyPrint, doParallel: doParallel);

void _toTransEntryPoint(List workerInitMsg) =>
    parallelEntryPoint(workerInitMsg, _toTrans);

Future<Msg> _toTrans(DataMsg msg, InitMsg initPar) async {
  Iterable<FileInfo> infos = scanFileInfos(msg);
  FileInfo first = infos.first;

  var wset = uniqueFilesWords(infos);
  var count = 0;
  for(var intv in ut.Interval.intervalsMaxLen(wset.length, 10000)) {
    var sb = StringBuffer();
    sb.write(_htmlStart(first.dataLang));
    for(var w in wset.skip(intv.skip).take(intv.take)) sb.write(_htmlWord(w));
    sb.write(_htmlEnd);
    fileSystem.transTasks.writeAsString('${first.dataLang}.${count.toString()}.html', sb.toString());
    count++;
  }

  return Parallel.workerReturnFuture;
}

String _htmlStart (String lang) => 
'''<!DOCTYPE html>

<html lang="'''
+ lang +
'''">

<head>
  <meta charset="UTF-8">
  <title>Web App</title>
  <script defer src="../trans_main.dart.js"></script>
  <style>
    p {display: inline-block; width: 100px; margin: 0; white-space: nowrap; }
  </style>
</head>

<body>
  <h1 id="title"></h1>
  <div>
''';
String _htmlWord (String w) => '<p>${conv.htmlEscape.convert(w)}</p>';

String _htmlEnd = '''
  </div>
</body>

</html>
''';