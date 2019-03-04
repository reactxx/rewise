import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/env.dart' as env;
import 'package:rewise_low_utils/toBinary.dart' as binary;

main() {
  test.setUp(() => env.DEV__ = false);
  test.tearDown(() => env.DEV__ = false);

  test.group("huffman", () {
    test.test('tree', () {
      final encoder = binary.EncoderString.fromString('aaaaaaaabbbccc');

      var dump = encoder.result.dump;
      test.expect(dump, test.equals('{"a":"1","b":"00","c":"01"}'));

      dump = binary.dumpBytesBits(encoder.result.decodingTree);
      test.expect(dump, test.equals('0011 0110 0010 1101 1000 1111 0110 0001'));

      final decoder = binary.DecoderString(encoder.result.decodingTree);
      var strIn = 'abc';
      var code = encoder.encodeString(strIn);
      dump = binary.dumpBytesBits(code);
      test.expect(dump, test.equals('1000 1000'));
      var strOut = decoder.decodeString(code, strIn.length);
      test.expect(strIn, test.equals(strOut));
    });
  });
}
