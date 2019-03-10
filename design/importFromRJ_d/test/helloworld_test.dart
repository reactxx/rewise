import 'dart:typed_data';
import 'dart:convert' as convert;
import 'package:test/test.dart' as test;
import 'package:protobuf/protobuf.dart' as proto;
import 'package:import_from_rj/helloWorldServer.dart' as helloSrv;
import 'package:rewise_low_utils/utils.dart' as grcpRequest;
import 'package:rewise_low_utils/toBinary.dart' as binary;

main() {
  test.test('grpc', () async {
    var resp = await grcpRequest.grcpRequest<helloSrv.HelloReply>(
        helloSrv.getChannel(),
        (channel) => helloSrv
            .getClient(channel)
            .sayHello(helloSrv.HelloRequest()..name = 'world'));

    var ok = resp == null || resp.message == 'Hello world';
    test.expect(ok, test.equals(true));
  });

  test.test('encoding x decoding', () {
    var msg = helloSrv.HelloReply()..message = "Hello";
    Uint8List res = msg.writeToBuffer();
    res = writeToBuffer(msg);
    var str = msg.toDebugString();
    var json = convert.jsonEncode(msg.writeToJsonMap());
    json = msg.writeToJson();
    msg = helloSrv.HelloReply.fromJson(json);
    msg = helloSrv.HelloReply.fromBuffer(res);
    msg = null;
  });

  test.test('streaming: does not work by this manner', () {
    final wr = proto.CodedBufferWriter();
    var msg = helloSrv.HelloReply()..message = "Hello";
    msg.writeToCodedBufferWriter(wr);
    msg..message = 'h2'..writeToCodedBufferWriter(wr);
    msg..message = 'h3'..writeToCodedBufferWriter(wr);
    Uint8List res = wr.toBuffer();
    final rdr = proto.CodedBufferReader(res);
    //helloSrv.HelloReply.mergeFromCodedBufferReader(rdr);
    msg = helloSrv.HelloReply.fromBuffer(res);
    msg = helloSrv.HelloReply.fromBuffer(res);
    msg = helloSrv.HelloReply.fromBuffer(res);
    String resStr = wr.toString();
    msg = null;
  });

  test.test('streaming', () {
    final wr = binary.ByteWriter();
    wr.writeMessages([
      helloSrv.HelloReply()..message = "Hello",
      helloSrv.HelloReply()..message = "h1",
      helloSrv.HelloReply()..message = "h2",
    ]);
    final rdr =binary.ByteReader(wr.toBytes());
    final msgs = List<helloSrv.HelloReply>.from(rdr.readMessages((b) => helloSrv.HelloReply.fromBuffer(b)));
  });
}

Uint8List writeToBuffer(msg) => msg.writeToBuffer();
