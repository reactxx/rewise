import 'package:rewise_low_utils/rw/to_raw.dart' as ToRaw;
import 'package:rewise_low_utils/utils.dart' as utils;
import 'package:rewise_low_utils/rw/client.dart' as client;
import 'package:rewise_low_utils/designTime.dart' show fileSystem;

const _devFilter = r'goetheverlag\.csv';

Future<ToRaw.Response> matrixsToBooksFromRJ() {
  final msg = ToRaw.Request();
  final relFiles = fileSystem.rjCsv.list(regExp: _devFilter);
  final srcFiles = fileSystem.rjCsv.toAbsolute(relFiles);
  var destFiles = fileSystem.rjMsg.changeExtension(relFiles, '.msg');
  destFiles = fileSystem.rjMsg.toAbsolute(destFiles);
  final filenames =
      utils.zip(srcFiles, destFiles).map((tuple) => ToRaw.Files()
        ..srcRj = tuple.item1
        ..destRaw = tuple.item2);

  msg.files.addAll(filenames);
  return client.ToRaw_Run(msg);
  //makeRequest((client) => client.matrixsToBooksFromRJ(msg));
}

