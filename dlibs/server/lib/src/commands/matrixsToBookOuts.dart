import 'package:rewise_low_utils/messages.dart' as messages;
import 'package:rewise_low_utils/utils.dart' as utils;
import 'package:server_dart/utils.dart' show fileSystem, makeRequest;

const _devFilter = r'goetheverlag\.csv';

Future<messages.Empty> matrixsToBookOuts() {
  final msg = messages.ImportRJRequest();
  final relFiles = fileSystem.rjCsv.list(regExp: _devFilter);
  final srcFiles = fileSystem.rjCsv.toAbsolute(relFiles);
  var destFiles = fileSystem.rjMsg.changeExtension(relFiles, '.msg');
  destFiles = fileSystem.rjMsg.toAbsolute(destFiles);
  final filenames =
      utils.zip(srcFiles, destFiles).map((tuple) => messages.RJFileNames()
        ..matrix = tuple.item1
        ..bin = tuple.item2);

  msg.fileNames.addAll(filenames);
  return makeRequest((client) => client.matrixsToBookOuts(msg));
}

