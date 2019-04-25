import 'package:path/path.dart' as p;

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

class FactFlags {
  // unexpected ) bracket
  static const feUnexpectedBr = 0x1;
  // unexpected } bracket
  static const feUnexpectedCurlBr = 0x2;
  // unexpected ] bracket
  static const feUnexpectedSqBr = 0x4;
  // no word in fact
  static const feNoWordInFact = 0x8;
  // mixing different brakets
  static const feMixingBrs = 0x10;
  // more than single []
  static const feSingleWClassAllowed = 0x20;
  // ^ or | in brackets
  static const feDelimInBracket = 0x40;
  // missing ) bracket
  static const feMissingBr = 0x80;
  // missing } bracket
  static const feMissingCurlBr = 0x100;
  // missing ] bracket
  static const feMissingSqBr = 0x200;
  // [] not in first fact
  static const feWClassNotInFirstFact = 0x400;
  // missing []
  static const feMissingWClass = 0x800;


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

  static const _codes = <int, String>{
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
  };
  static final factErrors = _codes.keys.toList();
  static final factErrorsFlag = _codes.keys.fold(0, (r, i) => r | i);
}

class WordFlags {
  static const cldr = 0x1; // all chars is CLDR alphabet
  static const ok = 0x2; // all chars is lang script
  static const spell = 0x4; // spell OK
  static const nonLetter = 0x08; // all non letter

  static const cldrSpell = cldr | spell;
  static const okSpell = ok | spell;

  // word is in () bracket
  static const wInBr = 0x10;
  // word is part of another word
  static const wIsPartOf = 0x20;
  // word has parts, don't use it for stemming
  static const wHasParts = 0x40;
  // word contains whole content of [] brackets
  static const wBrSq = 0x80;
  // word contains whole content of {} brackets
  static const wBrCurl = 0x100;
  static const latin = 0x200; // for non Latn script: all Latn

  static const nonLetterAny = 0x400; // some non letter, rest OK
  static const latinAny = 0x800; // some Latn, rest OK
  static const mixAny = 0x1000; // some mix, rest OK
  static const wrong = 0x2000; //  all chars in another script(s)
  static const mix = 0x4000; // all mix

  static final _codes = <_FlagName>[
    _FlagName(cldrSpell, 'cldrSpell'), 
    _FlagName(okSpell, 'okSpell'), 
    //----
    _FlagName(cldr, 'cldr'),
    _FlagName(ok, 'ok'),
    _FlagName(spell, 'okSpell'),
    _FlagName(latin, 'latin'),
    _FlagName(nonLetter, 'nonLetter'),
    _FlagName(nonLetterAny, 'nonLetterAny'),
    _FlagName(latinAny, 'latinAny'),
    _FlagName(mix, 'mix'),
    _FlagName(mixAny, 'mixAny'),
    _FlagName(wrong, 'wrong'),
];
  

  static String toTextFirst(int f) => _codes.firstWhere((fl) => (f & fl.flag) == fl.flag)?.name;

  // static String toText(int f) {
  //   _buf.clear();
  //   for (var i = 0; i <= _codes.length; i++)
  //     if (((f >> i) & 0x1) != 0) {
  //       if (_buf.length > 0) _buf.write(', ');
  //       _buf.write(_codes[1 << i]);
  //     }
  //   return _buf.toString();
  // }
  // static final _buf = StringBuffer();

}

class FileInfo {
  FileInfo();
  factory FileInfo.infoFromPath(String path) {
    final parts = p.split(path);
    assert(parts.length == 3);
    final np = parts[2].split('.');
    assert(np.length == 3 || np.length == 2);
    return FileInfo.low(
        parts[0],
        parts[1],
        np[0],
        np.length == 2
            ? FileType.LANG
            : (np[0].isEmpty ? FileType.LEFT : FileType.LANGLEFT),
        bookNameToType(parts));
  }
  FileInfo.low(
      this.leftLang, this.bookName, this.lang, this.fileType, this.bookType);

  FileInfo.fromDataMsg(Iterator iter) {
    leftLang = (iter..moveNext()).current;
    bookName = (iter..moveNext()).current;
    lang = (iter..moveNext()).current;
    bookType = (iter..moveNext()).current;
    fileType = (iter..moveNext()).current;
  }

  Iterable toDataMsg() sync* {
    yield leftLang;
    yield bookName;
    yield lang;
    yield bookType;
    yield fileType;
  }

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
  String get otherLang =>
      bookType == BookType.ETALK || fileType == FileType.LEFT ? '' : leftLang;

  static int bookNameToType(List<String> parts) {
    if (parts[0] == 'all') return BookType.ETALK;
    if (parts[1] == '#kdictionaries') return BookType.KDICT;
    return parts[1].startsWith('#') ? BookType.DICT : BookType.BOOK;
  }
}

class _FlagName {
  _FlagName(this.flag, this.name);
  final int flag;
  final String name;
}