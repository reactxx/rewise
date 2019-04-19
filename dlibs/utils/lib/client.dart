//***** generated code
import 'package:rw_utils/utils.dart' show getHost, MakeRequest;
import 'package:rw_utils/dom/word_breaking.dart' as WordBreaking;
import 'package:rw_utils/dom/stemming.dart' as Stemming;
import 'package:rw_utils/dom/spellCheck.dart' as Spellcheck;

Future<Spellcheck.Response> Spellcheck_Spellcheck(Spellcheck.Request request) => 
  MakeRequest<Spellcheck.Response>(
      (channel) => Spellcheck.CSharpServiceClient(channel).spellcheck(request),
      getHost('Spellcheck'));

Future<Stemming.Response> Stemming_Stemm(Stemming.Request request) => 
  MakeRequest<Stemming.Response>(
      (channel) => Stemming.CSharpServiceClient(channel).stemm(request),
      getHost('Stemming'));

Future<WordBreaking.Response2> WordBreaking_Run2(WordBreaking.Request2 request) => 
  MakeRequest<WordBreaking.Response2>(
      (channel) => WordBreaking.CSharpServiceClient(channel).run2(request),
      getHost('WordBreaking'));

