import 'dart:collection';
import 'dart:math';
import 'dart:typed_data';
import 'dart:io' as io;
import 'package:rw_utils/utils.dart' as utils;
import 'package:rw_utils/toBinary.dart' as bin;

  HashMap<String, WordDisk> readWords(bin.StreamReader rdr) {
    rdr.position = 0;
    final res = HashMap<String, WordDisk>();
    var idx = 0;
    while(rdr.position < rdr.length) {
      final word = rdr.readString();
      final groupPos = rdr.readSizedInt(3);
      res[word] = WordDisk(idx++, groupPos);
    }
    return res;
  }

class WordDisk {
  WordDisk(this.id, this.groupPos);
  final int id;
  final int groupPos;
}
