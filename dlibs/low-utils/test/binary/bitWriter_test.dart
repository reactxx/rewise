import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/env.dart' as env;
import 'package:rewise_low_utils/binary.dart' as binary;
import 'dart:typed_data';

main() {
  test.setUp(() => env.DEV__ = false);
  test.tearDown(() => env.DEV__ = false);

  test.group("bit writer", () {
    test.test('', () {
      final wr = binary.BitWriter();
      wr.writeBit(true);
    });

  });
}
