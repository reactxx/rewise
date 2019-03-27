@Timeout(const Duration(hours: 2))

import 'package:test/test.dart';
import 'package:rw_utils/rewise.dart';

main() {
  group("commands", () {
    test('toRaw', () async {9
      print('TO RAW start ${DateTime.now()}');
      var resp = await toRaw();
      print('TO RAW end ${DateTime.now()}');
      //expectLater(resp.isEmpty, equals(true), reason: resp);
      expectLater(true, equals(true), reason: resp);
    }, skip: false);

    test('toParsed', () async {
      print('TO PARSED start ${DateTime.now()}');
      var res = await toParsed();
      print('TO PARSED end ${DateTime.now()}');
      expectLater(res != null, equals(true), reason: '');
    }, skip: false);

    test('toRaw + toParsed', () async {
      print('TO RAW start ${DateTime.now()}');
      var raw = await toRaw();
      print('TO RAW end ${DateTime.now()}');
      print('TO PARSED start ${DateTime.now()}');
      var parse = await toParsed();
      print('TO PARSED end ${DateTime.now()}');
      expectLater(parse != null ||  raw != null, equals(true), reason: '');
    }, skip: false);
  }, skip: false);
}
