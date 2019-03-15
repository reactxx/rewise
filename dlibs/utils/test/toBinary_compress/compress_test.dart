import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/toBinary.dart' as binary;

main() {

  test.group("compress", () {
    test.test('string', () {
      final encoder = binary.EncoderString.fromString(['aaaaaaaabbbccc']);

      var dump = encoder.buildResult.dump;
      test.expect(dump, test.equals('{"\\u0000":"010","a":"1","b":"00","c":"011"}'));

      dump = binary.dumpBytesBits(encoder.buildResult.decodingTree);
      test.expect(dump, test.equals('0011 0110 0010 0110 0000 0001 1011 0001 1110 1100 0010 0000'));

      final decoder = binary.DecoderString(encoder.buildResult.decodingTree);
      var strIn = 'abc';
      var code = encoder.encodeString(strIn);
      dump = binary.dumpBytesBits(code);
      test.expect(dump, test.equals('1000 1101 0000 0000'));
      var strOut = decoder.decodeString(code);
      test.expect(strIn, test.equals(strOut));
    });

    test.test('int simple', () {
      var strIn = [1];
      final encoder = binary.EncoderInt.fromData([strIn]);

      var dump = encoder.buildResult.dump;
      test.expect(dump, test.equals('{"2147483647":"0","1":"1"}'));

      dump = binary.dumpBytesBits(encoder.buildResult.decodingTree);
      test.expect(dump, test.equals('0100 0001 1111 1111 1111 1111 1111 1111 1111 1111 0001 0000'));

      final decoder = binary.DecoderInt(encoder.buildResult.decodingTree);
      var code = encoder.encodeData(strIn);
      dump = binary.dumpBytesBits(code);
      test.expect(dump, test.equals('1000 0000'));
      var strOut = decoder.decodeData(code).toList();
      test.expect(strIn.join(','), test.equals(strOut.join(',')));
    });

    test.test('int', () {
      var strIn = [1,1000,0x7f567890,1,1];
      final encoder = binary.EncoderInt.fromData([strIn]);

      var dump = encoder.buildResult.dump;
      test.expect(dump, test.equals('{"2147483647":"00","1":"11","1000":"10","2136373392":"01"}'));

      dump = binary.dumpBytesBits(encoder.buildResult.decodingTree);
      test.expect(dump, test.equals('0010 0000 1111 1111 1111 1111 1111 1111 1111 1111 0000 0111 1111 0101 0110 0111 1000 1001 0000 0100 1000 0001 1111 0100 0110 0010'));

      final decoder = binary.DecoderInt(encoder.buildResult.decodingTree);
      var code = encoder.encodeData(strIn);
      dump = binary.dumpBytesBits(code);
      test.expect(dump, test.equals('1110 0111 1100 0000'));
      var strOut = decoder.decodeData(code).join(',');
      test.expect(strIn.join(','), test.equals(strOut));
    });
  });
}
