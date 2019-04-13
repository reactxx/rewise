import 'package:rw_utils/rewise.dart' as rew;
import 'package:rw_utils/sources.dart' as s;
import 'wrongFacts.dart';
//import 'package:rw_utils/utils.dart' show Matrix;

main() async {

  // var file = s.File.fromMatrix(Matrix.fromFile(r'd:\rewise\data\02_source\en-GB\#cambridge\ru-RU.csv'));
  // var count = await s.refreshFileLow(file);
  // return Future.value(count);

  //await s.importCSVFiles();
  //await s.refreshFiles();
  await exportWrongFacts();
  return Future.value();
}

main_() async {
  await rew.toRaw();
  await rew.toParsed();
  await rew.toStemmCache();
  await rew.doStat();
  rew.stemmStat();
  return Future.value();
}
