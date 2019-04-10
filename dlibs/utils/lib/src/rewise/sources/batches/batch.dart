import 'package:rw_utils/utils.dart' show fileSystem;
import '../dom.dart';
import '../filer.dart';

//en-GB\#cambridge\th-TH.csv
//de-DE\gcg_a2\.left.csv
class Batch {
  static void run() {
    final all = Filer.files.where((f) => f.bookName == '#lingea').toList();
    for (final f in all) {
      final file = f.readFile;
      final wrongFacts =
          file.factss.where((f) => f.facts.any((ff) => ff.flags != 0));
      if (wrongFacts.isEmpty) continue;
      file.save(dir: fileSystem.edits, editMode: EditMode.ASSTRING, subFactss: wrongFacts);
    }
  }
}
