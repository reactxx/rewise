import 'package:rewise_low_utils/client.dart' as client;
import 'package:rw_dom/hack_json.dart' as hack;
import 'package:rw_dom/utils.dart' as utils;
import 'package:protobuf/protobuf.dart' as pb;
import 'dart:async' show Future;
import 'package:recase/recase.dart' show ReCase;

Future<String> hackToJson(pb.GeneratedMessage msg) async {
  var serverMsg = hack.HackJsonPar()
    ..isToJson = true
    ..b = msg.writeToBuffer()
    ..qualifiedMessageName = _qualifiedMessage(msg.info_.qualifiedMessageName);
  final res = await client.HackJson_HackJson(serverMsg);
  return Future.value(res.s);
}

Future<List<int>> hackFromJson(String str, String qualifiedMessageName) async {
  var msg = hack.HackJsonPar()
    ..isToJson = false
    ..s = str
    ..qualifiedMessageName = _qualifiedMessage(qualifiedMessageName);

  final res = await client.HackJson_HackJson(msg);
  return Future.value(res.b);
}

Future<void> hackJsonFile(String qualifiedMessageName, String src, String dest, bool isToJson) async {
  var msg = hack.HackJsonFilePar()
    ..isToJson = isToJson
    ..qualifiedMessageName = _qualifiedMessage(qualifiedMessageName)
    ..files = (utils.FromToFiles()..src=src..dest =dest);

  await client.HackJson_HackJsonFile(msg);
  return Future.value();
}
String _qualifiedMessage(String name) =>
    name.split('.').map((f) => ReCase(f).pascalCase).join('.');
