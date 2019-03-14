//***** generated code
import 'package:rewise_low_utils/utils.dart' show getHost, MakeRequest;
import 'hack_json.dart' as HackJson;
import 'to_raw.dart' as ToRaw;

Future<HackJson.HackJsonBytes> HackJson_HackFromJson(HackJson.HackJsonString request) => 
  MakeRequest<HackJson.HackJsonBytes>(
      (channel) => HackJson.CSharpServiceClient(channel).hackFromJson(request),
      getHost('HackJson'));

Future<HackJson.HackJsonString> HackJson_HackToJson(HackJson.HackJsonBytes request) => 
  MakeRequest<HackJson.HackJsonString>(
      (channel) => HackJson.CSharpServiceClient(channel).hackToJson(request),
      getHost('HackJson'));

Future<ToRaw.Response> ToRaw_Run(ToRaw.Request request) => 
  MakeRequest<ToRaw.Response>(
      (channel) => ToRaw.CSharpServiceClient(channel).run(request),
      getHost('ToRaw'));

