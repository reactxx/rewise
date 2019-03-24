import 'package:rw_utils/utils.dart' show fileSystem, hackToJson;
import 'package:rw_utils/rewise.dart' as rewise;
import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/langs.dart' show Langs;
import 'package:rw_utils/dom/stemming.dart' as stemm;
import 'package:rw_utils/stemming.dart' as stemm;

const _devFilter = r'goetheverlag\.msg';

Future toStemmCache (rewise.ParseBookResult parsed) async {
  final stemmLangs = Set.from(Langs.meta.where((m) => m.hasStemming).map((m)=> m.id));

  for (final fn in fileSystem.parsed.list(regExp: _devFilter)) {
      final books = toPars.ParsedBooks.fromBuffer(fileSystem.parsed.readAsBytes(fn));
      for(var book in books.books.where((b) => stemmLangs.contains(b.lang))) {
        final request = stemm.Request().writeToBuffer();
      }
  }
}

