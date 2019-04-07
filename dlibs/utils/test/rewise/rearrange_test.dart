import 'package:test/test.dart' as test;
import 'package:rw_utils/rearrange.dart' show SrcFiles, oldLangs;
import 'package:rw_utils/utils.dart' show fileSystem;

main() {
  test.group("REARRANGE", () {
    test.test('old langs', () async {
      // var fs = fileSystem.csv.list(regExp: r'^templates\\.*' + r'\.csv$').toList();
      // var all = [0,1,2,3].expand((t) => SrcFiles.getFiles(t)).toList();
      // var olds = oldLangs().join(',');
      // olds = null;
      var res = SrcFiles.getData().toList();
      res = null;
    });
  });
}
