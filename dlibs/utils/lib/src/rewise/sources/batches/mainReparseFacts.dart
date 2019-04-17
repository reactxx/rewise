import '../reparseFacts.dart';

main() async {
  await refreshFiles(force: false, emptyPrint: false);
  print('DONE refreshFiles');
}