@Timeout(const Duration(hours: 1))

import 'package:test/test.dart';
import 'package:server_dart/commands.dart';

main() {
  group("commands", () {
    test('toRaw', () async {
      var resp = await toRaw();
      expectLater(resp.isEmpty, equals(true), reason: resp);
    }, skip: true);

    test('toParsed', () async {
      print(DateTime.now().toString());
      var res = await toParsed();
      print(DateTime.now().toString());
      expectLater(res != null, equals(true), reason: '');
    }, skip: false);

  }, skip: true);
}

