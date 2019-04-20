import 'dart:math';
import 'package:rw_utils/toBinary.dart' as bin;
import 'package:rw_utils/dom/stemming.dart' as stemm;

class Group {
  int id;
  int position;
  String key;
  List<Word> ownWords;
  List<String> words;
  String alias; // stemmm source is not in stemms

  Group.fromReader(bin.StreamReader rdr) {
    position = rdr.position;
    key = rdr.readString();
    if (key.isEmpty) { // => alias
      key = rdr.readString();
      alias = rdr.readString();
    } else {
      id = rdr.readSizedInt(3);
      final len = rdr.readByte();
      if (len > 0) {
        ownWords = List<Word>(len);
        for (var i = 0; i < len; i++) ownWords[i] = Word.fromReader(rdr);
        words = rdr.readStrings();
      }
    }
  }
  void write(bin.StreamWriter wr) {
    assert(wr.position == wr.length);
    assert(key.isNotEmpty);
    position = wr.position;
    if (alias != null) {
      wr.writeString('');
      wr.writeString(key);
      wr.writeString(alias);
    } else {
      wr.writeString(key);
      wr.writeSizedInt(id, 3);
      final len =
          ownWords == null || ownWords.length == 0 ? 0 : ownWords.length;
      wr.writeByte(len);
      if (len > 0) {
        for (final w in ownWords) w.write(wr);
        wr.writeStrings(words);
      }
    }
  }

  Group.fromAlias(Group aliasOf, this.alias): key = aliasOf.key;

  // Group does not have ID and POSITION, fill it during write
  Group.fromStemmResult(stemm.Word res) {
    assert(res.stemms.length > 0);
    if (res.stemms.length == 1) {
      // single items stemms
      key = res.stemms[0];
      assert(key != null);
      return;
    }
    // get own worlds with min length (as a key)
    var minLen = 256;
    for (final w in res.stemms.take(res.ownLen)) minLen = min(minLen, w.length);
    // sort and select first
    final minLens = res.stemms
        .take(res.ownLen)
        .where((w) => w.length == minLen)
        .toList()
          ..sort();
    key = minLens.length == 0 ? '' : minLens[0];
    //  other
    ownWords = res.stemms.take(res.ownLen).map((w) => Word(0, w)).toList();
    words = res.stemms.skip(res.ownLen).toList();
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
