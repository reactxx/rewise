import 'package:rewise_low_utils/messages.dart' as messages;
import 'package:server_dart/utils.dart' show makeRequest;
import 'package:protobuf/protobuf.dart' as pb;

Future<String> hackToJson(pb.GeneratedMessage msg) async {
  var serverMsg = messages.HackJsonBytes()
        ..value = (messages.BytesValue()..value = msg.writeToBuffer())
        ..qualifiedMessageName = msg.info_.qualifiedMessageName;
  final res =
      await makeRequest((client) => client.hackToJson(serverMsg));
  return Future.value(res.value.value);
}

Future<List<int>> hackFromJson(String str, String qualifiedMessageName) async {
  var msg = messages.HackJsonString()
    ..value = (messages.StringValue()..value = str)
    ..qualifiedMessageName = qualifiedMessageName;

  final res = await makeRequest((client) => client.hackFromJson(msg));
  return Future.value(res.value.value);
}
