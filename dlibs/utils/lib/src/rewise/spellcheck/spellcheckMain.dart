import 'spellcheck.dart';
import '../sources/filer.dart';

main() async {
  await spellCheck(doParallel: null, groupBy: GroupByType.dataLang);
  return Future.value();
}
