import 'dart:typed_data';

import 'package:rewise_low_utils/toBinary.dart' as binary;
import 'package:rewise_low_utils/utils.dart' as utils;

class EncoderString extends binary.Encoder<int> {
  EncoderString.fromInput(binary.BuildInput<int> input)
      : super.fromInput(input);

  EncoderString.fromData(Iterable<int> data) : super.fromData(data);

  EncoderString.fromString(String data) : super() {
    final counts = Map<int, int>();
    for(var code in utils.stringCodeUnits(data))
      counts.update(code, (v) => v + 1, ifAbsent: () => 1);
    final input = binary.BuildInput<int>()
      ..counts = counts
      ..countAll = data.length;
    result = binary.build(input, this);
  }

  Uint8List encodeString(String data) {
    final wr = binary.BitWriter();
    encode(wr, utils.stringCodeUnits(data).toList());
    return wr.toBytes();
  }

  String keyToDump(int key) => String.fromCharCode(key);
  void keyToBits(int key, binary.BitWriter wr) {
    binary.encode_8_16(key, wr);
  }
}

class DecoderString extends binary.Decoder<int> {

  DecoderString(Uint8List decodingTree):super(decodingTree);

  int bitsToKey(binary.BitReader rdr) => binary.decode_8_16(rdr);

  String decodeString(Uint8List encoded, int count) {
    final rdr = binary.BitReader(encoded);
    return String.fromCharCodes(decode(rdr, count));
  }
      
}
