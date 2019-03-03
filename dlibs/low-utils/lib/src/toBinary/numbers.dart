import 'package:rewise_low_utils/toBinary.dart' as binary;

void encode_8_16(int n, binary.BitWriter wr) {
  if (n <= 0xff) {
    wr.writeBool(true);
    wr.writeInt(n, 8);
  } else if (n < 0xffff) {
    wr.writeBool(false);
    wr.writeInt(n, 16);
  } else
    throw Exception('encode_8_16: $n');
}

int decode_8_16(binary.BitReader rdr) {
  if (rdr.readBit())
    return rdr.readInt(8);
  else
    return rdr.readInt(16);
}

void encode_4_8_16(int n, binary.BitWriter wr) {
  if (n <= 0xf) {
    wr.writeBool(true);
    wr.writeInt(n, 4);
  } else {
    wr.writeBool(false);
    encode_8_16(n, wr);
  }
}

int decode_4_8_16(binary.BitReader rdr) {
  if (rdr.readBit())
    return rdr.readInt(4);
  else {
    return decode_8_16(rdr);
  }
}

