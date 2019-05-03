import 'dart:convert' as conv;
import 'package:rw_utils/utils.dart' as ut;
import 'package:path/path.dart' as p;
import 'package:rw_utils/threading.dart';
import '../sources/filer.dart';
import '../sources/consts.dart';
import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_utils/langs.dart' show Langs;

Future toTrans({bool doParallel, bool emptyPrint}) async {
  await useSources(_toTransEntryPoint, _toTrans, GroupByType.dataLang,
      emptyPrint: emptyPrint, doParallel: doParallel, filter: _filter);
  var files = fileSystem.transTasks
      .list(regExp: r'\.html$')
      .map((f) => p.basenameWithoutExtension(f))
      .toList();
  fileSystem.transTasks.writeAsString('_content.json', conv.jsonEncode(files));
  fileSystem.transTasks.writeAsString('_content.html', _contentHtml(files));
}

bool _filter(FileInfo fi) => Langs.toGoogleTrans(fi.dataLang).isNotEmpty;

void _toTransEntryPoint(List workerInitMsg) =>
    parallelEntryPoint(workerInitMsg, _toTrans);

Future<Msg> _toTrans(DataMsg msg, InitMsg initPar) async {
  Iterable<FileInfo> infos = scanFileInfos(msg);
  FileInfo first = infos.first;

  var googleLang = Langs.nameToMeta[first.dataLang].googleTransId;
  var wset = uniqueFilesWords(infos);
  var count = 0;
  for (var intv in ut.Interval.intervalsMaxLen(wset.length, 5000)) {
    var sb = StringBuffer();
    sb.write(_htmlStart(googleLang));
    for (var w in wset.skip(intv.skip).take(intv.take)) sb.write(_htmlWord(w));
    sb.write(_htmlEnd);
    fileSystem.transTasks.writeAsString(
        '${first.dataLang}.${count.toString()}.html', sb.toString());
    count++;
  }

  return Parallel.workerReturnFuture;
}

String _htmlStart(String lang) =>
    '''<!DOCTYPE html>
<html lang="''' +
    lang +
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
  <div>
''';
String _htmlWord(String w) => '<p>${conv.htmlEscape.convert(w)}</p>';
String _htmlEnd = '''
  </div>
</body>
</html>
''';

String _contentHtml(List<String> urls) {
  var sb = StringBuffer();
  sb.write(_contentStart);
  for (var url in urls) sb.write(_contentItem(url));
  sb.write(_contentEnd);
  return sb.toString();
}
String _contentStart = '''<!DOCTYPE html>
<html>
<head>
  <meta charset="UTF-8">
  <title>Web App</title>
</head>
<body>
''';
String _contentItem(String url) => '<p><a href="$url.html">$url</a></p>';
String _contentEnd = '''
</body>
</html>
''';
