import 'package:rewise_low_utils/messages.dart' as messages;
import 'package:rewise_low_utils/utils.dart' as utils;
import 'package:server_dart/utils.dart';

const _devFilter = r'goetheverlag\.msg';

bookOutsToRawBook() {
  final relFiles = fileSystem.rjMsg.list(regExp: _devFilter);
  for(var fn in relFiles) {
    var bookOut = messages.BooksFromRJ.fromBuffer(fileSystem.rjMsg.readAsBytes(fn));
    bookOut = null;
  }
}
