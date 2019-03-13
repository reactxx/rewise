import 'package:server_dart/utils.dart';

void refreshMessagesDart() {
  final relFiles =
      fileSystem.codeDartUtils.list(from: 'lib/src/messages', relTo: 'lib');
  final lines = relFiles.map((f) =>
      "export 'package:rewise_low_utils/${f.replaceAll(RegExp(r'\\'), '/')}';");
  fileSystem.codeDartUtils.writeAsLines(r'lib\messages.dart', lines);
}
