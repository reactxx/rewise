import 'package:rewise_low_utils/rw/to_raw.dart' as ToRaw;
import 'package:rewise_low_utils/utils.dart' as utils;
import 'package:rewise_low_utils/rw/client.dart' as client;
import 'package:rewise_low_utils/designTime.dart' show fileSystem;

const _devFilter = r'goetheverlag\.csv';

Future<String> toRaw() async {
  final msg = ToRaw.Request();
  final relFiles = fileSystem.csv.list(regExp: _devFilter);
  final srcFiles = fileSystem.csv.toAbsolute(relFiles);
  var destFiles = fileSystem.raw.changeExtension(relFiles, '.msg');
  destFiles = fileSystem.raw.toAbsolute(destFiles);
  final filenames = utils.zip(srcFiles, destFiles).map((tuple) => ToRaw.Files()
    ..srcRj = tuple.item1
    ..destRaw = tuple.item2);

  msg.files.addAll(filenames);
  final resp = await client.ToRaw_Run(msg);
  if (resp.error.isNotEmpty)
    fileSystem.log.writeAsString('toRaw.log', resp.error);
  return resp.error;
}
