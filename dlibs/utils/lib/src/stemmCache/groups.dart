import 'dart:math';
//import 'dart:collection';
import 'package:rw_utils/toBinary.dart' as bin;
import 'cache.dart';

class GroupHeader {
  int id;
  int position;
  String key; // shortest of own words
  // int mpd5First;
  int dataLen;

  GroupHeader.fromStemmResult(this.key);

  GroupHeader.fromReader(bin.StreamReader rdr, {bool skipData = false}) {
    position = rdr.position;
    final hdr = rdr.readSizedIntsLow(2);
    id = hdr[0];
    //mpd5First = hdr[2];
    dataLen = hdr[1];
    key = rdr.readString();
    if (skipData) rdr.position += dataLen;
  }
  static const headerLen = 4 * 4; // four int's
  void write(bin.StreamWriter wr, List<int> data) {
    position = wr.position;
    dataLen = data.length;
    wr.writeSizedIntsLow([id, dataLen]);
    wr.writeString(key);
    wr.writeBytes(data);
  }

  // GroupHeader.fromMD5(List<int> md5, this.id, this.md5Hash) {
  //   final bd = ByteData.view(Uint8List.fromList(md5).buffer);
  //   final ints = List<int>(4);
  //   for (var i = 0; i < 4; i++) ints[i] = bd.getUint32(i << 2, Endian.big);
  //   mpd5First = ints[0];
  // }

  // proxy management
  // Uint32List get proxy =>
  //     Uint32List.fromList([id, position, md5Hash, mpd5First]);

  // GroupHeader.fromProxy(this.id, this.position, this.md5Hash, this.mpd5First);

  // static GroupHeader checkExistingHeaders(
  //     List<GroupHeader> existing, String key) {
  //    return existing == null ? null : existing.singleWhere((h) => h.key==key, orElse: () => null);
  // }
}

class GroupDisk {
  GroupHeader header; // 3 bytes: numOfBytes, md5Hash, mpd5First
  //List<int> md5; // 16 bytes MD5
  //int ownLen; // words[0..ownLen-1] are words, which stemming produces words
  List<WordDisk> ownWords;
  List<String> words;

  GroupDisk.fromReader(bin.StreamReader rdr) {
    header = GroupHeader.fromReader(rdr);
    //ownWords
    final len = rdr.readByte();
    ownWords = List<WordDisk>(len);
    for(var i=0; i<len; i++)
      ownWords.add(WordDisk(rdr.readSizedInt(3), rdr.readString()));
    //other words
    words = rdr.readStrings();
  }
  void write(bin.StreamWriter wr) {
    assert(wr.position == wr.length);
    final subWr = bin.MemoryWriter();
    //ownWords
    subWr.writeByte(ownWords.length);
    for(final w in ownWords) {
      wr.writeSizedInt(w.id,3);
      wr.writeString(w.word);
    }
    subWr.writeStrings(words);
    header.write(wr, subWr.byteList);
  }

  GroupDisk.fromStemmResult(StemmResult res) {
    // own worlds with min length
    var minLen = 256;
    for (final w in res.words.take(res.ownLen)) minLen = min(minLen, w.length);
    // get alphabet-first one
    final minLens = res.words.take(res.ownLen).where((w) => w.length == minLen).toList()
      ..sort();
    header = GroupHeader.fromStemmResult(minLens[0]);
    words = res.words.skip(res.ownLen).toList();
    ownWords = res.words.take(res.ownLen).map((w) => WordDisk(0, w)).toList();
  }
}

class WordDisk {
  WordDisk(this.id, this.word);
  int id;
  final String word;
}
