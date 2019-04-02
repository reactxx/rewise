import 'package:rw_utils/rewise.dart' as rew;

main() async {
  await rew.toRaw();
  await rew.toParsed();
  await rew.toStemmCache();
  await rew.doStat();
  rew.stemmStat();
  return Future.value();
}
