import 'dart:math';
import 'package:rw_utils/toBinary.dart' as bin;
import 'cache.dart';

class Group {
  int id;
  int position;
  String key;
  List<Word> ownWords;
  List<String> words;

  Group.fromReader(bin.StreamReader rdr) {
    position = rdr.position;
    id = rdr.readSizedInt(3);
    key = rdr.readString();
    final len = rdr.readByte();
    ownWords = List<Word>(len);
    for (var i = 0; i < len; i++) ownWords.add(Word.fromReader(rdr));
    words = rdr.readStrings();
  }
  void write(bin.StreamWriter wr) {
    assert(wr.position == wr.length);
    position = wr.position;
    wr.writeSizedInt(id, 3);
    wr.writeString(key);
    wr.writeByte(ownWords.length);
    for (final w in ownWords) w.write(wr);
    wr.writeStrings(words);
  }

  // missing ID and POSITION
  Group.fromStemmResult(StemmResult res) {
    // own worlds with min length
    var minLen = 256;
    for (final w in res.words.take(res.ownLen)) minLen = min(minLen, w.length);
    // sort and select first
    final minLens = res.words
        .take(res.ownLen)
        .where((w) => w.length == minLen)
        .toList()
          ..sort();
    key = minLens[0];
    //  other
    ownWords = res.words.take(res.ownLen).map((w) => Word(0, w)).toList();
    words = res.words.skip(res.ownLen).toList();
  }
}

class Word {
  Word(this.id, this.word);
  Word.fromReader(bin.StreamReader rdr)
      : id = rdr.readSizedInt(3),
        word = rdr.readString();
  void write(bin.StreamWriter wr) {
    wr.writeSizedInt(id, 3);
    wr.writeString(word);
  }

  int id;
  final String word;
}
