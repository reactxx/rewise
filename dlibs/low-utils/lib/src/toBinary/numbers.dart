import 'package:rewise_low_utils/toBinary.dart' as binary;

// getByte(0x01000002, 3) == 1,  getByte(0x01000002, 0) == 2
int getByte(int n, int idx /*0..3*/) => (n >> (idx * 8)) & 0xff;

void encode_8_16(int n, binary.BitWriter wr) {
  if (n <= 0xff) {
    wr.writeBool(true);
    wr.writeBitsList([getByte(n, 0)], 8);
  } else if (n < 0xffff) {
    wr.writeBool(false);
    wr.writeBitsList([getByte(n, 0), getByte(n, 1)], 16);
  } else
    throw Exception('encode_8_16: $n');
}

int decode_8_16(binary.BitReader rdr) {
  if (rdr.readBit())
    return rdr.readByte();
  else
    return (rdr.readByte()) | (rdr.readByte() << 8);
}

void encode_4_8_16(int n, binary.BitWriter wr) {
  if (n <= 0xf) {
    wr.writeBool(true);
    wr.writeBitsList([getByte(n, 0) & 0xf], 8);
  } else {
    wr.writeBool(false);
    encode_8_16(n, wr);
  }
}

int decode_4_8_16(binary.BitReader rdr) {
  if (rdr.readBit())
    return rdr.readByte(/*4*/);
  else {
    rdr.readBit();
    return decode_8_16(rdr);
  }
}

