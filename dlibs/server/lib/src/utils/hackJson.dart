import 'package:rewise_low_utils/rw/client.dart' as client;
import 'package:rewise_low_utils/rw/hack_json.dart' as hack;
import 'package:protobuf/protobuf.dart' as pb;
import 'dart:async' show Future;
import 'package:rewise_low_utils/rw/google.dart' as google;
import 'package:recase/recase.dart' show ReCase;

Future<String> hackToJson(pb.GeneratedMessage msg) async {
  var serverMsg = hack.HackJsonBytes()
    ..value = (google.BytesValue()..value = msg.writeToBuffer())
    ..qualifiedMessageName = _qualifiedMessage(msg.info_.qualifiedMessageName);
  final res = await client.HackJson_HackToJson(serverMsg);
  return Future.value(res.value.value);
}

Future<List<int>> hackFromJson(String str, String qualifiedMessageName) async {
  var msg = hack.HackJsonString()
    ..value = (google.StringValue()..value = str)
    ..qualifiedMessageName = _qualifiedMessage(qualifiedMessageName);

  final res = await client.HackJson_HackFromJson(msg);
  return Future.value(res.value.value);
}

String _qualifiedMessage (String name) => name.split('.').map((f) => ReCase(f).pascalCase).join('.');
