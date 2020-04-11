import 'package:grpc/grpc.dart' as grpc;
import 'package:test/test.dart' as test;
import 'dart:io' as io;

import 'package:rw_utils/dom/google.dart' as gcloud;

main() {
  test.test('test to speech', () async {
    // https://grpc.io/docs/guides/auth/
    final serviceAccountJson =
        io.File(r'c:\Dokumenty\google-cloud-service-account.json')
            .readAsStringSync();
    final credentials = grpc.JwtServiceAccountAuthenticator(serviceAccountJson);
    // https://cloud.google.com/text-to-speech/docs/reference/rpc/
    // https://github.com/googleapis/googleapis.github.io/blob/master/examples/rpc/javascript/src/index.js
    // https://cloud.google.com/text-to-speech/docs/voices
    final client = gcloud.TextToSpeechClient(
        grpc.ClientChannel('texttospeech.googleapis.com'),
        options: credentials.toCallOptions);
    final listResp =
        await client.listVoices(gcloud.ListVoicesRequest()..languageCode = '');
    final json = listResp.writeToJson();
    io.File(r'c:\temp\google-cloud-tts.voices.json').writeAsStringSync(json);
    return;
    final input = gcloud.SynthesisInput()
      ..text =
          'Ještě že to dobře dopadlo, je to tak? Já jsem celkem spokojený, že to moc nešustí';
    final voice = gcloud.VoiceSelectionParams()
      ..languageCode =
          'cs-CZ'; //..ssmlGender = gcloud.SsmlVoiceGender.SSML_VOICE_GENDER_UNSPECIFIED;
    final audioConfig = gcloud.AudioConfig()
      ..audioEncoding = gcloud.AudioEncoding.MP3;
    final req = new gcloud.SynthesizeSpeechRequest()
      ..input = input
      ..voice = voice
      ..audioConfig = audioConfig;
    final syntResp = await client.synthesizeSpeech(req);
    final bytes = syntResp.audioContent;
    io.File(r'c:\temp\google-cloud-tts.hallo.mp3').writeAsBytesSync(bytes);
  }, skip: false);
}
