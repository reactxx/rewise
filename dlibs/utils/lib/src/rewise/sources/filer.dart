import 'package:rw_utils/utils.dart' show fileSystem;
import 'consts.dart';

class Filer {

  static List<FileInfo> get files =>
      _files ??
      (_files = fileSystem.source
          .list(regExp: r'\.csv$')
          .map((f) => FileInfo.infoFromPath(f))
          .toList());

  static List<FileInfo> _files;
}
