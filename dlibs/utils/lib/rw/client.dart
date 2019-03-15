//***** generated code
import 'package:rewise_low_utils/utils.dart' show getHost, MakeRequest;
import 'google.dart' as Google;
import 'utils.dart' as Utils;
import 'hack_json.dart' as HackJson;
import 'hallo_world.dart' as HalloWorld;
import 'to_raw.dart' as ToRaw;
import 'word_breaking.dart' as WordBreaking;

Future<HackJson.HackJsonBytes> HackJson_HackFromJson(HackJson.HackJsonString request) => 
  MakeRequest<HackJson.HackJsonBytes>(
      (channel) => HackJson.CSharpServiceClient(channel).hackFromJson(request),
      getHost('HackJson'));

Future<HackJson.HackJsonString> HackJson_HackToJson(HackJson.HackJsonBytes request) => 
  MakeRequest<HackJson.HackJsonString>(
      (channel) => HackJson.CSharpServiceClient(channel).hackToJson(request),
      getHost('HackJson'));

Future<Google.Empty> HackJson_HackFromJsonFile(Utils.FromToFiles request) => 
  MakeRequest<Google.Empty>(
      (channel) => HackJson.CSharpServiceClient(channel).hackFromJsonFile(request),
      getHost('HackJson'));

Future<Google.Empty> HackJson_HackToJsonFile(Utils.FromToFiles request) => 
  MakeRequest<Google.Empty>(
      (channel) => HackJson.CSharpServiceClient(channel).hackToJsonFile(request),
      getHost('HackJson'));

Future<HalloWorld.HelloReply> HalloWorld_SayHello(HalloWorld.HelloRequest request) => 
  MakeRequest<HalloWorld.HelloReply>(
      (channel) => HalloWorld.CSharpServiceClient(channel).sayHello(request),
      getHost('HalloWorld'));

Future<ToRaw.Response> ToRaw_Run(ToRaw.Request request) => 
  MakeRequest<ToRaw.Response>(
      (channel) => ToRaw.CSharpServiceClient(channel).run(request),
      getHost('ToRaw'));

Future<WordBreaking.Response> WordBreaking_Run(WordBreaking.Request request) => 
  MakeRequest<WordBreaking.Response>(
      (channel) => WordBreaking.CSharpServiceClient(channel).run(request),
      getHost('WordBreaking'));

Future<WordBreaking.Response> WordBreaking_RunEx(WordBreaking.Request request) => 
  MakeRequest<WordBreaking.Response>(
      (channel) => WordBreaking.CSharpServiceClient(channel).runEx(request),
      getHost('WordBreaking'));

