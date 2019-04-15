import 'package:rw_utils/sources.dart' as s;
import 'wrongFacts.dart';

main() async {
  // await s.importCSVFiles(doParallel: true, emptyPrint: false);
  // await s.refreshFiles(doParallel: true, emptyPrint: false, force: true);
  await exportWrongFacts(doParallel: true, emptyPrint: false);
  // await importWrongFacts(doParallel: false, emptyPrint: false);
  return Future.value();
}
