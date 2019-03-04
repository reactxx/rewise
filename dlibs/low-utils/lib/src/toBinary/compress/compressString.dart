import 'dart:typed_data';

import 'package:rewise_low_utils/toBinary.dart' as binary;
import 'package:rewise_low_utils/utils.dart' as utils;

class EncoderString extends binary.Encoder<int> {
  EncoderString.fromInput(binary.BuildInput<int> input)
      : super.fromInput(input);

  EncoderString.fromData(Iterable<Iterable<int>> data) : super.fromData(data);

  EncoderString.fromString(Iterable<String> datas) : super() {
    final input = binary.BuildInput<int>();
    for (var data in datas) {
      input.counts.update(eof, (v) => v + 1, ifAbsent: () => 1);
      input.countAll++;
      for (var code in utils.stringCodeUnits(data)) {
        input.counts.update(code, (v) => v + 1, ifAbsent: () => 1);
        input.countAll++;
      }
    }
    buildResult = binary.build(input, this);
  }

  Uint8List encodeString(String data) {
    final wr = binary.BitWriter();
    encode(wr, utils.stringCodeUnits(data + String.fromCharCode(eof)).toList());
    return wr.toBytes();
  }

  int eof = 0;
  String keyToDump(int key) => String.fromCharCode(key);
  void keyToBits(int key, binary.BitWriter wr) {
    binary.encode_8_16(key, wr);
  }

  bool validKey(int key) => key > 0 && key <= 0xffff;
}

class DecoderString extends binary.Decoder<int> {
  DecoderString(Uint8List decodingTree) : super(decodingTree);

  int eof = 0;
  int bitsToKey(binary.BitReader rdr) => binary.decode_8_16(rdr);

  String decodeString(Uint8List encoded) {
    final rdr = binary.BitReader(encoded);
    return String.fromCharCodes(decode(rdr));
  }
}
