import '../reparseFacts.dart';

main() async {
  await refreshFiles(doParallel: null, emptyPrint: true, force: false);
  print('DONE refreshFiles');
}