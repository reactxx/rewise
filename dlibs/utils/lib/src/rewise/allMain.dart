import 'package:rw_utils/rewise.dart' as rew;
import 'package:rw_utils/sources.dart' as s;
import 'package:rw_utils/utils.dart' show Matrix;

main() async {

  var file = s.File.fromMatrix(Matrix.fromFile(r'h:\rewise\data\02_source\en-GB\#cambridge\th-TH.csv'));
  var count = await s.refreshFileLow(file);
  if (count==0) return Future.value();

  await s.importCSVFiles();
  await s.refreshFiles();
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
