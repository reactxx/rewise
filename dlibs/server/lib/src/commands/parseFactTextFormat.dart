import 'package:rewise_low_utils/messages.dart' as messages;
import 'package:rewise_low_utils/utils.dart' as utils;
import 'package:server_dart/utils.dart' show fileSystem, makeRequest;

const devFilter = r'goetheverlag\.msg';

callWordBreaks() {
  final relFiles = fileSystem.rjMsg.list(regExp: devFilter);
}
