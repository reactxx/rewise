import 'package:grpc/grpc.dart' as grpc;
import 'package:test/test.dart' as test;
import 'dart:io' as io;

import 'package:rw_utils/dom/google.dart' as gcloud;

main() {
  test.test('gcloud speech', () async {
    final serviceAccountJson =
        io.File(r'c:\Dokumenty\google-cloud-service-account.json')
            .readAsStringSync();
    final credentials = grpc.JwtServiceAccountAuthenticator(serviceAccountJson);
    // https://cloud.google.com/speech-to-text/docs/
    final client = gcloud.SpeechClient(
        grpc.ClientChannel('speech.googleapis.com'),
        options: credentials.toCallOptions);
    final config = gcloud.RecognitionConfig()
      ..languageCode = 'cs-CZ'
      ..audioChannelCount = 2
      ..enableWordTimeOffsets = true
      ..encoding = gcloud.RecognitionConfig_AudioEncoding.FLAC;
    final audio = gcloud.RecognitionAudio()
      ..content = io.File(r'c:\temp\Recording.flac').readAsBytesSync();
    final text = await client.recognize(gcloud.RecognizeRequest()
      ..config = config
      ..audio = audio);
    final json = text.writeToJson();
    io.File(r'c:\temp\google-cloud-speech.recognize.json')
        .writeAsStringSync(json);
  }, skip: true);
}
