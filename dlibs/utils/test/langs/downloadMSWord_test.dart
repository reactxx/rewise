@Timeout(const Duration(seconds: 3600))

import 'package:test/test.dart';
import 'dart:io';
import 'dart:convert';

main() {
  test('uniBlocks', () async {

    final jsonTxt = File(r'd:\rewise\design\langsDesign\appdata\msword\spellCheckSupportDownload.json').readAsStringSync();
    Map<String, dynamic> map = json.decode(jsonTxt);

    final res = await Future.wait(map.keys.map((lang) => download(map[lang], 'd:\\rewise\\design\\langsDesign\\appdata\\msword\\$lang.exe')));

    print(res);
  });
}

Future download(String url, String file) async {
  HttpClient client = new HttpClient();
  final req = await client.getUrl(Uri.parse(url));
  final resp = await req.close();

  final data = List<int>();
  final substr = resp.listen((d) => data.addAll(d));
  await substr.asFuture();
  File(file).writeAsBytes(data);
}
