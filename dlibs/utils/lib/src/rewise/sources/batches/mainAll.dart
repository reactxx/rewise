import 'package:rw_utils/sources.dart' as s;
import 'wrongFacts.dart';
import 'analyzeSources.dart';
import 'cyrillic.dart';
import '../filer.dart';
import '../../spellcheck/spellcheck.dart';
import '../../spellcheck/analyze.dart';

main() async {
  //await s.importCSVFiles(doParallel: null, emptyPrint: true);
  //await s.refreshFiles(doParallel: null, emptyPrint: true, force: false);
  await s.refreshFiles(
      doParallel: null,
      emptyPrint: true,
      force: false,
      groupByType: s.GroupByType.fileNameDataLang,
      groupFilter: (grp) => grp.values.first.bookName=='#eurotalk' || grp.values.first.bookName=='#goetheverlag');
  // await exportWrongFacts(doParallel: true, emptyPrint: false);
  // await importWrongFacts(doParallel: false, emptyPrint: false);
  //await analyzeSources(doParallel: null, groupBy: GroupByType.fileNameDataLang, emptyPrint: false);
  //await analyzeSources(doParallel: null, groupBy: GroupByType.dataLang, emptyPrint: false);
  // cyrillic();
  //await spellCheck();
  //dumpSpellCaches();
  //dumpSpellCheckFiles(bookName: '#eurotalk');
  //dumpSpellCheckFiles(bookName: '#goetheverlag');
  return Future.value();
}
