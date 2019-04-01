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

class LangWords {
  String lang;
  final bracketsSq = HashMap<String, Bracket>();
  final bracketsCurl = HashMap<String, Bracket>();
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

  final srcFiles = fileSystem.parsed
      .list(regExp: fileSystem.devFilter + r'\\stat\.msg$')
      .toList();
  final stats = HashMap<String, LangWords>();
  var count = 0;
  for (final fn in srcFiles) {
    print('${count++} / ${srcFiles.length}');
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
  void br(String type, HashMap<String, Bracket> brs) {
    for (final br in book.brackets.where((br) => br.type == type)) {
      brs.update(br.value, (v) {
        v.bookIds.add(bookId);
        v.count++;
        return v;
      }, ifAbsent: () {
        if (br.value == 'fc_ auta')
          return Bracket(br.value, HashSet<int>.from([bookId]));
        else
          return Bracket(br.value, HashSet<int>.from([bookId]));
      });
    }
  }

  br('[', stat.bracketsSq);
  br('{', stat.bracketsCurl);

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
