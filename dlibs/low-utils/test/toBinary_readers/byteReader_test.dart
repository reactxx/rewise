import 'dart:typed_data';

import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/toBinary.dart' as toBinary;

main() {

  test.group("binary reader", () {
    test.test('binary search', () {
      final rdr = toBinary.ByteReader(
          Uint8List.fromList([1, 3, 5, 7, 9, 11, 13, 15, 17]));
      final subRdr = rdr.createSubReaderFromPos(2, 5);
      final f1 = subRdr.BinarySearch(1, 5);
      test.expect(f1.item1, test.equals(0));
      final f2 = subRdr.BinarySearch(1, 13);
      test.expect(f2.item1, test.equals(4));
    });
  });
}
