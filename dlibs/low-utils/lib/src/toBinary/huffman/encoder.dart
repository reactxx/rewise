import 'dart:typed_data';
import 'package:rewise_low_utils/toBinary.dart' as binary;

class NodeEnc extends binary.BitData {
  NodeEnc(Uint8List bits, int bitsCount, this.dump) : super(bits, bitsCount);
  String dump;
}

class Encoder<T extends Comparable> {
  Encoder(this.map);
  Map<T, binary.NodeEnc> map;

  int encode(Iterable<T> data, binary.ByteWriter wr) {
    final bwr = binary.BitWriter.fromByteWriter(wr);
    var count = 0;
    bwr.writeDatas(data.map((d) {
      count++;
      return map[d];
    }));
    bwr.align();
    return count;
  }

  void encodeWithLen(Iterable<T> data, binary.ByteWriter wr) {
    final subWr = binary.ByteWriter();
    var count = encode(data, subWr);
    // length
    wr.writeWriter(subWr);
  }
}
