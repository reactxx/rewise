import 'package:test/test.dart' as test;
import 'package:rewise_low_utils/utils.dart';

main() {
  test.test('diff-test', () {
    var dir = Dir(r'c:\temp');
    var mapDir = Dir(r'c:\new');
    List<String> list = List.from(dir.files());
    list = List.from(dir.files(filter: RegExp(r'\.zip$', caseSensitive: false)));
    list =List.from(mapDir.map(list));
    list = List.from(dir.files(file: null));
    list = List.from(dir.files(file: false));
    test.expect(list, test.equals(list));
  });
}
