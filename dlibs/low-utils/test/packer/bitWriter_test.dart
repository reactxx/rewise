import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/env.dart' as env;
import 'package:rewise_low_utils/packer.dart' as binary;
import 'package:convert/convert.dart' as convert;
import 'dart:typed_data';

main() {
  test.setUp(() => env.DEV__ = false);
  test.tearDown(() => env.DEV__ = false);

  test.group("bit writer", () {
    test.test('simple', () {
      final wr = binary.BitWriter();
      wr.writeBits(Uint8List.fromList([0x01,0x02,0xff]), 23);
      wr.writeBits(Uint8List.fromList([0xff]), 2);
      wr.writeBits(Uint8List.fromList([0xff]), 7);
      wr.writeBits(Uint8List.fromList([0xff]), 2);
      wr.writeBits(Uint8List.fromList([0xff]), 7);
      wr.align();
      var dump = convert.hex.encode(wr.byteList);
      return;


      wr.writeBit(true);
      wr.writeBools([true, true, true, true, true, true, true, true]);
    });

  });
}
