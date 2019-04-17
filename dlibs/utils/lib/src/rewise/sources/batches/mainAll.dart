import 'package:rw_utils/sources.dart' as s;
import 'wrongFacts.dart';
import 'analyzeSources.dart';
import 'cyrillic.dart';
import '../filer.dart';

main() async {
  //await s.importCSVFiles(doParallel: null, emptyPrint: true);
  //await s.refreshFiles(doParallel: null, emptyPrint: true, force: false);
  // await exportWrongFacts(doParallel: true, emptyPrint: false);
  // await importWrongFacts(doParallel: false, emptyPrint: false);
  await analyzeSources(doParallel: null, groupBy: GroupByType.fileNameDataLang, emptyPrint: false);
  await analyzeSources(doParallel: null, groupBy: GroupByType.dataLang, emptyPrint: false);
  // cyrillic();
  return Future.value();
}
