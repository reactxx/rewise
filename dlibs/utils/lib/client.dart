//***** generated code
import 'package:rw_utils/utils.dart' show getHost, MakeRequest;
import 'package:rw_utils/dom/google.dart' as Google;
import 'package:rw_utils/dom/hack_json.dart' as HackJson;
import 'package:rw_utils/dom/hallo_world.dart' as HalloWorld;
import 'package:rw_utils/dom/to_raw.dart' as ToRaw;
import 'package:rw_utils/dom/word_breaking.dart' as WordBreaking;
import 'package:rw_utils/dom/stemming.dart' as Stemming;

Future<HackJson.HackJsonPar> HackJson_HackJson(HackJson.HackJsonPar request) => 
  MakeRequest<HackJson.HackJsonPar>(
      (channel) => HackJson.CSharpServiceClient(channel).hackJson(request),
      getHost('HackJson'));

Future<Google.Empty> HackJson_HackJsonFile(HackJson.HackJsonFilePar request) => 
  MakeRequest<Google.Empty>(
      (channel) => HackJson.CSharpServiceClient(channel).hackJsonFile(request),
      getHost('HackJson'));

Future<HalloWorld.HelloReply> HalloWorld_SayHello(HalloWorld.HelloRequest request) => 
  MakeRequest<HalloWorld.HelloReply>(
      (channel) => HalloWorld.CSharpServiceClient(channel).sayHello(request),
      getHost('HalloWorld'));

Future<Stemming.Response> Stemming_Stemm(Stemming.Request request) => 
  MakeRequest<Stemming.Response>(
      (channel) => Stemming.CSharpServiceClient(channel).stemm(request),
      getHost('Stemming'));

Future<ToRaw.Response> ToRaw_Run(ToRaw.Request request) => 
  MakeRequest<ToRaw.Response>(
      (channel) => ToRaw.CSharpServiceClient(channel).run(request),
      getHost('ToRaw'));

Future<WordBreaking.Response> WordBreaking_Run(WordBreaking.Request request) => 
  MakeRequest<WordBreaking.Response>(
      (channel) => WordBreaking.CSharpServiceClient(channel).run(request),
      getHost('WordBreaking'));

