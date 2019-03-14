import 'package:path/path.dart' as p;
import 'package:tuple/tuple.dart';

import 'package:server_dart/utils.dart';
import 'package:rewise_low_utils/utils.dart';

void refreshMessagesDart() {
  final relFiles = fileSystem.codeDartUtils
      .list(from: 'lib/src/messages', relTo: 'lib')
      .map((f) => Tuple2(f, p.split(f)));
  //var x = List<Tuple2<String, List<String>>>.from(relFiles);
  final grps = group<Tuple2<String, List<String>>, String, String>(relFiles, (t) {
    if (t.item2[2] == 'google') return 'google';
    if (t.item2.length == 4) return 'rewise';
    return t.item2[3];
  }, valuesAs: (t) =>  t.item1);
  for (final grp in grps) {
    final lines = grp.values.map((f) =>
        "export 'package:rewise_low_utils/${f.replaceAll(RegExp(r'\\'), '/')}';");
    fileSystem.codeDartUtils.writeAsLines('lib\\messages\\${grp.key}.dart', lines);
  }
}

void refreshMessagesCmd() {
  final relFiles =
      fileSystem.protobufs.list(from: 'rewise', regExp: r'\.proto$');
  final lines = relFiles.map((f) => ' ${p.withoutExtension(f)}^');
  fileSystem.protobufs.writeAsLines(r'fragment.cmd', lines);
}
