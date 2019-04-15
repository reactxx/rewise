import '../reparseFacts.dart';

main() async {
  await refreshFiles(force: false);
  print('DONE refreshFiles');
}