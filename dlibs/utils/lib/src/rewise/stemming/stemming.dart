import 'dart:io' as io;
import 'package:path/path.dart' as p;
import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:rw_utils/langs.dart' show Langs;
import 'package:rw_utils/dom/stemming.dart' as stemm;
import 'package:rw_utils/client.dart' as client;
import 'package:rw_low/code.dart' show Linq;
import 'package:rw_utils/rewise.dart' show BreakConverter;
import 'package:rw_utils/toBinary.dart' as bin;
import 'package:rw_utils/threading.dart';

import 'cache/cache.dart';
import '../parallel.dart';

Future toStemmCache() async {
  final stemmLangs =
      Set.from(Langs.meta.where((m) => m.hasStemming).map((m) => m.id));

  final fileLangs = Set.from(fileSystem.parsed.list(regExp: r'\.msg$').map((f) {
    final ps = p.split(f).last.split('.');
    return ps[ps.length - 2];
  }));

  final existedLangs = stemmLangs.intersection(fileLangs);

  print('***** LANGS: ${existedLangs.length}: $existedLangs');

  return Future.wait(existedLangs.map((lang) async {
    if (fileSystem.desktop) {
      final tasks = existedLangs.map((lang) => StringMsg.encode(lang));
      await ParallelString(tasks, tasks.length, _entryPoint, 3);
    } else {
      await _toStemmCacheLang(lang);
    }
    return Future.value();
  }));
}

void _entryPoint(List workerInitMsg) =>
    parallelStringEntryPoint(workerInitMsg, _toStemmCacheLang);

Future<List> _toStemmCacheLang(StringMsg msg) async {
  final lang = msg.relPath;
  final fn = fileSystem.stemmCache.adjustExists('$lang\cache.bin');

  StemmCache cache;
  bin.StreamReader.fromPath(fn).use((rdr) => cache = StemmCache(rdr));

  final files = fileSystem.parsed.list(regExp: lang + r'\.msg$').toList();
  print('***** $lang START ${files.length} files');
  for (var bookFn in files) {
    //.list(regExp: r'^wordlists\\.*\\' + lang + r'.msg$')) {
    final book =
        toPars.ParsedBook.fromBuffer(fileSystem.parsed.readAsBytes(bookFn));
    final texts = Linq.distinct(book.facts.expand((f) => f.childs.expand((sf) {
          final txt = sf.breakText.isEmpty ? sf.text : sf.breakText;
          return BreakConverter.getStemms(txt, sf.breaks)
              .where((w) => !cache.words.containsKey(w.toLowerCase()));
        }))).toList();

    // all words are already stemmed => return
    if (texts.length == 0) {
      print('  -.$lang.$bookFn');
      continue;
    }

    final bookStemms = await client.Stemming_Stemm(stemm.Request()
      ..lang = book.lang
      ..words.addAll(texts));

    bin.StreamWriter.fromPath(fn, mode: io.FileMode.append)
        .use((wr) => cache.importStemmResults(bookStemms.words, wr));
    print('  + .$lang.$bookFn');
  }

  print('***** $lang END');
  return Parallel.workerReturnValue;
}

