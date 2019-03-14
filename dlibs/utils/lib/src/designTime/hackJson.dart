import 'package:rewise_low_utils/rw/google.dart' as google;
import 'package:rewise_low_utils/rw/hack_json.dart' as hack;
import 'package:rewise_low_utils/rw/client.dart' as client;
import 'package:protobuf/protobuf.dart' as pb;

Future<String> hackToJson(pb.GeneratedMessage msg) async {
  var serverMsg = hack.HackJsonBytes()
    ..value = (google.BytesValue()..value = msg.writeToBuffer())
    ..qualifiedMessageName = msg.info_.qualifiedMessageName;
  final res = await client.HackJson_HackToJson(serverMsg);
  return Future.value(res.value.value);
}

Future<List<int>> hackFromJson(String str, String qualifiedMessageName) async {
  var msg = hack.HackJsonString()
    ..value = (google.StringValue()..value = str)
    ..qualifiedMessageName = qualifiedMessageName;

  final res = await client.HackJson_HackFromJson(msg);
  return Future.value(res.value.value);
}
