import 'dart:typed_data';
import 'dart:convert' as conv;
import 'package:crypto/crypto.dart' as crypto;
import 'package:rw_utils/toBinary.dart' as bin;
import 'cache.dart';

List<GroupHeader> readGroups(bin.StreamReader rdr) {
  var groupId = 0;
  final headers = List<GroupHeader>();
  while(rdr.position < rdr.length) {
    final header = GroupHeader.fromReader(rdr, skipData: true);
    assert(header.id == groupId++);
    headers.add(header);
  }
  return headers;
}

class GroupHeader {
  GroupHeader.fromReader(bin.StreamReader rdr, {bool skipData = false}) {
    position = rdr.position;
    final hdr = rdr.readSizedIntsLow(4, 4);
    id = hdr[0];
    md5Hash = hdr[1];
    mpd5First = hdr[2];
    dataLen = hdr[3];
    if (skipData) rdr.position += dataLen;
  }
  static const headerLen = 4 * 4; // four int's
  void write(bin.StreamWriter wr, List<int> md5Stemms) {
    position = wr.position;
    dataLen = md5Stemms.length;
    wr.writeSizedIntsLow([id, md5Hash, mpd5First, dataLen], 4);
    wr.writeBytes(md5Stemms);
  }

  GroupHeader.fromMD5(List<int> md5, this.id, this.md5Hash) {
    final bd = ByteData.view(Uint8List.fromList(md5).buffer);
    final ints = List<int>(4);
    for (var i = 0; i < 4; i++) ints[i] = bd.getUint32(i << 2, Endian.big);
    mpd5First = ints[0];
  }

  int dataLen;
  int id;
  int position;
  int md5Hash;
  int mpd5First;

  // proxy management
  Uint32List get proxy =>
      Uint32List.fromList([id, position, md5Hash, mpd5First]);

  GroupHeader.fromProxy(this.id, this.position, this.md5Hash, this.mpd5First);

  static List<GroupHeader> checkExistingHeaders(
      Uint32List existingProxies, GroupHeader newHdr) {
    if (existingProxies == null) return null;
    final res = List<GroupHeader>();
    var idx = 0;
    while (idx < existingProxies.length)
      if (newHdr.mpd5First == existingProxies[idx + 3] /* mpd5First */)
        res.add(GroupHeader.fromProxy(
            existingProxies[idx++],
            existingProxies[idx++],
            existingProxies[idx++],
            existingProxies[idx++]));
    return res.length > 0 ? res : null;
  }
}

class GroupDisk {
  GroupHeader header; // 3 bytes: numOfBytes, md5Hash, mpd5First
  List<int> md5; // 16 bytes MD5
  List<String> words;

  GroupDisk.fromReader(bin.StreamReader rdr) {
    header = GroupHeader.fromReader(rdr);
    md5 = rdr.readBytesLow(16);
    words = rdr.readStrings();
  }
  void write(bin.StreamWriter wr) {
    assert(wr.position == wr.length);
    final subWr = bin.MemoryWriter();
    subWr.writeBytesLow(md5);
    subWr.writeStrings(words);
    header.write(wr, subWr.byteList);
  }

  GroupDisk.fromStemmResult(StemmResult res, int id) {
    res.groupStemms.sort();
    final toMD5 = words.join(',');
    var bytes = conv.utf8.encode(toMD5);
    md5 = crypto.md5.convert(bytes).bytes;
    header = GroupHeader.fromMD5(md5, id, toMD5.hashCode);
    words = res.groupStemms;
  }
}
