import '../fromCSV.dart';

main() async {
  await importCSVFiles(emptyPrint: false);
  print('DONE importCSVFiles');
}