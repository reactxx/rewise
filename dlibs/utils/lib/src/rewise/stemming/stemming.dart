import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/langs.dart' show Langs;
import 'package:rw_utils/dom/stemming.dart' as stemm;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_low/code.dart' show Linq;
//import 'package:rw_utils/stemming.dart' as stemm;

Future toStemmCache() async {
   final stemmLangs =
       Set.from(Langs.meta.where((m) => m.hasStemming).map((m) => m.id));
  //final stemmLangs = ['cs-CZ'];
  var count = 0;
  for (final fn
      in fileSystem.parsed.list(regExp: fileSystem.devFilter + r'msg$')) {
    final books =
        toPars.ParsedBooks.fromBuffer(fileSystem.parsed.readAsBytes(fn));
    for (var book in books.books.where((b) => stemmLangs.contains(b.lang))) {
      final texts =
          Linq.distinct(book.facts.expand((f) => f.childs.expand((sf) {
                final txt = sf.breakText.isEmpty ? sf.text : sf.breakText;
                return _wordsTostemm(txt, sf.breaks);
              })));

      final req = stemm.Request()
        ..lang = book.lang
        ..words.addAll(texts);
      final resp = await client.Stemming_Stemm(req);
      for (var i = 0; i < resp.words.length; i++) {
        count += resp.words[i].stemms.length;
      }
    }
  }
  return Future.value(count);
}

Iterable<String> _wordsTostemm(String text, List<int> breaks) sync* {
  if (breaks == null || breaks.length == 0) return;
  for (var i = 0; i < breaks.length; i += 2)
    yield text.substring(breaks[i], breaks[i] + breaks[i + 1]);
  // var lastPos = 0;
  // for (var i = 0; i < breaks.length; i += 2) {
  //   final pos = lastPos + breaks[i];
  //   final end = pos + breaks[i + 1];
  //   final t = text.substring(pos, end);
  //   yield t;
  //   lastPos = end;
  // }
}

main() async {
  await toStemmCache();
}

/*
message Request {
  string lang = 1;
  repeated string words = 2;
}
message Response {
  repeated Word words = 1;
}
message Word {
  repeated string stemms = 1;
  int32 ownLen = 2; // words[0..ownLen-1] are words, with stemms's stemming result
}

*/
