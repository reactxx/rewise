import 'dart:collection';
import 'package:rw_utils/dom/to_parsed.dart' as toPars;
import 'package:path/path.dart' as p;
import 'package:rw_utils/utils.dart' show fileSystem;
import 'toMatrix.dart';

class Word {
  Word(this.text, this.count, this.bookIds, this.wrongUnicode, this.wrongCldr);
  String text;
  int count;
  HashSet<int> bookIds;
  String wrongCldr;
  String wrongUnicode;
}

class Bracket {
  Bracket(this.value, this.bookIds);
  final String value;
  int count = 1;
  final HashSet<int> bookIds;
}

class StatLang {
  String lang;
  final ok = HashMap<String, Word>();
  final latin = HashMap<String, Word>();
  final wrongs = HashMap<String, Word>();
  final okAlpha = HashSet<int>();
  final wrongsCldrAlpha = HashSet<int>();
  final wrongsUnicodeAlpha = HashSet<int>();
}

class Stats {
  final bracketsSq = HashMap<String, Bracket>();
  final bracketsCurl = HashMap<String, Bracket>();
  final bracketsCurlIndex = HashMap<String, Bracket>();
  final stats = HashMap<String, StatLang>();
  final bookIds = Map<String, int>();
}

void stat() async {
  // unique temporary INT book id
  final relDirs = fileSystem.parsed.list(file: false).toList();
  final stats = Stats();
  for (final dir in relDirs) stats.bookIds[dir] = stats.bookIds.length;

  final srcFiles = fileSystem.parsed
      .list(regExp: fileSystem.devFilter + r'\\stat\.msg$')
      .toList();
  var count = 0;
  for (final fn in srcFiles) {
    print('${count++} / ${srcFiles.length}');
    final bookId = stats.bookIds[p.dirname(fn)];
    final srcBook =
        toPars.BracketBooks.fromBuffer(fileSystem.parsed.readAsBytes(fn));
    for (final lSrcBook in srcBook.books) {
      final lStat = stats.stats
          .putIfAbsent(lSrcBook.lang, () => StatLang()..lang = lSrcBook.lang);
      _putToLang(lStat, lSrcBook, bookId);
      _putBrakets(stats, lSrcBook, bookId);
    }
  }
  toMatrixes(stats);
}

void _putBrakets(
    Stats stats, toPars.BracketBook book, int bookId) {
  String getValue(String val, bool isIndex) =>
      val == null || val.isEmpty || !isIndex ? val : val.split(' ')[0];

  void br(String type, HashMap<String, Bracket> brs, bool isIndex) {
    for (final br in book.brackets.where((br) => br.type == type)) {
      brs.update(getValue(br.value, isIndex), (v) {
        v.bookIds.add(bookId);
        v.count++;
        return v;
      }, ifAbsent: () => Bracket(getValue(br.value, isIndex), HashSet<int>.from([bookId])));
    }
  }

  br('[', stats.bracketsSq, false);
  br('{', stats.bracketsCurl, false);
  br('{', stats.bracketsCurlIndex, true);
}

void _putToLang(StatLang stat, toPars.BracketBook book, int bookId) {
  // OK words
  for (final w in book.okWords) {
    stat.ok.update(w, (v) {
      v.count++;
      v.bookIds.add(bookId);
      return v;
    }, ifAbsent: () {
      stat.okAlpha.addAll(w.codeUnits);
      return Word(w, 1, HashSet<int>.from([bookId]), null, null);
    });
  }
  // latin words
  for (final w in book.latinWords) {
    stat.latin.update(w, (v) {
      v.count++;
      v.bookIds.add(bookId);
      return v;
    }, ifAbsent: () => Word(w, 1, HashSet<int>.from([bookId]), null, null));
  }
  // wrong words
  for (final w in book.latinWords) {
    final p = w.split('|');
    stat.latin.update(p[0], (v) {
      v.count++;
      v.bookIds.add(bookId);
      return v;
    }, ifAbsent: () {
      stat.wrongsUnicodeAlpha.addAll(p[1].codeUnits);
      stat.wrongsCldrAlpha.addAll(p[2].codeUnits);
      return Word(w, 1, HashSet<int>.from([bookId]), p[1], p[2]);
    });
  }
}
