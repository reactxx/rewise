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
  static final wInBr = 0x1;
  // word is part of another word
  static final wIsPartOf = 0x2;
  // word has parts, don't use it for stemming
  static final wHasParts = 0x4;
  // word contains whole content of [] brackets
  static final wBrSq = 0x8;
  // word contains whole content of {} brackets
  static final wBrCurl = 0x10;
  // latin word in non latin text
  static final wIsLatin = 0x20;

  // ^ or | in brackets
  static final feDelimInBracket = 0x40;
  // missing ) bracket
  static final feMissingBr = 0x80;
  // missing } bracket
  static final feMissingCurlBr = 0x100;
  // missing ] bracket
  static final feMissingSqBr = 0x200;
  // unexpected ) bracket
  static final feUnexpectedBr = 0x400;
  // unexpected } bracket
  static final feUnexpectedCurlBr = 0x800;
  // unexpected ] bracket
  static final feUnexpectedSqBr = 0x1000;
  // no word in fact
  static final feNoWordInFact = 0x2000;
  // mixing different brakets
  static final feMixingBrs = 0x4000;
  // more than single []
  static final feSingleWClassAllowed = 0x8000;
  // [] not in first fact
  static final feWClassNotInFirstFact = 0x10000;
  // missing []
  static final feMissingWClass = 0x20000;

  static final factErrors =
      Linq.range(6, 17 - 6 + 1).map((i) => 1 << i).toList();

  // e.g. left word is in right script
  static final weOtherScript = 0x40000;
  static final weWrongUnicode = 0x80000;
  static final weWrongCldr = 0x100000;

  static String toText(int f) {
    _buf.clear();
    for (var i = 0; i <= _codes.length; i++)
      if (((f >> i) & 0x1) != 0) {
        if (_buf.length > 0) _buf.write(', ');
        _buf.write(_codes[1 << i]);
      }
    return _buf.toString();
  }

  static final _buf = StringBuffer();

  static final _codes = <int, String>{
    wInBr: 'wInBr',
    wIsPartOf: 'wIsPartOf',
    wHasParts: 'wHasParts',
    wBrSq: 'wBrSq',
    wBrCurl: 'wBrCurl',
    wIsLatin: 'wIsLatin',
    feDelimInBracket: 'feDelimInBracket',
    feMissingBr: 'feMissingBr',
    feMissingCurlBr: 'feMissingCurlBr',
    feMissingSqBr: 'feMissingSqBr',
    feUnexpectedBr: 'feUnexpectedBr',
    feUnexpectedCurlBr: 'feUnexpectedCurlBr',
    feUnexpectedSqBr: 'feUnexpectedSqBr',
    feNoWordInFact: 'feNoWordInFact',
    feMixingBrs: 'feMixingBrs',
    feSingleWClassAllowed: 'feSingleWClassAllowed',
    feWClassNotInFirstFact: 'feWClassNotInFirstFact',
    feMissingWClass: 'feMissingWClass',
    weOtherScript: 'weOtherScript',
    weWrongUnicode: 'weWrongUnicode',
    weWrongCldr: 'weWrongCldr',
  };
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
