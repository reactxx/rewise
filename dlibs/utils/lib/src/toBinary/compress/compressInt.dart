import 'dart:typed_data';

import 'package:rewise_low_utils/toBinary.dart' as binary;

class EncoderInt extends binary.Encoder<int> {
  EncoderInt.fromInput(binary.BuildInput<int> input) : super.fromInput(input);

  EncoderInt.fromData(Iterable<Iterable<int>> data) : super.fromData(data);

  void keyToBits(int key, binary.BitWriter wr) {
    binary.encode_4_8_16_24_32(key, wr);
  }

  bool validKey(int key) => key >= 0 && key < binary.maxInt;

  int eof = binary.maxInt;
}

class DecoderInt extends binary.Decoder<int> {
  DecoderInt(Uint8List decodingTree) : super(decodingTree);

  int bitsToKey(binary.BitReader rdr) => binary.decode_4_8_16_24_32(rdr);
  int eof = binary.maxInt;
}
