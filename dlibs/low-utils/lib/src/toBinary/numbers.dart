import 'package:rewise_low_utils/toBinary.dart' as binary;

// getByte(0x01000002, 3) == 1,  getByte(0x01000002, 0) == 2
int getByte(int n, int idx /*0..3*/) => (n >> (idx * 8)) & 0xff;

void encode_8_16(int n, binary.BitWriter wr) {
  if (n<=0xff) {
    wr.writeBool(true);
    wr.writeBitslist([getByte(n, 0)], 8);
  } else {
    assert(n<0xffff);
    wr.writeBool(false);
    wr.writeBitslist([getByte(n, 0), getByte(n, 1)], 16);
  }
}

int decode_8_16(binary.BitReader rdr) {
  if (rdr.readBit())
    return rdr.readByte();
  else
    return (rdr.readByte()) | (rdr.readByte() << 8);
}
