import 'package:rw_utils/stemming.dart' as st;

main() async {
  await st.toStemmCache();
  return Future.value();
}
