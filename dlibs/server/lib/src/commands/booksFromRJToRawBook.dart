//import 'package:rewise_low_utils/rw/to_raw.dart' as toRaw;
//import 'package:rewise_low_utils/utils.dart' as utils;
import 'package:rewise_low_utils/designTime.dart';
//import 'package:server_dart/utils.dart';

const _devFilter = r'goetheverlag\.msg';

bookFromRJToRawBook() {
  final relFiles = fileSystem.rjMsg.list(regExp: _devFilter);
  for(var fn in relFiles) {
    //var bookOut = toRaw.BooksFromRJ.fromBuffer(fileSystem.rjMsg.readAsBytes(fn));
    //bookOut = null;
  }
}
