import 'package:rewise_low_utils/toBinary.dart' as binary;

void writeInt(binary.ByteWriter wr, int number, int size /*0,1,2,3*/) {
  if (size == 0) return;
  wr.writeList(size == 1
      ? [number]
      : (size == 2
          ? [number & 0xFF, (number >> 8) & 0xFF]
          : [number & 0xFF, (number >> 8) & 0xFF, (number >> 16) & 0xFF]));
}

  int readInt(binary.ByteReader rdr, int size /*0,1,2,3*/) {
    switch (size) {
      case 0:
        return 0;
      case 1:
        return rdr.readByte();
      case 2:
        return rdr.readByte() | rdr.readByte() << 8;
      case 3:
        return rdr.readByte() | rdr.readByte() << 8 | rdr.readByte() << 16;
      default:
        throw UnimplementedError();
    }
  }


int getIntSize(int number) {
  // returns 0,1,2 or 3
  if (number == null) return 0;
  if (number > 0xffffff || number < 0) throw ArgumentError();
  return number == 0 ? 0 : (number <= 0xff ? 1 : (number <= 0xffff ? 2 : 3));
}

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

void encode_4_8_16_24_32(int n, binary.BitWriter wr) {
  if (n <= 0xf) {
    wr.writeBool(true);
    wr.writeInt(n, 4);
  } else {
    wr.writeBool(false);
    if (n <= 0xff) {
      wr.writeBool(true);
      wr.writeInt(n, 8);
    } else {
      wr.writeBool(false);
      if (n <= 0xffff) {
        wr.writeBool(true);
        wr.writeInt(n, 16);
      } else {
        wr.writeBool(false);
        if (n <= 0xffffff) {
          wr.writeBool(true);
          wr.writeInt(n, 24);
        } else {
          wr.writeBool(false);
          wr.writeInt(n, 32);
        }
      }
    }
  }
}

int decode_4_8_16_24_32(binary.BitReader rdr) {
  if (rdr.readBit())
    return rdr.readInt(4);
  else if (rdr.readBit())
    return rdr.readInt(8);
  else if (rdr.readBit())
    return rdr.readInt(16);
  else if (rdr.readBit())
    return rdr.readInt(24);
  else
    return rdr.readInt(32);
}
