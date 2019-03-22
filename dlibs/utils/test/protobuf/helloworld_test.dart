import 'dart:typed_data';
import 'package:test/test.dart' as test;
import 'package:rw_utils/toBinary.dart' as binary;
import 'gen/helloworld.pb.dart';

main() {

  test.test('streaming', () {
    final wr = binary.MemoryWriter();
    wr.writeMessages([
      HelloReply()..message = "Hello",
      HelloReply()..message = "h1",
      HelloReply()..message = "h2",
    ]);
    final rdr =binary.MemoryReader(wr.toBytes());
    final msgs = List<HelloReply>.from(rdr.readMessages((b) => HelloReply.fromBuffer(b)));
    test.expect(msgs[2].message, test.equals("h2"));
  });
}

Uint8List writeToBuffer(msg) => msg.writeToBuffer();
