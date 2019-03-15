import 'package:rewise_low_utils/rw/to_parsed.dart' as ToParsed;
//import 'package:rewise_low_utils/utils.dart' as utils;
import 'package:rewise_low_utils/designTime.dart';
//import 'package:server_dart/utils.dart';

const _devFilter = r'goetheverlag\.msg';

toParsed() {
  final relFiles = fileSystem.raw.list(regExp: _devFilter);
  for(final fn in relFiles) {
    final raw = ToParsed.RawBook.fromBuffer(fileSystem.raw.readAsBytes(fn));
    for(var idx =0; idx<raw.facts.length; idx++) {

    }
    //bookOut = null;
  }
}
