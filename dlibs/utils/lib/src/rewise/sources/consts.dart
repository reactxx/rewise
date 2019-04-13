import 'package:path/path.dart' as p;
import 'package:rw_low/code.dart' show Linq;

class EditMode {
  static const NONE = 0; // standart format
  static const ASSTRING = 1; // asString only
}

class BookType {
  static const KDICT = 0; // kdict
  static const DICT = 1; // lingea and other dicts
  static const ETALK = 2; // goethe, eurotalk
  static const BOOK = 3; // templates and local dicts
}

class FileType {
  static const LEFT = 0;
  static const LANG = 1;
  static const LANGLEFT = 2;
}

class Flags {
  // word is in () bracket
  static final wInBr = _r('wInBr', 0);
  // word is part of another word
  static final wIsPartOf = _r('wInOtherWord', 1);
  // word has parts, don't use it for stemming
  static final wHasParts = _r('wHasParts', 2);
  // word contains whole content of [] brackets
  static final wBrSq = _r('wBrSq', 3);
  // word contains whole content of {} brackets
  static final wBrCurl = _r('wBrCurl', 4);
  // latin word in non latin text
  static final wIsLatin = _r('wIsLatin', 5);

  // ^ or | in brackets
  static final feDelimInBracket = _r('feDelimInBracket', 6);
  // missing ) bracket
  static final feMissingBr = _r('feMissingBr', 7);
  // missing } bracket
  static final feMissingCurlBr = _r('feMissingCurlBr', 8);
  // missing ] bracket
  static final feMissingSqBr = _r('feMissingSqBr', 9);
  // unexpected ) bracket
  static final feUnexpectedBr = _r('feUnexpectedBr', 10);
  // unexpected } bracket
  static final feUnexpectedCurlBr = _r('feUnexpectedCurlBr', 11);
  // unexpected ] bracket
  static final feUnexpectedSqBr = _r('feUnexpectedSqBr', 12);
  // no word in fact
  static final feNoWordInFact = _r('feNoWordInFact', 13);
  // mixing different brakets
  static final feMixingBrs = _r('feMixingBrs', 14);
  // // more than single []
  // static final feSingleWClassAllowed = _r('feSingleWClassAllowed', 0x2000);
  // // missing []
  // static final feMissingWClass = _r('feMissingWClass', 0x4000);
  // // [] not in first fact
  // static final feWClassNotInFirstFact = _r('feWClassNotInFirstFact', 0x8000);

  static final factErrors = Linq.range(6, 14-6).map((i) => 1<<i).toList();

  // e.g. left word is in right script
  static final weOtherScript = _r('weOtherScript', 15);
  static final weWrongUnicode = _r('weWrongUnicode', 16);
  static final weWrongCldr = _r('weWrongCldr', 17);

  static String toText(int f) {
    _buf.clear();
    for (var i = 0; i < 16; i++)
      if (((f >> i) & 0x1) != 0) {
        if (_buf.length > 0) _buf.write(', ');
        _buf.write(_codes[1 << i]);
      }
    return _buf.toString();
  }

  static final _buf = StringBuffer();

  static int _r(String id, int s) {
    final f = 1 << s;
    _codes[f] = id;
    return f;
  }

  static final _codes = Map<int, String>();
}


class FileInfo {
  FileInfo();
  factory FileInfo.infoFromPath(String path) {
    final parts = p.split(path);
    assert(parts.length == 3);
    final np = parts[2].split('.');
    assert(np.length == 3 || np.length == 2);
    return FileInfo._(
        parts[0],
        parts[1],
        np[0],
        np.length == 2
            ? FileType.LANG
            : (np[0].isEmpty ? FileType.LEFT : FileType.LANGLEFT),
        bookNameToType(parts));
  }
  FileInfo._(
      this.leftLang, this.bookName, this.lang, this.fileType, this.bookType);

  String leftLang = '';
  String bookName = '';
  String lang = '';
  int bookType = 0;
  int fileType = 0;

  String get fileName {
    String path =
        '${bookType == BookType.ETALK ? 'all' : leftLang}\\${bookName}\\';
    switch (fileType) {
      case FileType.LEFT:
        return '$path.left.csv';
      case FileType.LANG:
        return '$path${lang}.csv';
      case FileType.LANGLEFT:
        return '$path${lang}.left.csv';
      default:
        throw Exception();
    }
  }

  String get dataLang => fileType == FileType.LANG ? lang : leftLang;

  static int bookNameToType(List<String> parts) {
    if (parts[0] == 'all') return BookType.ETALK;
    if (parts[1] == '#kdictionaries') return BookType.KDICT;
    return parts[1].startsWith('#') ? BookType.DICT : BookType.BOOK;
  }

}
