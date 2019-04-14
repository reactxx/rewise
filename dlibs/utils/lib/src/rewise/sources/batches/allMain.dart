import 'package:rw_utils/sources.dart' as s;
import 'wrongFacts.dart';

main() async {
  await s.importCSVFiles(doParallel: true, emptyPrint: false);
  await s.refreshFiles(force: true, doParallel: true, emptyPrint: false);
  await exportWrongFacts(doParallel: true, emptyPrint: false);
  return Future.value();
}
