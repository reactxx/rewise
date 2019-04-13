import '../refresh.dart';

main() async {
  await refreshFiles(reparse: false);
  print('DONE refreshFiles');
}