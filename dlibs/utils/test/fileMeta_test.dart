import 'dart:collection';
import 'package:test/test.dart' as test;

import 'package:rw_utils/utils.dart' show fileSystem;
import 'package:path/path.dart' as p;

getfileMeta() {
  var langs = HashSet<String>.from(fileSystem.rawCsv.list(regExp: r'\.csv').map((f) => p.split(p.withoutExtension(f))).expand((f) => [f[f.length-2], f.last]));
  fileSystem.rawCsv.writeAsLines('dir.txt', langs);
  langs = null;
  
}

main() {
    test.test('?', () {
      getfileMeta();
    });
}

