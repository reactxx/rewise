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
  Bracket(this.type, this.value, this.count, this.bookIds);
  String type;
  String value;
  int count;
  HashSet<int> bookIds;
}

class LangWords {
  String lang;
  final brackets = HashMap<String, Bracket>();
  final ok = HashMap<String, Word>();
  final latin = HashMap<String, Word>();
  final wrongs = HashMap<String, Word>();
  final okAlpha = HashSet<int>();
  final wrongsCldrAlpha = HashSet<int>();
  final wrongsUnicodeAlpha = HashSet<int>();
}

void stat() async {
  // unique temporary INT book id
  final relDirs = fileSystem.parsed.list(file: false).toList();
  final tempBookIds = Map<String, int>();
  for (final dir in relDirs) tempBookIds[dir] = tempBookIds.length;

  final srcFiles = fileSystem.parsed.list(regExp: r'\\stat\.msg$').toList();
  final stats = HashMap<String, LangWords>();
  for (final fn in srcFiles) {
    final bookId = tempBookIds[p.dirname(fn)];
    final srcBook =
        toPars.BracketBooks.fromBuffer(fileSystem.parsed.readAsBytes(fn));
    for (final lSrcBook in srcBook.books) {
      final lStat = stats.putIfAbsent(
          lSrcBook.lang, () => LangWords()..lang = lSrcBook.lang);
      _putToStat(lStat, lSrcBook, bookId);
    }
  }
  toMatrixes(tempBookIds, stats.values);
}

void _putToStat(LangWords stat, toPars.BracketBook book, int bookId) {
  // brackets
  for (final br in book.brackets) {
    stat.brackets.update(br.type + br.value, (v) {
      v.bookIds.add(bookId);
      v.count++;
    },
        ifAbsent: () =>
            Bracket(br.type, br.value, 1, HashSet<int>.from([bookId])));
  }
  // OK words
  for (final w in book.okWords) {
    stat.ok.update(w, (v) {
      v.count++;
      v.bookIds.add(bookId);
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
    }, ifAbsent: () => Word(w, 1, HashSet<int>.from([bookId]), null, null));
  }
  // wrong words
  for (final w in book.latinWords) {
    final p = w.split('|');
    stat.latin.update(p[0], (v) {
      v.count++;
      v.bookIds.add(bookId);
    }, ifAbsent: () {
      stat.wrongsUnicodeAlpha.addAll(p[1].codeUnits);
      stat.wrongsCldrAlpha.addAll(p[2].codeUnits);
      return Word(w, 1, HashSet<int>.from([bookId]), p[1], p[2]);
    });
  }
}
