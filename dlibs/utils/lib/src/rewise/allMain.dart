import 'package:rw_utils/rewise.dart' as rew;
import 'package:rw_utils/sources.dart' as s;

main() async {
  //await s.importCSVFiles();
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
