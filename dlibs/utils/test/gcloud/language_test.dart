import 'package:grpc/grpc.dart' as grpc;
import 'package:test/test.dart' as test;
import 'dart:io' as io;

import 'package:rw_utils/dom/google.dart' as gcloud;

const text = '''
The Fed’s move on Wednesday may cheer President Trump, who has jawboned the central bank for a year over its 2018 rate increases, saying the economy would have gone up “like a rocket” had the Fed not gotten it wrong.
''';

main() {
  test.test('gcloud language', () async {
    final serviceAccountJson =
        io.File(r'c:\Dokumenty\google-cloud-service-account.json')
            .readAsStringSync();
    final credentials = grpc.JwtServiceAccountAuthenticator(serviceAccountJson);
    // https://cloud.google.com/natural-language/docs/quickstarts
    final client = gcloud.LanguageServiceClient(
        grpc.ClientChannel('language.googleapis.com'),
        options: credentials.toCallOptions);
    final anotated = await client.annotateText(gcloud.AnnotateTextRequest()
      ..encodingType = gcloud.EncodingType.UTF8
      ..document = (gcloud.Document()
        ..type = gcloud.Document_Type.PLAIN_TEXT
        ..content = text
        ..language = 'en')
      ..features = (gcloud.AnnotateTextRequest_Features()
        //..extractEntities = true..extractDocumentSentiment = true..extractEntitySentiment = true..ex));
        ..extractSyntax = true));
    final json = anotated.writeToJson();
    io.File(r'c:\temp\google-cloud-language.anotation.json')
        .writeAsStringSync(json);
  }, skip: true);
}
