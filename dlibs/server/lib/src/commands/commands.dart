import 'package:rewise_low_utils/messages.dart' as messages;
import 'package:rewise_low_utils/utils.dart' as utils;
import 'package:server_dart/utils.dart' show fileSystem, makeRequest;

Future<messages.Empty> matrixsToBookOuts() {
  final msg = messages.FileNamesRequest();
  final relFiles =
      fileSystem.rjCsv.list(regExp: r'goetheverlag\.csv');
  final srcFiles = fileSystem.rjCsv.toAbsolute(relFiles);
  final destFiles = fileSystem.rjMsg.toAbsolute(relFiles);
  final filenames = utils.zip(srcFiles, destFiles).map((tuple) => messages.FileNames()
    ..matrix = tuple.item1
    ..bin = tuple.item2);

  msg.fileNames.addAll(filenames);
  return makeRequest((client) => client.matrixsToBookOuts(msg));
}
