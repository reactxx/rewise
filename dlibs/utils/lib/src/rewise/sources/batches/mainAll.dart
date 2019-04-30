import 'package:rw_utils/sources.dart' as s;
import 'wrongFacts.dart';
import 'analyzeSources.dart';
import 'cyrillic.dart';
import '../filer.dart';
import '../consts.dart'; 
import '../../spellcheck/spellcheck.dart';
import '../../spellcheck/analyze.dart';

main() async {
  //await s.importCSVFiles(doParallel: null, emptyPrint: true);

  //await s.refreshFiles(doParallel: null, emptyPrint: true, force: false);
  // await s.refreshFiles(
  //     doParallel: null,
  //     emptyPrint: true,
  //     force: true,
  //     filter: filter);
  //|| grp.values.first.bookName == '#goetheverlag');
  // await exportWrongFacts(doParallel: true, emptyPrint: false);
  // await importWrongFacts(doParallel: false, emptyPrint: false);
  //await analyzeSources(doParallel: null, groupBy: GroupByType.fileNameDataLang, emptyPrint: false);
  //await analyzeSources(doParallel: null, groupBy: GroupByType.dataLang, emptyPrint: false);
  // cyrillic();
  //await spellCheck();
  //dumpSpellCaches();
  dumpSpellCheckFiles(filter: filter);
  //dumpSpellCheckFiles(bookName: '#goetheverlag');
  return Future.value();
}

//bool filter(FileInfo fi) => fi.bookName == '#eurotalk' && fi.dataLang=='cs-CZ';
bool filter(FileInfo fi) => fi.bookName == '#kdictionaries' && fi.dataLang=='ar-SA';
//bool filter(FileInfo fi) => true;